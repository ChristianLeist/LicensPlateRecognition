using System;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    abstract class Layer
    {
        protected double[][][] imgMatrix;
        protected double[] flatArray;
        protected Layer prevLayer;
        protected Layer nextLayer;
        protected int width, height, depth;

        protected Layer(Layer prevLayer)
        {
            this.prevLayer = prevLayer;
        }

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

        public Layer NextLayer
        {
            get => this.nextLayer;
            set => this.nextLayer = value;
        }

        public Layer PrevLayer
        {
            get => this.prevLayer;
            set => this.prevLayer = value;
        }

        public int Width
        {
            get => this.width;
            set => this.width = value;
        }

        public int Height
        {
            get => this.height;
            set => this.height = value;
        }

        public int Depth
        {
            get => this.depth;
            set => this.depth = value;
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
