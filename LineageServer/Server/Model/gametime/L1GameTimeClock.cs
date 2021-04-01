using LineageServer.Enum;
using LineageServer.Interfaces;
using LineageServer.Models;
using System.Collections.Generic;
using System.Linq;
namespace LineageServer.Server.Model.Gametime
{
	public class L1GameTimeClock : IGameTimeClock
	{
		private L1GameTime currentTime = new L1GameTime();

		private L1GameTime previousTime = null;

		private readonly HashSet<IL1GameTimeListener> listeners = new HashSet<IL1GameTimeListener>();

		private bool IsFieldChanged(DateTimeFieldTypeEnum dateTimeFieldType)
		{
			return previousTime.GetValue(dateTimeFieldType) != currentTime.GetValue(dateTimeFieldType);
		}

		private void NotifyChanged()
		{
			IL1GameTimeListener[] array = this.listeners.ToArray();
			if (IsFieldChanged(DateTimeFieldTypeEnum.Month))
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].OnMonthChanged(currentTime);
				}
			}
			if (IsFieldChanged(DateTimeFieldTypeEnum.Day))
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].OnDayChanged(currentTime);
				}
			}
			if (IsFieldChanged(DateTimeFieldTypeEnum.Hour))
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].OnHourChanged(currentTime);
				}
			}
			if (IsFieldChanged(DateTimeFieldTypeEnum.Minute))
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].OnMinuteChanged(currentTime);
				}
			}
		}

		public virtual L1GameTime CurrentTime()
		{
			return currentTime;
		}

		public virtual void AddListener(IL1GameTimeListener listener)
		{
			this.listeners.Add(listener);
		}

		public virtual void RemoveListener(IL1GameTimeListener listener)
		{
			this.listeners.Remove(listener);
		}

		public void Initialize()
		{
			Container.Instance.Resolve<ITaskController>().execute(new TimeUpdater(this), 0, 500);
		}

		private class TimeUpdater : TimerTask
		{
			private readonly L1GameTimeClock outerInstance;

			public TimeUpdater(L1GameTimeClock outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public void run()
			{
				outerInstance.previousTime = outerInstance.currentTime;
				outerInstance.currentTime = new L1GameTime();
				outerInstance.NotifyChanged();
			}
		}
	}

}