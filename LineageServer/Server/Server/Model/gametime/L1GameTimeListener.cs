namespace LineageServer.Server.Server.Model.Gametime
{
	/// <summary>
	/// <para>
	/// アデン時間の変化を受け取るためのリスナーインターフェース。
	/// </para>
	/// <para>
	/// アデン時間の変化を監視すべきクラスは、このインターフェースに含まれているすべてのメソッドを定義してこのインターフェースを実装するか、
	/// 関連するメソッドだけをオーバーライドしてabstractクラスL1GameTimeAdapterを拡張する。
	/// </para>
	/// <para>
	/// そのようなクラスから作成されたリスナーオブジェクトは、L1GameTimeClockのaddListenerメソッドを使用してL1GameTimeClockに登録される。
	/// アデン時間変化の通知は、月日時分がそれぞれ変わったときに行われる。
	/// </para>
	/// <para>
	/// これらのメソッドは、L1GameTimeClockのスレッド上で動作する。
	/// これらのメソッドの処理に時間がかかった場合、他のリスナーへの通知が遅れる可能性がある。
	/// 完了までに時間を要する処理や、スレッドをブロックするメソッドの呼び出しが含まれる処理を行う場合は、内部で新たにスレッドを作成して処理を行うべきである。
	/// </para>
	/// 
	/// </summary>
	public interface L1GameTimeListener
	{
		/// <summary>
		/// アデン時間で月が変わったときに呼び出される。
		/// </summary>
		/// <param name="time">
		///            最新のアデン時間 </param>
		void onMonthChanged(L1GameTime time);

		/// <summary>
		/// アデン時間で日が変わったときに呼び出される。
		/// </summary>
		/// <param name="time">
		///            最新のアデン時間 </param>
		void onDayChanged(L1GameTime time);

		/// <summary>
		/// アデン時間で時間が変わったときに呼び出される。
		/// </summary>
		/// <param name="time">
		///            最新のアデン時間 </param>
		void onHourChanged(L1GameTime time);

		/// <summary>
		/// アデン時間で分が変わったときに呼び出される。
		/// </summary>
		/// <param name="time">
		///            最新のアデン時間 </param>
		void onMinuteChanged(L1GameTime time);
	}

}