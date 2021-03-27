using LineageServer.Server;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;

namespace LineageServer.Serverpackets
{
    class S_SPMR : ServerBasePacket
    {
        private const string S_SPMR_Conflict = "[S] S_S_SPMR";

        private byte[] _byte = null;

        /// <summary>
        /// 更新魔防Mr以及魔攻Sp </summary>
        public S_SPMR(L1PcInstance pc)
        {
            buildPacket(pc);
        }

        private void buildPacket(L1PcInstance pc)
        {
            WriteC(Opcodes.S_OPCODE_SPMR);
            // ウィズダムポーションのSPはS_SkillBrave送信時に更新されるため差し引いておく
            if (pc.hasSkillEffect(L1SkillId.STATUS_WISDOM_POTION))
            {
                WriteC(pc.Sp - pc.TrueSp - 2); // 装備増加したSP
            }
            else
            {
                WriteC(pc.Sp - pc.TrueSp); // 装備増加したSP
            }
            WriteH(pc.TrueMr - pc.BaseMr); // 装備や魔法で増加したMR		
        }

        public override string Type
        {
            get
            {
                return S_SPMR_Conflict;
            }
        }
    }

}