using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_PinkName : ServerBasePacket
    {

        public S_PinkName(int objecId, int time)
        {
            WriteC(Opcodes.S_OPCODE_PINKNAME);
            WriteD(objecId);
            WriteD(time);
        }

        public override string Type
        {
            get
            {
                return "[S] S_PinkName";
            }
        }
    }

}