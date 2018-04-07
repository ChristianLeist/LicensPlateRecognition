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
            //FullyConnectedLayer fullyConnectedLayer = new FullyConnectedLayer(4,4);
            //fullyConnectedLayer.InitLayerMat();
            ConvolutionLayer convLayer = new ConvolutionLayer();
            string bitmapFilePath = @"C:\Users\Chris\source\repos\LicensPlateRecognition\LicensPlateRecognition\LicensPlateRecognition\Image\Lenna.jpg";
            Bitmap b = new Bitmap(bitmapFilePath);
            b = convLayer.ZeroPadding(b, 1);
            b.Save(@"C:\Users\Chris\source\repos\LicensPlateRecognition\LicensPlateRecognition\LicensPlateRecognition\Image\ImageLennaPadding.jpg");

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
