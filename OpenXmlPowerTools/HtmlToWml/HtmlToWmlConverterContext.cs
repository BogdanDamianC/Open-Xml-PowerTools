using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using OpenXmlPowerTools.HtmlToWml;
using OpenXmlPowerTools.HtmlToWml.CSS;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenXmlPowerTools.HtmlToWml
{
    internal class HtmlToWmlConverterContext
    {
        public HtmlToWmlConverterSettings ConversionSettings;
        public WmlDocument emptyDocument;
        public XElement PreparedHeaderHtml;
        public XElement PreparedBodyHtml;
        public XElement PreparedFooterHtml;
        public WordprocessingDocument WordDocument;
        public Dictionary<XElement, bool> cachedHasBlockChildren = new Dictionary<XElement, bool>();

        public HtmlToWmlConverterContext(HtmlToWmlConverterSettings settings, WmlDocument emptyDocument, XElement headerxhtml, XElement bodyxhtml, XElement footerxhtml)
        {
            this.ConversionSettings = settings;
            this.emptyDocument = emptyDocument ?? HtmlToWmlConverter.EmptyDocument;
            if (headerxhtml != null)
                this.PreparedHeaderHtml = PrepareForProcessing(headerxhtml);
            this.PreparedBodyHtml = PrepareForProcessing(bodyxhtml);
        }

        private static XElement PrepareForProcessing(XElement html)
        {
            // clone and transform all element names to lower case
            var preparedHtml = (XElement)TransformToLower(html);

            // add pseudo cells for rowspan
            preparedHtml = (XElement)AddPseudoCells(preparedHtml);

            preparedHtml = (XElement)TransformWhiteSpaceInPreCodeTtKbdSamp(preparedHtml, false, false);
            return preparedHtml;
        }

        private static object TransformToLower(XNode node)
        {
            XElement element = node as XElement;
            if (element != null)
            {
                XElement e = new XElement(element.Name.LocalName.ToLower(),
                    element.Attributes().Select(a => new XAttribute(a.Name.LocalName.ToLower(), a.Value)),
                    element.Nodes().Select(n => TransformToLower(n)));
                return e;
            }
            return node;
        }

        private static XElement AddPseudoCells(XElement html)
        {
            while (true)
            {
                var rowSpanCell = html
                    .Descendants(XhtmlNoNamespace.td)
                    .FirstOrDefault(td => td.Attribute(XhtmlNoNamespace.rowspan) != null && td.Attribute("HtmlToWmlVMergeRestart") == null);
                if (rowSpanCell == null)
                    break;
                rowSpanCell.Add(
                    new XAttribute("HtmlToWmlVMergeRestart", "true"));
                int colNumber = rowSpanCell.ElementsBeforeSelf(XhtmlNoNamespace.td).Count();
                int numberPseudoToAdd = (int)rowSpanCell.Attribute(XhtmlNoNamespace.rowspan) - 1;
                var tr = rowSpanCell.Ancestors(XhtmlNoNamespace.tr).FirstOrDefault();
                if (tr == null)
                    throw new OpenXmlPowerToolsException("Invalid HTML - td does not have parent tr");
                var rowsToAddTo = tr
                    .ElementsAfterSelf(XhtmlNoNamespace.tr)
                    .Take(numberPseudoToAdd)
                    .ToList();
                foreach (var rowToAddTo in rowsToAddTo)
                {
                    if (colNumber > 0)
                    {
                        var tdToAddAfter = rowToAddTo
                            .Elements(XhtmlNoNamespace.td)
                            .Skip(colNumber - 1)
                            .FirstOrDefault();
                        var td = new XElement(XhtmlNoNamespace.td,
                            rowSpanCell.Attributes(),
                            new XAttribute("HtmlToWmlVMergeNoRestart", "true"));
                        tdToAddAfter.AddAfterSelf(td);
                    }
                    else
                    {
                        var tdToAddBefore = rowToAddTo
                            .Elements(XhtmlNoNamespace.td)
                            .Skip(colNumber)
                            .FirstOrDefault();
                        var td = new XElement(XhtmlNoNamespace.td,
                            rowSpanCell.Attributes(),
                            new XAttribute("HtmlToWmlVMergeNoRestart", "true"));
                        tdToAddBefore.AddBeforeSelf(td);
                    }
                }
            }
            return html;
        }

        private static object TransformWhiteSpaceInPreCodeTtKbdSamp(XNode node, bool inPre, bool inOther)
        {
            XElement element = node as XElement;
            if (element != null)
            {
                if (element.Name == XhtmlNoNamespace.pre)
                {
                    return new XElement(element.Name,
                        element.Attributes(),
                        element.Nodes().Select(n => TransformWhiteSpaceInPreCodeTtKbdSamp(n, true, false)));
                }
                if (element.Name == XhtmlNoNamespace.code ||
                    element.Name == XhtmlNoNamespace.tt ||
                    element.Name == XhtmlNoNamespace.kbd ||
                    element.Name == XhtmlNoNamespace.samp)
                {
                    return new XElement(element.Name,
                        element.Attributes(),
                        element.Nodes().Select(n => TransformWhiteSpaceInPreCodeTtKbdSamp(n, false, true)));
                }
                return new XElement(element.Name,
                    element.Attributes(),
                    element.Nodes().Select(n => TransformWhiteSpaceInPreCodeTtKbdSamp(n, false, false)));
            }
            XText xt = node as XText;
            if (xt != null && inPre)
            {
                var val = xt.Value.TrimStart('\r', '\n').TrimEnd('\r', '\n');
                var groupedCharacters = val.GroupAdjacent(c => c == '\r' || c == '\n');
                var newNodes = groupedCharacters.Select(g =>
                {
                    if (g.Key == true)
                        return (object)(new XElement(XhtmlNoNamespace.br));
                    string x = g.Select(c => c.ToString()).StringConcatenate();
                    return new XText(x);
                });
                return newNodes;
            }
            if (xt != null && inOther)
            {
                var val = xt.Value.TrimStart('\r', '\n', '\t', ' ').TrimEnd('\r', '\n', '\t', ' ');
                return new XText(val);
            }
            return node;
        }
    }
}
