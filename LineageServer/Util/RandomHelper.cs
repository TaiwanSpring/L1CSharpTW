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
        static bool _hasDeviate;
        static double _storedDeviate;
        public static double NextGaussian(double mu = 0, double sigma = 1)
        {
            if (sigma <= 0)
                throw new ArgumentOutOfRangeException("sigma", "Must be greater than zero.");

            if (_hasDeviate)
            {
                _hasDeviate = false;
                return _storedDeviate * sigma + mu;
            }

            double v1, v2, rSquared;
            do
            {
                // two random values between -1.0 and 1.0
                v1 = 2 * random.NextDouble() - 1;
                v2 = 2 * random.NextDouble() - 1;
                rSquared = v1 * v1 + v2 * v2;
                // ensure within the unit circle
            } while (rSquared >= 1 || rSquared == 0);

            // calculate polar tranformation for each deviate
            var polar = Math.Sqrt(-2 * Math.Log(rSquared) / rSquared);
            // store first deviate
            _storedDeviate = v2 * polar;
            _hasDeviate = true;
            // return second deviate
            return v1 * polar * sigma + mu;
        }
    }
}
