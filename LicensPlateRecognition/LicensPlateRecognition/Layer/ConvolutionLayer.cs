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
        // TODO: Filter list instad of single filter
        private int[,,] filterMat;
        private int stride;

        public ConvolutionLayer()
        {
            this.stride = 2;
            // Test filter ini
            this.filterMat = new int[3, 3, 3] { { { 0, 0, 0 }, { -1, -1, -1 }, { 0, 0, 0 } },
                                                { { -1, -1, -1 }, { 5, 5, 5 }, { -1, -1, -1 } },
                                                { { 0, 0, 0 }, { -1, -1, -1 }, { 0, 0, 0 } } };
        }

        // TODO: Change Method to convolution with 3-dim array
        public Bitmap Convolution(Bitmap inputBitmap)
        {
            // Zero padding border formular with fxf filter: (f-1)/2
            int f = this.filterMat.GetLength(0);
            int border = (f - 1) / 2;
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
                    int inputImgPixel = y * inputImageData.Stride * this.stride + x * 4 * this.stride;
                    int outImgPixel = y * outImageData.Stride + x * 4;
                    for (int d = 0; d < this.filterMat.GetLength(2); d++)
                    {
                        int convVal = 0;
                        for (int h = 0; h < f; h++)
                        {
                            for (int w = 0; w < f; w++)
                            {
                                convVal += filterMat[h, w, d] * inputImageArray[inputImageData.Stride * h + 4 * w + inputImgPixel + d];
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

                        outImageArray[outImgPixel + d] = (byte)convVal;
                    }

                }
            }
            // Copy array values into output image
            Marshal.Copy(outImageArray, 0, outImageData.Scan0, outImageByte);
            outImage.UnlockBits(outImageData);

            return outImage;
        }

        public int[,,] ZeroPadding(int[,,] inputImage, int border)
        {
            int width = inputImage.GetLength(0);
            int heigth = inputImage.GetLength(1);
            int depth = inputImage.GetLength(2);

            int padWidth = width + 2 * border;
            int padHeigth = heigth + 2 * border;

            int[,,] padImage = new int[padWidth, padHeigth, depth];

            // Create Matrix with zero padded borders
            for (int y = 0; y < padHeigth; y++)
            {
                for (int x = 0; x < padWidth; x++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        if (y < border || y >= padHeigth - border || x < border || x >= padWidth - border)
                        {
                            padImage[x, y, z] = 0;
                        }
                        else
                        {
                            padImage[x, y, z] = inputImage[x, y, z];
                        }
                    }
                }
            }
            return padImage;
        }

        public override void InitLayerMat()
        {
            // Not necessary in an Convolution layer of a convNet
        }

        public void InitFilterMat()
        {
            // TODO
        }
    }
}
