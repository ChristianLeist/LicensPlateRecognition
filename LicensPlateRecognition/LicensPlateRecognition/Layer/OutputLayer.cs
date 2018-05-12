using LicensPlateRecognition.Calc;
using LicensPlateRecognition.Network;
using System;
using System.Drawing;

namespace LicensPlateRecognition.Layer
{
    public class OutputLayer : Layer
    {
        private Function activation;
        private double[] outputArray;

        public OutputLayer(NeuralNetwork network) : base(network)
        {
            activation = new Function();
        }

        public override void FeedForward(Image img, double[] flat, double[][][] matrix)
        {
            ComputeOutput(flat);
        }

        public override void BackwardPass(double[] setValueArray, double[][][] deltaMatrix)
        {
            this.DeltaArray = new double[this.outputArray.Length];
            double[] DSoftmaxArray = activation.DSoftmax(this.FlatArray);
            for (int i = 0; i < this.outputArray.Length; i++)
            {
                // delta error * derivative z
                this.DeltaArray[i] = activation.DLossFunction(this.outputArray[i], setValueArray[i]) * DSoftmaxArray[i];
            }
        }

        public void ComputeOutput(double[] flatArray)
        {
            // unactivated values
            this.FlatArray = flatArray;
            // activated values
            this.outputArray = activation.Softmax(flatArray);
        }

        public override double[] GetOutputArray()
        {
            return this.outputArray;
        }

        public override void RandInitFilter()
        {
            throw new NotImplementedException();
        }

        public override void RandInitLayerMat()
        {
            throw new NotImplementedException();
        }

        public override void InitLayer(int height, int width)
        {
            throw new NotImplementedException();
        }

        public override void StoreWeights()
        {
            throw new NotImplementedException();
        }

        public override void LoadWeights()
        {
            throw new NotImplementedException();
        }

        public override void UpdateWeights(double learningRate, int miniBatchSize)
        {
            throw new NotImplementedException();
        }
    }
}
