using LineageServer.Models;
using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Templates;
using System;
namespace LineageServer.Server.Server.Command.Executor
{
    class L1Present : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                StringTokenizer st = new StringTokenizer(arg);
                string account = st.nextToken();
                int itemid = Convert.ToInt32(st.nextToken(), 10);
                int enchant = Convert.ToInt32(st.nextToken(), 10);
                int count = Convert.ToInt32(st.nextToken(), 10);

                L1Item temp = ItemTable.Instance.getTemplate(itemid);
                if (temp == null)
                {
                    pc.sendPackets(new S_SystemMessage("不存在的道具編號。"));
                    return;
                }

                L1DwarfInventory.present(account, itemid, enchant, count);
                pc.sendPackets(new S_SystemMessage(temp.IdentifiedNameId + "數量" + count + "個發送出去了。", true));
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入 : " + ".present 帳號 道具編號 數量 強化等級。（* 等於所有帳號）"));
            }
        }
    }

}