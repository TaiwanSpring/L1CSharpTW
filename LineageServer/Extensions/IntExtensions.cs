namespace System.Extensions
{
    static class IntExtensions
    {
        public static byte GetHigh(this int i)
        {
            return (byte)((i >> 8) & 0xFF);
        }

        public static byte GetLow(this int i)
        {
            return (byte)(i & 0xFF);
        }
        public static int Signum(this int i)
        {
            if (i < 0)
            {
                return -1;
            }
            else if (i > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
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

        public static byte ToByte(this int i)
        {
            if (i < byte.MinValue)
            {
                return byte.MinValue;
            }

            if (i > byte.MaxValue)
            {
                return byte.MaxValue;
            }

            return (byte)i;
        }
    }
}