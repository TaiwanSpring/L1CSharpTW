using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using System;
using System.Text;
namespace LineageServer.Server.Server.Command.Executor
{
	/// <summary>
	/// GM指令：描述
	/// </summary>
	class L1Describe : IL1CommandExecutor
	{
		public void Execute(L1PcInstance pc, string cmdName, string arg)
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