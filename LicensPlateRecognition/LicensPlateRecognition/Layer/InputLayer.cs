using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    class InputLayer
    {
        public Color[][] GetBitMapColorMatrix()
        {
            string bitmapFilePath = @"..\Image\";
            Bitmap b = new Bitmap(bitmapFilePath);

            int height = b.Height;
            int width = b.Width;

            Color[][] colorMatrix = new Color[width][];
            for (int i = 0; i < width; i++)
            {
                colorMatrix[i] = new Color[height];
                for (int j = 0; j < height; j++)
                {
                    colorMatrix[i][j] = b.GetPixel(i, j);
                }
            }
            return colorMatrix;
        }
    }
}
