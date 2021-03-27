using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;

namespace LineageServer.Command.Executors
{
    /// <summary>
    /// GM指令：返回指令被輸入不帶任何參數。測試，調試和命令執行情況的樣本。
    /// </summary>
    class L1Echo : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            pc.sendPackets(new S_SystemMessage(arg));
        }
    }
}