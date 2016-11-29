namespace aaAllFIlters.Filters
{
    using System.Collections.Generic;

    using ParametersSDK;

    using Plugins.Filters;

    using ProcessingImageSDK;

    public class BlurFilter : IFilter
    {
        private readonly float[,] H = {
                                    {(float)1/9,(float)1/9,(float)1/9},
                                    {(float)1/9,(float)1/9,(float)1/9},
                                    {(float)1/9,(float)1/9,(float)1/9}
                              };

        public static List<IParameters> getParametersList()
        {
            return new List<IParameters>(); ;
        }

        public ImageDependencies getImageDependencies()
        {
            return new ImageDependencies();
        }

        public ProcessingImage filter(ProcessingImage inputImage)
        {
            ProcessingImage outputImage = inputImage.convolution(this.H);

            float[,] alphaConvoluted = ProcessingImageUtils.delayedConvolution(inputImage.getAlpha(), this.H);
            byte[,] alphaConvolutedTruncated = ProcessingImageUtils.truncateToDisplay(alphaConvoluted);
            outputImage.setAlpha(alphaConvolutedTruncated);

            outputImage.addWatermark("Blur Filter - sayuri.programmer.girl");
            return outputImage;
        }
    }
}
