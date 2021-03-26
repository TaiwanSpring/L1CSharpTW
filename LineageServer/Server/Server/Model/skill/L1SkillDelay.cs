using LineageServer.Interfaces;
using LineageServer.Models;
namespace LineageServer.Server.Server.Model.skill
{
	class L1SkillDelay
	{

		private L1SkillDelay()
		{
		}

		internal class SkillDelayTimer : IRunnable
		{
			internal L1Character _cha;

			public SkillDelayTimer(L1Character cha, int time)
			{
				_cha = cha;
			}

			public void run()
			{
				stopDelayTimer();
			}

			public virtual void stopDelayTimer()
			{
				_cha.SkillDelay = false;
			}
		}

		public static void onSkillUse(L1Character cha, int time)
		{
			cha.SkillDelay = true;
			RunnableExecuter.Instance.execute(new SkillDelayTimer(cha, time), time);
		}

	}

}