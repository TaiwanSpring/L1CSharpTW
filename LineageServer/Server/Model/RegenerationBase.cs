using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;
using System;

namespace LineageServer.Server.Model
{
    abstract class PcInstanceRunnableBase : TimerTask
    {
        protected readonly L1PcInstance _pc;
        public bool IsCancelled { get; private set; }
        public PcInstanceRunnableBase(L1PcInstance pc)
        {
            _pc = pc;
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
