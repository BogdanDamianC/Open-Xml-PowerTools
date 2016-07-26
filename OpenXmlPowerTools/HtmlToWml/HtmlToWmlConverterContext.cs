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
        public WordprocessingDocument WordDocument;
        public Dictionary<XElement, bool> cachedHasBlockChildren = new Dictionary<XElement, bool>();

        public HtmlToWmlConverterContext(HtmlToWmlConverterSettings settings, WmlDocument emptyDocument)
        {
            this.ConversionSettings = settings;
            this.emptyDocument = emptyDocument ?? HtmlToWmlConverter.EmptyDocument;
        }
    }
}
