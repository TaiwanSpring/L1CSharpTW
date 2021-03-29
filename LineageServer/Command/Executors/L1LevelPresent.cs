using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using System;
namespace LineageServer.Command.Executors
{
    class L1LevelPresent : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {

            try
            {
                StringTokenizer st = new StringTokenizer(arg);
                int minlvl = Convert.ToInt32(st.nextToken(), 10);
                int maxlvl = Convert.ToInt32(st.nextToken(), 10);
                int itemid = Convert.ToInt32(st.nextToken(), 10);
                int enchant = Convert.ToInt32(st.nextToken(), 10);
                int count = Convert.ToInt32(st.nextToken(), 10);

                L1Item temp = ItemTable.Instance.getTemplate(itemid);
                if (temp == null)
                {
                    pc.sendPackets(new S_SystemMessage("不存在的道具編號。"));
                    return;
                }

                L1DwarfInventory.present(minlvl, maxlvl, itemid, enchant, count);
                pc.sendPackets(new S_SystemMessage(temp.Name + "數量" + count + "個發送出去了。(Lv" + minlvl + "～" + maxlvl + ")"));
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入 .lvpresent minlvl maxlvl 道具編號  強化等級 數量。"));
            }
        }
    }

}