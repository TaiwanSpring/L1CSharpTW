using System;

namespace LineageServer.Interfaces
{
	interface IRestartController
	{
		TimeSpan WillRestartTime { get; }
		void Start(TimeSpan timeSpan);
		void Stop();
	}
}
