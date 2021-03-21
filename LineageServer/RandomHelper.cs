using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer
{
    class RandomHelper
    {
        static Random random = new Random();
        public static int Next(int next)
        {
            return RandomHelper.Next(next);
        }
        public static int Next(int min, int max)
        {
            return RandomHelper.Next(min, max);
        }
        public static double NextDouble()
        {
            return RandomHelper.NextDouble();
        }
    }
}
