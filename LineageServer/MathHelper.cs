namespace System
{
    static class MathHelper
    {
        public static int ParseInt(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            int.TryParse(str, out int i);
            return i;
        }
        public static bool Includes(this int i, int min, int max)
        {
            if (i < min)
            {
                return false;
            }

            if (i > max)
            {
                return false;
            }

            return true;
        }
        public static int Ensure(this int i, int min, int max)
        {
            if (i < min)
            {
                return min;
            }

            if (i > max)
            {
                return max;
            }

            return i;
        }

        private static Random randomInstance = null;

        public static double NextDouble
        {
            get
            {
                if (randomInstance == null)
                    randomInstance = new Random();

                return randomInstance.NextDouble();
            }
        }

        public static double Expm1(double x)
        {
            if (Math.Abs(x) < 1e-5)
                return x + 0.5 * x * x;
            else
                return Math.Exp(x) - 1.0;
        }

        public static double Log1p(double x)
        {
            double y = x;
            return ((1 + y) == 1) ? y : y * (Math.Log(1 + y) / ((1 + y) - 1));
        }
    }
}