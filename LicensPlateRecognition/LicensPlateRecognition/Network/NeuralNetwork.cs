using LicensPlateRecognition.Kernel;
using LicensPlateRecognition.Layer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LicensPlateRecognition.Network
{
    class NeuralNetwork
    {
        static void Main(string[] args)
        {
            string bitmapFilePath = @"C:\Users\Chris\source\repos\LicensPlateRecognition\LicensPlateRecognition\LicensPlateRecognition\Image\Lenna.jpg";
            Bitmap b = new Bitmap(bitmapFilePath);

            InputLayer inputLayer = new InputLayer(64, 64);
            inputLayer.LoadImage(b);

            ConvolutionLayer convLayer1 = new ConvolutionLayer(new Filter(5, 5, 3), 5, 2);
            convLayer1.RandInitFilter();
            convLayer1.Convolution(inputLayer.ImgMatrix);

            ConvolutionLayer convLayer2 = new ConvolutionLayer(new Filter(3, 3, 5), 10, 1);
            convLayer2.RandInitFilter();
            convLayer2.Convolution(convLayer1.ImgMatrix);

            PoolingLayer pooling = new PoolingLayer();
            pooling.MaxPooling(convLayer2.ImgMatrix);

            for (int z = 0; z < pooling.ImgMatrix[0][0].Length; z++)
            {
                for (int y = 0; y < pooling.ImgMatrix[0].Length; y++)
                {
                    for (int x = 0; x < pooling.ImgMatrix.Length; x++)
                    {
                        Console.Write(pooling.ImgMatrix[x][y][z] + " ");
                    }
                    Console.WriteLine();
                }
            }

            //b.Save(@"C:\Users\cleist\source\repos\LicensPlateRecognition\LicensPlateRecognition\LicensPlateRecognition\Image\LennaFilter.jpg");

            //FullyConnectedLayer fullyConnectedLayer = new FullyConnectedLayer(4,4);
            //fullyConnectedLayer.InitLayerMat();

            Console.WriteLine("Press any key to continue...");
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
