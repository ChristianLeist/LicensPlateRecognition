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
        private double[] activationValueArray;
        private double[] zValueArray;
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
            this.activationValueArray = new double[width];
        }

        public override void FeedForward(Image img, double[] flat, double[][][] matrix)
        {
            for (int y = 0; y < this.Height - 1; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    // unactivated values
                    this.zValueArray[x] = flat[x];
                    // Relu activation of the neurons
                    this.FlatArray[x] = activation.ReLU(flat[x]);
                    // Fill activated value array
                    this.activationValueArray[x] = this.FlatArray[x];
                    // Compute a * w + b
                    this.FlatArray[x] += flat[y] * this.layerMat[x][y] + this.layerMat[x][this.Height - 1];
                }
            }
        }

        public override void BackwardPass(double[] delta, double[][][] gradientMatrix)
        {
            this.GradientLayerMat = new double[this.Width][];
            // compute input gradients for layer + 1
            for (int y = 0; y < this.Height - 1; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    if (y == 0)
                    {
                        this.GradientLayerMat[x] = new double[this.Height];
                        this.GradientLayerMat[x][this.Height - 1] = delta[x];
                    }
                    this.GradientLayerMat[x][y] = delta[x] * this.activationValueArray[y];
                }
            }

            // compute delta for layer - 1
            this.DeltaArray = new double[this.Height];
            for (int y = 0; y < this.Height - 1; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    // delta = w * delta * derivative z
                    // gradients add at branches
                    this.DeltaArray[y] += this.layerMat[x][y] * delta[x] * activation.DReLU(this.zValueArray[y]);
                }
            }
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
