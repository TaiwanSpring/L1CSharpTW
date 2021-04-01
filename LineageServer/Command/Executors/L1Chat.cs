using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Command.Executors
{
    /// <summary>
    /// GM指令：全體聊天
    /// </summary>
    class L1Chat : ILineageCommand
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
                    if (flag == "on")
                    {
                        Container.Instance.Resolve<IGameWorld>().WorldChatElabled = true;
                        msg = "開啟全體聊天。";
                    }
                    else if (flag == "off")
                    {
                        Container.Instance.Resolve<IGameWorld>().WorldChatElabled = false;
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
                    if (Container.Instance.Resolve<IGameWorld>().WorldChatElabled)
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