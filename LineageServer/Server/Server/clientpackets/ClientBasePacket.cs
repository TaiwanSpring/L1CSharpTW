using LineageServer.Server.Server.serverpackets;
using System;

namespace LineageServer.Server.Server.Clientpackets
{
    abstract class ClientBasePacket
    {
        private static readonly string CLIENT_LANGUAGE_CODE = Config.CLIENT_LANGUAGE_CODE;

        private sbyte[] _decrypt;

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

        public ClientBasePacket(sbyte[] abyte0)
        {
            _decrypt = abyte0;
            _off = 1;
        }
        public ClientBasePacket(sbyte[] abyte0, ClientThread clientthread)
        {
        }
        public ClientBasePacket(ByteBuffer bytebuffer, ClientThread clientthread)
        {
        }

        public virtual int readD()
        {
            int i = _decrypt[_off++] & 0xff;
            i |= _decrypt[_off++] << 8 & 0xff00;
            i |= _decrypt[_off++] << 16 & 0xff0000;
            i |= unchecked((int)(_decrypt[_off++] << 24 & 0xff000000));
            return i;
        }

        public virtual byte readC()
        {
            byte i = (byte)(_decrypt[_off++] & 0xff);
            return i;
        }

        public virtual int readH()
        {
            int i = _decrypt[_off++] & 0xff;
            i |= _decrypt[_off++] << 8 & 0xff00;
            return i;
        }

        public virtual int readCH()
        {
            int i = _decrypt[_off++] & 0xff;
            i |= _decrypt[_off++] << 8 & 0xff00;
            i |= _decrypt[_off++] << 16 & 0xff0000;
            return i;
        }

        public virtual double readF()
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

        public virtual string readS()
        {
            string s = null;
            try
            {
                s = StringHelper.NewString(_decrypt, _off, _decrypt.Length - _off, CLIENT_LANGUAGE_CODE);
                s = s.Substring(0, s.IndexOf('\0'));
                _off += s.GetBytes(CLIENT_LANGUAGE_CODE).Length + 1;
            }
            catch (Exception e)
            {
                //_log.log(Enum.Level.Server, "OpCode=" + (_decrypt[0] & 0xff), e);
                throw;
            }
            return s;
        }

        public virtual sbyte[] readByte()
        {
            sbyte[] result = new sbyte[_decrypt.Length - _off];
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