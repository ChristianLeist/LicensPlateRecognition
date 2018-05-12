﻿using LicensPlateRecognition.Kernel;
using LicensPlateRecognition.Layer;
using LicensPlateRecognition.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LicensPlateRecognition.Network
{
    public class Program
    {
        public static void Main(string[] args)
        {
            NeuralNetwork network = new NeuralNetwork(ExecMode.Learning, 0.1);
            Random rnd = new Random();
            string imageFilePath = @"C:\Users\Chris\source\repos\LicensPlateRecognition\LicensPlateRecognition\LicensPlateRecognition\Image\";
            string[] trainingData = Directory.GetFiles(imageFilePath + "TrainingData", "*.jpg");
            string[] testData = Directory.GetFiles(imageFilePath + "TestData", "*.jpg");
            // key value pairs for training or test input and desired output
            Dictionary<string, double[]> keyValuePairs = new Dictionary<string, double[]>();

            // Declare network layers: declare in order of traversion! Since it will be the order of the layers list in network class
            InputLayer inputLayer = new InputLayer(256, 256, 3, network);
            ConvolutionLayer convLayer1 = new ConvolutionLayer(new Filter(5, 5, inputLayer.Depth), 4, 2, network);
            ConvolutionLayer convLayer2 = new ConvolutionLayer(new Filter(5, 5, convLayer1.Filters.Count), 4, 1, network);
            PoolingLayer pooling1 = new PoolingLayer(network);
            ConvolutionLayer convLayer3 = new ConvolutionLayer(new Filter(3, 3, convLayer2.Filters.Count), 8, 1, network);
            ConvolutionLayer convLayer4 = new ConvolutionLayer(new Filter(3, 3, convLayer3.Filters.Count), 8, 1, network);
            PoolingLayer pooling2 = new PoolingLayer(network);
            ConvolutionLayer convLayer5 = new ConvolutionLayer(new Filter(3, 3, convLayer4.Filters.Count), 12, 1, network);
            ConvolutionLayer convLayer6 = new ConvolutionLayer(new Filter(3, 3, convLayer5.Filters.Count), 12, 1, network);
            PoolingLayer pooling3 = new PoolingLayer(network);
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
                var epochs = 1000;
                // must be divisible through number of training data
                var miniBatchSize = 1;

                foreach (var key in rndList)
                {
                    rndKeyValuePairs.Add(key, keyValuePairs[key]);
                }

                network.Learning(rndKeyValuePairs, outClass, epochs, miniBatchSize);
            }

            if (network.ExecMode == ExecMode.Testing)
            {
                network.Testing();
            }

            if (network.ExecMode == ExecMode.Normal)
            {
                // training data
                network.LoadCSV(imageFilePath, keyValuePairs, "training.csv", outClass);

                network.ForwardPass(outClass, keyValuePairs.ElementAt(0).Key);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
