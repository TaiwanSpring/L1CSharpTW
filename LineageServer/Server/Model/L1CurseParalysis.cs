using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;
using System.Threading;
namespace LineageServer.Server.Model
{
    /*
	 * L1ParalysisPoisonと被るコードが多い。特にタイマー。何とか共通化したいが難しい。
	 */
    class L1CurseParalysis : L1Paralysis
    {
        private readonly L1Character _target;

        private readonly int _delay;

        private readonly int _time;

        private ITimerTask _timer;

        private class ParalysisDelayTimer : TimerTask
        {
            private readonly L1CurseParalysis outerInstance;

            public ParalysisDelayTimer(L1CurseParalysis outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public void run()
            {
                outerInstance._target.setSkillEffect(L1SkillId.STATUS_CURSE_PARALYZING, 0);

                Thread.Sleep(outerInstance._delay); // 麻痺するまでの猶予時間を待つ。

                if (outerInstance._target is L1PcInstance)
                {
                    L1PcInstance player = (L1PcInstance)outerInstance._target;
                    if (!player.Dead)
                    {
                        player.sendPackets(new S_Paralysis(1, true)); // 麻痺状態にする
                    }
                }
                outerInstance._target.Paralyzed = true;
                outerInstance._timer = new ParalysisTimer(outerInstance);
                RunnableExecuter.Instance.execute(outerInstance._timer); // 麻痺タイマー開始
                if (IsCancel)
                {
                    outerInstance._timer.cancel();
                }
            }
        }

        private class ParalysisTimer : TimerTask
        {
            private readonly L1CurseParalysis outerInstance;

            public ParalysisTimer(L1CurseParalysis outerInstance)
            {
                this.outerInstance = outerInstance;
            }

            public void run()
            {
                outerInstance._target.killSkillEffectTimer(L1SkillId.STATUS_CURSE_PARALYZING);
                outerInstance._target.setSkillEffect(L1SkillId.STATUS_CURSE_PARALYZED, 0);

                Thread.Sleep(outerInstance._time);

                outerInstance._target.killSkillEffectTimer(L1SkillId.STATUS_CURSE_PARALYZED);
                if (outerInstance._target is L1PcInstance)
                {
                    L1PcInstance player = (L1PcInstance)outerInstance._target;
                    if (!player.Dead)
                    {
                        player.sendPackets(new S_Paralysis(1, false)); // 麻痺状態を解除する
                    }
                }
                outerInstance._target.Paralyzed = false;
                outerInstance.cure(); // 解呪処理
            }
        }

        private L1CurseParalysis(L1Character cha, int delay, int time)
        {
            _target = cha;
            _delay = delay;
            _time = time;

            curse();
        }

        private void curse()
        {
            if (_target is L1PcInstance)
            {
                L1PcInstance player = (L1PcInstance)_target;
                player.sendPackets(new S_ServerMessage(212));
            }

            _target.PoisonEffect = 2;

            _timer = new ParalysisDelayTimer(this);
            RunnableExecuter.Instance.execute(_timer);
        }

        public static bool curse(L1Character cha, int delay, int time)
        {
            if (!((cha is L1PcInstance) || (cha is L1MonsterInstance)))
            {
                return false;
            }
            if (cha.hasSkillEffect(L1SkillId.STATUS_CURSE_PARALYZING) || cha.hasSkillEffect(L1SkillId.STATUS_CURSE_PARALYZED))
            {
                return false; // 既に麻痺している
            }

            cha.Paralaysis = new L1CurseParalysis(cha, delay, time);
            return true;
        }

        public override int EffectId
        {
            get
            {
                return 2;
            }
        }

        public override void cure()
        {
            if (_timer != null)
            {
                _timer.cancel(); // 麻痺タイマー解除
            }

            _target.PoisonEffect = 0;
            _target.Paralaysis = null;
        }
    }

}