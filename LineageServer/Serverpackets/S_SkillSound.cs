using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_SkillSound : ServerBasePacket
    {
        private const string S_SKILL_SOUND = "[S] S_SkillSound";

        private byte[] _byte = null;

        public S_SkillSound(int objid, int gfxid, int aid)
        {

            buildPacket(objid, gfxid, aid);
        }

        public S_SkillSound(int objid, int gfxid)
        {
            buildPacket(objid, gfxid, 0);
        }

        private void buildPacket(int objid, int gfxid, int aid)
        {
            // aidは使われていない
            WriteC(Opcodes.S_OPCODE_SKILLSOUNDGFX);
            WriteD(objid);
            WriteH(gfxid);
        }

        public override string Type
        {
            get
            {
                return S_SKILL_SOUND;
            }
        }
    }

}