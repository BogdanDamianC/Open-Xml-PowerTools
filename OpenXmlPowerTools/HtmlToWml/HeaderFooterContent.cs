using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpenXmlPowerTools.HtmlToWml
{
    public enum HeaderFooterContentType { Header, Footer};
    public enum HeaderFooterContentLocation { Default, First, Even, Odd };
    public class HeaderFooterContent
    {
        public HeaderFooterContent(HeaderFooterContentType Type, HeaderFooterContentLocation Location, XElement XHtmlContent)
        {
            this.Type = Type;
            this.Location = Location;
            this.XHtmlContent = XHtmlContent;
        }
        public HeaderFooterContentType Type { get; private set; }
        public HeaderFooterContentLocation Location { get; private set; }
        public XElement XHtmlContent { get; private set; }
        public string GetLocationOpenXMLName()
        {
            switch (Location)
            {
                case HeaderFooterContentLocation.First: return "first";
                case HeaderFooterContentLocation.Even: return "even";
                case HeaderFooterContentLocation.Odd: return "odd";
                default: return "default";
            }
        }

    }
}
