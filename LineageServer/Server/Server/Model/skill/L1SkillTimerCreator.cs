
namespace LineageServer.Server.Server.Model.skill
{
	class L1SkillTimerCreator
	{
		public static IL1SkillTimer create(L1Character cha, int skillId, int timeMillis)
		{
			if (Config.SKILLTIMER_IMPLTYPE == 1)
			{
				return new L1SkillTimerTimerImpl(cha, skillId, timeMillis);
			}
			else if (Config.SKILLTIMER_IMPLTYPE == 2)
			{
				return new L1SkillTimerTimerImpl(cha, skillId, timeMillis);
			}

			// 不正な値の場合は、とりあえずTimer
			return new L1SkillTimerTimerImpl(cha, skillId, timeMillis);
		}
	}

}