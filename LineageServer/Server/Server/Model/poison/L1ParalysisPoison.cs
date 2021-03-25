using System.Threading;

/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Server.Model.poison
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_POISON_PARALYZED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_POISON_PARALYZING;
	using RunnableExecuter = LineageServer.Server.Server.RunnableExecuter;
	using L1Character = LineageServer.Server.Server.Model.L1Character;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_Paralysis = LineageServer.Server.Server.serverpackets.S_Paralysis;

	public class L1ParalysisPoison : L1Poison
	{
		// 麻痺毒の性能一覧 猶予 持続 (参考値、未適用)
		// グール 20 45
		// アステ 10 60
		// 蟻穴ムカデ 14 30
		// D-グール 39 45

		private readonly L1Character _target;

		private Thread _timer;

		private readonly int _delay;

		private readonly int _time;

		private int _effectId = 1;

		private class ParalysisPoisonTimer : IRunnable
		{
			private readonly L1ParalysisPoison outerInstance;

			public ParalysisPoisonTimer(L1ParalysisPoison outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void run()
			{
				outerInstance._target.setSkillEffect(STATUS_POISON_PARALYZING, 0);

				try
				{
					Thread.Sleep(outerInstance._delay); // 麻痺するまでの猶予時間を待つ。
				}
				catch (InterruptedException)
				{
					outerInstance._target.killSkillEffectTimer(STATUS_POISON_PARALYZING);
					return;
				}

				// エフェクトを緑から灰色へ
				outerInstance._effectId = 2;
				outerInstance._target.PoisonEffect = 2;

				if (outerInstance._target is L1PcInstance)
				{
					L1PcInstance player = (L1PcInstance) outerInstance._target;
					if (player.Dead == false)
					{
						player.sendPackets(new S_Paralysis(1, true)); // 麻痺状態にする
						outerInstance._timer = new ParalysisTimer(outerInstance);
						RunnableExecuter.Instance.execute(outerInstance._timer); // 麻痺タイマー開始
						if (Interrupted)
						{
							outerInstance._timer.Interrupt();
						}
					}
				}
			}
		}

		private class ParalysisTimer : IRunnable
		{
			private readonly L1ParalysisPoison outerInstance;

			public ParalysisTimer(L1ParalysisPoison outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void run()
			{
				outerInstance._target.killSkillEffectTimer(STATUS_POISON_PARALYZING);
				outerInstance._target.setSkillEffect(STATUS_POISON_PARALYZED, 0);
				try
				{
					Thread.Sleep(outerInstance._time);
				}
				catch (InterruptedException)
				{
				}

				outerInstance._target.killSkillEffectTimer(STATUS_POISON_PARALYZED);
				if (outerInstance._target is L1PcInstance)
				{
					L1PcInstance player = (L1PcInstance) outerInstance._target;
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
				_timer.Interrupt(); // 麻痺毒タイマー解除
			}

			_target.PoisonEffect = 0;
			_target.Poison = null;
		}
	}

}