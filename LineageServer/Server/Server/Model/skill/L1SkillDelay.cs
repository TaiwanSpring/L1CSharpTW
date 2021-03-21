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
namespace LineageServer.Server.Server.Model.skill
{
	using GeneralThreadPool = LineageServer.Server.Server.GeneralThreadPool;
	using L1Character = LineageServer.Server.Server.Model.L1Character;

	// Referenced classes of package l1j.server.server.model:
	// L1SkillDelay

	public class L1SkillDelay
	{

		private L1SkillDelay()
		{
		}

		internal class SkillDelayTimer : IRunnableStart
		{
			internal L1Character _cha;

			public SkillDelayTimer(L1Character cha, int time)
			{
				_cha = cha;
			}

			public override void run()
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
			GeneralThreadPool.Instance.schedule(new SkillDelayTimer(cha, time), time);
		}

	}

}