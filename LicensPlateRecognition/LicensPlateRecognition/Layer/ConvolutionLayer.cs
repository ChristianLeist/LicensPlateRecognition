using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    class ConvolutionLayer : Layer
    {
        private int[,] filterMat;
        private int stride;

        public ConvolutionLayer()
        {
            this.stride = 2;
            // Edge detection filter
            this.filterMat = new int[3, 3] { { -1, -1, -1 }, { -1, 8, -1 }, { -1, -1, -1 } };
        }

        public void Convolution(Bitmap inputBitmap)
        {
            for (int i = 0; i < inputBitmap.Height; i++)
            {
                for (int j = 0; j < inputBitmap.Width; j++)
                {
                    // TODO: convolution
                }
            }
        }

        public Bitmap ZeroPadding(Bitmap inputImage, int border)
        {
            int width = inputImage.Width;
            int heigth = inputImage.Height;

            int padWidth = width + 2 * border;
            int padHeigth = heigth + 2 * border;

            Bitmap padImage = new Bitmap(padWidth, padHeigth);
            BitmapData padImageData = padImage.LockBits(new Rectangle(0, 0, padWidth, padHeigth), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            BitmapData inputImageData = inputImage.LockBits(new Rectangle(0, 0, width, heigth), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            int inputImageByte = inputImageData.Stride * inputImageData.Height;
            int padImageByte = padImageData.Stride * padImageData.Height;
            byte[] inputImageArray = new byte[inputImageByte];
            byte[] padImageArray = new byte[padImageByte];

            // alpha value init
            for (int i = 3; i < padImageByte; i += 4)
            {
                padImageArray[i] = 255;
            }
            // Copy image values into array
            Marshal.Copy(inputImageData.Scan0, inputImageArray, 0, inputImageByte);
            inputImage.UnlockBits(inputImageData);

            // Create image with zero padded borders
            for (int y = 0; y < heigth; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int inputImgPixel = y * inputImageData.Stride + x * 4;
                    int padImgPixel = y * padImageData.Stride + x * 4;
                    for (int i = 0; i < 3; i++)
                    {
                        padImageArray[(padImageData.Stride + 4) * border + padImgPixel + i] = inputImageArray[inputImgPixel + i];
                    }
                }
            }
            // Copy array values into padded image
            Marshal.Copy(padImageArray, 0, padImageData.Scan0, padImageByte);
            padImage.UnlockBits(padImageData);

            return padImage;
        }

        public override void InitLayerMat()
        {
            // TODO
        }

        public void InitFilterMat()
        {
            // TODO
        }
    }
}
