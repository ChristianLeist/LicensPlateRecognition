using LicensPlateRecognition.Network;
using System;
using System.Drawing;

namespace LicensPlateRecognition.Layer
{
    public class PoolingLayer : Layer
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

        public override void BackwardPass(double[] deltaArray, double[][][] deltaMatrix)
        {
            if (deltaArray != null)
            {
                this.DeltaArray = deltaArray;
                this.DeFlattening();
                deltaMatrix = this.DeltaMatrix;
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
                        for (int i = 0; i < stride; i++)
                        {
                            for (int j = 0; j < stride; j++)
                            {
                                // init outMatrix
                                if (z == 0)
                                {
                                    if (y + i == 0)
                                    {
                                        outMatrix[x * this.stride + j] = new double[heigth * this.stride][];
                                    }
                                    outMatrix[x * this.stride + j][y * this.stride + i] = new double[depth];
                                }

                                // if more than one max value, all are used to backpropagate gradients else 0 value in matrix
                                if (this.inMatrix[x * this.stride + j][y * this.stride + i][z] == this.ImgMatrix[x][y][z])
                                {
                                    outMatrix[x * this.stride + j][y * this.stride + i][z] = deltaMatrix[x][y][z];
                                }
                                else
                                {
                                    outMatrix[x * this.stride + j][y * this.stride + i][z] = 0;
                                }
                            }
                        }
                    }
                }
            }

            this.DeltaMatrix = outMatrix;
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

        public override double[] GetOutputArray()
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

        public override void StoreWeights()
        {
            throw new NotImplementedException();
        }

        public override void LoadWeights()
        {
            throw new NotImplementedException();
        }

        public override void UpdateWeights(double learningRate, int miniBatchSize)
        {
            throw new NotImplementedException();
        }
    }
}
