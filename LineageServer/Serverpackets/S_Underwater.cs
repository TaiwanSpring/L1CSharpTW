using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_Underwater : ServerBasePacket
    {

        public S_Underwater(int playerobjecId, int type)
        {
            WriteC(Opcodes.S_OPCODE_UNDERWATER);
            WriteD(playerobjecId);
            WriteC(type);
        }
        public override string Type
        {
            get
            {
                return "[S] S_Underwater";
            }
        }
    }

}