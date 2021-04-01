using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Gametime;
using System;
using System.Collections.Generic;
using System.Threading;
namespace LineageServer.william
{
	public class L1GameReStart : IGameComponent, IRestartController
	{
		private static ILogger _log = Logger.GetLogger(nameof(L1GameReStart));

		private volatile L1GameTime _currentTime = new L1GameTime();

		private L1GameTime _previousTime = null;

		private HashSet<IL1GameTimeListener> _listeners = new HashSet<IL1GameTimeListener>();

		private TimeUpdaterRestar timeUpdaterRestar;

		public TimeSpan WillRestartTime { get { return this.timeUpdaterRestar == null ? default(TimeSpan) : this.timeUpdaterRestar.WillRestartTime; } }
		public static L1GameReStart Instance { get; }

		public void Initialize()
		{
			TimeSpan restartTime = TimeSpan.FromMinutes(Config.REST_TIME);

			if (restartTime.TotalSeconds > 0)
			{
				Console.WriteLine($"【讀取】 【自動重新啟動】 【設定】【完成】【{restartTime.TotalMinutes}】【分鐘】。");
				Start(restartTime);
			}
		}

		public void Start(TimeSpan timeSpan)
		{
			Stop();

			this.timeUpdaterRestar = new TimeUpdaterRestar(timeSpan);

			Container.Instance.Resolve<ITaskController>().execute(this.timeUpdaterRestar);
		}

		public void Stop()
		{
			if (this.timeUpdaterRestar != null)
			{
				this.timeUpdaterRestar.cancel();
				this.timeUpdaterRestar = null;
			}
		}

		private class TimeUpdaterRestar : TimerTask
		{
			public TimeSpan WillRestartTime { get; private set; }

			private readonly TimeSpan restartTime;

			public TimeUpdaterRestar(TimeSpan restartTime)
			{
				this.restartTime = restartTime;
			}

			public override void run()
			{
				IContainerAdapter containerAdapter = Container.Instance;

				ISendMessageTo sendMessageTo = containerAdapter.Resolve<ISendMessageTo>();

				//EX 5 min = 5 * 60 = 300
				TimeSpan timeSpan = restartTime;

				TimeSpan sleep = TimeSpan.FromSeconds(1);

				while (timeSpan.TotalSeconds > 0)
				{
					if (IsCancel)
					{
						sendMessageTo.SendToAll("伺服器重新啟動中止。");
						Console.WriteLine("伺服器重新啟動中止。");
						return;
					}

					WillRestartTime = timeSpan;

					//TODO if (五分鐘內 一分鐘一次)
					if (timeSpan.TotalSeconds <= 300 && timeSpan.TotalSeconds % 60 == 0 && timeSpan.TotalSeconds != 0)
					{
						sendMessageTo.SendToAll($"伺服器將於 {(int)timeSpan.TotalMinutes} 分鐘後重新啟動,請至安全區域準備登出。");

						Console.WriteLine($"伺服器將於 {(int)timeSpan.TotalMinutes} 分鐘後重新啟動");
					}
					//TODO else if (30秒內 一秒一次)
					else if (timeSpan.TotalSeconds <= 30 && timeSpan.TotalSeconds != 0)
					{
						sendMessageTo.SendToAll($"伺服器將於 {(int)timeSpan.TotalSeconds} 秒後重新啟動,煩請儘速下線！");

						Console.WriteLine($"伺服器將於 {(int)timeSpan.TotalSeconds} 秒後重新啟動");
					}
					else if (timeSpan.TotalSeconds <= 0)
					{
						sendMessageTo.SendToAll("伺服器重新啟動。");
						Console.WriteLine("伺服器重新啟動。");
						//踢掉所有完家，會執行存檔
						containerAdapter.Resolve<ICharacterController>().disconnectAllCharacters();
						break;
					}

					Thread.Sleep(sleep);

					timeSpan.Subtract(sleep);
				}

				Thread.Sleep(sleep);
				Environment.Exit(1);
			}
		}
	}
}