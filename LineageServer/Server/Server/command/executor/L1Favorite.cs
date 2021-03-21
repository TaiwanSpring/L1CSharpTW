using System;
using System.Collections.Generic;
using System.Text;

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

	using GMCommands = LineageServer.Server.Server.GMCommands;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	/// <summary>
	/// GM指令：我的最愛
	/// </summary>
	public class L1Favorite : L1CommandExecutor
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1Favorite).FullName);

		private static readonly IDictionary<int, string> _faviCom = Maps.newMap();

		private L1Favorite()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1Favorite();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				if (!_faviCom.ContainsKey(pc.Id))
				{
					_faviCom[pc.Id] = "";
				}
				string faviCom = _faviCom[pc.Id];
				if (arg.StartsWith("set", StringComparison.Ordinal))
				{
					// コマンドの登録
					StringTokenizer st = new StringTokenizer(arg);
					st.nextToken();
					if (!st.hasMoreTokens())
					{
						pc.sendPackets(new S_SystemMessage("指令不存在。"));
						return;
					}
					StringBuilder cmd = new StringBuilder();
					string temp = st.nextToken(); // コマンドタイプ
					if (temp.Equals(cmdName, StringComparison.OrdinalIgnoreCase))
					{
						pc.sendPackets(new S_SystemMessage(cmdName + " 不能加入自己的名字。"));
						return;
					}
					cmd.Append(temp + " ");
					while (st.hasMoreTokens())
					{
						cmd.Append(st.nextToken() + " ");
					}
					faviCom = cmd.ToString().Trim();
					_faviCom[pc.Id] = faviCom;
					pc.sendPackets(new S_SystemMessage(faviCom + " 被登記在好友名單。"));
				}
				else if (arg.StartsWith("show", StringComparison.Ordinal))
				{
					pc.sendPackets(new S_SystemMessage("目前登記的指令: " + faviCom));
				}
				else if (faviCom.Length == 0)
				{
					pc.sendPackets(new S_SystemMessage("沒有被登記的名字。"));
				}
				else
				{
					StringBuilder cmd = new StringBuilder();
					StringTokenizer st = new StringTokenizer(arg);
					StringTokenizer st2 = new StringTokenizer(faviCom);
					while (st2.hasMoreTokens())
					{
						string temp = st2.nextToken();
						if (temp.StartsWith("%", StringComparison.Ordinal))
						{
							cmd.Append(st.nextToken() + " ");
						}
						else
						{
							cmd.Append(temp + " ");
						}
					}
					while (st.hasMoreTokens())
					{
						cmd.Append(st.nextToken() + " ");
					}
					pc.sendPackets(new S_SystemMessage(cmd + " 實行。"));
					GMCommands.Instance.handleCommands(pc, cmd.ToString());
				}
			}
			catch (Exception e)
			{
				pc.sendPackets(new S_SystemMessage("請輸入 " + cmdName + " set 玩家名稱 " + "| " + cmdName + " show | " + cmdName + " [數量]。"));
				_log.log(Enum.Level.Server, e.Message, e);
			}
		}
	}

}