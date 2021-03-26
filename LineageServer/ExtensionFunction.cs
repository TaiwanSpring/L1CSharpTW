namespace System
{
    static class ExtensionFunction
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
    }
}