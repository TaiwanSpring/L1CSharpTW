using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;
namespace LineageServer.Command.Executors
{
    /// <summary>
    /// GM指令：我的最愛
    /// </summary>
    class L1Favorite : ILineageCommand
    {
        private static readonly IDictionary<int, string> _faviCom = MapFactory.NewMap<int, string>();


        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                if (!_faviCom.ContainsKey(pc.Id))
                {
                    _faviCom[pc.Id] = "";
                }
                string faviCom = _faviCom[pc.Id];
                if (arg.StartsWith("set", StringComparison.Ordinal))
                {
                    // コマンドの登録
                    StringTokenizer st = new StringTokenizer(arg);
                    st.nextToken();
                    if (!st.hasMoreTokens())
                    {
                        pc.sendPackets(new S_SystemMessage("指令不存在。"));
                        return;
                    }
                    StringBuilder cmd = new StringBuilder();
                    string temp = st.nextToken(); // コマンドタイプ
                    if (temp == cmdName)
                    {
                        pc.sendPackets(new S_SystemMessage(cmdName + " 不能加入自己的名字。"));
                        return;
                    }
                    cmd.Append(temp + " ");
                    while (st.hasMoreTokens())
                    {
                        cmd.Append(st.nextToken() + " ");
                    }
                    faviCom = cmd.ToString().Trim();
                    _faviCom[pc.Id] = faviCom;
                    pc.sendPackets(new S_SystemMessage(faviCom + " 被登記在好友名單。"));
                }
                else if (arg.StartsWith("show", StringComparison.Ordinal))
                {
                    pc.sendPackets(new S_SystemMessage("目前登記的指令: " + faviCom));
                }
                else if (faviCom.Length == 0)
                {
                    pc.sendPackets(new S_SystemMessage("沒有被登記的名字。"));
                }
                else
                {
                    StringBuilder cmd = new StringBuilder();
                    StringTokenizer st = new StringTokenizer(arg);
                    StringTokenizer st2 = new StringTokenizer(faviCom);
                    while (st2.hasMoreTokens())
                    {
                        string temp = st2.nextToken();
                        if (temp.StartsWith("%", StringComparison.Ordinal))
                        {
                            cmd.Append(st.nextToken() + " ");
                        }
                        else
                        {
                            cmd.Append(temp + " ");
                        }
                    }
                    while (st.hasMoreTokens())
                    {
                        cmd.Append(st.nextToken() + " ");
                    }
                    pc.sendPackets(new S_SystemMessage(cmd + " 實行。"));
                    GMCommands.Instance.HandleCommands(pc, cmd.ToString());
                }
            }
            catch (Exception e)
            {
                pc.sendPackets(new S_SystemMessage("請輸入 " + cmdName + " set 玩家名稱 " + "| " + cmdName + " show | " + cmdName + " [數量]。"));
            }
        }
    }
}