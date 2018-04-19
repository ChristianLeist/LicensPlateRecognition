using LicensPlateRecognition.Calc;
using System;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    class FullyConnectedLayer : Layer
    {
        private double[][] layerMat;
        private RandomGaussNumberGen randNumGen;
        private Function activation;

        public FullyConnectedLayer(Layer prev) : base(prev)
        {
            this.randNumGen = new RandomGaussNumberGen(0, 1);
            activation = new Function();
        }

        public void Init(int height, int width)
        {
            // height = number of neurons in this layer
            // height + 1 = bias values of the following layer
            this.height = height + 1;
            // width = number of neurons in next layer
            this.width = width;
            this.flatArray = new double[width];
        }

        public void FeedForward(double[] inArray)
        {
            for (int y = 0; y < this.height - 1; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    // sigmoid activation of the neurons
                    this.flatArray[x] += activation.Sigmoid(inArray[y] * this.layerMat[x][y] + this.layerMat[x][this.height - 1]);
                }
            }
        }

        public void RandInitLayerMat()
        {
            this.layerMat = new double[this.width][];
            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    // init 
                    if (y == 0)
                    {
                        this.layerMat[x] = new double[this.height];
                    }

                    this.layerMat[x][y] = this.randNumGen.CreateRandomNum();
                }
            }
        }
    }
}
