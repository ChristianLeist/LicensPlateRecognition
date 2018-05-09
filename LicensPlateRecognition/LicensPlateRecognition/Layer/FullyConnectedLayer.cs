using LicensPlateRecognition.Calc;
using LicensPlateRecognition.Network;
using System;
using System.Drawing;

namespace LicensPlateRecognition.Layer
{
    public class FullyConnectedLayer : Layer
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
            this.activationValueArray = new double[height];
        }

        public override void FeedForward(Image img, double[] flat, double[][][] matrix)
        {
            // unactivated values
            this.zValueArray = flat;

            for (int y = 0; y < this.Height - 1; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    // Fill activated value array
                    this.activationValueArray[y] = activation.ReLU(flat[y]);
                    // Compute a * w + b
                    this.FlatArray[x] += this.activationValueArray[y] * this.layerMat[x][y] + this.layerMat[x][this.Height - 1];
                }
            }
        }

        public override void BackwardPass(double[] deltaArray, double[][][] deltaMatrix)
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
                        this.GradientLayerMat[x][this.Height - 1] = deltaArray[x];
                    }
                    this.GradientLayerMat[x][y] = deltaArray[x] * this.activationValueArray[y];
                }
            }

            // compute delta for layer - 1
            this.DeltaArray = new double[this.Height - 1];
            for (int y = 0; y < this.Height - 1; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    // delta = w * delta * derivative z
                    // gradients add at branches
                    this.DeltaArray[y] += this.layerMat[x][y] * deltaArray[x] * activation.DReLU(this.zValueArray[y]);
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

        public override void StoreWeights()
        {
            // TODO
        }

        public override void LoadWeights()
        {
            // TODO
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
