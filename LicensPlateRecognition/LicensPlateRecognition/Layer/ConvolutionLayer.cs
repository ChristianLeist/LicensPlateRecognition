using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    class ConvolutionLayer : Layer
    {
        private int[,] filterMat;
        private int stride;

        public ConvolutionLayer()
        {
            this.stride = 2;
            // Edge detection filter
            this.filterMat = new int[3, 3] { { -1, -1, -1 }, { -1, 8, -1 }, { -1, -1, -1 } };
        }

        public void Convolution(Bitmap inputBitmap)
        {
            for (int i = 0; i < inputBitmap.Height; i++)
            {
                for (int j = 0; j < inputBitmap.Width; j++)
                {
                    // TODO: convolution
                }
            }
        }

        public Bitmap ZeroPadding(Bitmap inputImage, int border)
        {
            int width = inputImage.Width;
            int heigth = inputImage.Height;

            int padWidth = width + 2 * border;
            int padHeigth = heigth + 2 * border;

            Bitmap padImage = new Bitmap(padWidth, padHeigth);
            BitmapData padImageData = padImage.LockBits(new Rectangle(0, 0, padWidth, padHeigth), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            BitmapData inputImageData = inputImage.LockBits(new Rectangle(0, 0, width, heigth), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            int inputImageByte = inputImageData.Stride * inputImageData.Height;
            int borb = padImageData.Stride * padImageData.Height;
            byte[] imga = new byte[inputImageByte];
            byte[] bora = new byte[borb];

            for (int i = 3; i < borb; i += 4)
            {
                bora[i] = 255;
            }
            Marshal.Copy(inputImageData.Scan0, imga, 0, inputImageByte);
            inputImage.UnlockBits(inputImageData);

            for (int y = 0; y < heigth; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int ip = y * inputImageData.Stride + x * 4;
                    int rp = y * padImageData.Stride + x * 4;
                    for (int i = 0; i < 3; i++)
                    {
                        bora[(padImageData.Stride + 4) * border + rp + i] = imga[ip + i];
                    }
                }
            }
            Marshal.Copy(bora, 0, padImageData.Scan0, borb);
            padImage.UnlockBits(padImageData);

            return padImage;
        }

        public override void InitLayerMat()
        {
            // TODO
        }

        public void InitFilterMat()
        {
            // TODO
        }
    }
}
