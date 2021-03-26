using LineageServer.Interfaces;
using System;
using System.IO;
using System.Text;

namespace LineageServer.Server.Server.serverpackets
{
    abstract class ServerBasePacket
    {
        protected static ILogger _log = Logger.getLogger(nameof(ServerBasePacket));

        private static readonly string CLIENT_LANGUAGE_CODE = Config.CLIENT_LANGUAGE_CODE;

        internal MemoryStream memoryStream = new MemoryStream();

        protected internal virtual void WriteD(int value)
        {
            this.memoryStream.WriteByte((byte)(value & 0xff));
            this.memoryStream.WriteByte((byte)(value >> 8 & 0xff));
            this.memoryStream.WriteByte((byte)(value >> 16 & 0xff));
            this.memoryStream.WriteByte((byte)(value >> 24 & 0xff));
        }

        protected internal virtual void WriteH(int value)
        {
            this.memoryStream.WriteByte((byte)(value & 0xff));
            this.memoryStream.WriteByte((byte)(value >> 8 & 0xff));
        }

        protected internal virtual void WriteC(int value)
        {
            //this._bao.WriteByte(BitConverter.GetBytes(value)[0]);
            this.memoryStream.WriteByte((byte)(value & 0xff));
        }

        protected internal virtual void WriteP(int value)
        {
            //this._bao.WriteByte(BitConverter.GetBytes(value)[0]);
            this.memoryStream.WriteByte((byte)value);
        }

        protected internal virtual void WriteL(long value)
        {
            //this._bao.WriteByte(BitConverter.GetBytes(value)[0]);
            this.memoryStream.WriteByte((byte)(value & 0xff));
        }
        protected internal virtual void WriteExp(in long value)
        {
            //this._bao.Write(BitConverter.GetBytes(value));
            this.memoryStream.WriteByte((byte)(value & 0xff));
            this.memoryStream.WriteByte((byte)(value >> 8 & 0xff));
            this.memoryStream.WriteByte((byte)(value >> 16 & 0xff));
            this.memoryStream.WriteByte((byte)(value >> 24 & 0xff));
        }
        protected internal virtual void WriteF(double org)
        {
            long value = BitConverter.DoubleToInt64Bits(org);
            this.memoryStream.WriteByte((byte)(value & 0xff));
            this.memoryStream.WriteByte((byte)(value >> 8 & 0xff));
            this.memoryStream.WriteByte((byte)(value >> 16 & 0xff));
            this.memoryStream.WriteByte((byte)(value >> 24 & 0xff));
            this.memoryStream.WriteByte((byte)(value >> 32 & 0xff));
            this.memoryStream.WriteByte((byte)(value >> 40 & 0xff));
            this.memoryStream.WriteByte((byte)(value >> 48 & 0xff));
            this.memoryStream.WriteByte((byte)(value >> 56 & 0xff));
        }

        protected internal virtual void WriteS(string text)
        {
            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    this.memoryStream.Write(Encoding.GetEncoding(CLIENT_LANGUAGE_CODE).GetBytes(text));
                }
            }
            catch (Exception e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }

            this.memoryStream.WriteByte(0);
        }

        protected internal virtual void WriteByte(byte[] buffer)
        {
            this.memoryStream.Write(buffer, 0, buffer.Length);
        }

        public byte[] BuildBuffer()
        {
            byte padding = (byte)(this.memoryStream.Length % 4);

            if (padding != 0)
            {
                for (byte i = padding; i < 4; i++)
                {
                    WriteC(0x00);
                }
            }
            byte[] buffer = this.memoryStream.ToArray();
            this.memoryStream.Close();
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