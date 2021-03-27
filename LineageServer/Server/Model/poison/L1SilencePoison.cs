using LineageServer.Server.Model.skill;

namespace LineageServer.Server.Model.poison
{
	class L1SilencePoison : L1Poison
	{
		private readonly L1Character _target;

		public static bool doInfection(L1Character cha)
		{
			if (!L1Poison.isValidTarget(cha))
			{
				return false;
			}

			cha.Poison = new L1SilencePoison(cha);
			return true;
		}

		private L1SilencePoison(L1Character cha)
		{
			_target = cha;

			doInfection();
		}

		private void doInfection()
		{
			_target.PoisonEffect = 1;
			sendMessageIfPlayer(_target, 310);

			_target.setSkillEffect(L1SkillId.STATUS_POISON_SILENCE, 0);
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
			_target.PoisonEffect = 0;
			sendMessageIfPlayer(_target, 311);

			_target.killSkillEffectTimer(L1SkillId.STATUS_POISON_SILENCE);
			_target.Poison = null;
		}
	}

}