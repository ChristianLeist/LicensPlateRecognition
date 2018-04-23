using LicensPlateRecognition.Calc;
using LicensPlateRecognition.Network;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    class FullyConnectedLayer : Layer
    {
        private double[][] layerMat;
        private RandomGaussNumberGen randNumGen;
        private Function activation;

        public FullyConnectedLayer(NeuralNetwork network) : base(network)
        {
            this.randNumGen = new RandomGaussNumberGen(0, 1);
            activation = new Function();
        }

        public override void InitLayer(int height, int width)
        {
            // height = number of neurons in this layer
            // height + 1 = bias values of the following layer
            this.Height = height + 1;
            // width = number of neurons in next layer
            this.Width = width;
            this.FlatArray = new double[width];
        }

        public override void FeedForward(Image img, double[] flat, double[][][] matrix)
        {
            for (int y = 0; y < this.Height - 1; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    // Relu activation of the neurons
                    this.FlatArray[x] = activation.ReLU(this.FlatArray[x]);
                    this.FlatArray[x] += flat[y] * this.layerMat[x][y] + this.layerMat[x][this.Height - 1];
                }
            }
        }

        public override void BackwardPass(double[] gradientArray, double[][][] gradientMatrix)
        {
            // TODO
        }

        public override void RandInitLayerMat()
        {
            this.layerMat = new double[this.Width][];
            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    // init 
                    if (y == 0)
                    {
                        this.layerMat[x] = new double[this.Height];
                    }

                    this.layerMat[x][y] = this.randNumGen.CreateRandomNum();
                }
            }
        }

        public override void RandInitFilter()
        {
            throw new NotImplementedException();
        }

        public override void PrintArray()
        {
            throw new NotImplementedException();
        }
    }
}
