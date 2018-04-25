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
        public List<Layer.Layer> Layers { get; }
        private ExecMode execMode;
        private double lossFunction;
        private double learningRate;

        public NeuralNetwork()
        {
            this.Layers = new List<Layer.Layer>();
            this.execMode = ExecMode.Learning;
            this.lossFunction = 0;
            this.learningRate = 0.1;
        }

        static void Main(string[] args)
        {
            NeuralNetwork network = new NeuralNetwork();
            Random rnd = new Random();
            string imageFilePath = @"C:\Users\Chris\source\repos\LicensPlateRecognition\LicensPlateRecognition\LicensPlateRecognition\Image\";
            string[] trainingData = Directory.GetFiles(imageFilePath + "TrainingData", "*.jpg");
            string[] testData = Directory.GetFiles(imageFilePath + "TestData", "*.jpg");

            // Declare network layers: declare in order of traversion! Since it will be the order of the layers list in network class
            InputLayer inputLayer = new InputLayer(64, 64, 3, network);
            ConvolutionLayer convLayer1 = new ConvolutionLayer(new Filter(5, 5, inputLayer.Depth), 5, 2, network);
            ConvolutionLayer convLayer2 = new ConvolutionLayer(new Filter(3, 3, convLayer1.Filters.Count), 10, 1, network);
            PoolingLayer pooling1 = new PoolingLayer(network);
            FullyConnectedLayer fullyConnectedLayer1 = new FullyConnectedLayer(network);
            FullyConnectedLayer fullyConnectedLayer2 = new FullyConnectedLayer(network);
            OutputLayer outputLayer = new OutputLayer(network);
            // Declare Output Classes
            int outClass = 2;

            if (network.execMode == ExecMode.Learning)
            {
                // shuffle training input
                string[] rndTraining = trainingData.OrderBy(x => rnd.Next()).ToArray();
                // forward pass
                for (int i = 0; i < rndTraining.Length; i++)
                {
                    for (int j = 0; j < network.Layers.Count; j++)
                    {
                        if (network.Layers[j].GetType().Equals(typeof(InputLayer)))
                        {
                            network.Layers[j].FeedForward(new Bitmap(rndTraining[i]), null, null);
                        }

                        if (network.Layers[j].GetType().Equals(typeof(ConvolutionLayer)))
                        {
                            if (i == 0)
                                network.Layers[j].RandInitFilter();
                            network.Layers[j].FeedForward(null, null, network.Layers[j - 1].ImgMatrix);
                        }

                        if (network.Layers[j].GetType().Equals(typeof(PoolingLayer)))
                        {
                            network.Layers[j].FeedForward(null, null, network.Layers[j - 1].ImgMatrix);
                            if (network.Layers[j + 1].GetType().Equals(typeof(FullyConnectedLayer)))
                                network.Layers[j].Flattening();
                        }

                        if (network.Layers[j].GetType().Equals(typeof(FullyConnectedLayer)))
                        {
                            if (network.Layers[j + 1].GetType().Equals(typeof(OutputLayer)))
                                network.Layers[j].InitLayer(network.Layers[j - 1].FlatArray.Length, outClass);
                            else
                                network.Layers[j].InitLayer(network.Layers[j - 1].FlatArray.Length, network.Layers[j - 1].FlatArray.Length);

                            if (i == 0)
                                network.Layers[j].RandInitLayerMat();
                            network.Layers[j].FeedForward(null, network.Layers[j - 1].FlatArray, null);
                        }

                        if (network.Layers[j].GetType().Equals(typeof(OutputLayer)))
                        {
                            network.Layers[j].FeedForward(null, network.Layers[j - 1].FlatArray, null);
                            network.Layers[j].PrintArray();
                        }
                    }

                    // backward pass
                    for (int j = network.Layers.Count - 1; j <= 0; j--)
                    {
                        if (network.Layers[j].GetType().Equals(typeof(InputLayer)))
                        {
                            // TODO
                        }

                        if (network.Layers[j].GetType().Equals(typeof(ConvolutionLayer)))
                        {
                            // TODO
                        }

                        if (network.Layers[j].GetType().Equals(typeof(PoolingLayer)))
                        {
                            if(network.Layers[j + 1].GetType().Equals(typeof(FullyConnectedLayer)))
                                network.Layers[j].BackwardPass(network.Layers[j + 1].DeltaArray, null);
                            else
                                network.Layers[j].BackwardPass(null, network.Layers[j + 1].DeltaMatrix);
                        }

                        if (network.Layers[j].GetType().Equals(typeof(FullyConnectedLayer)))
                        {
                            network.Layers[j].BackwardPass(network.Layers[j + 1].DeltaArray, null);
                        }

                        if (network.Layers[j].GetType().Equals(typeof(OutputLayer)))
                        {
                            network.Layers[j].BackwardPass(null /*TODO: Sollwerte hier übergeben*/, null);
                        }
                    }
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
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
