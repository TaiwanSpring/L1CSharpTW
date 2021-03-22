using LineageServer.Models;
using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using System;
namespace LineageServer.Server.Server.Command.Executor
{
    class L1InvGfxId : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                StringTokenizer st = new StringTokenizer(arg);
                int gfxid = Convert.ToInt32(st.nextToken(), 10);
                int count = Convert.ToInt32(st.nextToken(), 10);
                for (int i = 0; i < count; i++)
                {
                    L1ItemInstance item = ItemTable.Instance.createItem(40005);
                    item.Item.GfxId = gfxid + i;
                    item.Item.Name = (gfxid + i).ToString();
                    pc.Inventory.storeItem(item);
                }
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage(cmdName + " 請輸入 id 出現的數量。"));
            }
        }
    }

}