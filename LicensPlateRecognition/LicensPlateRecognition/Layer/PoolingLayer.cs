using LicensPlateRecognition.Network;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    class PoolingLayer : Layer
    {
        public PoolingLayer(NeuralNetwork network) : base(network)
        {
            ;
        }

        public override void FeedForward(Image img, double[] flat, double[][][] matrix)
        {
            MaxPooling(matrix);
        }

        public override void BackwardPass(double[] gradientArray, double[][][] gradientMatrix)
        {
            // TODO
        }

        public void MaxPooling(double[][][] inMatrix)
        {
            int stride = 2;
            int outWidth = inMatrix.Length / stride;
            int outHeigth = inMatrix[0].Length / stride;
            int outDepth = inMatrix[0][0].Length;

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
                        for (int i = 0; i < stride; i++)
                        {
                            for (int j = 0; j < stride; j++)
                            {
                                valueArray[k++] = inMatrix[x * stride + j][y * stride + i][z];
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
