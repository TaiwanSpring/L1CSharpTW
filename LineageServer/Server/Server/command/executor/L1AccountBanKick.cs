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
	using Account = LineageServer.Server.Server.Account;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_Disconnect = LineageServer.Server.Server.serverpackets.S_Disconnect;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;

	/// <summary>
	/// GM指令：踢掉且禁止帳號登入
	/// </summary>
	public class L1AccountBanKick : L1CommandExecutor
	{
		private L1AccountBanKick()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1AccountBanKick();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				L1PcInstance target = L1World.Instance.getPlayer(arg);

				if (target != null)
				{
					// アカウントをBANする
					Account.ban(target.AccountName);
					pc.sendPackets(new S_SystemMessage(target.Name + "被您強制踢除遊戲並封鎖IP"));
					target.sendPackets(new S_Disconnect());
				}
				else
				{
					pc.sendPackets(new S_SystemMessage(arg + "不在線上。"));
				}
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage("請輸入 " + cmdName + " 玩家名稱。"));
			}
		}
	}

}