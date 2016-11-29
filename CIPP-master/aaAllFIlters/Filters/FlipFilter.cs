namespace aaAllFIlters.Filters
{
    using System.Collections.Generic;

    using ParametersSDK;

    using Plugins.Filters;

    using ProcessingImageSDK;

    public class FlipFilter : IFilter
    {
        private readonly FlipType flipType;

        private static readonly List<IParameters> parameters = new List<IParameters>();

        static FlipFilter()
        {
            parameters.Add(new ParametersEnum("Flip Type", 0, new[] { FlipType.Vertical.ToString(), FlipType.Horizontal.ToString() }, DisplayType.listBox));
        }

        public static List<IParameters> getParametersList()
        {
            return parameters;
        }

        public FlipFilter(int flipType)
        {
            this.flipType = (FlipType)flipType;
        }

        public ImageDependencies getImageDependencies()
        {
            return new ImageDependencies();
        }

        public ProcessingImage filter(ProcessingImage inputImage)
        {
            ProcessingImage outputImage = new ProcessingImage();
            outputImage.copyAttributesAndAlpha(inputImage);
            outputImage.addWatermark("Flip Filter - sayuri.programmer.girl");

            if (!inputImage.grayscale)
            {
                outputImage.setRed(this.FlipChannel(inputImage.getRed()));
                outputImage.setGreen(this.FlipChannel(inputImage.getGreen()));
                outputImage.setBlue(this.FlipChannel(inputImage.getBlue()));
                outputImage.setAlpha(this.FlipChannel(inputImage.getAlpha()));
            }
            else
            {
                outputImage.setGray(this.FlipChannel(inputImage.getGray()));
                outputImage.setAlpha(this.FlipChannel(inputImage.getAlpha()));
            }

            return outputImage;
        }


        public byte[,] FlipChannel(byte[,] channel)
        {
            var lines = channel.GetLength(0);
            var columns = channel.GetLength(1);

            byte[,] result = new byte[lines, columns];

            for (int i = 0; i < lines; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (this.flipType == FlipType.Vertical)
                    {
                        result[i, j] = channel[lines - 1 - i, j];
                    }
                    else if (this.flipType == FlipType.Horizontal)
                    {
                        result[i, j] = channel[i, columns - 1 - j];
                    }
                }
            }

            return result;
        }
    }
}