namespace System.Extensions
{
    static class IntExtensions
    {
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
    }
}