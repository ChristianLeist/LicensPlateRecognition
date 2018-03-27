using System;

namespace LicensPlateRecognition
{
    public class Neuron
    {
        private double weight;
        private double bias;

        public double Weight
        {
            get { return this.weight; }
            set { this.weight = value; }
        }

        public double Bias 
        {
            get {return this.bias; }
            set { this.bias = value; }
        }
    }
}
