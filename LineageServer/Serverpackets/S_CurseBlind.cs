using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_CurseBlind : ServerBasePacket
    {
        private const string S_CurseBlind_Conflict = "[S] S_CurseBlind";

        private byte[] _byte = null;

        public S_CurseBlind(int type)
        {
            // type 0:OFF 1:自分以外見えない 2:周りのキャラクターが見える
            buildPacket(type);
        }

        private void buildPacket(int type)
        {
            WriteC(Opcodes.S_OPCODE_CURSEBLIND);
            WriteH(type);
        }
        public override string Type
        {
            get
            {
                return S_CurseBlind_Conflict;
            }
        }
    }

}