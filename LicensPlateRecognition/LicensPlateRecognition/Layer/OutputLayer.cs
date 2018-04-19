using LicensPlateRecognition.Calc;
using System;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    class OutputLayer : Layer
    {
        private Function activation;

        public OutputLayer(Layer prev) : base(prev)
        {
            activation = new Function();
        }

        public void ComputeOutput()
        {
            this.flatArray = prevLayer.FlatArray;
            this.flatArray = activation.Softmax(this.flatArray);
        }

        public void PrintArray()
        {
            for (int i = 0; i < this.flatArray.Length; i++)
            {
                Console.WriteLine(this.flatArray[i]);
            }
        }
    }
}
