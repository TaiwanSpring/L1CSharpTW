using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.Model
{
    class MpReductionByAwake : PcInstanceRunnableBase
    {
        public MpReductionByAwake(L1PcInstance pc) : base(pc)
        {

        }

        public virtual void decreaseMp()
        {
            int newMp = _pc.CurrentMp - 8;
            if (newMp < 0)
            {
                newMp = 0;
                L1Awake.stop(_pc);
            }
            _pc.CurrentMp = newMp;
        }

        protected override void DoRun()
        {
            decreaseMp();
        }
    }

}