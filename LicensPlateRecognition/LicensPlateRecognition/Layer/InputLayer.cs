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
        private int inRangeMin, inRangeMax;

        public InputLayer(int width, int height) : base(null)
        {
            this.width = width;
            this.height = height;
            this.depth = 3;
            this.inRangeMax = 255;
            this.inRangeMin = 0;
            this.imgMatrix = new double[width][][];
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
            for (int z = 0; z < this.depth; z++)
            {
                for (int y = 0; y < this.height; y++)
                {
                    for (int x = 0; x < this.width; x++)
                    {
                        // init imgMatrix
                        if (z == 0)
                        {
                            if (y == 0)
                            {
                                this.imgMatrix[x] = new double[this.height][];
                            }
                            this.imgMatrix[x][y] = new double[this.depth];
                        }

                        // fill imgMatrix
                        int inputImgPixel = y * inputImageData.Stride + x * 4;
                        this.imgMatrix[x][y][z] = inputImageArray[inputImgPixel + z] - this.inRangeMin / 
                                                                    (this.inRangeMax - this.inRangeMin);
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
    }
}
