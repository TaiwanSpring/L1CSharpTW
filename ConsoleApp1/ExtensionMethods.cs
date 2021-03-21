using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public static class ExtensionMethods
    {
        public static byte GetHigh(this int i)
        {
            return (byte)((i >> 8) & 0xFF);
        }

        public static byte GetLow(this int i)
        {
            return (byte)(i & 0xFF);
        }
    }
}
