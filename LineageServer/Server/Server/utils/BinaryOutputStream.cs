using System;
using System.IO;
namespace LineageServer.Server.Server.utils
{
    public class BinaryOutputStream
    {
        private static readonly string CLIENT_LANGUAGE_CODE = Config.CLIENT_LANGUAGE_CODE;

        private readonly MemoryStream _bao = new MemoryStream();

        public void write(int b)
        {
            _bao.WriteByte((byte)b);
        }

        public virtual void writeD(int value)
        {
            _bao.WriteByte((byte)(value & 0xff));
            _bao.WriteByte((byte)(value >> 8 & 0xff));
            _bao.WriteByte((byte)(value >> 16 & 0xff));
            _bao.WriteByte((byte)(value >> 24 & 0xff));
        }

        public virtual void writeH(int value)
        {
            _bao.WriteByte((byte)(value & 0xff));
            _bao.WriteByte((byte)(value >> 8 & 0xff));
        }

        public virtual void writeC(int value)
        {
            _bao.WriteByte((byte)(value & 0xff));
        }

        public virtual void writeP(int value)
        {
            _bao.WriteByte((byte)value);
        }

        public virtual void writeL(long value)
        {
            _bao.WriteByte((byte)(value & 0xff));
        }

        public virtual void writeF(double org)
        {
            long value = BitConverter.DoubleToInt64Bits(org);
            _bao.WriteByte((byte)(value & 0xff));
            _bao.WriteByte((byte)(value >> 8 & 0xff));
            _bao.WriteByte((byte)(value >> 16 & 0xff));
            _bao.WriteByte((byte)(value >> 24 & 0xff));
            _bao.WriteByte((byte)(value >> 32 & 0xff));
            _bao.WriteByte((byte)(value >> 40 & 0xff));
            _bao.WriteByte((byte)(value >> 48 & 0xff));
            _bao.WriteByte((byte)(value >> 56 & 0xff));
        }

        public virtual void writeS(string text)
        {
            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    _bao.Write(GobalParameters.Encoding.GetBytes(text));
                }
            }
            catch (Exception)
            {
            }

            _bao.WriteByte(0);
        }

        public virtual void writeByte(sbyte[] text)
        {
            try
            {
                if (text != null)
                {
                    byte[] buffer = new byte[text.Length];
                    Buffer.BlockCopy(text, 0, buffer, 0, text.Length);
                    _bao.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception)
            {
            }
        }

        public virtual int Length
        {
            get
            {
                return (int)_bao.Length + 2;
            }
        }

        public virtual byte[] Bytes
        {
            get
            {
                byte[] buffer = _bao.ToArray();
                _bao.Close();
                return buffer;
            }
        }
    }

}