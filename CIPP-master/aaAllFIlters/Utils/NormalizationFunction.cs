namespace aaAllFIlters.Utils
{
    public class NormalizationFunction
    {
        private readonly byte newMin;

        private readonly byte newMax;

        public NormalizationFunction(byte newMin, byte newMax)
        {
            this.newMin = newMin;
            this.newMax = newMax;
        }

        public byte[,] NormalizeChannel(byte[,] channel)
        {
            byte min = this.GetMinimum(channel);
            byte max = this.GetMaximum(channel);

            var lines = channel.GetLength(0);
            var columns = channel.GetLength(1);

            byte[,] result = new byte[lines, columns];
            
            for (int i = 0; i < lines; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[i, j] = (byte)this.GetNormalizedValue(channel[i,j], min, max);
                }
            }

            return result;
        }

        private float GetNormalizedValue(byte currentValue, byte min, byte max)
        {
            int newValueRange = this.newMax - this.newMin;
            int oldValueRange = max - min;

            int oldValueZeroBased = currentValue - min;
            float newNumberZeroBased = ((float)oldValueZeroBased * newValueRange) / (float)oldValueRange;

            float newNumber = newNumberZeroBased + this.newMin;

            return newNumber;
        }

        private byte GetMaximum(byte[,] channel)
        {
            byte max = byte.MinValue;

            for (int i = 0; i < channel.GetLength(0); i++)
            {
                for (int j = 0; j < channel.GetLength(1); j++)
                {
                    if (max < channel[i, j]) max = channel[i, j];
                }
            }

            return max;
        }

        private byte GetMinimum(byte[,] channel)
        {
            byte min = byte.MaxValue;

            for (int i = 0; i < channel.GetLength(0); i++)
            {
                for (int j = 0; j < channel.GetLength(1); j++)
                {
                    if (min > channel[i, j]) min = channel[i, j];
                }
            }

            return min;
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