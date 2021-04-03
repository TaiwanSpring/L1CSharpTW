using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
using System.Threading;
namespace LineageServer.Server.Model
{
    class L1PinkName
    {
        private L1PinkName()
        {
        }

        internal class PinkNameTimer : IRunnable
        {
            internal L1PcInstance _attacker = null;

            public PinkNameTimer(L1PcInstance attacker)
            {
                _attacker = attacker;
            }

            public void run()
            {
                for (int i = 0; i < 180; i++)
                {
                    Thread.Sleep(1000);

                    // 死亡、または、相手を倒して赤ネームになったら終了
                    if (_attacker.Dead)
                    {
                        // setPinkName(false);はL1PcInstance#death()で行う
                        break;
                    }
                    if (_attacker.Lawful < 0)
                    {
                        _attacker.PinkName = false;
                        break;
                    }
                }
                stopPinkName(_attacker);
            }

            internal virtual void stopPinkName(L1PcInstance attacker)
            {
                attacker.sendPackets(new S_PinkName(attacker.Id, 0));
                attacker.broadcastPacket(new S_PinkName(attacker.Id, 0));
                attacker.PinkName = false;
            }
        }

        public static void onAction(L1PcInstance pc, L1Character cha)
        {
            if ((pc == null) || (cha == null))
            {
                return;
            }

            if (!(cha is L1PcInstance))
            {
                return;
            }
            L1PcInstance attacker = (L1PcInstance)cha;
            if (pc.Id == attacker.Id)
            {
                return;
            }
            if (attacker.FightId == pc.Id)
            {
                return;
            }

            bool isNowWar = false;
            int castleId = L1CastleLocation.getCastleIdByArea(pc);
            if (castleId != 0)
            { // 旗内に居る
                isNowWar = Container.Instance.Resolve<IWarController>().isNowWar(castleId);
            }

            if ((pc.Lawful >= 0) && !pc.PinkName && (attacker.Lawful >= 0) && !attacker.PinkName)
            {
                if ((pc.ZoneType == 0) && (attacker.ZoneType == 0) && (isNowWar == false))
                {
                    attacker.PinkName = true;
                    attacker.sendPackets(new S_PinkName(attacker.Id, 180));
                    if (!attacker.GmInvis)
                    {
                        attacker.broadcastPacket(new S_PinkName(attacker.Id, 180));
                    }
                    PinkNameTimer pink = new PinkNameTimer(attacker);
                    Container.Instance.Resolve<ITaskController>().execute(pink);
                }
            }
        }
    }

}