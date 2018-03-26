using System;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition
{
    public class Neuron
    {
        private double weight;

        public double Weight
        {
            get { return this.weight; }
            set { this.weight = value; }
        }
    }
}
