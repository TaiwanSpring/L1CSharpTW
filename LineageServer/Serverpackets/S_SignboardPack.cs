using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_SignboardPack : ServerBasePacket
    {

        private const string S_SIGNBOARD_PACK = "[S] S_SignboardPack";

        private const int STATUS_POISON = 1;

        private byte[] _byte = null;

        public S_SignboardPack(L1SignboardInstance signboard)
        {
            WriteC(Opcodes.S_OPCODE_CHARPACK);
            WriteH(signboard.X);
            WriteH(signboard.Y);
            WriteD(signboard.Id);
            WriteH(signboard.GfxId);
            WriteC(0);
            WriteC(getDirection(signboard.Heading));
            WriteC(0);
            WriteC(0);
            WriteD(0);
            WriteH(0);
            WriteS(null);
            WriteS(signboard.NameId);
            int status = 0;
            if (signboard.Poison != null)
            { // 毒状態
                if (signboard.Poison.EffectId == 1)
                {
                    status |= STATUS_POISON;
                }
            }
            WriteC(status);
            WriteD(0);
            WriteS(null);
            WriteS(null);
            WriteC(0);
            WriteC(0xFF);
            WriteC(0);
            WriteC(0);
            WriteS(null);
            WriteH(0xFFFF);
        }

        private int getDirection(int heading)
        {
            int dir = 0;
            switch (heading)
            {
                case 2:
                    dir = 1;
                    break;
                case 3:
                    dir = 2;
                    break;
                case 4:
                    dir = 3;
                    break;
                case 6:
                    dir = 4;
                    break;
                case 7:
                    dir = 5;
                    break;
            }
            return dir;
        }

        public override string Type
        {
            get
            {
                return S_SIGNBOARD_PACK;
            }
        }

    }

}