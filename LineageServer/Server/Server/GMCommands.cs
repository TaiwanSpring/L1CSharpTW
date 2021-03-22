using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Server.Command;
using LineageServer.Server.Server.Command.Executor;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Templates;
using LineageServer.Server.Server.utils.collections;
using System.Collections.Generic;
using System.Text;
namespace LineageServer.Server.Server
{
    class GMCommands
    {
        private static ILogger _log = Logger.getLogger(nameof(GMCommands));
        public static GMCommands Instance { get; } = new GMCommands();

        private static IDictionary<int, string> _lastCommands = Maps.newMap<int, string>();

        private bool ExecuteCommand(L1PcInstance pc, string name, string arg)
        {
            L1Command command = L1Commands.GetCommand(name);

            if (command == null)
            {
                return false;
            }

            if (command.CanExecute(pc.AccessLevel))
            {
                pc.sendPackets(new S_ServerMessage(74, $"指令 .{name}")); // \f1%0は使用できません。
                return true;
            }

            IL1CommandExecutor commandExecutor = L1Commands.GetCommandExecutor(command.Name);
            if (commandExecutor == null)
            {
                pc.sendPackets(new S_SystemMessage($"指令 .{name} 未實裝")); // \f1%0は使用できません。
                return false;
            }
            else
            {
                commandExecutor.Execute(pc, name, arg);
                _log.info($"{pc.Name}使用 .{name} {arg}的指令。");
                return true;
            }
        }

        public void HandleCommands(L1PcInstance gm, string cmdLine)
        {
            StringTokenizer token = new StringTokenizer(cmdLine);
            // 命令，直到第一個空白，並在其後當作參數空格隔開
            string cmd = token.nextToken();
            if (string.IsNullOrEmpty(cmd))
            {

            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();

                while (token.hasMoreTokens())
                {
                    stringBuilder.Append(token.nextToken()).Append(' ');
                }

                string param = stringBuilder.ToString().Trim();

                if (cmd == "r")
                {
                    if (_lastCommands.ContainsKey(gm.Id))
                    {
                        Redo(gm, param);
                    }
                    else
                    {
                        gm.sendPackets(new S_ServerMessage(74, "指令" + cmd)); // \f1%0は使用できません。                           
                    }
                }
                else
                {
                    // 將使用過的指令存起來
                    if (ExecuteCommand(gm, cmd, param))
                    {
                        if (_lastCommands.ContainsKey(gm.Id))
                        {
                            _lastCommands.Add(gm.Id, cmdLine);
                        }
                        else
                        {
                            _lastCommands[gm.Id] = cmdLine;
                        }
                    }
                    else
                    {
                        gm.sendPackets(new S_SystemMessage($"指令 {cmd} 執行失敗。"));
                    }
                }
            }
        }

        private void Redo(L1PcInstance pc, string arg)
        {
            string lastCmd = _lastCommands[pc.Id];
            if (arg.Length == 0)
            {
                pc.sendPackets(new S_SystemMessage("指令 " + lastCmd + " 重新執行。"));
                HandleCommands(pc, lastCmd);
            }
            else
            {
                // 引数を変えて実行
                StringTokenizer token = new StringTokenizer(lastCmd);
                string cmd = token.nextToken() + " " + arg;
                pc.sendPackets(new S_SystemMessage("指令 " + cmd + " 執行。"));
                HandleCommands(pc, cmd);
            }
        }
    }
}