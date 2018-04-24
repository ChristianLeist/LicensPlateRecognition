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
                    // Relu activation of the neurons
                    this.FlatArray[x] = activation.ReLU(flat[x]);
                    // Fill activated value array
                    this.activationValueArray[x] = this.FlatArray[x];
                    // Compute F = x * w + b
                    this.FlatArray[x] += flat[y] * this.layerMat[x][y] + this.layerMat[x][this.Height - 1];
                }
            }
        }

        public override void BackwardPass(double[] gradientArray, double[][] gradientLayerMat, double[][][] gradientMatrix)
        {
            this.GradientLayerMat = new double[this.Width][];
            // compute gradients for output layer
            if (gradientArray != null)
            {
                for (int y = 0; y < this.Height - 1; y++)
                {
                    for (int x = 0; x < this.Width; x++)
                    {
                        if (y == 0)
                        {
                            this.GradientLayerMat[x] = new double[this.Height];
                        }
                        this.GradientLayerMat[x][y] = gradientArray[x] * this.activationValueArray[y];
                        // TODO: bias noch dazu geben
                    }
                }
            }

            for (int y = 0; y < this.Height - 1; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    /* for l in xrange(2, self.num_layers):
                       z = zs[-l]
                       sp = sigmoid_prime(z)
                       delta = np.dot(self.weights[-l+1].transpose(), delta) * sp
                       nabla_b[-l] = delta
                       nabla_w[-l] = np.dot(delta, activations[-l-1].transpose()) */
                    if (gradientArray == null)
                    {
                        if (y == 0)
                        {
                            this.GradientLayerMat[x] = new double[this.Height];
                        }
                        // TODO: gradienten mit gradientLayerMat berechnen
                    }
                    else
                    {
                        // TODO: gradienten mit this.GradientLayerMat berechnen
                    }
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
