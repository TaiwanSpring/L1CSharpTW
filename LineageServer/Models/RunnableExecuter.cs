using LineageServer.DataStruct;
using LineageServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LineageServer.Models
{
	class RunnableExecuter
	{
		public static RunnableExecuter Instance { get; } = new RunnableExecuter();

		public const int TicksPerMillisecond = 10_000;

		private readonly Dictionary<TimerTask, TimerTaskArgs> timerTaskMapping = new Dictionary<TimerTask, TimerTaskArgs>();

		private bool isStart;

		private object locker = new object();

		public void Start()
		{
			lock (this.locker)
			{
				if (!this.isStart)
				{
					this.isStart = true;
					Task.Run(Run);
				}
			}
		}
		public void Stop()
		{
			lock (this.locker)
			{
				if (this.isStart)
				{
					this.isStart = false;
				}
			}
		}

		private void Run()
		{
			long oldTicks = DateTime.Now.Ticks;
			while (this.isStart)
			{
				long currentTicks = DateTime.Now.Ticks;
				IEnumerable<KeyValuePair<TimerTask, TimerTaskArgs>> items = this.timerTaskMapping.ToArray();
				long ticks = currentTicks - oldTicks;
				foreach (var item in items)
				{
					if (item.Key.IsCancel)
					{
						this.timerTaskMapping.Remove(item.Key);
					}
					else
					{
						if (item.Value.Ticks <= 0)
						{
							this.timerTaskMapping.Remove(item.Key);
						}
						else
						{
							item.Value.Ticks = item.Value.Ticks - ticks;

							if (item.Value.Ticks <= 0)
							{
								item.Value.Ticks += item.Value.Interval;

								Task.Run(item.Key.run);
							}
						}
					}
				}
			}
		}
		public void scheduleAtFixedRate(TimerTask task, int delay)
		{
			this.timerTaskMapping.Add(task, new TimerTaskArgs()
			{
				Ticks = delay * TicksPerMillisecond,
				Interval = 0
			});
		}
		public void scheduleAtFixedRate(TimerTask task, int delay, int period)
		{
			this.timerTaskMapping.Add(task, new TimerTaskArgs()
			{
				Ticks = delay * TicksPerMillisecond,
				Interval = period * TicksPerMillisecond
			});
		}
		public void execute(IRunnable task)
		{
			Task.Run(task.run);
		}
		public void execute(IRunnable task, int delay)
		{
			Task.Delay(delay).ContinueWith(t => task.run());
		}
	}
}
