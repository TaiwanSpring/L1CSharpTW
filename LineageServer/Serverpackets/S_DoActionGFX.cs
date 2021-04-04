using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_DoActionGFX : ServerBasePacket
    {
        private const string S_DOACTIONGFX = "[S] S_SkillGFX";

        public static int ACTION_MAGIC = 0x16;

        private byte[] _byte = null;

        public S_DoActionGFX(int objectId, int actionId)
        {
            WriteC(Opcodes.S_OPCODE_DOACTIONGFX);
            WriteD(objectId);
            WriteC(actionId);
        }
        public override string Type
        {
            get
            {
                return S_DOACTIONGFX;
            }
        }
    }

}