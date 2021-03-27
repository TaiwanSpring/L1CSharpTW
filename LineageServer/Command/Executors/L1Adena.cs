using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.identity;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
using System.Text;
namespace LineageServer.Command.Executors
{
    /// <summary>
    /// GM指令：增加金幣
    /// </summary>
    class L1Adena : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                StringTokenizer stringtokenizer = new StringTokenizer(arg);

                string str = stringtokenizer.nextToken();

                if (string.IsNullOrEmpty(str))
                {
                    return;
                }

                if (int.TryParse(str, out int count))
                {
                    L1ItemInstance adena = pc.Inventory.storeItem(L1ItemId.ADENA, count);
                    if (adena != null)
                    {
                        pc.sendPackets(new S_SystemMessage((new StringBuilder()).Append(count).Append(" 金幣產生。").ToString()));
                    }
                }
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage((new StringBuilder()).Append("請輸入 .adena 數量||.金幣  數量。").ToString()));
            }
        }
    }
}