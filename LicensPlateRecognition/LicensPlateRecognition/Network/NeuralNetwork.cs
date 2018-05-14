using CsvHelper;
using LicensPlateRecognition.Layer;
using LicensPlateRecognition.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace LicensPlateRecognition.Network
{
    public class NeuralNetwork
    {
        public List<Layer.Layer> Layers { get; }
        public ExecMode ExecMode { get; }
        public double LearningRate { get; }

        public NeuralNetwork(ExecMode execMode, double learningRate)
        {
            this.Layers = new List<Layer.Layer>();
            this.ExecMode = execMode;
            this.LearningRate = learningRate;
        }

        public void Learning(Dictionary<string, double[]> rndKeyValuePairs, int outClass, int epochs, int miniBatchSize)
        {
            for (int e = 0; e < epochs; e++)
            {
                Console.WriteLine("Processing epoch {0} of {1}", e + 1, epochs);
                var recognition = 0;
                var recRate = 0.0;
                // forward pass
                for (int i = 0; i < rndKeyValuePairs.Count; i++)
                {
                    double[] output = new double[outClass];
                    for (int j = 0; j < this.Layers.Count; j++)
                    {
                        if (this.Layers[j].GetType().Equals(typeof(InputLayer)))
                            this.Layers[j].FeedForward(new Bitmap(rndKeyValuePairs.ElementAt(i).Key), null, null);

                        if (this.Layers[j].GetType().Equals(typeof(ConvolutionLayer)))
                        {
                            if (e == 0 && i == 0)
                                this.Layers[j].RandInitFilter();

                            this.Layers[j].FeedForward(null, null, this.Layers[j - 1].ImgMatrix);
                        }

                        if (this.Layers[j].GetType().Equals(typeof(PoolingLayer)))
                        {
                            this.Layers[j].FeedForward(null, null, this.Layers[j - 1].ImgMatrix);

                            if (this.Layers[j + 1].GetType().Equals(typeof(FullyConnectedLayer)))
                                this.Layers[j].Flattening();
                        }

                        if (this.Layers[j].GetType().Equals(typeof(FullyConnectedLayer)))
                        {
                            if (this.Layers[j + 1].GetType().Equals(typeof(OutputLayer)))
                                this.Layers[j].InitLayer(this.Layers[j - 1].FlatArray.Length, outClass);
                            else
                                this.Layers[j].InitLayer(this.Layers[j - 1].FlatArray.Length, this.Layers[j - 1].FlatArray.Length);

                            if (e == 0 && i == 0)
                                this.Layers[j].RandInitLayerMat();

                            this.Layers[j].FeedForward(null, this.Layers[j - 1].FlatArray, null);
                        }

                        if (this.Layers[j].GetType().Equals(typeof(OutputLayer)))
                        {
                            this.Layers[j].FeedForward(null, this.Layers[j - 1].FlatArray, null);
                            output = this.Layers[j].GetOutputArray();
                        }
                    }

                    // backward pass
                    for (int j = this.Layers.Count - 1; j >= 0; j--)
                    {
                        if (this.Layers[j].GetType().Equals(typeof(ConvolutionLayer)))
                            this.Layers[j].BackwardPass(null, this.Layers[j + 1].DeltaMatrix);

                        if (this.Layers[j].GetType().Equals(typeof(PoolingLayer)))
                        {
                            if (this.Layers[j + 1].GetType().Equals(typeof(FullyConnectedLayer)))
                                this.Layers[j].BackwardPass(this.Layers[j + 1].DeltaArray, null);
                            else
                                this.Layers[j].BackwardPass(null, this.Layers[j + 1].DeltaMatrix);
                        }

                        if (this.Layers[j].GetType().Equals(typeof(FullyConnectedLayer)))
                            this.Layers[j].BackwardPass(this.Layers[j + 1].DeltaArray, null);

                        if (this.Layers[j].GetType().Equals(typeof(OutputLayer)))
                            this.Layers[j].BackwardPass(rndKeyValuePairs.ElementAt(i).Value, null);
                    }

                    // update for every mini Batch
                    if (i % miniBatchSize == miniBatchSize - 1)
                    {
                        // update forward pass
                        for (int j = 0; j < this.Layers.Count; j++)
                        {
                            if (this.Layers[j].GetType().Equals(typeof(ConvolutionLayer)) ||
                                this.Layers[j].GetType().Equals(typeof(FullyConnectedLayer)))
                            {
                                this.Layers[j].UpdateWeights(miniBatchSize);
                                // store weights in last epoch and last training input
                                if (e == epochs - 1 && i == rndKeyValuePairs.Count - 1)
                                    this.Layers[j].StoreWeights();
                            }
                        }
                        Console.WriteLine("\t" + "Weights updated after {0} inputs", i + 1);
                    }

                    // recognition rate computation
                    recognition += RecognitionRate(output, rndKeyValuePairs.ElementAt(i).Value);
                }

                recRate = (double)recognition / (double)rndKeyValuePairs.Count;
                Console.WriteLine("Recognition rate in epoch {0}: {1}" , e + 1, recRate);
            }
        }

        public void Testing()
        {
            // TODO: Testbetrieb
        }

        public void ForwardPass(int outClass, string input)
        {
            for (int j = 0; j < this.Layers.Count; j++)
            {
                if (this.Layers[j].GetType().Equals(typeof(InputLayer)))
                    this.Layers[j].FeedForward(new Bitmap(input), null, null);

                if (this.Layers[j].GetType().Equals(typeof(ConvolutionLayer)))
                {
                    this.Layers[j].LoadWeights();
                    this.Layers[j].FeedForward(null, null, this.Layers[j - 1].ImgMatrix);
                }

                if (this.Layers[j].GetType().Equals(typeof(PoolingLayer)))
                {
                    this.Layers[j].FeedForward(null, null, this.Layers[j - 1].ImgMatrix);

                    if (this.Layers[j + 1].GetType().Equals(typeof(FullyConnectedLayer)))
                        this.Layers[j].Flattening();
                }

                if (this.Layers[j].GetType().Equals(typeof(FullyConnectedLayer)))
                {
                    this.Layers[j].LoadWeights();

                    if (this.Layers[j + 1].GetType().Equals(typeof(OutputLayer)))
                        this.Layers[j].InitLayer(this.Layers[j - 1].FlatArray.Length, outClass);
                    else
                        this.Layers[j].InitLayer(this.Layers[j - 1].FlatArray.Length, this.Layers[j - 1].FlatArray.Length);

                    this.Layers[j].FeedForward(null, this.Layers[j - 1].FlatArray, null);
                }

                if (this.Layers[j].GetType().Equals(typeof(OutputLayer)))
                    this.Layers[j].FeedForward(null, this.Layers[j - 1].FlatArray, null);
            }
        }

        public void CreateCSV(string filePath, string[] files, string CSVName)
        {
            using (TextWriter textWriter = new StreamWriter(filePath + CSVName))
            {
                var csv = new CsvWriter(textWriter);
                for (int i = 0; i < files.Length; i++)
                {
                    var tokens = files[i].Split('_', '.');
                    csv.WriteField(files[i]);
                    csv.WriteField(tokens[tokens.Count() - 2]);
                    csv.NextRecord();
                }
            }
        }

        public void LoadCSV(string filePath, Dictionary<string, double[]> keyValuePairs, string csvName, int outClass)
        {
            using (TextReader textReader = new StreamReader(filePath + csvName))
            {
                var csv = new CsvReader(textReader);
                while (csv.Read())
                {
                    var binaryString = Convert.ToString(Int32.Parse(csv.GetField(1)), 2).PadLeft(outClass, '0');
                    var binaryInt = new double[binaryString.Length];
                    for (int i = 0; i < binaryString.Length; i++)
                    {
                        var charArray = binaryString.ToCharArray();
                        binaryInt[i] = (int) Char.GetNumericValue(charArray[binaryString.Length - 1 - i]);
                    }
                    keyValuePairs.Add(csv.GetField(0), binaryInt);
                }
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

        public int RecognitionRate(double[] output, double[] desiredOutput)
        {
            for (int j = 0; j < output.Length; j++)
            {
                if (desiredOutput[j] == 1 && output[j] > 0.5)
                {
                    return 1;
                }
            }

            return 0;
        }
    }
}
