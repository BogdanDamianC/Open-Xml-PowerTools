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

namespace OpenXmlPowerTools.HtmlToWml
{
    /// <summary>
    /// common interface to access the document parts common properties for the MainDocumentPart HeaderPart and FooterPart - they don't seem to have a common interface
    /// </summary>
    internal interface IOpenXMLDocumentPartWrapper
    {
        IEnumerable<ImagePart> ImageParts { get; }
        ImagePart AddImagePart(ImagePartType partType, string id);
        HyperlinkRelationship AddHyperlinkRelationship(Uri hyperlinkUri, bool isExternal, string id);
    }

    internal class OpenXMLDocumentPartWrapper : IOpenXMLDocumentPartWrapper
    {
        private dynamic documentPart;
        public OpenXMLDocumentPartWrapper(object documentPart)
        {
            this.documentPart = documentPart;
        }

        public IEnumerable<ImagePart> ImageParts { get { return this.documentPart.ImageParts; } }
        public ImagePart AddImagePart(ImagePartType partType, string id)
        {
            return this.documentPart.AddImagePart(partType, id);
        }
        public HyperlinkRelationship AddHyperlinkRelationship(Uri hyperlinkUri, bool isExternal, string id)
        {
            return this.documentPart.AddHyperlinkRelationship(hyperlinkUri, isExternal, id);
        }
    }
}
