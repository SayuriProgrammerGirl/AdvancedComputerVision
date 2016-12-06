namespace aaAllFIlters.Filters
{
    using System;
    using System.Collections.Generic;

    using ParametersSDK;

    using Plugins.Filters;

    using ProcessingImageSDK;

    public class CannyFilter : IFilter
    {
        private readonly int size;

        private readonly float sigma;

        private static List<IParameters> parameters = new List<IParameters>();

        static CannyFilter()
        {
            parameters.Add(new ParametersInt32(0, 10, 5, "Blur kernel size", DisplayType.textBox));
            parameters.Add(new ParametersFloat(0, 10, (float)1.4, "Sigma", DisplayType.textBox));
        }
        public static List<IParameters> getParametersList()
        {
            return parameters;
        }

        public ImageDependencies getImageDependencies()
        {
            return new ImageDependencies();
        }

        public CannyFilter(int size, float sigma)
        {
            this.size = size;
            this.sigma = sigma;
        }

        public ProcessingImage filter(ProcessingImage inputImage)
        {
            ProcessingImage outputImage = new ProcessingImage();
            outputImage.copyAttributesAndAlpha(inputImage);

            var func = new ConvolutionFunction(this.GetBlurKernel());
            var bluredAlpha = func.ComputeLossy(inputImage.getAlpha());
            var bluredGrey = func.ComputeLossy(inputImage.getGray());

            return inputImage;
        }

        public float[,] GetBlurKernel()
        {
            int k = this.size / 2;
            float[,] H = new float[this.size, this.size];

            float coef = (float)(1 / (2 * Math.PI * Math.Pow(this.sigma, 2)));
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    float ipart = (float)Math.Pow(i - (k + 1), 2);
                    float jpart = (float)Math.Pow(j - (k + 1), 2);
                    float exponent = (float)(-(ipart + jpart) / (2 * Math.Pow(this.sigma, 2)));
                    H[i, j] = (float)(coef * Math.Exp(exponent));
                }
            }

            return H;
        }
    }
}