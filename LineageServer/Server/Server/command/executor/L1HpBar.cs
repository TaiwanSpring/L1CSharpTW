using System;

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
namespace LineageServer.Server.Server.command.executor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.GMSTATUS_HPBAR;
	using L1Object = LineageServer.Server.Server.Model.L1Object;
	using L1MonsterInstance = LineageServer.Server.Server.Model.Instance.L1MonsterInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1PetInstance = LineageServer.Server.Server.Model.Instance.L1PetInstance;
	using L1SummonInstance = LineageServer.Server.Server.Model.Instance.L1SummonInstance;
	using S_HPMeter = LineageServer.Server.Server.serverpackets.S_HPMeter;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;

	public class L1HpBar : L1CommandExecutor
	{
		private L1HpBar()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1HpBar();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			if (arg.Equals("on", StringComparison.OrdinalIgnoreCase))
			{
				pc.setSkillEffect(GMSTATUS_HPBAR, 0);
			}
			else if (arg.Equals("off", StringComparison.OrdinalIgnoreCase))
			{
				pc.removeSkillEffect(L1SkillId.GMSTATUS_HPBAR);

				foreach (L1Object obj in pc.KnownObjects)
				{
					if (isHpBarTarget(obj))
					{
						pc.sendPackets(new S_HPMeter(obj.Id, 0xFF));
					}
				}
			}
			else
			{
				pc.sendPackets(new S_SystemMessage("請輸入 : " + cmdName + " on|off 。"));
			}
		}

		public static bool isHpBarTarget(L1Object obj)
		{
			if (obj is L1MonsterInstance)
			{
				return true;
			}
			if (obj is L1PcInstance)
			{
				return true;
			}
			if (obj is L1SummonInstance)
			{
				return true;
			}
			if (obj is L1PetInstance)
			{
				return true;
			}
			return false;
		}
	}

}