using LineageServer.Model;
using LineageServer.Server.Server.datatables;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using System;
using System.Text;
namespace LineageServer.Server.Server.Command.Executor
{
    /// <summary>
    /// GM指令：禁止登入
    /// </summary>
    class L1BanIp : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                StringTokenizer stringtokenizer = new StringTokenizer(arg);
                // IPを指定
                string s1 = stringtokenizer.nextToken();

                // add/delを指定(しなくてもOK)
                string s2 = stringtokenizer.nextToken();

                IpTable iptable = IpTable.Instance;

                bool isBanned = iptable.isBannedIp(s1);

                foreach (L1PcInstance tg in L1World.Instance.AllPlayers)
                {
                    if (s1 == tg.NetConnection.Ip)
                    {
                        string msg = (new StringBuilder()).Append("IP:").Append(s1).Append(" 連線中的角色名稱:").Append(tg.Name).ToString();
                        pc.sendPackets(new S_SystemMessage(msg));
                    }
                }

                if (s2 == "add" && !isBanned)
                {
                    iptable.banIp(s1); // BANリストへIPを加える
                    string msg = (new StringBuilder()).Append("IP:").Append(s1).Append(" 被新增到封鎖名單。").ToString();
                    pc.sendPackets(new S_SystemMessage(msg));
                }
                else if (s2 == "del" && isBanned)
                {
                    if (iptable.liftBanIp(s1))
                    { // BANリストからIPを削除する
                        string msg = (new StringBuilder()).Append("IP:").Append(s1).Append(" 已從封鎖名單中刪除。").ToString();
                        pc.sendPackets(new S_SystemMessage(msg));
                    }
                }
                else
                {
                    // BANの確認
                    if (isBanned)
                    {
                        string msg = (new StringBuilder()).Append("IP:").Append(s1).Append(" 已被登記在封鎖名單中。").ToString();
                        pc.sendPackets(new S_SystemMessage(msg));
                    }
                    else
                    {
                        string msg = (new StringBuilder()).Append("IP:").Append(s1).Append(" 尚未被登記在封鎖名單中。").ToString();
                        pc.sendPackets(new S_SystemMessage(msg));
                    }
                }
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入 " + cmdName + " IP [ add | del ]。"));
            }
        }
    }

}