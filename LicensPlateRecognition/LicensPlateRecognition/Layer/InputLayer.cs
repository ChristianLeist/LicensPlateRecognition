using LicensPlateRecognition.Network;
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

        public InputLayer(int width, int height, int depth, NeuralNetwork network) : base(network)
        {
            this.Width = width;
            this.Height = height;
            this.Depth = depth;
            this.inRangeMax = 255;
            this.inRangeMin = 0;
            this.ImgMatrix = new double[width][][];
        }

        public override void FeedForward(Image img, double[] flat, double[][][] matrix)
        {
            LoadImage(ResizeImage(img));
        }

        public override void BackwardPass(double[] deltaArray, double[][][] deltaMatrix)
        {
            Console.WriteLine("Backwardpass done!");
        }

        public void LoadImage(Bitmap inputImg)
        {
            BitmapData inputImageData = inputImg.LockBits(new Rectangle(0, 0, inputImg.Width, inputImg.Height),
                                                          ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            int inputImageByte = inputImageData.Stride * inputImageData.Height;
            byte[] inputImageArray = new byte[inputImageByte];

            // Copy image values into byte array
            Marshal.Copy(inputImageData.Scan0, inputImageArray, 0, inputImageByte);
            inputImg.UnlockBits(inputImageData);

            // Copy bitmap values into imgArray
            for (int z = 0; z < this.Depth; z++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    for (int x = 0; x < this.Width; x++)
                    {
                        // init imgMatrix
                        if (z == 0)
                        {
                            if (y == 0)
                            {
                                this.ImgMatrix[x] = new double[this.Height][];
                            }
                            this.ImgMatrix[x][y] = new double[this.Depth];
                        }

                        // fill imgMatrix
                        int inputImgPixel = y * inputImageData.Stride + x * 4;
                        this.ImgMatrix[x][y][z] = inputImageArray[inputImgPixel + z] - this.inRangeMin /
                                                                    (this.inRangeMax - this.inRangeMin);
                    }
                }
            }
        }

        public Bitmap ResizeImage(Image image)
        {
            var destRect = new Rectangle(0, 0, this.Width, this.Height);
            var destImage = new Bitmap(this.Width, this.Height);

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

        public override void InitLayer(int height, int width)
        {
            throw new NotImplementedException();
        }

        public override void PrintArray()
        {
            throw new NotImplementedException();
        }

        public override void RandInitFilter()
        {
            throw new NotImplementedException();
        }

        public override void RandInitLayerMat()
        {
            throw new NotImplementedException();
        }
    }
}
