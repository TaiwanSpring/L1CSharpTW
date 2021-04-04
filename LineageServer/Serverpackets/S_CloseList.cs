using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_CloseList : ServerBasePacket
    {
        public S_CloseList(int objid)
        {
            WriteC(Opcodes.S_OPCODE_SHOWHTML);
            WriteD(objid);
            WriteS("");
        }
    }

}