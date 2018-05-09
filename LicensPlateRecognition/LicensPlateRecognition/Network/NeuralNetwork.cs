using CsvHelper;
using LicensPlateRecognition.Util;
using System;
using System.Collections.Generic;
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
