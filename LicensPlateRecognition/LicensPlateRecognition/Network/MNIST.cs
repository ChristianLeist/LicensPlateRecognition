using System;
using System.Collections.Generic;
using System.IO;

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

        public void ReadMNIST()
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
                for (int di = 0; di < numImages; ++di)
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
                    //DigitImage img = new DigitImage(pixels, lbl, numRows, numCols);
                    //Console.WriteLine(img.ToString());
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
        public byte[][] Pixels { get; private set; }
        public byte Label { get; private set; }
        public int numRows;
        public int numCols;

        public DigitImage(byte[][] pixels, byte label, int numRows, int numCols)
        {
            this.numRows = numRows;
            this.numCols = numCols;
            this.Pixels = new byte[numRows][];
            for (int i = 0; i < this.Pixels.Length; ++i)
                this.Pixels[i] = new byte[numCols];

            for (int i = 0; i < numRows; ++i)
                for (int j = 0; j < numCols; ++j)
                    this.Pixels[i][j] = pixels[i][j];

            this.Label = label;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < numRows; ++i)
            {
                for (int j = 0; j < numCols; ++j)
                {
                    if (this.Pixels[i][j] == 0)
                        s += "#"; // white
                    else if (this.Pixels[i][j] == 255)
                        s += " "; // black
                    else
                        s += "."; // gray
                }
                s += "\n";
            }
            s += this.Label.ToString();
            return s;
        }
    }
}