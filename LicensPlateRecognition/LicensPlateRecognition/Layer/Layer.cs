using System;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    abstract class Layer
    {
        protected double[][][] imgMatrix;

        public double[][][] ImgMatrix
        {
            get => this.imgMatrix;
        }
    }
}
