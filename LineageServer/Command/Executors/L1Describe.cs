using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
using System.Text;
namespace LineageServer.Command.Executors
{
    /// <summary>
    /// GM指令：描述
    /// </summary>
    class L1Describe : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                if (pc.Inventory is L1PcInventory pcInventory)
                {
                    StringBuilder msg = new StringBuilder();
                    pc.sendPackets(new S_SystemMessage($"-- describe: {pc.Name} --"));
                    int hpr = pc.Hpr + pcInventory.hpRegenPerTick();
                    int mpr = pc.Mpr + pcInventory.mpRegenPerTick();
                    msg.Append($"Dmg: +{pc.Dmgup} / ");
                    msg.Append($"Hit: +{pc.Hitup} / ");
                    msg.Append($"MR: {pc.Mr} / ");
                    msg.Append($"HPR: {hpr} / ");
                    msg.Append($"MPR: {mpr} / ");
                    msg.Append($"Karma: {pc.Karma} / ");
                    msg.Append($"Item: {pc.Inventory.Size} / ");
                    pc.sendPackets(new S_SystemMessage(msg.ToString()));
                }

            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage(cmdName + " 指令錯誤"));
            }
        }
    }

}