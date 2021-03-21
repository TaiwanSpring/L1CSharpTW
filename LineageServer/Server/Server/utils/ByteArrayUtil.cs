using System.Text;

namespace LineageServer.Server.Server.utils
{
    // 全部staticにしてもいいかもしれない
    public class ByteArrayUtil
    {
        private readonly byte[] _byteArray;

        public ByteArrayUtil(byte[] byteArray)
        {
            _byteArray = byteArray;
        }

        public virtual string getTerminatedString(int i)
        {
            StringBuilder stringbuffer = new StringBuilder();
            for (int j = i; j < _byteArray.Length && _byteArray[j] != 0; j++)
            {
                stringbuffer.Append((char)_byteArray[j]);
            }

            return stringbuffer.ToString();
        }

        //public virtual string dumpToString()
        //{
        //    StringBuilder stringbuffer = new StringBuilder();
        //    int j = 0;
        //    for (int k = 0; k < _byteArray.Length; k++)
        //    {
        //        if (j % 16 == 0)
        //        {
        //            stringbuffer.Append((new StringBuilder()).Append(fillHex(k, 4)).Append(": ").ToString());
        //        }
        //        stringbuffer.Append((new StringBuilder()).Append(fillHex(_byteArray[k] & 0xff, 2)).Append(" ").ToString());
        //        if (++j != 16)
        //        {
        //            continue;
        //        }
        //        stringbuffer.Append("   ");
        //        int i1 = k - 15;
        //        for (int l1 = 0; l1 < 16; l1++)
        //        {
        //            sbyte byte0 = (sbyte)_byteArray[i1++];
        //            if (byte0 > 31 && byte0 < unchecked((sbyte)128))
        //            {
        //                stringbuffer.Append((char)byte0);
        //            }
        //            else
        //            {
        //                stringbuffer.Append('.');
        //            }
        //        }

        //        stringbuffer.Append("\n");
        //        j = 0;
        //    }

        //    int l = _byteArray.Length % 16;
        //    if (l > 0)
        //    {
        //        for (int j1 = 0; j1 < 17 - l; j1++)
        //        {
        //            stringbuffer.Append("   ");
        //        }

        //        int k1 = _byteArray.Length - l;
        //        for (int i2 = 0; i2 < l; i2++)
        //        {
        //            sbyte byte1 = _byteArray[k1++];
        //            if (byte1 > 31 && byte1 < unchecked((sbyte)128))
        //            {
        //                stringbuffer.Append((char)byte1);
        //            }
        //            else
        //            {
        //                stringbuffer.Append('.');
        //            }
        //        }

        //        stringbuffer.Append("\n");
        //    }
        //    return stringbuffer.ToString();
        //}

        private string fillHex(int i, int j)
        {
            string s = i.ToString("x");
            for (int k = s.Length; k < j; k++)
            {
                s = (new StringBuilder()).Append("0").Append(s).ToString();
            }

            return s;
        }
    }

}