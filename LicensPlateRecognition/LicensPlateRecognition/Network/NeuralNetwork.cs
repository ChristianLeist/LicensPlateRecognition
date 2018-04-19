using LicensPlateRecognition.Kernel;
using LicensPlateRecognition.Layer;
using LicensPlateRecognition.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace LicensPlateRecognition.Network
{
    class NeuralNetwork
    {
        private ExecMode execMode = ExecMode.Learning;
        private double lossFunction = 0;
        private double learningRate = 0.1;

        static void Main(string[] args)
        {
            NeuralNetwork network = new NeuralNetwork();
            string imageFilePath = @"C:\Users\cleist\source\repos\LicensPlateRecognition\LicensPlateRecognition\LicensPlateRecognition\Image\";
            string[] trainingData = Directory.GetFiles(imageFilePath + "TrainingData", "*.jpg");
            string[] testData = Directory.GetFiles(imageFilePath + "TestData", "*.jpg");
            Random rnd = new Random();

            if (network.execMode == ExecMode.Learning)
            {
                // shuffle training input
                string[] rndTraining = trainingData.OrderBy(x => rnd.Next()).ToArray();
                for (int i = 0; i < rndTraining.Length; i++)
                {
                    network.TraverseNetwork(new Bitmap(rndTraining[i]));
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public void TraverseNetwork(Bitmap b)
        {
            InputLayer inputLayer;
            ConvolutionLayer convLayer1;
            ConvolutionLayer convLayer2;
            PoolingLayer pooling1;
            FullyConnectedLayer fullyConnectedLayer1;
            FullyConnectedLayer fullyConnectedLayer2;
            OutputLayer outputLayer;

            inputLayer = new InputLayer(64, 64);
            convLayer1 = new ConvolutionLayer(new Filter(5, 5, 3), 5, 2, inputLayer);
            convLayer2 = new ConvolutionLayer(new Filter(3, 3, 5), 10, 1, convLayer1);
            pooling1 = new PoolingLayer(convLayer2);
            fullyConnectedLayer1 = new FullyConnectedLayer(pooling1);
            fullyConnectedLayer2 = new FullyConnectedLayer(fullyConnectedLayer1);
            outputLayer = new OutputLayer(fullyConnectedLayer2);

            // TODO: liste mit layer erzeugen, durch iterieren und next layer setzen
            outputLayer.NextLayer = null;
            fullyConnectedLayer2.NextLayer = outputLayer;
            //

            inputLayer.LoadImage(b);

            convLayer1.RandInitFilter();
            convLayer1.Convolution(inputLayer.ImgMatrix);

            convLayer2.RandInitFilter();
            convLayer2.Convolution(convLayer1.ImgMatrix);

            pooling1.MaxPooling(convLayer2.ImgMatrix);
            pooling1.Flattening();

            fullyConnectedLayer1.Init(pooling1.FlatArray.Length, pooling1.FlatArray.Length);
            fullyConnectedLayer1.RandInitLayerMat();
            fullyConnectedLayer1.FeedForward(pooling1.FlatArray);

            fullyConnectedLayer2.Init(fullyConnectedLayer1.FlatArray.Length, 2);
            fullyConnectedLayer2.RandInitLayerMat();
            fullyConnectedLayer2.FeedForward(fullyConnectedLayer1.FlatArray);

            outputLayer.ComputeOutput();
            outputLayer.PrintArray();

            // Backpropagation
            if (this.execMode == ExecMode.Learning)
            {
                // TODO
            }
        }

        public void PrintMatrix(double[][][] inMatrix)
        {
            for (int z = 0; z < inMatrix[0][0].Length; z++)
            {
                for (int y = 0; y < inMatrix[0].Length; y++)
                {
                    for (int x = 0; x < inMatrix.Length; x++)
                    {
                        Console.Write(inMatrix[x][y][z] + " ");
                    }
                    Console.WriteLine();
                }
            }
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
