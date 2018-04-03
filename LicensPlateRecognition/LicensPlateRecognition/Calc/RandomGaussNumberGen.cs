using System;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition.Calc
{
    class RandomGaussNumberGen
    {
        private Random rand;
        private int mean;
        private int stdDev;

        public RandomGaussNumberGen(int mean, int stdDev)
        {
            this.rand = new Random();
            this.mean = mean;
            this.stdDev = stdDev;
        }

        public double CreateRandomNum()
        {
            //uniform(0,1] random doubles
            double u1 = 1.0 - this.rand.NextDouble();
            double u2 = 1.0 - this.rand.NextDouble();
            //random normal(0,1)
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            //random normal(mean,stdDev^2)
            return this.mean + this.stdDev * randStdNormal;
        }
    }
}
