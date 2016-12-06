namespace aaAllFIlters.Filters
{
    using System;
    using System.Collections.Generic;

    using ParametersSDK;

    using Plugins.Filters;

    using ProcessingImageSDK;

    public class BlurCustomConvolutionFilter : IFilter
    {
        private readonly double[,] fraction = {
                                           {(double)1/9,(double)1/9,(double)1/9},
                                           {(double)1/9,(double)1/9,(double)1/9},
                                           {(double)1/9,(double)1/9,(double)1/9}
                                       };


        public static List<IParameters> getParametersList()
        {
            return new List<IParameters>();
        }

        public ImageDependencies getImageDependencies()
        {
            return new ImageDependencies();
        }

        public ProcessingImage filter(ProcessingImage inputImage)
        {
            ProcessingImage outputImage = new ProcessingImage();
            outputImage.copyAttributesAndAlpha(inputImage);

            if (!inputImage.grayscale)
            {
                outputImage.setRed(this.BlurChannel(inputImage.getRed()));
                outputImage.setGreen(this.BlurChannel(inputImage.getGreen()));
                outputImage.setBlue(this.BlurChannel(inputImage.getBlue()));
                outputImage.setAlpha(this.BlurChannel(inputImage.getAlpha()));
            }
            else
            {
                outputImage.setGray(this.BlurChannel(inputImage.getGray()));
                outputImage.setAlpha(this.BlurChannel(inputImage.getAlpha()));
            }

            outputImage.addWatermark("Blur Custom Convolution Filter - sayuri.programmer.girl");
            return outputImage;
        }

        private byte[,] BlurChannel(byte[,] channel)
        {
            var lines = channel.GetLength(0);
            var columns = channel.GetLength(1);

            byte[,] result = new byte[lines, columns];
            for (int rr = 0; rr < lines; rr++)
            {
                for (int cc = 0; cc < columns; cc++)
                {
                    result[rr, cc] = this.BlurPixel(channel, rr, cc);
                }
            }
            return result;
        }

        private byte BlurPixel(byte[,] channel, int pixelLine, int pixelColumn)
        {
            double bluredPixelValue = 0;

            int lines = this.fraction.GetLength(0);
            int columns = this.fraction.GetLength(1);

            for (int i = 0; i < lines; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var x = Math.Abs(pixelLine - i);
                    var y = Math.Abs(pixelColumn - j);

                    bluredPixelValue += this.fraction[i, j] * channel[x, y];
                }
            }

            return (byte)bluredPixelValue;
        }
    }
}