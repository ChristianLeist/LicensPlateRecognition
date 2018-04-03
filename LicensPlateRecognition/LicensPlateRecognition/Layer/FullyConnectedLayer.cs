using System;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    class FullyConnectedLayer
    {
        private double[,] layerMat { get; set; }

        public FullyConnectedLayer(int row, int column)
        {
            // column = number of neurons in next layer
            // row - 1 = number of neurons in this layer
            // last row is for bias values
            layerMat = new double[row, column];
            
            // TODO: Init layerMat with RandGaussNum
        }
    }
}
