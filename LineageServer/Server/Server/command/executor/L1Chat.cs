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

	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;

	/// <summary>
	/// GM指令：全體聊天
	/// </summary>
	public class L1Chat : L1CommandExecutor
	{
		private L1Chat()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1Chat();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				StringTokenizer st = new StringTokenizer(arg);
				if (st.hasMoreTokens())
				{
					string flag = st.nextToken();
					string msg;
					if (string.Compare(flag, "on", StringComparison.OrdinalIgnoreCase) == 0)
					{
						L1World.Instance.set_worldChatElabled(true);
						msg = "開啟全體聊天。";
					}
					else if (string.Compare(flag, "off", StringComparison.OrdinalIgnoreCase) == 0)
					{
						L1World.Instance.set_worldChatElabled(false);
						msg = "關閉全體聊天。";
					}
					else
					{
						throw new Exception();
					}
					pc.sendPackets(new S_SystemMessage(msg));
				}
				else
				{
					string msg;
					if (L1World.Instance.WorldChatElabled)
					{
						msg = "全體聊天已開啟。.chat off 能使其關閉。";
					}
					else
					{
						msg = "全體聊天已關閉。.chat on 能使其開啟。";
					}
					pc.sendPackets(new S_SystemMessage(msg));
				}
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage("請輸入 " + cmdName + " [on|off]"));
			}
		}
	}

}