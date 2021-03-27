using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using System.Threading;
namespace LineageServer.Server.Model.poison
{
	class L1DamagePoison : L1Poison
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

			public void run()
			{
				while (true)
				{
					Thread.Sleep(outerInstance._damageSpan);

					if (!outerInstance._target.hasSkillEffect(L1SkillId.STATUS_POISON))
					{
						break;
					}
					if (outerInstance._target is L1PcInstance)
					{
						L1PcInstance player = (L1PcInstance)outerInstance._target;
						player.receiveDamage(outerInstance._attacker, outerInstance._damage, false);
						if (player.Dead)
						{ // 死亡したら解毒処理
							break;
						}
					}
					else if (outerInstance._target is L1MonsterInstance)
					{
						L1MonsterInstance mob = (L1MonsterInstance)outerInstance._target;
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
			return ( cha is L1PcInstance ) || ( cha is L1MonsterInstance );
		}

		private void doInfection()
		{
			_target.setSkillEffect(L1SkillId.STATUS_POISON, 30000);
			_target.PoisonEffect = 1;

			if (isDamageTarget(_target))
			{
				RunnableExecuter.Instance.execute(new NormalPoisonTimer(this)); // 通常毒タイマー開始
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
			_target.killSkillEffectTimer(L1SkillId.STATUS_POISON);
			_target.Poison = null;
		}
	}

}