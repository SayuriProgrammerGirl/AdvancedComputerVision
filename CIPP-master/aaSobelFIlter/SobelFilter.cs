namespace aaSobelFIlter
{
    using System;
    using System.Collections.Generic;

    using ParametersSDK;

    using Plugins.Filters;

    using ProcessingImageSDK;

    public class SobelFilter : IFilter
    {
        private readonly int option;

        private static readonly List<IParameters> parameters = new List<IParameters>();

        private float[,] Gx =
            {
                { -1, 0, 1}, 
                { -2, 0, 2}, 
                { -1, 0, 1}
            };

        private float[,] Gy =
            {
                { 1, 2, 1 }, 
                { 0, 0, 0 }, 
                { -1, -2, -1 }
            };

        static SobelFilter()
        {
            parameters.Add(new ParametersEnum("Compute for", 0, new[] { "Gradient Magnitude", "Gx", "Gy" }, DisplayType.listBox));
        }

        public static List<IParameters> getParametersList()
        {
            return parameters;
        }

        public SobelFilter(int option)
        {
            this.option = option;
        }

        public ImageDependencies getImageDependencies()
        {
            return new ImageDependencies();
        }

        public ProcessingImage filter(ProcessingImage inputImage)
        {

            ProcessingImage dx = inputImage.convolution(Gx);
            ProcessingImage dy = inputImage.convolution(Gy);

            if (!inputImage.grayscale)
            {
                var rx = dx.getRed();
                var gx = dx.getGreen();
                var bx = dx.getBlue();

                var ry = dy.getRed();
                var gy = dy.getGreen();
                var by = dy.getBlue();

                float[,] r = new float[inputImage.getSizeY(), inputImage.getSizeX()];
                float[,] g = new float[inputImage.getSizeY(), inputImage.getSizeX()];
                float[,] b = new float[inputImage.getSizeY(), inputImage.getSizeX()];

                var sizeY = inputImage.getSizeY();
                var sizeX = inputImage.getSizeX();

                for (int i = 0; i < sizeY; i++)
                {
                    for (int j = 0; j < sizeX; j++)
                    {
                        r[i, j] = (float)Math.Atan((double)ry[i, j] / rx[i, j]) * 100;
                        g[i, j] = (float)Math.Atan((double)gy[i, j] / gx[i, j]) * 100;
                        b[i, j] = (float)Math.Atan((double)by[i, j] / bx[i, j]) * 100;
                    }

                }


                ProcessingImage outputImage = new ProcessingImage();
                outputImage.copyAttributesAndAlpha(inputImage);
                outputImage.addWatermark("Sobel Filter - sayuri.programmer.girl");
                outputImage.setRed(ProcessingImageUtils.truncateToDisplay(r));
                outputImage.setGreen(ProcessingImageUtils.truncateToDisplay(g));
                outputImage.setBlue(ProcessingImageUtils.truncateToDisplay(b));
                return outputImage;
            }
            else
            {
                var grayx = dx.getGray();

                var grayy = dy.getGray();

                float[,] gray = new float[inputImage.getSizeY(), inputImage.getSizeX()];

                var sizeY = inputImage.getSizeY();
                var sizeX = inputImage.getSizeX();

                for (int i = 0; i < sizeY; i++)
                {
                    for (int j = 0; j < sizeX; j++)
                    {
                        var atan = (float)Math.Atan((double)grayy[i, j] / grayx[i, j]);
                        var magnitude = Math.Sqrt(grayx[i, j] * grayx[i, j] + grayy[i, j] * grayy[i, j]);

                        grayx[i, j] += 100;
                        grayy[i, j] += 100;

                        gray[i, j] = (float)magnitude;
                    }
                }




                ProcessingImage outputImage = new ProcessingImage();
                outputImage.copyAttributesAndAlpha(inputImage);
                outputImage.addWatermark("Sobel Filter - sayuri.programmer.girl");
                outputImage.setGray(ProcessingImageUtils.truncateToDisplay(gray));

                var ggg = ProcessingImageUtils.truncateToDisplay(Util.Normalization(gray, 0, 255));
                var xxx = grayx;//ProcessingImageUtils.truncateToDisplay(Util.Stretch(grayx));
                var yyy = grayy;//ProcessingImageUtils.truncateToDisplay(Util.Stretch(grayy));

                if (option == 0)
                {
                    outputImage.setGray(ggg);
                }
                if (option == 1)
                {
                    outputImage.setGray(xxx);
                }
                if (option == 2)
                {
                    outputImage.setGray(yyy);
                }

                return outputImage;
            }
        }
    }

    public class Util
    {
        public static float[,] Normalization(byte[,] matrix, byte newMin, byte newMax)
        {
            var x = matrix.GetLength(0);
            var y = matrix.GetLength(1);
            float[,] newMatrix = new float[x, y];
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    newMatrix[i, j] = (float)matrix[i, j];
                }
            }
            var result = Normalization(newMatrix, newMin, newMax);

            return result;
        }

        public static float[,] Normalization(float[,] matrix, float newMin, float newMax)
        {
            float min = float.MaxValue;
            float max = float.MinValue;


            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (min > matrix[i, j]) min = matrix[i, j];
                    if (max < matrix[i, j]) max = matrix[i, j];
                }
            }

            float[,] result = new float[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i, j] = ((matrix[i, j] - min) * (newMax - newMin)) / (max - min) + newMin;
                }
            }

            return result;
        }
    }
}
