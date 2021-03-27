using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Server.Model.monitor
{
	/// <summary>
	/// L1PcInstanceの定期処理、監視処理等を行う為の共通的な処理を実装した抽象クラス
	/// 
	/// 各タスク処理は<seealso cref="run()"/>ではなく<seealso cref="execTask(L1PcInstance)"/>にて実装する。
	/// PCがログアウトするなどしてサーバ上に存在しなくなった場合、run()メソッドでは即座にリターンする。
	/// その場合、タスクが定期実行スケジューリングされていたら、ログアウト処理等でスケジューリングを停止する必要がある。
	/// 停止しなければタスクは止まらず、永遠に定期実行されることになる。
	/// 定期実行でなく単発アクションの場合はそのような制御は不要。
	/// 
	/// L1PcInstanceの参照を直接持つことは望ましくない。
	/// 
	/// @author frefre
	/// 
	/// </summary>
	abstract class L1PcMonitor : IRunnable
	{

		/// <summary>
		/// モニター対象L1PcInstanceのオブジェクトID </summary>
		protected internal int _id;

		/// <summary>
		/// 指定されたパラメータでL1PcInstanceに対するモニターを作成する。 </summary>
		/// <param name="oId"> <seealso cref="L1PcInstance.getId()"/>で取得できるオブジェクトID </param>
		public L1PcMonitor(int oId)
		{
			_id = oId;
		}

		public void run()
		{
			L1PcInstance pc = (L1PcInstance)L1World.Instance.findObject(_id);
			if (pc == null || pc.NetConnection == null)
			{
				return;
			}
			execTask(pc);
		}

		/// <summary>
		/// タスク実行時の処理 </summary>
		/// <param name="pc"> モニター対象のPC </param>
		public abstract void execTask(L1PcInstance pc);
	}

}