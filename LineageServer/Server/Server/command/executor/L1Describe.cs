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
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;

	/// <summary>
	/// GM指令：描述
	/// </summary>
	public class L1Describe : L1CommandExecutor
	{
		private L1Describe()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1Describe();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				StringBuilder msg = new StringBuilder();
				pc.sendPackets(new S_SystemMessage("-- describe: " + pc.Name + " --"));
				int hpr = pc.Hpr + pc.Inventory.hpRegenPerTick();
				int mpr = pc.Mpr + pc.Inventory.mpRegenPerTick();
				msg.Append("Dmg: +" + pc.Dmgup + " / ");
				msg.Append("Hit: +" + pc.Hitup + " / ");
				msg.Append("MR: " + pc.Mr + " / ");
				msg.Append("HPR: " + hpr + " / ");
				msg.Append("MPR: " + mpr + " / ");
				msg.Append("Karma: " + pc.Karma + " / ");
				msg.Append("Item: " + pc.Inventory.Size + " / ");
				pc.sendPackets(new S_SystemMessage(msg.ToString()));
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage(cmdName + " 指令錯誤"));
			}
		}
	}

}