using System;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition.Kernel
{
    class Filter
    {
        private double[,,] filterMat;

        private double bias;

        public double[,,] FilterMat
        {
            get => this.filterMat;
            set => this.filterMat = value;
        }

        public double Bias
        {
            get => this.bias;
            set => this.bias = value;
        }
    }
}
