using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_ShowPolyList : ServerBasePacket
    {
        public S_ShowPolyList(int objid)
        {
            WriteC(Opcodes.S_OPCODE_SHOWHTML);
            WriteD(objid);
            WriteS("monlist");
        }
    }

}