using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
using System.Collections.Generic;
using System.Text;
namespace LineageServer.Command.Executors
{
    class L1Who : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                ICollection<L1PcInstance> players = Container.Instance.Resolve<IGameWorld>().AllPlayers;
                string amount = players.Count.ToString();
                S_WhoAmount s_whoamount = new S_WhoAmount(amount);
                pc.sendPackets(s_whoamount);

                // オンラインのプレイヤーリストを表示
                if (arg == "all")
                {
                    pc.sendPackets(new S_SystemMessage("-- 線上玩家 --"));
                    StringBuilder buf = new StringBuilder();
                    foreach (L1PcInstance each in players)
                    {
                        buf.Append(each.Name);
                        buf.Append(" / ");
                        if (buf.Length > 50)
                        {
                            pc.sendPackets(new S_SystemMessage(buf.ToString()));
                            buf.Remove(0, buf.Length - 1);
                        }
                    }
                    if (buf.Length > 0)
                    {
                        pc.sendPackets(new S_SystemMessage(buf.ToString()));
                    }
                }
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入: .who [all] 。"));
            }
        }
    }

}