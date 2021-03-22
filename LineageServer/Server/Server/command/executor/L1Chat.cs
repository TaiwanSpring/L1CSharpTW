﻿using LineageServer.Models;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using System;
namespace LineageServer.Server.Server.Command.Executor
{
    /// <summary>
    /// GM指令：全體聊天
    /// </summary>
    class L1Chat : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            try
            {
                StringTokenizer st = new StringTokenizer(arg);
                if (st.hasMoreTokens())
                {
                    string flag = st.nextToken();
                    string msg;
                    if (string.Compare(flag, "on", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        L1World.Instance.set_worldChatElabled(true);
                        msg = "開啟全體聊天。";
                    }
                    else if (string.Compare(flag, "off", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        L1World.Instance.set_worldChatElabled(false);
                        msg = "關閉全體聊天。";
                    }
                    else
                    {
                        throw new Exception();
                    }
                    pc.sendPackets(new S_SystemMessage(msg));
                }
                else
                {
                    string msg;
                    if (L1World.Instance.WorldChatElabled)
                    {
                        msg = "全體聊天已開啟。.chat off 能使其關閉。";
                    }
                    else
                    {
                        msg = "全體聊天已關閉。.chat on 能使其開啟。";
                    }
                    pc.sendPackets(new S_SystemMessage(msg));
                }
            }
            catch (Exception)
            {
                pc.sendPackets(new S_SystemMessage("請輸入 " + cmdName + " [on|off]"));
            }
        }
    }

}