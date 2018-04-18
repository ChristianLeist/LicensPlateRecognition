using System;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition.Calc
{
    class Function
    {
        public double[] Softmax(double[] classArray)
        {
            int length = classArray.Length;
            double[] ar = new double[length];
            double denom = 0;

            classArray.CopyTo(ar, 0);
            Array.Sort(ar);

            for (int i = 0; i < length; i++)
            {
                classArray[i] -= ar[length - 1];
            }

            for (int i = 0; i < length; i++)
            {
                denom += Math.Pow(Math.E, classArray[i]);
            }

            for (int i = 0; i < length; i++)
            {
                ar[i] = Math.Pow(Math.E, classArray[i]) / denom;
            }

            return ar;
        }

        public double Sigmoid(double inVal)
        {
            return 1 / 1 + Math.Pow(Math.E, -inVal);
        }

        public double LossFunction(double[] classArray, double[] targetValue)
        {
            double outVal = 0;
            for (int i = 0; i < classArray.Length; i++)
            {
                outVal += 0.5 * Math.Pow((targetValue[i] - classArray[i]), 2);
            }
            return outVal;
        }
    }
}
