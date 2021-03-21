using System;
using System.Collections.Generic;
using System.Threading;

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
namespace LineageServer.Server.Server.Model.gametime
{

	using GeneralThreadPool = LineageServer.Server.Server.GeneralThreadPool;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;

	public class L1GameTimeClock
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1GameTimeClock).FullName);

		private static L1GameTimeClock _instance;

		private volatile L1GameTime _currentTime = L1GameTime.fromSystemCurrentTime();

		private L1GameTime _previousTime = null;

		private IList<L1GameTimeListener> _listeners = Lists.newConcurrentList();

		private class TimeUpdater : IRunnableStart
		{
			private readonly L1GameTimeClock outerInstance;

			public TimeUpdater(L1GameTimeClock outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void run()
			{
				while (true)
				{
					outerInstance._previousTime = outerInstance._currentTime;
					outerInstance._currentTime = L1GameTime.fromSystemCurrentTime();
					outerInstance.notifyChanged();

					try
					{
						Thread.Sleep(500);
					}
					catch (InterruptedException e)
					{
						_log.log(Enum.Level.Server, e.Message, e);
					}
				}
			}
		}

		private bool isFieldChanged(int field)
		{
			return _previousTime.get(field) != _currentTime.get(field);
		}

		private void notifyChanged()
		{
			if (isFieldChanged(DateTime.MONTH))
			{
				foreach (L1GameTimeListener listener in _listeners)
				{
					listener.onMonthChanged(_currentTime);
				}
			}
			if (isFieldChanged(DateTime.DAY_OF_MONTH))
			{
				foreach (L1GameTimeListener listener in _listeners)
				{
					listener.onDayChanged(_currentTime);
				}
			}
			if (isFieldChanged(DateTime.HOUR_OF_DAY))
			{
				foreach (L1GameTimeListener listener in _listeners)
				{
					listener.onHourChanged(_currentTime);
				}
			}
			if (isFieldChanged(DateTime.MINUTE))
			{
				foreach (L1GameTimeListener listener in _listeners)
				{
					listener.onMinuteChanged(_currentTime);
				}
			}
		}

		private L1GameTimeClock()
		{
			GeneralThreadPool.Instance.execute(new TimeUpdater(this));
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