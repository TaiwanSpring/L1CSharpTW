using LineageServer.DataStruct;
using LineageServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LineageServer.Models
{
    class RunnableExecuter : ITaskController
    {
        public const int TicksPerMillisecond = 10_000;
        private readonly Dictionary<ITimerTask, TimerTaskArgs> timerTaskMapping = new Dictionary<ITimerTask, TimerTaskArgs>();
        private object locker = new object();
        private bool isStart;
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
            while (this.isStart)//無窮Loop
            {
                long currentTicks = DateTime.Now.Ticks;//現在時間
                IEnumerable<KeyValuePair<ITimerTask, TimerTaskArgs>> items;

                lock (timerTaskMapping)
                {
                    items = this.timerTaskMapping.ToArray();//Jobs
                }

                long ticks = currentTicks - oldTicks;//經過時間
                oldTicks = currentTicks;
                foreach (var item in items)//For loop
                {
                    if (item.Key.IsCancel)//已取消
                    {
                        this.timerTaskMapping.Remove(item.Key);
                    }
                    else
                    {
                        item.Value.Ticks = item.Value.Ticks - ticks;

                        if (item.Value.Ticks <= 0) // 檢查是否要執行
                        {
                            if (item.Value.Interval <= 0)// Interval 為0代表執行一次
                            {
                                this.timerTaskMapping.Remove(item.Key);
                            }
                            else
                            {
                                item.Value.Ticks = item.Value.Interval;
                            }
                            Task.Run(item.Key.run);
                        }
                    }
                }
            }
        }

        public void execute(Action action)
        {
            Task.Run(action);
        }
        public void execute(Action action, int delay)
        {
            Task.Delay(delay).ContinueWith(task => action);
        }
        public void execute(ITimerTask task, int delay, int period)
        {
            lock (timerTaskMapping)
            {
                this.timerTaskMapping.Add(task, new TimerTaskArgs()
                {
                    Ticks = delay * TicksPerMillisecond,
                    Interval = period * TicksPerMillisecond
                });
            }
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
