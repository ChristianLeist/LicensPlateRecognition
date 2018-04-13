using LicensPlateRecognition.Calc;
using LicensPlateRecognition.Kernel;
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
        private List<Filter> filters;
        // Test filter
        //private int[,,] filterMat;
        private int stride;
        private RandomGaussNumberGen randNumGen;

        public ConvolutionLayer(Filter filter, int numberOfFilters, int stride)
        {
            filters = new List<Filter>();
            for (int i = 0; i < numberOfFilters; i++)
            {
                this.filters.Add(new Filter(filter.Width, filter.Height, filter.Depth));
            }
            this.stride = stride;
            this.randNumGen = new RandomGaussNumberGen(0, 1);
            // Test filter
            //this.filterMat = new int[3, 3, 3] { { { 0, 0, 0 }, { -1, -1, -1 }, { 0, 0, 0 } },
            //                                    { { -1, -1, -1 }, { 5, 5, 5 }, { -1, -1, -1 } },
            //                                    { { 0, 0, 0 }, { -1, -1, -1 }, { 0, 0, 0 } } };
        }

        //public Bitmap Convolution(Bitmap inputBitmap)
        //{
        //    // Zero padding border formular with fxf filter: (f-1)/2
        //    int f = this.filterMat.GetLength(0);
        //    int border = (f - 1) / 2;
        //    Bitmap padInputBitmap = ZeroPadding(inputBitmap, border);

        //    int width = padInputBitmap.Width;
        //    int heigth = padInputBitmap.Height;

        //    int outWidth = inputBitmap.Width / this.stride;
        //    int outHeigth = inputBitmap.Height / this.stride;

        //    Bitmap outImage = new Bitmap(outWidth, outHeigth);
        //    BitmapData outImageData = outImage.LockBits(new Rectangle(0, 0, outWidth, outHeigth), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        //    BitmapData inputImageData = padInputBitmap.LockBits(new Rectangle(0, 0, width, heigth), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

        //    int inputImageByte = inputImageData.Stride * inputImageData.Height;
        //    int outImageByte = outImageData.Stride * outImageData.Height;
        //    byte[] inputImageArray = new byte[inputImageByte];
        //    byte[] outImageArray = new byte[outImageByte];

        //    // alpha value init
        //    for (int i = 3; i < outImageByte; i += 4)
        //    {
        //        outImageArray[i] = 255;
        //    }
        //    // Copy image values into array
        //    Marshal.Copy(inputImageData.Scan0, inputImageArray, 0, inputImageByte);
        //    padInputBitmap.UnlockBits(inputImageData);

        //    // Do convolution
        //    for (int y = 0; y < outHeigth; y++)
        //    {
        //        for (int x = 0; x < outWidth; x++)
        //        {
        //            int inputImgPixel = y * inputImageData.Stride * this.stride + x * 4 * this.stride;
        //            int outImgPixel = y * outImageData.Stride + x * 4;
        //            for (int d = 0; d < this.filterMat.GetLength(2); d++)
        //            {
        //                int convVal = 0;
        //                for (int h = 0; h < f; h++)
        //                {
        //                    for (int w = 0; w < f; w++)
        //                    {
        //                        convVal += filterMat[h, w, d] * inputImageArray[inputImageData.Stride * h + 4 * w + inputImgPixel + d];
        //                    }
        //                }
        //                if (convVal > 255)
        //                {
        //                    convVal = 255;
        //                }
        //                else if (convVal < 0)
        //                {
        //                    convVal = 0;
        //                }

        //                outImageArray[outImgPixel + d] = (byte)convVal;
        //            }

        //        }
        //    }
        //    // Copy array values into output image
        //    Marshal.Copy(outImageArray, 0, outImageData.Scan0, outImageByte);
        //    outImage.UnlockBits(outImageData);

        //    return outImage;
        //}

        public double[,,] Convolution(double[,,] inputImage)
        {
            int filterDepth = this.filters.Find(filter => true).FilterMat.GetLength(2);
            // Zero padding border formular with fxf filter: (f - 1) / 2
            int f = this.filters.Find(filter => true).FilterMat.GetLength(0);
            int border = (f - 1) / 2;
            double[,,] padInputImage = ZeroPadding(inputImage, border);

            int width = padInputImage.GetLength(0);
            int heigth = padInputImage.GetLength(1);
            int depth = padInputImage.GetLength(2);

            int outWidth = inputImage.GetLength(0) / this.stride;
            int outHeigth = inputImage.GetLength(1) / this.stride;
            // Number of filters
            int outDepth = this.filters.Count;

            double[,,] outImage = new double[outWidth, outHeigth, outDepth];

            // Filter loop
            for (int z = 0; z < outDepth; z++)
            {
                // Create feature map
                for (int y = 0; y < outHeigth; y++)
                {
                    for (int x = 0; x < outWidth; x++)
                    {
                        // Full depth convolution
                        double convVal = 0.00;
                        for (int d = 0; d < filterDepth; d++)
                        {
                            for (int h = 0; h < f; h++)
                            {
                                for (int w = 0; w < f; w++)
                                {
                                    convVal += this.filters[z].FilterMat[w, h, d] * padInputImage[this.stride * x + w, this.stride * y + h, d];
                                }
                            }
                        }
                        // Relu: f(x) = max(x,0)
                        if (convVal + this.filters[z].Bias < 0)
                        {
                            outImage[x, y, z] = 0;
                        }
                        else
                        {
                            outImage[x, y, z] = convVal + this.filters[z].Bias;
                        }
                    }
                }
            }

            return outImage;
        }

        public double[,,] ZeroPadding(double[,,] inputImage, int border)
        {
            int width = inputImage.GetLength(0);
            int heigth = inputImage.GetLength(1);
            int depth = inputImage.GetLength(2);

            int padWidth = width + 2 * border;
            int padHeigth = heigth + 2 * border;

            double[,,] padImage = new double[padWidth, padHeigth, depth];

            // Create Matrix with zero padded borders
            for (int y = 0; y < padHeigth; y++)
            {
                for (int x = 0; x < padWidth; x++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        if (y < border || y >= padHeigth - border * 2 || x < border || x >= padWidth - border * 2)
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

        public override void RandInitLayerMat()
        {
            // Not necessary in an Convolution layer of a convNet
        }

        public void RandInitFilter()
        {
            foreach (Filter filter in this.filters)
            {
                filter.Bias = this.randNumGen.CreateRandomNum();
                for (int z = 0; z < filter.FilterMat.GetLength(2); z++)
                {
                    for (int y = 0; y < filter.FilterMat.GetLength(1); y++)
                    {
                        for (int x = 0; x < filter.FilterMat.GetLength(0); x++)
                        {
                            filter.FilterMat[x, y, z] = randNumGen.CreateRandomNum();
                            Console.Write(filter.FilterMat[x, y, z] + " ");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
