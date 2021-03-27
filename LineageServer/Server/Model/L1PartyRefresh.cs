using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;

namespace LineageServer.Server.Model
{
    class L1PartyRefresh : TimerTask
    {
        private static ILogger _log = Logger.GetLogger(nameof(L1PartyRefresh));

        private readonly L1PcInstance _pc;

        public L1PartyRefresh(L1PcInstance pc)
        {
            _pc = pc;
        }

        /// <summary>
        /// 3.3C 更新隊伍封包
        /// </summary>
        public virtual void fresh()
        {
            _pc.sendPackets(new S_Party(110, _pc));
        }

        public void run()
        {
            try
            {
                if (_pc.Dead || _pc.Party == null)
                {
                    _pc.stopRefreshParty();
                    return;
                }
                fresh();
            }
            catch (Exception e)
            {
                _pc.stopRefreshParty();
                _log.Error(e);
            }
        }
    }

}