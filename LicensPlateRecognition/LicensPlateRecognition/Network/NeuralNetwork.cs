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
            double[,,] image = convLayer1.Convolution(inputLayer.ImgArray);

            ConvolutionLayer convLayer2 = new ConvolutionLayer(new Filter(3, 3, 5), 10, 1);
            convLayer2.RandInitFilter();
            image = convLayer2.Convolution(image);

            PoolingLayer pooling = new PoolingLayer();
            image = pooling.MaxPooling(convLayer2.Convolution(image));
            for (int z = 0; z < image.GetLength(2); z++)
            {
                for (int y = 0; y < image.GetLength(1); y++)
                {
                    for (int x = 0; x < image.GetLength(0); x++)
                    {
                        Console.Write(image[x, y, z] + " ");
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
