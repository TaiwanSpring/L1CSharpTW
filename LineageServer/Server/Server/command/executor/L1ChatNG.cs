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
//	import static l1j.server.server.model.skill.L1SkillId.STATUS_CHAT_PROHIBITED;

	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using S_SkillIconGFX = LineageServer.Server.Server.serverpackets.S_SkillIconGFX;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;

	/// <summary>
	/// GM指令：禁言
	/// </summary>
	public class L1ChatNG : L1CommandExecutor
	{
		private L1ChatNG()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1ChatNG();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				StringTokenizer st = new StringTokenizer(arg);
				string name = st.nextToken();
				int time = int.Parse(st.nextToken());

				L1PcInstance tg = L1World.Instance.getPlayer(name);

				if (tg != null)
				{
					tg.setSkillEffect(STATUS_CHAT_PROHIBITED, time * 60 * 1000);
					tg.sendPackets(new S_SkillIconGFX(36, time * 60));
					tg.sendPackets(new S_ServerMessage(286, time.ToString())); // \f3ゲームに適合しない行動であるため、今後%0分間チャットを禁じます。
					pc.sendPackets(new S_ServerMessage(287, name)); // %0のチャットを禁じました。
				}
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage("請輸入 " + cmdName + " 玩家名稱 時間(分)。"));
			}
		}
	}

}