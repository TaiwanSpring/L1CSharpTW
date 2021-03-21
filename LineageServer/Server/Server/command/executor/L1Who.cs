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

	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;
	using S_WhoAmount = LineageServer.Server.Server.serverpackets.S_WhoAmount;

	public class L1Who : L1CommandExecutor
	{
		private L1Who()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1Who();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				ICollection<L1PcInstance> players = L1World.Instance.AllPlayers;
				string amount = players.Count.ToString();
				S_WhoAmount s_whoamount = new S_WhoAmount(amount);
				pc.sendPackets(s_whoamount);

				// オンラインのプレイヤーリストを表示
				if (arg.Equals("all", StringComparison.OrdinalIgnoreCase))
				{
					pc.sendPackets(new S_SystemMessage("-- 線上玩家 --"));
					StringBuilder buf = new StringBuilder();
					foreach (L1PcInstance each in players)
					{
						buf.Append(each.Name);
						buf.Append(" / ");
						if (buf.Length > 50)
						{
							pc.sendPackets(new S_SystemMessage(buf.ToString()));
							buf.Remove(0, buf.Length - 1);
						}
					}
					if (buf.Length > 0)
					{
						pc.sendPackets(new S_SystemMessage(buf.ToString()));
					}
				}
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage("請輸入: .who [all] 。"));
			}
		}
	}

}