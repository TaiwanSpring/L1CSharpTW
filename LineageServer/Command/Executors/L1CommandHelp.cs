using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using System.Collections.Generic;
using System.Text;
namespace LineageServer.Command.Executors
{
    /// <summary>
    /// GM指令：取得所有指令
    /// </summary>
    class L1CommandHelp : ILineageCommand
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