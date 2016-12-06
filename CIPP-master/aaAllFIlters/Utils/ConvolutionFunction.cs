namespace aaAllFIlters.Filters
{
    using System;

    public class ConvolutionFunction
    {
        private readonly float[,] kernel;

        public ConvolutionFunction(float[,] kernel)
        {
            this.kernel = kernel;
        }

        public byte[,] ComputeLossy(byte[,] channel)
        {
            var result = ComputeWithAccuracy(channel);
            byte[,] blured = new byte[channel.GetLength(0), channel.GetLength(1)];
            for (int i = 0; i < channel.GetLength(0); i++)
            {
                for (int j = 0; j < channel.GetLength(1); j++)
                {
                    blured[i, j] = (byte)(result[i, j] > 255 ? 255 : result[i, j]);
                }
            }

            return blured;
        }

        public float[,] ComputeWithAccuracy(byte[,] channel)
        {
            int lines = channel.GetLength(0);
            int columns = channel.GetLength(1);

            float[,] result = new float[lines, columns];
            for (int cc = 0; cc < columns; cc++)
            {
                for (int rr = 0; rr < lines; rr++)
                {
                    result[rr, cc] = this.ComputePixelValue(channel, rr, cc);
                }
            }

            return result;
        }

        private float ComputePixelValue(byte[,] channel, int pixelLine, int pixelColumn)
        {
            double bluredPixelValue = 0;

            int lines = this.kernel.GetLength(0);
            int columns = this.kernel.GetLength(1);

            for (int i = 0; i < lines; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int x = Math.Abs(pixelLine - i);
                    int y = Math.Abs(pixelColumn - j);

                    bluredPixelValue += this.kernel[i, j] * (float)channel[x, y];
                }
            }

            return (float)bluredPixelValue;
        }
    }
}