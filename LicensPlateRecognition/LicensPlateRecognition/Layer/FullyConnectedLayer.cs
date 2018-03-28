using System;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    class FullyConnectedLayer
    {
        private double[,] weightMat;
        private double[,] biasMat;

        public FullyConnectedLayer(int row, int column)
        {
            // column = number of neurons in next layer
            // row = number of neurons in this layer
            weightMat = new double[row, column];
            biasMat = new double[row, column];
        }
    }
}
