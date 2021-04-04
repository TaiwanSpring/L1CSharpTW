using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_Drawal : ServerBasePacket
    {

        public S_Drawal(int objectId, int count)
        {
            WriteC(Opcodes.S_OPCODE_DRAWAL);
            WriteD(objectId);
            WriteD(count);
        }
        public override string Type
        {
            get
            {
                return "[S] S_Drawal";
            }
        }
    }

}