using System;
using System.Collections.Generic;
using System.Drawing;
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
                    // TODO: Padding, filtering
                }
            }
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
