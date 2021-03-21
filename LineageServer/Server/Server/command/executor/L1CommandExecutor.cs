using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.Command.Executor
{
    /// <summary>
    /// コマンド実行処理インターフェース
    /// 
    /// コマンド処理クラスは、このインターフェースメソッド以外に<br>
    /// public static L1CommandExecutor getInstance()<br>
    /// を実装しなければならない。
    /// 通常、自クラスをインスタンス化して返すが、必要に応じてキャッシュされたインスタンスを返したり、他のクラスをインスタンス化して返すことができる。
    /// </summary>
    interface IL1CommandExecutor
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