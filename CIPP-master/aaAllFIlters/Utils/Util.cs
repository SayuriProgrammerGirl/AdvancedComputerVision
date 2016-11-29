namespace aaAllFIlters.Filters
{
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
