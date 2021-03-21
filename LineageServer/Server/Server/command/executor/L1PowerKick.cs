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
	using S_Disconnect = LineageServer.Server.Server.serverpackets.S_Disconnect;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;

	public class L1PowerKick : L1CommandExecutor
	{
		private L1PowerKick()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1PowerKick();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				L1PcInstance target = L1World.Instance.getPlayer(arg);

				IpTable iptable = IpTable.Instance;
				if (target != null)
				{
					iptable.banIp(target.NetConnection.Ip); // 加入IP至BAN名單
					pc.sendPackets(new S_SystemMessage((new StringBuilder()).Append(target.Name).Append("被您強制踢除遊戲並封鎖IP。").ToString()));
					target.sendPackets(new S_Disconnect());
				}
				else
				{
					pc.sendPackets(new S_SystemMessage("您指定的腳色名稱不存在。"));
				}
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage("請輸入 : " + cmdName + " 玩家名稱。"));
			}
		}
	}

}