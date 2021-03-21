using LineageServer.Interfaces;
using System;
using System.IO;
using System.Text;

namespace LineageServer.Server.Server.serverpackets
{
    abstract class ServerBasePacket
    {
        private static ILogger _log = Logger.getLogger(nameof(ServerBasePacket));

        private static readonly string CLIENT_LANGUAGE_CODE = Config.CLIENT_LANGUAGE_CODE;

        internal MemoryStream _bao = new MemoryStream();

        protected internal virtual void writeD(int value)
        {
            _bao.WriteByte((byte)(value & 0xff));
            _bao.WriteByte((byte)(value >> 8 & 0xff));
            _bao.WriteByte((byte)(value >> 16 & 0xff));
            _bao.WriteByte((byte)(value >> 24 & 0xff));
        }

        protected internal virtual void writeH(int value)
        {
            _bao.WriteByte((byte)(value & 0xff));
            _bao.WriteByte((byte)(value >> 8 & 0xff));
        }

        protected internal virtual void writeC(int value)
        {
            //this._bao.WriteByte(BitConverter.GetBytes(value)[0]);
            _bao.WriteByte((byte)(value & 0xff));
        }

        protected internal virtual void writeP(int value)
        {
            //this._bao.WriteByte(BitConverter.GetBytes(value)[0]);
            _bao.WriteByte((byte)value);
        }

        protected internal virtual void writeL(long value)
        {
            //this._bao.WriteByte(BitConverter.GetBytes(value)[0]);
            _bao.WriteByte((byte)(value & 0xff));
        }
        protected internal virtual void writeExp(in long value)
        {
            //this._bao.Write(BitConverter.GetBytes(value));
            this._bao.WriteByte((byte)(value & 0xff));
            this._bao.WriteByte((byte)(value >> 8 & 0xff));
            this._bao.WriteByte((byte)(value >> 16 & 0xff));
            this._bao.WriteByte((byte)(value >> 24 & 0xff));
        }
        protected internal virtual void writeF(double org)
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

        protected internal virtual void writeS(string text)
        {
            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    _bao.Write(Encoding.GetEncoding(CLIENT_LANGUAGE_CODE).GetBytes(text));
                }
            }
            catch (Exception e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }

            _bao.WriteByte(0);
        }

        protected internal virtual void writeByte(sbyte[] text)
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
            catch (Exception e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
        }

        public byte[] BuildBuffer()
        {
            byte padding = (byte)(_bao.Length % 4);

            if (padding != 0)
            {
                for (byte i = padding; i < 4; i++)
                {
                    writeC(0x00);
                }
            }
            byte[] buffer = _bao.ToArray();
            _bao.Close();
            return buffer;
        }

        /// <summary>
        /// サーバーパケットの種類を表す文字列を返す。("[S] S_WhoAmount" 等)
        /// </summary>
        public virtual string Type
        {
            get
            {
                return $"[S] {GetType().Name}";
            }
        }
    }

}