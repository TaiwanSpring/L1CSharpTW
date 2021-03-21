using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Templates;
namespace LineageServer.Server.Server.Model
{

    class MpRegenerationByDoll : PcInstanceRunnableBase
    {
        public MpRegenerationByDoll(L1PcInstance pc) : base(pc)
        {

        }

        public virtual void regenMp()
        {
            int newMp = _pc.CurrentMp + L1MagicDoll.getMpByDoll(_pc);
            if (newMp < 0)
            {
                newMp = 0;
            }
            _pc.sendPackets(new S_SkillSound(_pc.Id, 6321));
            _pc.broadcastPacket(new S_SkillSound(_pc.Id, 6321));
            _pc.CurrentMp = newMp;
        }

        protected override void DoRun()
        {
            regenMp();
        }
    }
}