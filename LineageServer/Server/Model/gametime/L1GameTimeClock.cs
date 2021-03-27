using LineageServer.Enum;
using LineageServer.Interfaces;
using LineageServer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace LineageServer.Server.Model.Gametime
{
    public class L1GameTimeClock
    {
        private static L1GameTimeClock _instance;
        public static L1GameTimeClock Instance
        {
            get
            {
                return _instance;
            }
        }
        public static void Init()
        {
            _instance = new L1GameTimeClock();
        }

        private L1GameTime currentTime = new L1GameTime();

        private L1GameTime previousTime = null;

        private HashSet<IL1GameTimeListener> listeners = new HashSet<IL1GameTimeListener>();

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

        private L1GameTimeClock()
        {
            RunnableExecuter.Instance.execute(new TimeUpdater(this));
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
                    outerInstance.previousTime = outerInstance.currentTime;
                    outerInstance.currentTime = new L1GameTime();
                    outerInstance.NotifyChanged();
                    Thread.Sleep(500);
                }
            }
        }
    }

}