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
            this.stride = 1;
            // sharpen filter
            this.filterMat = new int[3, 3] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } };
        }

        public Bitmap Convolution(Bitmap inputBitmap)
        {
            // Zero padding border formular with FxF filter: (F-1)/2
            int f = (int)Math.Sqrt(this.filterMat.Length);
            int border = (int)(f - 1) / 2;
            Bitmap padInputBitmap = ZeroPadding(inputBitmap, border);

            int width = padInputBitmap.Width;
            int heigth = padInputBitmap.Height;

            int outWidth = inputBitmap.Width / this.stride;
            int outHeigth = inputBitmap.Height / this.stride;

            Bitmap outImage = new Bitmap(outWidth, outHeigth);
            BitmapData outImageData = outImage.LockBits(new Rectangle(0, 0, outWidth, outHeigth), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            BitmapData inputImageData = padInputBitmap.LockBits(new Rectangle(0, 0, width, heigth), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            int inputImageByte = inputImageData.Stride * inputImageData.Height;
            int outImageByte = outImageData.Stride * outImageData.Height;
            byte[] inputImageArray = new byte[inputImageByte];
            byte[] outImageArray = new byte[outImageByte];

            // alpha value init
            for (int i = 3; i < outImageByte; i += 4)
            {
                outImageArray[i] = 255;
            }
            // Copy image values into array
            Marshal.Copy(inputImageData.Scan0, inputImageArray, 0, inputImageByte);
            padInputBitmap.UnlockBits(inputImageData);

            // Do convolution
            for (int y = 0; y < outHeigth; y++)
            {
                for (int x = 0; x < outWidth; x++)
                {
                    int inputImgPixel = y * inputImageData.Stride + x * 4;
                    int outImgPixel = y * outImageData.Stride + x * 4;
                    for (int i = 0; i < 3; i++)
                    {
                        // Filter loop
                        int convVal = 0;
                        for (int h = 0; h < f; h++)
                        {
                            for (int w = 0; w < f; w++)
                            {

                                convVal += filterMat[h, w] * inputImageArray[inputImageData.Stride * h + 4 * w + inputImgPixel + i];
                            }
                        }
                        if (convVal > 255)
                        {
                            convVal = 255;
                        }
                        else if (convVal < 0)
                        {
                            convVal = 0;
                        }

                        outImageArray[outImgPixel + i] = (byte)convVal;
                    }

                }
            }
            // Copy array values into output image
            Marshal.Copy(outImageArray, 0, outImageData.Scan0, outImageByte);
            outImage.UnlockBits(outImageData);

            return outImage;
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
