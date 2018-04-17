using System;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    class OutputLayer : Layer
    {
        public OutputLayer(double[] flatArray)
        {
            this.flatArray = flatArray;
        }

        public void Softmax()
        {
            int length = this.flatArray.Length;
            double[] ar = new double[length];
            double denom = 0;

            this.flatArray.CopyTo(ar, 0);
            Array.Sort(ar);

            for (int i = 0; i < length; i++)
            {
                this.flatArray[i] -= ar[length - 1];
            }

            for (int i = 0; i < length; i++)
            {
                denom += Math.Pow(Math.E, this.flatArray[i]);
            }

            for (int i = 0; i < length; i++)
            {
                ar[i] = Math.Pow(Math.E, this.flatArray[i]) / denom;
            }

            this.flatArray = ar;
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
