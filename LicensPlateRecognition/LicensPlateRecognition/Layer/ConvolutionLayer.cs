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

        private Bitmap ZeroPadding(Bitmap img, int border)
        {
            int w = img.Width;
            int h = img.Height;
            int wp = w + 2 * border;
            int hp = h + 2 * border;
            Bitmap ri = new Bitmap(wp, hp);
            BitmapData rd = ri.LockBits(new Rectangle(0, 0, wp, hp),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            BitmapData id = img.LockBits(new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int imgb = id.Stride * id.Height;
            int borb = rd.Stride * rd.Height;
            byte[] imga = new byte[imgb];
            byte[] bora = new byte[borb];
            for (int i = 3; i < borb; i += 4)
            {
                bora[i] = 255;
            }
            Marshal.Copy(id.Scan0, imga, 0, imgb);
            img.UnlockBits(id);
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int ip = y * id.Stride + x * 4;
                    int rp = y * rd.Stride + x * 4;
                    for (int i = 0; i < 3; i++)
                    {
                        bora[(rd.Stride + 4) * border + rp + i] = imga[ip + i];
                    }
                }
            }
            Marshal.Copy(bora, 0, rd.Scan0, borb);
            ri.UnlockBits(rd);
            return ri;
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
