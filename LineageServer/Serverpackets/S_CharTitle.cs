using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_CharTitle : ServerBasePacket
    {

        public S_CharTitle(int objid, string title)
        {
            WriteC(Opcodes.S_OPCODE_CHARTITLE);
            WriteD(objid);
            WriteS(title);
        }

        public override string Type
        {
            get
            {
                return "[S] S_CharTitle";
            }
        }
    }

}