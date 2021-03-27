using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System.Threading;
using System.Threading.Tasks;
namespace LineageServer.Server.Model
{

    class L1PcDeleteTimer : PcInstanceRunnableBase
    {
        CancellationTokenSource tokenSource = new CancellationTokenSource();
        public L1PcDeleteTimer(L1PcInstance pc) : base(pc)
        {
            Cancel += L1PcDeleteTimer_Cancel;
        }

        private void L1PcDeleteTimer_Cancel(Interfaces.ICancel obj)
        {
            this.tokenSource.Cancel();
        }

        public virtual void begin()
        {
            try
            {
                Task.Delay(10 * 60 * 1000, this.tokenSource.Token).ContinueWith(task => DoRun());
            }
            catch (TaskCanceledException e)
            {

            }
        }

        protected override void DoRun()
        {
            _pc.sendPackets(new S_Disconnect());
        }

    }

}