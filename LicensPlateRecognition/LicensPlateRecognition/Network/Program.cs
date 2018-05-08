using LicensPlateRecognition.Kernel;
using LicensPlateRecognition.Layer;
using LicensPlateRecognition.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace LicensPlateRecognition.Network
{
    class Program
    {
        static void Main(string[] args)
        {
            NeuralNetwork network = new NeuralNetwork(ExecMode.Learning, 0.1);
            Random rnd = new Random();
            string imageFilePath = @"C:\Users\Chris\source\repos\LicensPlateRecognition\LicensPlateRecognition\LicensPlateRecognition\Image\";
            string[] trainingData = Directory.GetFiles(imageFilePath + "TrainingData", "*.jpg");
            string[] testData = Directory.GetFiles(imageFilePath + "TestData", "*.jpg");
            // key value pairs for training or test input and desired output
            Dictionary<string, double[]> keyValuePairs = new Dictionary<string, double[]>();

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

            if (network.ExecMode == ExecMode.Learning)
            {
                // just needed once to create a csv with tuple of image and class value
                // uncomment if you want to create the traning data
                //network.CreateCSV(imageFilePath, trainingData, "training.csv");

                network.LoadCSV(imageFilePath, keyValuePairs, "training.csv", outClass);

                // shuffle training input
                var list = keyValuePairs.Keys.ToList();
                var rndList = list.OrderBy(x => rnd.Next());
                Dictionary<string, double[]> rndKeyValuePairs = new Dictionary<string, double[]>();

                foreach (var key in rndList)
                {
                    rndKeyValuePairs.Add(key, keyValuePairs[key]);
                }

                Learning(network, rndKeyValuePairs, outClass);
            }

            if (network.ExecMode == ExecMode.Testing)
            {
                Testing();
            }

            if (network.ExecMode == ExecMode.Normal)
            {
                ForwardPass();
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void Learning(NeuralNetwork network, Dictionary<string, double[]> rndKeyValuePairs, int outClass)
        {
            // forward pass
            for (int i = 0; i < rndKeyValuePairs.Count; i++)
            {
                for (int j = 0; j < network.Layers.Count; j++)
                {
                    if (network.Layers[j].GetType().Equals(typeof(InputLayer)))
                    {
                        network.Layers[j].FeedForward(new Bitmap(rndKeyValuePairs.ElementAt(i).Key), null, null);
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
                for (int j = network.Layers.Count - 1; j >= 0; j--)
                {
                    if (network.Layers[j].GetType().Equals(typeof(InputLayer)))
                    {
                        network.Layers[j].BackwardPass(null, network.Layers[j + 1].DeltaMatrix);
                    }

                    if (network.Layers[j].GetType().Equals(typeof(ConvolutionLayer)))
                    {
                        network.Layers[j].BackwardPass(null, network.Layers[j + 1].DeltaMatrix);
                    }

                    if (network.Layers[j].GetType().Equals(typeof(PoolingLayer)))
                    {
                        if (network.Layers[j + 1].GetType().Equals(typeof(FullyConnectedLayer)))
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
                        network.Layers[j].BackwardPass(rndKeyValuePairs.ElementAt(i).Value, null);
                    }
                }
            }
        }

        public static void Testing()
        {
            // TODO: Testbetrieb
        }

        public static void ForwardPass()
        {
            // TODO: Produktivbetrieb
        }

    }
}
