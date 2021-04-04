using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_MapID : ServerBasePacket
    {

        public S_MapID(int mapid, bool isUnderwater)
        {
            WriteC(Opcodes.S_OPCODE_MAPID);
            WriteH(mapid);
            WriteC(isUnderwater ? 1 : 0); // 水底:1
            WriteD(0);
            WriteD(0);
            WriteD(0);
        }
    }

}