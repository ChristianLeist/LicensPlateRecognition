using LicensPlateRecognition.Calc;
using System;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    class FullyConnectedLayer : Layer
    {
        private int row, column;
        private RandomGaussNumberGen randNumGen;

        public FullyConnectedLayer(int row, int column)
        {
            // row = number of neurons in this layer
            // row + 1 = bias values of the following layer
            this.row = row + 1;
            // column = number of neurons in next layer
            this.column = column;
            this.layerMat = new double[this.row, this.column];
            this.randNumGen = new RandomGaussNumberGen(0, 1);
        }

        public override void RandInitLayerMat()
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    this.layerMat[i, j] = this.randNumGen.CreateRandomNum();
                }
            }
        }
    }
}
