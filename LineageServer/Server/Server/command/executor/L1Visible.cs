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
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_Invis = LineageServer.Server.Server.serverpackets.S_Invis;
	using S_OtherCharPacks = LineageServer.Server.Server.serverpackets.S_OtherCharPacks;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;

	public class L1Visible : L1CommandExecutor
	{
		private L1Visible()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1Visible();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				pc.GmInvis = false;
				pc.sendPackets(new S_Invis(pc.Id, 0));
				pc.broadcastPacket(new S_OtherCharPacks(pc));
				pc.sendPackets(new S_SystemMessage("隱形狀態解除。"));
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage(cmdName + " 玩家名稱"));
			}
		}
	}

}