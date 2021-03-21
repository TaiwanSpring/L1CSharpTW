using System;
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

	using IpTable = LineageServer.Server.Server.datatables.IpTable;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;

	/// <summary>
	/// GM指令：禁止登入
	/// </summary>
	public class L1BanIp : L1CommandExecutor
	{
		private L1BanIp()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1BanIp();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				StringTokenizer stringtokenizer = new StringTokenizer(arg);
				// IPを指定
				string s1 = stringtokenizer.nextToken();

				// add/delを指定(しなくてもOK)
				string s2 = null;
				try
				{
					s2 = stringtokenizer.nextToken();
				}
				catch (Exception)
				{
				}

				IpTable iptable = IpTable.Instance;
				bool isBanned = iptable.isBannedIp(s1);

				foreach (L1PcInstance tg in L1World.Instance.AllPlayers)
				{
					if (s1.Equals(tg.NetConnection.Ip))
					{
						string msg = (new StringBuilder()).Append("IP:").Append(s1).Append(" 連線中的角色名稱:").Append(tg.Name).ToString();
						pc.sendPackets(new S_SystemMessage(msg));
					}
				}

				if ("add".Equals(s2, StringComparison.OrdinalIgnoreCase) && !isBanned)
				{
					iptable.banIp(s1); // BANリストへIPを加える
					string msg = (new StringBuilder()).Append("IP:").Append(s1).Append(" 被新增到封鎖名單。").ToString();
					pc.sendPackets(new S_SystemMessage(msg));
				}
				else if ("del".Equals(s2, StringComparison.OrdinalIgnoreCase) && isBanned)
				{
					if (iptable.liftBanIp(s1))
					{ // BANリストからIPを削除する
						string msg = (new StringBuilder()).Append("IP:").Append(s1).Append(" 已從封鎖名單中刪除。").ToString();
						pc.sendPackets(new S_SystemMessage(msg));
					}
				}
				else
				{
					// BANの確認
					if (isBanned)
					{
						string msg = (new StringBuilder()).Append("IP:").Append(s1).Append(" 已被登記在封鎖名單中。").ToString();
						pc.sendPackets(new S_SystemMessage(msg));
					}
					else
					{
						string msg = (new StringBuilder()).Append("IP:").Append(s1).Append(" 尚未被登記在封鎖名單中。").ToString();
						pc.sendPackets(new S_SystemMessage(msg));
					}
				}
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage("請輸入 " + cmdName + " IP [ add | del ]。"));
			}
		}
	}

}