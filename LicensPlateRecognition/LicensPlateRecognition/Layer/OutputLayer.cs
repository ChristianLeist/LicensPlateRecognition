using LicensPlateRecognition.Calc;
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

        public OutputLayer(NeuralNetwork network) : base(network)
        {
            activation = new Function();
        }

        public override void FeedForward(double[] flatArray)
        {
            ComputeOutput(flatArray);
        }

        public void ComputeOutput(double[] flatArray)
        {
            this.FlatArray = activation.Softmax(flatArray);
        }

        public override void PrintArray()
        {
            for (int i = 0; i < this.FlatArray.Length; i++)
            {
                Console.WriteLine(this.FlatArray[i]);
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

        public override void FeedForward(Image input)
        {
            throw new NotImplementedException();
        }

        public override void FeedForward(double[][][] input)
        {
            throw new NotImplementedException();
        }
    }
}
