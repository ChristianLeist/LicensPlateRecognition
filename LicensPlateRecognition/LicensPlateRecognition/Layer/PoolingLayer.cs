using System;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    class PoolingLayer : Layer
    {
        public double[,,] MaxPooling(double[,,] inMatrix)
        {
            int stride = 2;
            int outWidth = inMatrix.GetLength(0) / stride;
            int outHeigth = inMatrix.GetLength(1) / stride;
            int outDepth = inMatrix.GetLength(2);

            double[,,] outMatrix = new double[outWidth, outHeigth, outDepth];
            double[] valueArray = new double[4];

            for (int z = 0; z < outDepth; z++)
            {
                for (int y = 0; y < outHeigth; y++)
                {
                    for (int x = 0; x < outWidth; x++)
                    {
                        int k = 0;
                        for (int i = 0; i < stride; i++)
                        {
                            for (int j = 0; j < stride; j++)
                            {
                                valueArray[k++] = inMatrix[x * stride + j, y * stride + i, z];
                            }
                        }
                        Array.Sort(valueArray);
                        outMatrix[x, y, z] = valueArray[k - 1];
                    }
                }
            }

            return outMatrix;
        }

        public override void RandInitLayerMat()
        {
            // Not necessary in a pooling layer of a convNet
        }
    }
}
