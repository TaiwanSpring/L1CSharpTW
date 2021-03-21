using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Templates;
namespace LineageServer.Server.Server.Model
{
    class HpRegenerationByDoll : PcInstanceRunnableBase
    {
        private readonly L1PcInstance _pc;

        public HpRegenerationByDoll(L1PcInstance pc) : base(pc)
        {
            _pc = pc;
        }

        public virtual void regenHp()
        {
            int newHp = _pc.CurrentHp + L1MagicDoll.getHpByDoll(_pc);
            if (newHp < 0)
            {
                newHp = 0;
            }
            _pc.sendPackets(new S_SkillSound(_pc.Id, 744));
            _pc.broadcastPacket(new S_SkillSound(_pc.Id, 744));
            _pc.CurrentHp = newHp;
        }

        protected override void DoRun()
        {
            regenHp();
        }
    }
}