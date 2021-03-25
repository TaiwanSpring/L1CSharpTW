namespace LineageServer.Interfaces
{
	interface ITimerTask : IRunnable
	{
		bool IsCancel { get; }
		void cancel();
	}
}
