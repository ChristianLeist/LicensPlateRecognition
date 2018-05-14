using LicensPlateRecognition.Calc;
using LicensPlateRecognition.Kernel;
using LicensPlateRecognition.Network;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace LicensPlateRecognition.Layer
{
    public class ConvolutionLayer : Layer
    {
        public List<Filter> Filters { get; private set; }
        private int stride;
        private double[][][] zValueMatrix;
        private double[][][] activationValueMatrix;
        private Function activation;

        private ConvolutionLayer(NeuralNetwork network) : base(network) { } // standard constructor

        public ConvolutionLayer(Filter filter, int numberOfFilters, int stride, NeuralNetwork network) : base(network)
        {
            Filters = new List<Filter>();
            for (int i = 0; i < numberOfFilters; i++)
            {
                this.Filters.Add(new Filter(filter.Width, filter.Height, filter.Depth));
            }
            this.stride = stride;
            activation = new Function();
        }

        public override void FeedForward(Image img, double[] flat, double[][][] matrix)
        {
            this.zValueMatrix = matrix;
            Convolution(matrix);
        }

        public override void BackwardPass(double[] deltaArray, double[][][] deltaMatrix)
        {
            int filterDepth = this.Filters.Find(filter => true).FilterMat[0][0].Length;
            // Zero padding border formular with fxf filter: (f - 1) / 2
            int f = this.Filters.Find(filter => true).FilterMat.Length;
            int border = (f - 1) / 2;
            this.activationValueMatrix = ZeroPadding(this.activationValueMatrix, border);

            int width = deltaMatrix.Length;
            int height = deltaMatrix[0].Length;
            int depth = this.Filters.Count;

            this.DeltaMatrix = new double[width * this.stride][][];

            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        // init DeltaMatrix
                        if (z == 0)
                        {
                            for (int i = 0; i < this.stride; i++)
                            {
                                for (int j = 0; j < this.stride; j++)
                                {
                                    if (y + i == 0)
                                    {
                                        this.DeltaMatrix[x * this.stride + j] = new double[height * this.stride][];
                                    }
                                    this.DeltaMatrix[x * this.stride + j][y * this.stride + i] = new double[filterDepth];
                                }
                            }
                        }

                        // filter loop
                        for (int d = 0; d < filterDepth; d++)
                        {
                            for (int h = 0; h < f; h++)
                            {
                                for (int w = 0; w < f; w++)
                                {
                                    // compute filter weight gradients and bias gradients for layer
                                    this.Filters[z].FilterGradientMat[w][h][d] += this.learningRate * deltaMatrix[x][y][z] *
                                    this.activationValueMatrix[this.stride * x + w][this.stride * y + h][d];
                                    this.Filters[z].BiasGradient += this.learningRate * deltaMatrix[x][y][z];

                                    // compute delta gradients for layer - 1
                                    this.DeltaMatrix[x * this.stride][y * this.stride][d] += this.Filters[z].FilterMat[w][h][d] * deltaMatrix[x][y][z] *
                                    activation.DReLU(this.zValueMatrix[this.stride * x][this.stride * y][d]);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Convolution(double[][][] inMatrix)
        {
            int filterDepth = this.Filters.Find(filter => true).FilterMat[0][0].Length;
            // Zero padding border formular with fxf filter: (f - 1) / 2
            int f = this.Filters.Find(filter => true).FilterMat.Length;
            int border = (f - 1) / 2;
            double[][][] padMatrix = ZeroPadding(inMatrix, border);

            int outWidth = inMatrix.Length / this.stride;
            int outHeigth = inMatrix[0].Length / this.stride;
            // Number of Filters
            int outDepth = this.Filters.Count;

            double[][][] outMatrix = new double[outWidth][][];
            this.activationValueMatrix = new double[this.zValueMatrix.Length][][];

            // Loop over Filters
            for (int z = 0; z < outDepth; z++)
            {
                // Create feature map
                for (int y = 0; y < outHeigth; y++)
                {
                    for (int x = 0; x < outWidth; x++)
                    {
                        // init outMatrix
                        if (z == 0)
                        {
                            if (y == 0)
                            {
                                outMatrix[x] = new double[outHeigth][];
                            }
                            outMatrix[x][y] = new double[outDepth];
                        }

                        // ReLU activation
                        if (z == 0)
                        {
                            for (int k = 0; k < this.zValueMatrix[0][0].Length; k++)
                            {
                                for (int i = 0; i < this.stride; i++)
                                {
                                    for (int j = 0; j < this.stride; j++)
                                    {
                                        // init activationValueMatrix
                                        if (k == 0)
                                        {
                                            if (y + i == 0)
                                            {
                                                this.activationValueMatrix[x * this.stride + j] = new double[this.zValueMatrix[0].Length][];
                                            }
                                            this.activationValueMatrix[x * this.stride + j][y * this.stride + i] = new double[this.zValueMatrix[0][0].Length];
                                        }

                                        this.activationValueMatrix[this.stride * x + j][this.stride * y + i][k] =
                                        activation.ReLU(this.zValueMatrix[this.stride * x + j][this.stride * y + i][k]);
                                    }
                                }
                            }
                        }

                        // Full depth convolution
                        double convVal = 0.00;
                        for (int d = 0; d < filterDepth; d++)
                        {
                            for (int h = 0; h < f; h++)
                            {
                                for (int w = 0; w < f; w++)
                                {
                                    convVal += this.Filters[z].FilterMat[w][h][d] * padMatrix[this.stride * x + w][this.stride * y + h][d];
                                }
                            }
                        }
                        outMatrix[x][y][z] = convVal + this.Filters[z].Bias;
                    }
                }
            }
            this.ImgMatrix = outMatrix;
        }

        public double[][][] ZeroPadding(double[][][] inputImage, int border)
        {
            int width = inputImage.Length;
            int heigth = inputImage[0].Length;
            int depth = inputImage[0][0].Length;

            int padWidth = width + 2 * border;
            int padHeigth = heigth + 2 * border;

            double[][][] outMatrix = new double[padWidth][][];

            // Create Matrix with zero padded borders
            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < padHeigth; y++)
                {
                    for (int x = 0; x < padWidth; x++)
                    {
                        // init
                        if (z == 0)
                        {
                            if (y == 0)
                            {
                                outMatrix[x] = new double[padHeigth][];
                            }
                            outMatrix[x][y] = new double[depth];
                        }

                        // pad
                        if (x < border || x >= padHeigth - border * 2 || y < border || y >= padWidth - border * 2)
                        {
                            outMatrix[x][y][z] = 0;
                        }
                        else
                        {
                            outMatrix[x][y][z] = inputImage[x][y][z];
                        }
                    }
                }
            }

            return outMatrix;
        }

        public override void RandInitFilter()
        {
            RandomGaussNumberGen randNumGen = new RandomGaussNumberGen(0, 1);
            foreach (Filter filter in this.Filters)
            {
                filter.Bias = randNumGen.CreateRandomNum();
                for (int z = 0; z < filter.Depth; z++)
                {
                    for (int y = 0; y < filter.Height; y++)
                    {
                        for (int x = 0; x < filter.Width; x++)
                        {
                            // init
                            if (z == 0)
                            {
                                if (y == 0)
                                {
                                    filter.FilterGradientMat[x] = new double[filter.Height][];
                                    filter.FilterMat[x] = new double[filter.Height][];
                                }
                                filter.FilterGradientMat[x][y] = new double[filter.Depth];
                                filter.FilterMat[x][y] = new double[filter.Depth];
                            }

                            // fill
                            filter.FilterMat[x][y][z] = randNumGen.CreateRandomNum();
                        }
                    }
                }
            }
        }

        // stochastic gradient descent weight update
        public override void UpdateWeights(int miniBatchSize)
        {
            foreach (Filter filter in this.Filters)
            {
                filter.Bias -= (1.0 / (double)miniBatchSize) * filter.BiasGradient;
                filter.BiasGradient = 0;
                for (int z = 0; z < filter.Depth; z++)
                {
                    for (int y = 0; y < filter.Height; y++)
                    {
                        for (int x = 0; x < filter.Width; x++)
                        {
                            filter.FilterMat[x][y][z] -= (1.0 / (double)miniBatchSize) * filter.FilterGradientMat[x][y][z];
                            filter.FilterGradientMat[x][y][z] = 0;
                        }
                    }
                }
            }
        }

        public override void StoreWeights()
        {
            XmlSerializer writer = new XmlSerializer(typeof(List<Filter>));

            using (FileStream file = File.OpenWrite(this.ToString() + this.LayerNum.ToString() + ".xml"))
            {
                writer.Serialize(file, this.Filters);
            }
        }

        public override void LoadWeights()
        {
            XmlSerializer reader = new XmlSerializer(typeof(List<Filter>));

            using (FileStream file = File.OpenRead(this.ToString() + this.LayerNum.ToString() + ".xml"))
            {
                this.Filters = (List<Filter>)reader.Deserialize(file);
            }
        }

        public override void RandInitLayerMat()
        {
            throw new NotImplementedException();
        }

        public override void InitLayer(int height, int width)
        {
            throw new NotImplementedException();
        }

        public override double[] GetOutputArray()
        {
            throw new NotImplementedException();
        }
    }
}
