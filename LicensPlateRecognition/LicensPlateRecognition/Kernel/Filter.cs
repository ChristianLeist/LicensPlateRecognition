using System;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition.Kernel
{
    class Filter
    {
        private double[,,] filterMat;
        private int width, depth, heigth;
        private double bias;

        public Filter(int width, int heigth, int depth)
        {
            this.width = width;
            this.heigth = heigth;
            this.depth = depth;
            this.filterMat = new double[width, heigth, depth];
        }

        public int Width
        {
            get => this.width;
        }

        public int Height
        {
            get => this.heigth;
        }

        public int Depth
        {
            get => this.depth;
        }

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
