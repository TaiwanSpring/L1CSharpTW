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
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_POISON;
	using GeneralThreadPool = LineageServer.Server.Server.GeneralThreadPool;
	using L1Character = LineageServer.Server.Server.Model.L1Character;
	using L1MonsterInstance = LineageServer.Server.Server.Model.Instance.L1MonsterInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;

	public class L1DamagePoison : L1Poison
	{
		private Thread _timer;

		private readonly L1Character _attacker;

		private readonly L1Character _target;

		private readonly int _damageSpan;

		private readonly int _damage;

		private L1DamagePoison(L1Character attacker, L1Character cha, int damageSpan, int damage)
		{
			_attacker = attacker;
			_target = cha;
			_damageSpan = damageSpan;
			_damage = damage;

			doInfection();
		}

		private class NormalPoisonTimer : IRunnable
		{
			private readonly L1DamagePoison outerInstance;

			public NormalPoisonTimer(L1DamagePoison outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void run()
			{
				while (true)
				{
					try
					{
						Thread.Sleep(outerInstance._damageSpan);
					}
					catch (InterruptedException)
					{
						break;
					}

					if (!outerInstance._target.hasSkillEffect(STATUS_POISON))
					{
						break;
					}
					if (outerInstance._target is L1PcInstance)
					{
						L1PcInstance player = (L1PcInstance) outerInstance._target;
						player.receiveDamage(outerInstance._attacker, outerInstance._damage, false);
						if (player.Dead)
						{ // 死亡したら解毒処理
							break;
						}
					}
					else if (outerInstance._target is L1MonsterInstance)
					{
						L1MonsterInstance mob = (L1MonsterInstance) outerInstance._target;
						mob.receiveDamage(outerInstance._attacker, outerInstance._damage);
						if (mob.Dead)
						{ // 死亡しても解毒しない
							return;
						}
					}
				}
				outerInstance.cure(); // 解毒処理
			}
		}

		internal virtual bool isDamageTarget(L1Character cha)
		{
			return (cha is L1PcInstance) || (cha is L1MonsterInstance);
		}

		private void doInfection()
		{
			_target.setSkillEffect(STATUS_POISON, 30000);
			_target.PoisonEffect = 1;

			if (isDamageTarget(_target))
			{
				_timer = new NormalPoisonTimer(this);
				GeneralThreadPool.Instance.execute(_timer); // 通常毒タイマー開始
			}
		}

		public static bool doInfection(L1Character attacker, L1Character cha, int damageSpan, int damage)
		{
			if (!isValidTarget(cha))
			{
				return false;
			}

			cha.Poison = new L1DamagePoison(attacker, cha, damageSpan, damage);
			return true;
		}

		public override int EffectId
		{
			get
			{
				return 1;
			}
		}

		public override void cure()
		{
			if (_timer != null)
			{
				_timer.Interrupt(); // 毒タイマー解除
			}

			_target.PoisonEffect = 0;
			_target.killSkillEffectTimer(STATUS_POISON);
			_target.Poison = null;
		}
	}

}