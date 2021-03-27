using System;

namespace LineageServer.Utils
{
    class RandomHelper
    {
        static Random random = new Random();
        public static int Next(int next)
        {
            return random.Next(next + 1);
        }
        public static int Next(int min, int max)
        {
            return random.Next(min, max + 1);
        }
        public static byte NextByte()
        {
            return (byte)random.Next(byte.MinValue, byte.MaxValue + 1);
        }
        public static double NextDouble()
        {
            return random.NextDouble();
        }
    }
}
