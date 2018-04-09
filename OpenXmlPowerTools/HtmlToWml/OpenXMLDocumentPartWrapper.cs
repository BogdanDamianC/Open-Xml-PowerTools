using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;

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

    internal class OpenXMLDocumentPartWrapperForHeaderPart : IOpenXMLDocumentPartWrapper
    {
        private HeaderPart documentPart;
        public OpenXMLDocumentPartWrapperForHeaderPart(HeaderPart documentPart)
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

    internal class OpenXMLDocumentPartWrapperForMainDocumentPart : IOpenXMLDocumentPartWrapper
    {
        private MainDocumentPart documentPart;
        public OpenXMLDocumentPartWrapperForMainDocumentPart(MainDocumentPart documentPart)
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

    internal class OpenXMLDocumentPartWrapperForFooterPart : IOpenXMLDocumentPartWrapper
    {
        private FooterPart documentPart;
        public OpenXMLDocumentPartWrapperForFooterPart(FooterPart documentPart)
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
