using System;
using System.Collections.Generic;
using System.Text;

namespace LicensPlateRecognition.Layer
{
    abstract class Layer
    {
        protected double[,] layerMat;

        abstract public void InitLayerMat();
    }
}
