using System;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition.Calc
{
    public class Function
    {
        public double[] Softmax(double[] classArray)
        {
            int length = classArray.Length;
            double[] ar1 = new double[length];
            double[] ar2 = new double[length];
            double denom = 0;

            classArray.CopyTo(ar1, 0);
            classArray.CopyTo(ar2, 0);
            Array.Sort(ar2);

            for (int i = 0; i < length; i++)
            {
                ar1[i] -= ar2[length - 1];
            }

            for (int i = 0; i < length; i++)
            {
                denom += Math.Pow(Math.E, ar1[i]);
            }

            for (int i = 0; i < length; i++)
            {
                ar1[i] = Math.Pow(Math.E, ar1[i]) / denom;
            }

            return ar1;
        }

        // derivative
        public double[] DSoftmax(double[] inArray)
        {
            inArray = Softmax(inArray);
            double[] outArray = new double[inArray.Length];
            for (int i = 0; i < inArray.Length; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    if (i == j)
                    {
                        outArray[i] = inArray[i] * (1 - inArray[i]);
                    }
                    else
                    {
                        outArray[i] = inArray[i] * -inArray[i];
                    }
                }
            }

            return outArray;
        }

        public double ReLU(double inVal)
        {
            return inVal < 0 ? 0 : inVal;
        }

        // derivative
        public double DReLU(double inVal)
        {
            return inVal < 0 ? 0 : 1;
        }

        public double Sigmoid(double inVal)
        {
            return 1 / (1 + Math.Pow(Math.E, -inVal));
        }

        // derivative
        public double Dsigmoid(double inVal)
        {
            double d = Sigmoid(inVal);
            return (1 - d) * d;
        }

        public double LossFunction(double[] classArray, double[] targetValue, int miniBatchSize)
        {
            double outVal = 0;
            for (int i = 0; i < classArray.Length; i++)
            {
                outVal += Math.Pow((targetValue[i] - classArray[i]), 2);
            }
            return (1 / (2 * miniBatchSize)) * outVal;
        }

        // derivative
        public double DLossFunction(double setValue, double targetValue)
        {
            return setValue - targetValue;
        }
    }
}
