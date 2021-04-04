using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_Door : ServerBasePacket
    {
        private const string S_DOOR = "[S] S_Door";
        private byte[] _byte = null;

        private const int PASS = 0;
        private const int NOT_PASS = 1;

        public S_Door(int x, int y, int direction, bool isPassable)
        {
            buildPacket(x, y, direction, isPassable);
        }

        private void buildPacket(int x, int y, int direction, bool isPassable)
        {
            WriteC(Opcodes.S_OPCODE_ATTRIBUTE);
            WriteH(x);
            WriteH(y);
            WriteC(direction); // ドアの方向 0: ／ 1: ＼
            WriteC(isPassable ? PASS : NOT_PASS);
        }
        public override string Type
        {
            get
            {
                return S_DOOR;
            }
        }
    }

}