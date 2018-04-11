using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    class InputLayer : Layer
    {
        private int[,,] imgArray;
        private int width, height;

        public InputLayer (int width, int height)
        {
            this.width = width;
            this.height = height;
            this.imgArray = new int[width, height, 3];
        }

        public int[,,] ImgArray
        {
            get => this.imgArray;
        }

        public void LoadImage(Bitmap inputImg)
        {
            inputImg = ResizeImage(inputImg);
            BitmapData inputImageData = inputImg.LockBits(new Rectangle(0, 0, inputImg.Width, inputImg.Height), 
                                                          ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            int inputImageByte = inputImageData.Stride * inputImageData.Height;
            byte[] inputImageArray = new byte[inputImageByte];

            // Copy image values into byte array
            Marshal.Copy(inputImageData.Scan0, inputImageArray, 0, inputImageByte);
            inputImg.UnlockBits(inputImageData);

            // Copy bitmap values into imgArray
            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    int inputImgPixel = y * inputImageData.Stride + x * 4;
                    for (int z = 0; z < this.imgArray.GetLength(2); z++)
                    {
                        imgArray[x, y, z] = inputImageArray[inputImgPixel + z];
                    }
                }
            }
        }

        public Bitmap ResizeImage(Image image)
        {
            var destRect = new Rectangle(0, 0, this.width, this.height);
            var destImage = new Bitmap(this.width, this.height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public override void InitLayerMat()
        {
            // Not necessary in an input layer of a convNet
        }
    }
}
