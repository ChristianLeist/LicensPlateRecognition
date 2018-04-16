using System;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    class PoolingLayer : Layer
    {
        public void MaxPooling(double[][][] inMatrix)
        {
            int stride = 2;
            int outWidth = inMatrix.Length / stride;
            int outHeigth = inMatrix[0].Length / stride;
            int outDepth = inMatrix[0][0].Length;

            double[][][] outMatrix = new double[outWidth][][];
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
                                outMatrix[x] = new double[outHeigth][];
                            }
                            outMatrix[x][y] = new double[outDepth];
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
                        outMatrix[x][y][z] = valueArray[k - 1];
                    }
                }
            }

            this.imgMatrix =  outMatrix;
        }
    }
}
