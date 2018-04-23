﻿using LicensPlateRecognition.Calc;
using LicensPlateRecognition.Kernel;
using LicensPlateRecognition.Network;
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
        public List<Filter> Filters { get; }
        private int stride;

        public ConvolutionLayer(Filter filter, int numberOfFilters, int stride, NeuralNetwork network) : base(network)
        {
            Filters = new List<Filter>();
            for (int i = 0; i < numberOfFilters; i++)
            {
                this.Filters.Add(new Filter(filter.Width, filter.Height, filter.Depth));
            }
            this.stride = stride;
        }

        //public Bitmap Convolution(Bitmap inputBitmap)
        //{
        //    // Zero padding border formular with fxf filter: (f-1)/2
        //    int f = this.filterMat.GetLength(0);
        //    int border = (f - 1) / 2;
        //    Bitmap padInputBitmap = ZeroPadding(inputBitmap, border);

        //    int width = padInputBitmap.Width;
        //    int height = padInputBitmap.Height;

        //    int outWidth = inputBitmap.Width / this.stride;
        //    int outHeigth = inputBitmap.Height / this.stride;

        //    Bitmap outMatrix = new Bitmap(outWidth, outHeigth);
        //    BitmapData outImageData = outMatrix.LockBits(new Rectangle(0, 0, outWidth, outHeigth), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        //    BitmapData inputImageData = padInputBitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

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
        //    outMatrix.UnlockBits(outImageData);

        //    return outMatrix;
        //}

        public override void FeedForward(double[][][] inMatrix)
        {
            Convolution(inMatrix);
        }

        public void Convolution(double[][][] inMatrix)
        {
            int filterDepth = this.Filters.Find(filter => true).FilterMat[0][0].Length;
            // Zero padding border formular with fxf filter: (f - 1) / 2
            int f = this.Filters.Find(filter => true).FilterMat.Length;
            int border = (f - 1) / 2;
            ZeroPadding(inMatrix, border);

            int width = this.ImgMatrix.Length;
            int heigth = this.ImgMatrix[0].Length;
            int depth = this.ImgMatrix[0][0].Length;

            int outWidth = inMatrix.Length / this.stride;
            int outHeigth = inMatrix[0].Length / this.stride;
            // Number of Filters
            int outDepth = this.Filters.Count;

            double[][][] outMatrix = new double[outWidth][][];

            // Loop over Filters
            for (int z = 0; z < outDepth; z++)
            {
                // Create feature map
                for (int y = 0; y < outHeigth; y++)
                {
                    for (int x = 0; x < outWidth; x++)
                    {
                        // init outMatrix
                        if (z == 0)
                        {
                            if (y == 0)
                            {
                                outMatrix[x] = new double[outHeigth][];
                            }
                            outMatrix[x][y] = new double[outDepth];
                        }

                        // Full depth convolution
                        double convVal = 0.00;
                        for (int d = 0; d < filterDepth; d++)
                        {
                            for (int h = 0; h < f; h++)
                            {
                                for (int w = 0; w < f; w++)
                                {
                                    convVal += this.Filters[z].FilterMat[w][h][d] * this.ImgMatrix[this.stride * x + w][this.stride * y + h][d];
                                }
                            }
                        }

                        // Relu: f(x) = max(x,0)
                        if (convVal + this.Filters[z].Bias < 0)
                        {
                            outMatrix[x][y][z] = 0;
                        }
                        else
                        {
                            outMatrix[x][y][z] = convVal + this.Filters[z].Bias;
                        }
                    }
                }
            }
            this.ImgMatrix = outMatrix;
        }

        public void ZeroPadding(double[][][] inputImage, int border)
        {
            int width = inputImage.Length;
            int heigth = inputImage[0].Length;
            int depth = inputImage[0][0].Length;

            int padWidth = width + 2 * border;
            int padHeigth = heigth + 2 * border;

            this.ImgMatrix = new double[padWidth][][];

            // Create Matrix with zero padded borders
            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < padHeigth; y++)
                {
                    for (int x = 0; x < padWidth; x++)
                    {
                        // init
                        if (z == 0)
                        {
                            if (y == 0)
                            {
                                this.ImgMatrix[x] = new double[padHeigth][];
                            }
                            this.ImgMatrix[x][y] = new double[depth];
                        }

                        // pad
                        if (x < border || x >= padHeigth - border * 2 || y < border || y >= padWidth - border * 2)
                        {
                            this.ImgMatrix[x][y][z] = 0;
                        }
                        else
                        {
                            this.ImgMatrix[x][y][z] = inputImage[x][y][z];
                        }
                    }
                }
            }
        }

        public override void RandInitFilter()
        {
            RandomGaussNumberGen randNumGen = new RandomGaussNumberGen(0, 1);
            foreach (Filter filter in this.Filters)
            {
                filter.Bias = randNumGen.CreateRandomNum();
                for (int z = 0; z < filter.Depth; z++)
                {
                    for (int y = 0; y < filter.Height; y++)
                    {
                        for (int x = 0; x < filter.Width; x++)
                        {
                            // init
                            if (z == 0)
                            {
                                if (y == 0)
                                {
                                    filter.FilterMat[x] = new double[filter.Height][];
                                }
                                filter.FilterMat[x][y] = new double[filter.Depth];
                            }

                            // fill
                            filter.FilterMat[x][y][z] = randNumGen.CreateRandomNum();
                            //Console.Write(filter.FilterMat[x][y][z] + " ");
                        }
                        //Console.WriteLine();
                    }
                    //Console.WriteLine();
                }
            }
        }

        public override void RandInitLayerMat()
        {
            throw new NotImplementedException();
        }

        public override void InitLayer(int height, int width)
        {
            throw new NotImplementedException();
        }

        public override void PrintArray()
        {
            throw new NotImplementedException();
        }

        public override void FeedForward(Image input)
        {
            throw new NotImplementedException();
        }

        public override void FeedForward(double[] input)
        {
            throw new NotImplementedException();
        }


    }
}
