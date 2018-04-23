﻿using LicensPlateRecognition.Calc;
using LicensPlateRecognition.Network;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    class OutputLayer : Layer
    {
        private Function activation;
        private double[] outputArray;

        public OutputLayer(NeuralNetwork network) : base(network)
        {
            activation = new Function();
        }

        public override void FeedForward(Image img, double[] flat, double[][][] matrix)
        {
            ComputeOutput(flat);
        }

        public override void BackwardPass(double[] setValueArray, double[][][] gradientMatrix)
        {
            this.GradientArray = new double[this.outputArray.Length];
            for (int i = 0; i < this.outputArray.Length; i++)
            {
                this.GradientArray[i] = activation.DLossFunction(this.outputArray[i], setValueArray[i]) * 
                                                                activation.DSoftmax(this.FlatArray[i]);
            }
        }

        public void ComputeOutput(double[] flatArray)
        {
            this.FlatArray = flatArray;
            this.outputArray = activation.Softmax(flatArray);
        }

        public override void PrintArray()
        {
            for (int i = 0; i < this.outputArray.Length; i++)
            {
                Console.WriteLine(this.outputArray[i]);
            }
        }

        public override void RandInitFilter()
        {
            throw new NotImplementedException();
        }

        public override void RandInitLayerMat()
        {
            throw new NotImplementedException();
        }

        public override void InitLayer(int height, int width)
        {
            throw new NotImplementedException();
        }
    }
}
