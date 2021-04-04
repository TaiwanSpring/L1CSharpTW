using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_SendLocation : ServerBasePacket
    {

        private const string S_SEND_LOCATION = "[S] S_SendLocation";

        public S_SendLocation(int type, string senderName, int mapId, int x, int y, int msgId)
        {
            WriteC(Opcodes.S_OPCODE_PACKETBOX);
            WriteC(0x6f);
            WriteS(senderName);
            WriteH(mapId);
            WriteH(x);
            WriteH(y);
            WriteC(msgId); // 發信者位在的地圖ID
        }

        public override string Type
        {
            get
            {
                return S_SEND_LOCATION;
            }
        }
    }

}