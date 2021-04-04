using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_ShowSummonList : ServerBasePacket
    {
        public S_ShowSummonList(int objid)
        {
            WriteC(Opcodes.S_OPCODE_SHOWHTML);
            WriteD(objid);
            WriteS("summonlist");
        }
        public override string Type
        {
            get
            {
                return "[S] S_ShowSummonList";
            }
        }
    }

}