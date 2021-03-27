using LineageServer.Server.Model.Instance;

namespace LineageServer.Interfaces
{
    /// <summary>
    /// 代表可執行的命令
    /// </summary>
    interface ILineageCommand
    {
        /// <summary>
        /// GM指令動作。
        /// </summary>
        /// <param name="pc">施行者 </param>
        /// <param name="cmdName">執行該指令的名稱 </param>
        /// <param name="arg">引數 </param>
        void Execute(L1PcInstance pc, string cmdName, string arg);
    }

}