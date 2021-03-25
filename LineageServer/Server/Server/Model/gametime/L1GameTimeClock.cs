using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Server.utils.collections;
using System;
using System.Collections.Generic;
using System.Threading;
namespace LineageServer.Server.Server.Model.Gametime
{
	public class L1GameTimeClock
	{
		private static L1GameTimeClock _instance;

		private volatile L1GameTime _currentTime = L1GameTime.fromSystemCurrentTime();

		private L1GameTime _previousTime = null;

		private IList<L1GameTimeListener> _listeners = Lists.newConcurrentList<L1GameTimeListener>();

		private class TimeUpdater : IRunnable
		{
			private readonly L1GameTimeClock outerInstance;

			public TimeUpdater(L1GameTimeClock outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public void run()
			{
				while (true)
				{
					outerInstance._previousTime = outerInstance._currentTime;
					outerInstance._currentTime = L1GameTime.fromSystemCurrentTime();
					outerInstance.notifyChanged();
					Thread.Sleep(500);
				}
			}
		}

		private bool isFieldChanged(L1GameTimeTypeEnum field)
		{
			return _previousTime.get(field) != _currentTime.get(field);
		}

		private void notifyChanged()
		{
			if (isFieldChanged(L1GameTimeTypeEnum.MONTH))
			{
				foreach (L1GameTimeListener listener in _listeners)
				{
					listener.onMonthChanged(_currentTime);
				}
			}
			if (isFieldChanged(L1GameTimeTypeEnum.DAY_OF_MONTH))
			{
				foreach (L1GameTimeListener listener in _listeners)
				{
					listener.onDayChanged(_currentTime);
				}
			}
			if (isFieldChanged(L1GameTimeTypeEnum.HOUR_OF_DAY))
			{
				foreach (L1GameTimeListener listener in _listeners)
				{
					listener.onHourChanged(_currentTime);
				}
			}
			if (isFieldChanged(L1GameTimeTypeEnum.MINUTE))
			{
				foreach (L1GameTimeListener listener in _listeners)
				{
					listener.onMinuteChanged(_currentTime);
				}
			}
		}

		private L1GameTimeClock()
		{
			RunnableExecuter.Instance.execute(new TimeUpdater(this));
		}

		public static void init()
		{
			_instance = new L1GameTimeClock();
		}

		public static L1GameTimeClock Instance
		{
			get
			{
				return _instance;
			}
		}

		public virtual L1GameTime currentTime()
		{
			return _currentTime;
		}

		public virtual void addListener(L1GameTimeListener listener)
		{
			_listeners.Add(listener);
		}

		public virtual void removeListener(L1GameTimeListener listener)
		{
			_listeners.Remove(listener);
		}
	}

}