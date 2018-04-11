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
            string bitmapFilePath = @"C:\Users\cleist\source\repos\LicensPlateRecognition\LicensPlateRecognition\LicensPlateRecognition\Image\Lenna.jpg";
            Bitmap b = new Bitmap(bitmapFilePath);

            InputLayer inputLayer = new InputLayer(500, 500);
            inputLayer.LoadImage(b);

            //ConvolutionLayer convLayer = new ConvolutionLayer();
            //b = convLayer.Convolution(b);
            //b.Save(@"C:\Users\cleist\source\repos\LicensPlateRecognition\LicensPlateRecognition\LicensPlateRecognition\Image\LennaFilter.jpg");

            //FullyConnectedLayer fullyConnectedLayer = new FullyConnectedLayer(4,4);
            //fullyConnectedLayer.InitLayerMat();

            //Console.WriteLine("Press any key to continue...");
            //Console.ReadKey();
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
