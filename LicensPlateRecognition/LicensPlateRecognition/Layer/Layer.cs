using System;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    abstract class Layer
    {
        protected double[][][] imgMatrix;
        protected double[] flatArray;


        public void Flattening()
        {
            int width = this.imgMatrix.Length;
            int heigth = this.imgMatrix[0].Length;
            int depth = this.imgMatrix[0][0].Length;

            this.flatArray = new double[width * heigth * depth];

            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < heigth; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        this.flatArray[x + width * (y + heigth * z)] = this.imgMatrix[x][y][z];
                    }
                }
            }
        }

        public double[] FlatArray
        {
            get => this.flatArray;
        }

        public double[][][] ImgMatrix
        {
            get => this.imgMatrix;
        }
    }
}
