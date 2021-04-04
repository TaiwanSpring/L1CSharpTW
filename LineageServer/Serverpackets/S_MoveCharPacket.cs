using LineageServer.Server;
using LineageServer.Server.Model;
using LineageServer.Utils;

namespace LineageServer.Serverpackets
{
    class S_MoveCharPacket : ServerBasePacket
    {

        private const string _S__1F_MOVECHARPACKET = "[S] S_MoveCharPacket";

        private byte[] _byte = null;

        public S_MoveCharPacket(L1Character cha)
        {
            int heading = cha.Heading;
            int x = cha.X - MoveUtil.MoveX(heading);
            int y = cha.Y - MoveUtil.MoveY(heading);

            WriteC(Opcodes.S_OPCODE_MOVEOBJECT);
            WriteD(cha.Id);
            WriteH(x);
            WriteH(y);
            WriteC(heading);
            WriteC(129);
            WriteD(0);
        }
        public override string Type
        {
            get
            {
                return _S__1F_MOVECHARPACKET;
            }
        }
    }
}