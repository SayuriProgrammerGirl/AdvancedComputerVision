namespace aaAllFIlters.Filters
{
    using System.Collections.Generic;

    using aaAllFIlters.Utils;

    using ParametersSDK;

    using Plugins.Filters;

    using ProcessingImageSDK;

    public class HistogramStretchFilter : IFilter
    {
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
            outputImage.addWatermark("Histogram Stretch Filter - sayuri.programmer.girl");

            var function = new NormalizationFunction(0, 255);
            if (!inputImage.grayscale)
            {
                outputImage.setRed(function.NormalizeChannel(inputImage.getRed()));
                outputImage.setGreen(function.NormalizeChannel(inputImage.getGreen()));
                outputImage.setBlue(function.NormalizeChannel(inputImage.getBlue()));
            }
            else
            {
                outputImage.setGray(function.NormalizeChannel(inputImage.getGray()));
            }

            return outputImage;
        }
    }
}