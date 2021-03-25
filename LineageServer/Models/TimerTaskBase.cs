using LineageServer.Interfaces;
using System;

namespace LineageServer.Models
{
	class TimerTask : ITimerTask
	{
		private readonly Action action;

		public bool IsCancel { get; protected set; }

		protected TimerTask() { }

		public TimerTask(Action action)
		{
			this.action = action;
		}
		public virtual void cancel()
		{
			IsCancel = true;
		}

		public virtual void run()
		{
			if (this.action != null)
			{
				this.action.Invoke();
			}
		}
	}
}
