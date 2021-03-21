using LineageServer.Interfaces;
using LineageServer.Server.Server.Model.Instance;
using System;

namespace LineageServer.Server.Server.Model
{
    abstract class PcInstanceRunnableBase : ICancel
    {
        protected readonly L1PcInstance _pc;
        public event Action<ICancel> Cancel;
        public bool IsCancelled { get; private set; }
        public PcInstanceRunnableBase(L1PcInstance pc)
        {
            _pc = pc;
        }
        public void cancel()
        {
            IsCancelled = true;
            if (Cancel != null)
            {
                Cancel.Invoke(this);
            }
        }

        public void run()
        {
            try
            {
                if (_pc.Dead)
                {
                    return;
                }
                DoRun();
            }
            catch (Exception e)
            {
                // _log.log(Level.WARNING, e.Message, e);
                throw;
            }
        }

        protected abstract void DoRun();
    }
}
