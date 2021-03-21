/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Server.command.executor
{
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;

	/// <summary>
	/// コマンド実行処理インターフェース
	/// 
	/// コマンド処理クラスは、このインターフェースメソッド以外に<br>
	/// public static L1CommandExecutor getInstance()<br>
	/// を実装しなければならない。
	/// 通常、自クラスをインスタンス化して返すが、必要に応じてキャッシュされたインスタンスを返したり、他のクラスをインスタンス化して返すことができる。
	/// </summary>
	public interface L1CommandExecutor
	{
		/// <summary>
		/// GM指令動作。
		/// </summary>
		/// <param name="pc">
		///            施行者 </param>
		/// <param name="cmdName">
		///            執行該指令的名稱 </param>
		/// <param name="arg">
		///            引數 </param>
		void execute(L1PcInstance pc, string cmdName, string arg);
	}

}