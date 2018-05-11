using LicensPlateRecognition.Network;
using System.Drawing;

namespace LicensPlateRecognition.Layer
{
    public abstract class Layer
    {
        public static int LayerCount { get; private set; }
        public int LayerNum { get; private set; }
        protected NeuralNetwork neuralNetwork;
        public double[][][] ImgMatrix { get; protected set; }
        public double[][][] DeltaMatrix { get; protected set; }
        public double[] FlatArray { get; protected set; }
        public double[] DeltaArray { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int Depth { get; protected set; }

        protected Layer(NeuralNetwork neuralNetwork)
        {
            this.neuralNetwork = neuralNetwork;
            neuralNetwork.Layers.Add(this);
            LayerCount++;
            this.LayerNum = LayerCount;
        }

        // used in conv layer
        public abstract void RandInitFilter();

        // used in fc layer
        public abstract void RandInitLayerMat();

        // used in fc layer
        public abstract void InitLayer(int height, int width);

        // used in output layer
        public abstract void PrintArray();

        public abstract void FeedForward(Image img, double[] flat, double[][][] matrix);

        public abstract void BackwardPass(double[] deltaArray, double[][][] deltaMatrix);

        // used in conv and fc layer
        public abstract void StoreWeights();

        // used in conv and fc layer
        public abstract void LoadWeights();

        // used in conv and fc layer
        public abstract void UpdateWeights(double learningRate, int miniBatchSize);

        // used before fc layer
        public void Flattening()
        {
            int width = this.ImgMatrix.Length;
            int heigth = this.ImgMatrix[0].Length;
            int depth = this.ImgMatrix[0][0].Length;

            this.FlatArray = new double[width * heigth * depth];
            this.DeltaMatrix = new double[width][][];

            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < heigth; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        // init delta matrix
                        if (z == 0)
                        {
                            if (y == 0)
                            {
                                this.DeltaMatrix[x] = new double[heigth][];
                            }
                            this.DeltaMatrix[x][y] = new double[depth];
                        }

                        this.FlatArray[x + width * (y + heigth * z)] = this.ImgMatrix[x][y][z];
                    }
                }
            }
        }

        // used after backward pass from fc layer
        public void DeFlattening()
        {
            int width = this.ImgMatrix.Length;
            int heigth = this.ImgMatrix[0].Length;
            int depth = this.DeltaArray.Length;

            for (int i = 0; i < depth; i++)
            {
                int index = i;
                int z = index / (width * heigth);
                index -= (z * width * heigth);
                int y = index / width;
                int x = index % width;
                this.DeltaMatrix[x][y][z] = this.DeltaArray[i];
            }
        }
    }
}
