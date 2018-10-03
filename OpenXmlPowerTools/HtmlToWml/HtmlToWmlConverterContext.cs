using System.Collections.Generic;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;

namespace OpenXmlPowerTools.HtmlToWml
{
    internal class HtmlToWmlConverterContext
    {
        public HtmlToWmlConverterSettings ConversionSettings;
        public WmlDocument emptyDocument;
        public WordprocessingDocument WordDocument;
        public Dictionary<XElement, bool> cachedHasBlockChildren = new Dictionary<XElement, bool>();

        public HtmlToWmlConverterContext(HtmlToWmlConverterSettings settings, WmlDocument emptyDocument)
        {
            this.ConversionSettings = settings;
            this.emptyDocument = emptyDocument ?? HtmlToWmlConverter.EmptyDocument;
        }
    }
}
