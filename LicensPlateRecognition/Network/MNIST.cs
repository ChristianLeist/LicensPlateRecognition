using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LicensPlateRecognition.Network
{
    public static class BigEndianConverter
    {
        public static int ReadBigInt32(this BinaryReader br)
        {
            var bytes = br.ReadBytes(sizeof(Int32));
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }

    public class MNIST
    {
        public List<DigitImage> TrainingImgs { get; private set; }
        public List<DigitImage> TestImgs { get; private set; }

        public MNIST()
        {
            TrainingImgs = new List<DigitImage>();
            TestImgs = new List<DigitImage>();
        }

        public void ShuffleTrainingImgs()
        {
            var rnd = new Random();
            TrainingImgs = TrainingImgs.OrderBy(x => rnd.Next()).ToList();
        }

        public void ReadTestMNIST()
        {
            try
            {
                FileStream ifsLabels =
                 new FileStream(@"C:\Users\cleist\BA\MNIST\t10k-labels.idx1-ubyte", FileMode.Open); // test labels
                FileStream ifsImages =
                 new FileStream(@"C:\Users\cleist\BA\MNIST\t10k-images.idx3-ubyte", FileMode.Open); // test images

                BinaryReader brLabels = new BinaryReader(ifsLabels);
                BinaryReader brImages = new BinaryReader(ifsImages);

                int magic1 = BigEndianConverter.ReadBigInt32(brImages); // discard
                int numImages = BigEndianConverter.ReadBigInt32(brImages);
                int numRows = BigEndianConverter.ReadBigInt32(brImages);
                int numCols = BigEndianConverter.ReadBigInt32(brImages);

                int magic2 = BigEndianConverter.ReadBigInt32(brLabels);
                int numLabels = BigEndianConverter.ReadBigInt32(brLabels);

                byte[][] pixels = new byte[numRows][];
                for (int i = 0; i < pixels.Length; ++i)
                    pixels[i] = new byte[numCols];

                // each image
                for (int di = 0; di < 100; ++di)
                {
                    for (int i = 0; i < numRows; ++i)
                    {
                        for (int j = 0; j < numCols; ++j)
                        {
                            byte b = brImages.ReadByte();
                            pixels[i][j] = b;
                        }
                    }

                    byte lbl = brLabels.ReadByte();

                    TestImgs.Add(new DigitImage(pixels, lbl, numRows, numCols));
                }

                ifsImages.Close();
                brImages.Close();
                ifsLabels.Close();
                brLabels.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        public void ReadTrainMNIST()
        {
            try
            {
                FileStream ifsLabels =
                 new FileStream(@"C:\Users\cleist\BA\MNIST\train-labels.idx1-ubyte", FileMode.Open); // train labels
                FileStream ifsImages =
                 new FileStream(@"C:\Users\cleist\BA\MNIST\train-images.idx3-ubyte", FileMode.Open); // train images

                BinaryReader brLabels = new BinaryReader(ifsLabels);
                BinaryReader brImages = new BinaryReader(ifsImages);

                int magic1 = BigEndianConverter.ReadBigInt32(brImages); // discard
                int numImages = BigEndianConverter.ReadBigInt32(brImages);
                int numRows = BigEndianConverter.ReadBigInt32(brImages);
                int numCols = BigEndianConverter.ReadBigInt32(brImages);

                int magic2 = BigEndianConverter.ReadBigInt32(brLabels);
                int numLabels = BigEndianConverter.ReadBigInt32(brLabels);

                byte[][] pixels = new byte[numRows][];
                for (int i = 0; i < pixels.Length; ++i)
                    pixels[i] = new byte[numCols];

                // each image
                for (int di = 0; di < 1000; ++di)
                {
                    for (int i = 0; i < numRows; ++i)
                    {
                        for (int j = 0; j < numCols; ++j)
                        {
                            byte b = brImages.ReadByte();
                            pixels[i][j] = b;
                        }
                    }

                    byte lbl = brLabels.ReadByte();

                    TrainingImgs.Add(new DigitImage(pixels, lbl, numRows, numCols));
                }

                ifsImages.Close();
                brImages.Close();
                ifsLabels.Close();
                brLabels.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }

    public class DigitImage
    {
        public double[][][] Pixels { get; private set; }
        public double[] Label { get; private set; }
        public byte integerLabel;
        public int numRows;
        public int numCols;

        public DigitImage(byte[][] pixels, byte label, int numRows, int numCols)
        {
            this.numRows = numRows;
            this.numCols = numCols;
            this.Pixels = new double[numRows][][];

            for (int k = 0; k < 1; k++)
            {
                for (int j = 0; j < numCols; ++j)
                {
                    for (int i = 0; i < numRows; ++i)
                    {
                        // init Pixels
                        if (k == 0)
                        {
                            if (j == 0)
                            {
                                this.Pixels[i] = new double[numCols][];
                            }
                            this.Pixels[i][j] = new double[1];
                        }
                        this.Pixels[i][j][k] = pixels[i][j];
                    }
                }
            }
            this.integerLabel = label;

            this.Label = ConvertToBinaryClass(label);
        }

        public override string ToString()
        {
            //string s = "";
            //for (int k = 0; k < 1; k++)
            //{
            //    for (int i = 0; i < numRows; ++i)
            //    {
            //        for (int j = 0; j < numCols; ++j)
            //        {
            //            if (this.Pixels[i][j][k] == 0)
            //                s += "#"; // white
            //            else if (this.Pixels[i][j][k] == 255)
            //                s += " "; // black
            //            else
            //                s += "."; // gray
            //        }
            //        s += "\n";
            //    }
            //}

            //s += this.Label.ToString();

            return integerLabel.ToString();
        }

        public double[] ConvertToBinaryClass(byte label)
        {
            var binaryInt = new double[10];
            for (int i = 0; i < binaryInt.Length; i++)
            {
                if (i == label)
                {
                    binaryInt[i] = 1;
                }
                else
                {
                    binaryInt[i] = 0;
                }
            }
            return binaryInt;
        }
    }
}