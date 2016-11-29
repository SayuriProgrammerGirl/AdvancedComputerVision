namespace aaAllFIlters.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;

    using aaAllFIlters.Utils;

    using ParametersSDK;

    using Plugins.Filters;

    using ProcessingImageSDK;

    public enum SobelOuptput
    {
        GradientMagnitude,

        Gx,

        Gy
    }

    public class SobelFilter : IFilter
    {
        private readonly SobelOuptput outputType;

        private static readonly List<IParameters> parameters = new List<IParameters>();

        private readonly float[,] Gx = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };

        private readonly float[,] Gy = { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };

        static SobelFilter()
        {
            parameters.Add(
                new ParametersEnum(
                    "Compute for",
                    0,
                    new[]
                        {
                            SobelOuptput.GradientMagnitude.ToString(), SobelOuptput.Gx.ToString(),
                            SobelOuptput.Gy.ToString()
                        },
                    DisplayType.listBox));
        }

        public static List<IParameters> getParametersList()
        {
            return parameters;
        }

        public SobelFilter(int output)
        {
            this.outputType = (SobelOuptput)output;
        }

        public ImageDependencies getImageDependencies()
        {
            return new ImageDependencies();
        }

        public ProcessingImage filter(ProcessingImage inputImage)
        {
            if (!inputImage.grayscale)
            {
                return null;
            }
            else
            {
                ProcessingImage outputImage = new ProcessingImage();
                outputImage.copyAttributesAndAlpha(inputImage);
                outputImage.addWatermark("Sobel Filter - sayuri.programmer.girl");
         
                if (this.outputType == SobelOuptput.GradientMagnitude)
                {
                    outputImage.setGray(this.GetGradientMagnitudeForChannel(inputImage.getGray()));
                }
                if (this.outputType == SobelOuptput.Gx)
                {
                    var differentialX = this.GetDifferentialX(inputImage.getGray());
                    var truncateToDisplay = ProcessingImageUtils.truncateToDisplay(differentialX);
                    outputImage.setGray(truncateToDisplay);
                }
                if (this.outputType == SobelOuptput.Gy)
                {
                    var differentialY = this.GetDifferentialY(inputImage.getGray());
                    outputImage.setGray(ProcessingImageUtils.truncateToDisplay(differentialY));
                }

                return outputImage;
            }
        }


        private float[,] GetDifferentialX(byte[,] channel)
        {
            var function = new ConvolutionFunction(this.Gx);
            return function.Compute(channel);
        }

        private float[,] GetDifferentialY(byte[,] channel)
        {
            var function = new ConvolutionFunction(this.Gy);
            return function.Compute(channel);
        }

        private byte[,] GetGradientMagnitudeForChannel(byte[,] channel)
        {
            float[,] difX = this.GetDifferentialX(channel);
            float[,] difY = this.GetDifferentialY(channel);

            int sizeX = difX.GetLength(0);
            int sizeY = difX.GetLength(1);

            byte[,] magnitude = new byte[sizeX, sizeY];

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    var sumOfSquares = Math.Pow(difX[i, j], 2) + Math.Pow(difY[i, j],2);
                    magnitude[i, j]  = (byte)Math.Sqrt(sumOfSquares);
                }
            }

            return magnitude;
        }
    }
}