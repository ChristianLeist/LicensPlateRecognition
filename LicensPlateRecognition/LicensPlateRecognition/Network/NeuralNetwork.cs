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

            InputLayer inputLayer = new InputLayer(50, 50);
            inputLayer.LoadImage(b);

            ConvolutionLayer convLayer = new ConvolutionLayer(new Filter(3, 3, 3), 10, 2);
            convLayer.RandInitFilter();
            double[,,] image = convLayer.Convolution(inputLayer.ImgArray);
            //for (int z = 0; z < image.GetLength(2); z++)
            //{
            //    for (int y = 0; y < image.GetLength(1); y++)
            //    {
            //        for (int x = 0; x < image.GetLength(0); x++)
            //        {
            //            Console.Write(image[x, y, z] + " ");
            //        }
            //        Console.WriteLine();
            //    }
            //}
            
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
