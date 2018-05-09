using System;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition.Kernel
{
    public class Filter
    {
        public double[][][] FilterMat { get; set; }
        public double[][][] FilterGradientMat { get; set; }
        public double Bias { get; set; }
        public double BiasGradient { get; set; }
        public int Width { get; }
        public int Depth { get; }
        public int Height { get; }

        private Filter() { } // standard constructor

        public Filter(int width, int heigth, int depth)
        {
            this.Width = width;
            this.Height = heigth;
            this.Depth = depth;
            this.FilterMat = new double[width][][];
            this.FilterGradientMat = new double[width][][];
        }
    }
}
