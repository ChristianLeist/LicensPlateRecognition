using System;

namespace LicensPlateRecognition.Calc
{
    public class RandomNumberGen
    {
        private Random rand;
        private int mean;
        private int stdDev;

        public RandomNumberGen(int mean, int stdDev)
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

            // only positiv values, to prevent excluding values due to ReLu activation
            //if (randStdNormal < 0)
            //    randStdNormal *= -1;

            //random normal(mean,stdDev^2)
            return this.mean + this.stdDev * randStdNormal;
        }
    }
}
