using LicensPlateRecognition.Network;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    abstract class Layer
    {
        protected NeuralNetwork neuralNetwork;
        public double[][][] ImgMatrix { get; protected set; }
        public double[][][] GradientMatrix { get; protected set; }
        public double[] FlatArray { get; protected set; }
        public double[] GradientArray { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int Depth { get; protected set; }

        protected Layer(NeuralNetwork neuralNetwork)
        {
            this.neuralNetwork = neuralNetwork;
            neuralNetwork.Layers.Add(this);
        }

        // used in conv layer
        public abstract void RandInitFilter();

        // used in fc layer
        public abstract void RandInitLayerMat();

        // used in fc layer
        public abstract void InitLayer(int height, int width);

        // used in output layer
        public abstract void PrintArray();

        public abstract void FeedForward(Image img, double[] flat, double[][][] matrix);

        public abstract void BackwardPass(double[] gradientArray, double[][][] gradientMatrix);

        // used before fc layer
        public void Flattening()
        {
            int width = this.ImgMatrix.Length;
            int heigth = this.ImgMatrix[0].Length;
            int depth = this.ImgMatrix[0][0].Length;

            this.FlatArray = new double[width * heigth * depth];

            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < heigth; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        this.FlatArray[x + width * (y + heigth * z)] = this.ImgMatrix[x][y][z];
                    }
                }
            }
        }
    }
}
