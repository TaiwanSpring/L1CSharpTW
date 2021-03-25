using LineageServer.Models;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Model
{
    class L1NpcDeleteTimer : TimerTask
    {
        public L1NpcDeleteTimer(L1NpcInstance npc, int timeMillis)
        {
            _npc = npc;
            _timeMillis = timeMillis;
        }

        public override void run()
        {
            // 龍之門扉存在時間到時
            if (_npc != null)
            {
                if (_npc.NpcId == 81273 || _npc.NpcId == 81274 || _npc.NpcId == 81275 || _npc.NpcId == 81276 || _npc.NpcId == 81277)
                {
                    if (_npc.NpcId == 81277)
                    { // 隱匿的巨龍谷入口關閉
                        L1DragonSlayer.Instance.HiddenDragonValleyStstus = 0;
                    }
                    // 結束屠龍副本
                    L1DragonSlayer.Instance.setPortalPack(_npc.PortalNumber, null);
                    L1DragonSlayer.Instance.endDragonPortal(_npc.PortalNumber);
                    // 門扉消失動作
                    _npc.Status = ActionCodes.ACTION_Die;
                    _npc.broadcastPacket(new S_DoActionGFX(_npc.Id, ActionCodes.ACTION_Die));
                }
                _npc.deleteMe();
                cancel();
            }
        }

        public virtual void begin()
        {
            RunnableExecuter.Instance.execute(this, _timeMillis);
        }

        private readonly L1NpcInstance _npc;

        private readonly int _timeMillis;
    }

}