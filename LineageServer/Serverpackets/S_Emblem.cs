using LineageServer.Server;
using System;
using System.IO;
namespace LineageServer.Serverpackets
{
    class S_Emblem : ServerBasePacket
    {
        private const string S_EMBLEM = "[S] S_Emblem";

        public S_Emblem(int emblemId)
        {
            string emblem_file = emblemId.ToString();
            FileInfo file = new FileInfo("emblem/" + emblem_file);
            if (file.Exists)
            {
                WriteC(Opcodes.S_OPCODE_EMBLEM);
                WriteD(emblemId);
                FileStream stream = file.OpenRead();
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Close();
                stream.Dispose();
                WriteByte(buffer);
            }
        }

        public override string Type
        {
            get
            {
                return S_EMBLEM;
            }
        }
    }

}