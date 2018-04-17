using LicensPlateRecognition.Calc;
using System;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    class FullyConnectedLayer : Layer
    {
        private double[][] layerMat;
        private int heigth, width;
        private RandomGaussNumberGen randNumGen;

        public FullyConnectedLayer(int width, int heigth)
        {
            // heigth = number of neurons in this layer
            // heigth + 1 = bias values of the following layer
            this.heigth = heigth + 1;
            // width = number of neurons in next layer
            this.width = width;
            this.flatArray = new double[width];
            this.randNumGen = new RandomGaussNumberGen(0, 1);
        }

        public int Width
        {
            get => this.width;
        }

        public void FeedForward(double[] inArray)
        {
            for (int y = 0; y < this.heigth - 1; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    this.flatArray[x] += inArray[y] * this.layerMat[x][y] + this.layerMat[x][this.heigth - 1];
                }
            }
        }

        public void RandInitLayerMat()
        {
            this.layerMat = new double[this.width][];
            for (int y = 0; y < this.heigth; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    // init 
                    if (y == 0)
                    {
                        this.layerMat[x] = new double[this.heigth];
                    }

                    this.layerMat[x][y] = this.randNumGen.CreateRandomNum();
                }
            }
        }
    }
}
