using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_Disconnect : ServerBasePacket
    {
        public S_Disconnect()
        {
            int content = 500;
            WriteC(Opcodes.S_OPCODE_DISCONNECT);
            WriteH(content);
            WriteD(0x00000000);
        }

        /// <summary>
        /// 0~21, 連線中斷 22, 有人以同樣的帳號登入，請注意，您的密碼可能已經外洩
        /// </summary>
        public S_Disconnect(int id)
        {
            WriteC(Opcodes.S_OPCODE_DISCONNECT);
            WriteC(id);
            WriteD(0x00000000);
        }
    }

}