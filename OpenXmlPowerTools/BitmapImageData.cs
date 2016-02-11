using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenXmlPowerTools
{
    public interface IBitmapImageData
    {
        ImagePartType ImageType { get; }
        byte[] ImageBytes { get; }
        float VerticalResolution { get; }
        float HorizontalResolution { get; }
        Size Size { get; }
    }

    public class BitmapImageData : IBitmapImageData
    {
        private Bitmap bmp;
        public BitmapImageData(byte[] imageBytes)
        {
            this.ImageBytes = imageBytes;
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                bmp = new Bitmap(ms);
            }
        }

        public BitmapImageData(Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                memoryStream.Position = 0;
                this.ImageBytes = memoryStream.ToArray();
                this.bmp = new Bitmap(memoryStream);
            }
        }

        public ImagePartType ImageType
        {
            get
            {
                if (bmp.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
                    return ImagePartType.Jpeg;
                else if (bmp.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Emf))
                    return ImagePartType.Emf;
                else if (bmp.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
                    return ImagePartType.Gif;
                else if (bmp.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
                    return ImagePartType.Bmp;
                else if (bmp.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Icon))
                    return ImagePartType.Icon;
                else if (bmp.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Tiff))
                    return ImagePartType.Tiff;
                else
                    return ImagePartType.Png;                
            }
        }

        public byte[] ImageBytes
        {
            get;
            private set;
        }


        public float VerticalResolution
        {
            get { return bmp.VerticalResolution; }
        }

        public float HorizontalResolution
        {
            get { return bmp.HorizontalResolution; }
        }


        public Size Size
        {
            get { return bmp.Size; }
        }
    }
}
