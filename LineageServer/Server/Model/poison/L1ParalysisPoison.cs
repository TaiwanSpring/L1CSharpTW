using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;
using System.Threading;
namespace LineageServer.Server.Model.poison
{
	class L1ParalysisPoison : L1Poison
	{
		// 麻痺毒の性能一覧 猶予 持続 (参考値、未適用)
		// グール 20 45
		// アステ 10 60
		// 蟻穴ムカデ 14 30
		// D-グール 39 45

		private readonly L1Character _target;

		private ITimerTask _timer;

		private readonly int _delay;

		private readonly int _time;

		private int _effectId = 1;

		private class ParalysisPoisonTimer : TimerTask
		{
			private readonly L1ParalysisPoison outerInstance;

			public ParalysisPoisonTimer(L1ParalysisPoison outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public void run()
			{
				outerInstance._target.setSkillEffect(L1SkillId.STATUS_POISON_PARALYZING, 0);

				Thread.Sleep(outerInstance._delay); // 麻痺するまでの猶予時間を待つ。

				// エフェクトを緑から灰色へ
				outerInstance._effectId = 2;
				outerInstance._target.PoisonEffect = 2;

				if (outerInstance._target is L1PcInstance)
				{
					L1PcInstance player = (L1PcInstance)outerInstance._target;
					if (player.Dead == false)
					{
						player.sendPackets(new S_Paralysis(1, true)); // 麻痺状態にする
						outerInstance._timer = new ParalysisTimer(outerInstance);
						RunnableExecuter.Instance.execute(outerInstance._timer); // 麻痺タイマー開始
						if (IsCancel)
						{
							outerInstance._timer.cancel();
						}
					}
				}
			}
		}

		private class ParalysisTimer : TimerTask
		{
			private readonly L1ParalysisPoison outerInstance;

			public ParalysisTimer(L1ParalysisPoison outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void run()
			{
				outerInstance._target.killSkillEffectTimer(L1SkillId.STATUS_POISON_PARALYZING);
				outerInstance._target.setSkillEffect(L1SkillId.STATUS_POISON_PARALYZED, 0);
				Thread.Sleep(outerInstance._time);

				outerInstance._target.killSkillEffectTimer(L1SkillId.STATUS_POISON_PARALYZED);
				if (outerInstance._target is L1PcInstance)
				{
					L1PcInstance player = (L1PcInstance)outerInstance._target;
					if (!player.Dead)
					{
						player.sendPackets(new S_Paralysis(1, false)); // 麻痺状態を解除する
						outerInstance.cure(); // 解毒処理
					}
				}
			}
		}

		private L1ParalysisPoison(L1Character cha, int delay, int time)
		{
			_target = cha;
			_delay = delay;
			_time = time;

			doInfection();
		}

		public static bool doInfection(L1Character cha, int delay, int time)
		{
			if (!L1Poison.isValidTarget(cha))
			{
				return false;
			}

			cha.Poison = new L1ParalysisPoison(cha, delay, time);
			return true;
		}

		private void doInfection()
		{
			sendMessageIfPlayer(_target, 212);
			_target.PoisonEffect = 1;

			if (_target is L1PcInstance)
			{
				_timer = new ParalysisPoisonTimer(this);
				RunnableExecuter.Instance.execute(_timer);
			}
		}

		public override int EffectId
		{
			get
			{
				return _effectId;
			}
		}

		public override void cure()
		{
			if (_timer != null)
			{
				_timer.cancel(); // 麻痺毒タイマー解除
			}

			_target.PoisonEffect = 0;
			_target.Poison = null;
		}
	}

}