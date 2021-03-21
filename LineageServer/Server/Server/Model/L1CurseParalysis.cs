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
namespace LineageServer.Server.Server.Model
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_CURSE_PARALYZED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_CURSE_PARALYZING;
	using GeneralThreadPool = LineageServer.Server.Server.GeneralThreadPool;
	using L1MonsterInstance = LineageServer.Server.Server.Model.Instance.L1MonsterInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_Paralysis = LineageServer.Server.Server.serverpackets.S_Paralysis;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;

	/*
	 * L1ParalysisPoisonと被るコードが多い。特にタイマー。何とか共通化したいが難しい。
	 */
	public class L1CurseParalysis : L1Paralysis
	{
		private readonly L1Character _target;

		private readonly int _delay;

		private readonly int _time;

		private Thread _timer;

		private class ParalysisDelayTimer : IRunnable
		{
			private readonly L1CurseParalysis outerInstance;

			public ParalysisDelayTimer(L1CurseParalysis outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void run()
			{
				outerInstance._target.setSkillEffect(STATUS_CURSE_PARALYZING, 0);

				try
				{
					Thread.Sleep(outerInstance._delay); // 麻痺するまでの猶予時間を待つ。
				}
				catch (InterruptedException)
				{
					outerInstance._target.killSkillEffectTimer(STATUS_CURSE_PARALYZING);
					return;
				}

				if (outerInstance._target is L1PcInstance)
				{
					L1PcInstance player = (L1PcInstance) outerInstance._target;
					if (!player.Dead)
					{
						player.sendPackets(new S_Paralysis(1, true)); // 麻痺状態にする
					}
				}
				outerInstance._target.Paralyzed = true;
				outerInstance._timer = new ParalysisTimer(outerInstance);
				GeneralThreadPool.Instance.execute(outerInstance._timer); // 麻痺タイマー開始
				if (Interrupted)
				{
					outerInstance._timer.Interrupt();
				}
			}
		}

		private class ParalysisTimer : IRunnable
		{
			private readonly L1CurseParalysis outerInstance;

			public ParalysisTimer(L1CurseParalysis outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void run()
			{
				outerInstance._target.killSkillEffectTimer(STATUS_CURSE_PARALYZING);
				outerInstance._target.setSkillEffect(STATUS_CURSE_PARALYZED, 0);
				try
				{
					Thread.Sleep(outerInstance._time);
				}
				catch (InterruptedException)
				{
				}

				outerInstance._target.killSkillEffectTimer(STATUS_CURSE_PARALYZED);
				if (outerInstance._target is L1PcInstance)
				{
					L1PcInstance player = (L1PcInstance) outerInstance._target;
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
				L1PcInstance player = (L1PcInstance) _target;
				player.sendPackets(new S_ServerMessage(212));
			}

			_target.PoisonEffect = 2;

			_timer = new ParalysisDelayTimer(this);
			GeneralThreadPool.Instance.execute(_timer);
		}

		public static bool curse(L1Character cha, int delay, int time)
		{
			if (!((cha is L1PcInstance) || (cha is L1MonsterInstance)))
			{
				return false;
			}
			if (cha.hasSkillEffect(STATUS_CURSE_PARALYZING) || cha.hasSkillEffect(STATUS_CURSE_PARALYZED))
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
				_timer.Interrupt(); // 麻痺タイマー解除
			}

			_target.PoisonEffect = 0;
			_target.Paralaysis = null;
		}
	}

}