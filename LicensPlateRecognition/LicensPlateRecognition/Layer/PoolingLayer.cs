using LicensPlateRecognition.Network;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    class PoolingLayer : Layer
    {
        private double[][][] inMatrix;
        private int stride;

        public PoolingLayer(NeuralNetwork network) : base(network)
        {
            this.stride = 2;
        }

        public override void FeedForward(Image img, double[] flat, double[][][] matrix)
        {
            this.inMatrix = matrix;
            MaxPooling();
        }

        public override void BackwardPass(double[] delta, double[][][] gradientMatrix)
        {
            if (delta != null)
            {
                this.DeltaArray = delta;
                this.DeFlattening();
                gradientMatrix = this.GradientMatrix;
            }

            int width = this.inMatrix.Length / this.stride;
            int heigth = this.inMatrix[0].Length / this.stride;
            int depth = this.inMatrix[0][0].Length;

            double[][][] outMatrix = new double[width * this.stride][][];
            // undo max pooling, but with gradient value at max value position and rest null
            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < heigth; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        // init outMatrix
                        if (z == 0)
                        {
                            if (y == 0)
                            {
                                outMatrix[x] = new double[heigth * stride][];
                            }
                            outMatrix[x][y] = new double[depth];
                        }

                        for (int i = 0; i < stride; i++)
                        {
                            for (int j = 0; j < stride; j++)
                            {
                                // if more than one max value, all are used to backpropagate gradients else null value in matrix
                                if (this.inMatrix[x * this.stride + j][y * this.stride + i][z] == this.ImgMatrix[x][y][z])
                                {
                                    outMatrix[x * this.stride + j][y * this.stride + i][z] = gradientMatrix[x][y][z];
                                }
                            }
                        }
                    }
                }
            }

            gradientMatrix = outMatrix;
        }

        public void MaxPooling()
        {
            int outWidth = this.inMatrix.Length / this.stride;
            int outHeigth = this.inMatrix[0].Length / this.stride;
            int outDepth = this.inMatrix[0][0].Length;

            this.ImgMatrix = new double[outWidth][][];
            double[] valueArray = new double[4];

            for (int z = 0; z < outDepth; z++)
            {
                for (int y = 0; y < outHeigth; y++)
                {
                    for (int x = 0; x < outWidth; x++)
                    {
                        // init outMatrix
                        if (z == 0)
                        {
                            if (y == 0)
                            {
                                this.ImgMatrix[x] = new double[outHeigth][];
                            }
                            this.ImgMatrix[x][y] = new double[outDepth];
                        }

                        int k = 0;
                        for (int i = 0; i < this.stride; i++)
                        {
                            for (int j = 0; j < this.stride; j++)
                            {
                                valueArray[k++] = this.inMatrix[x * this.stride + j][y * this.stride + i][z];
                            }
                        }
                        Array.Sort(valueArray);
                        this.ImgMatrix[x][y][z] = valueArray[k - 1];
                    }
                }
            }
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

        public override void InitLayer(int height, int width)
        {
            throw new NotImplementedException();
        }
    }
}
