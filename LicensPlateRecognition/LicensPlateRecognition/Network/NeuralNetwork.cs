using LicensPlateRecognition.Calc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition.Network
{
    class NeuralNetwork
    {
        static void Main(string[] args)
        {
            RandomGaussNumberGen randNumGen = new RandomGaussNumberGen(0,1);

            for(int i = 1; i < 20; i++)
            {
                Console.WriteLine(randNumGen.CreateRandomNum());
            }

            Console.ReadKey();
        }

        public void StoreNeuralNetwork()
        {
            // TODO: Save weights, biases, etc..
        }

        public void LoadNeuralNetwork()
        {
            // TODO: Load weights, biases, etc..
        }
    }
}
