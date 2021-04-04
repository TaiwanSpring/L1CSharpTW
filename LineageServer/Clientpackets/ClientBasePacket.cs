using LineageServer.Server;
using LineageServer.Serverpackets;
using System;

namespace LineageServer.Clientpackets
{
    abstract class ClientBasePacket
    {
        private static readonly string CLIENT_LANGUAGE_CODE = Config.CLIENT_LANGUAGE_CODE;

        private byte[] _decrypt;

        private int _off;

        /// <summary>
        /// 返回客戶端的封包類型。
        /// </summary>
        public virtual string Type
        {
            get
            {
                return $"[C] {this.GetType().Name}";
            }
        }

        public ClientBasePacket(byte[] abyte0)
        {
            _decrypt = abyte0;
            _off = 1;
        }
        public ClientBasePacket(byte[] abyte0, ClientThread clientthread)
        {
        }
        //public ClientBasePacket(ByteBuffer bytebuffer, ClientThread clientthread)
        //{
        //}

        public virtual int ReadD()
        {
            int i = _decrypt[_off++] & 0xff;
            i |= _decrypt[_off++] << 8 & 0xff00;
            i |= _decrypt[_off++] << 16 & 0xff0000;
            i |= unchecked((int)(_decrypt[_off++] << 24 & 0xff000000));
            return i;
        }

        public virtual byte ReadC()
        {
            byte i = (byte)(_decrypt[_off++] & 0xff);
            return i;
        }

        public virtual int ReadH()
        {
            int i = _decrypt[_off++] & 0xff;
            i |= (_decrypt[_off++] << 8) & 0xff00;
            return i;
        }

        public virtual int ReadCH()
        {
            int i = _decrypt[_off++] & 0xff;
            i |= (_decrypt[_off++] << 8) & 0xff00;
            i |= (_decrypt[_off++] << 16) & 0xff0000;
            return i;
        }

        public virtual double ReadF()
        {
            byte[] longBuffer = new byte[8];

            Buffer.BlockCopy(_decrypt, _off, longBuffer, 0, longBuffer.Length);
            _off += 8;
            return BitConverter.ToDouble(longBuffer);
            //long l = _decrypt[_off++] & 0xff;
            //l |= _decrypt[_off++] << 8 & 0xff00;
            //l |= _decrypt[_off++] << 16 & 0xff0000;
            //l |= _decrypt[_off++] << 24 & 0xff000000;
            //l |= (long)_decrypt[_off++] << 32 & 0xff00000000L;
            //l |= (long)_decrypt[_off++] << 40 & 0xff0000000000L;
            //l |= (long)_decrypt[_off++] << 48 & 0xff000000000000L;
            //l |= (long)_decrypt[_off++] << 56 & 0xff00000000000000L;
            //return BitConverter.Int64BitsToDouble(l);
        }

        public virtual string ReadS()
        {
            string s;
            try
            {
                s = GobalParameters.Encoding.GetString(_decrypt, _off, _decrypt.Length - _off);
                //s = StringHelper.NewString(_decrypt, _off, _decrypt.Length - _off, CLIENT_LANGUAGE_CODE);
                int index = s.IndexOf('\0');
                s = s.Substring(0, index);
                //_off += _decrypt.Length - _off + 1;
                _off += index + 1;
            }
            catch (Exception e)
            {
                //_log.log(Enum.Level.Server, "OpCode=" + (_decrypt[0] & 0xff), e);
                throw;
            }
            return s;
        }

        public virtual byte[] ReadByte()
        {
            byte[] result = new byte[_decrypt.Length - _off];
            try
            {
                Array.Copy(_decrypt, _off, result, 0, _decrypt.Length - _off);
                _off = _decrypt.Length;
            }
            catch (Exception e)
            {
                //_log.log(Enum.Level.Server, "OpCode=" + (_decrypt[0] & 0xff), e);
                throw;
            }
            return result;
        }
    }

}