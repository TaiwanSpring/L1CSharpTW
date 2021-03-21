using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Templates;
using System.Collections.Generic;
using System.Text;
namespace LineageServer.Server.Server.Command.Executor
{
    /// <summary>
    /// GM指令：取得所有指令
    /// </summary>
    class L1CommandHelp : IL1CommandExecutor
    {

        private string join(IList<L1Command> list, string with)
        {
            StringBuilder result = new StringBuilder();
            foreach (L1Command cmd in list)
            {
                if (result.Length > 0)
                {
                    result.Append(with);
                }
                result.Append(cmd.Name);
            }
            return result.ToString();
        }

        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            IList<L1Command> list = L1Commands.availableCommandList(pc.AccessLevel);
            pc.sendPackets(new S_SystemMessage(join(list, ", ")));
        }
    }

}