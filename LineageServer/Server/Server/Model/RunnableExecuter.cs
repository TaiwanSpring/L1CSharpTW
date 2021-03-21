using LineageServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Collections.Concurrent;
using System.Threading;

namespace LineageServer.Server.Server.model
{
    class RunnableExecuter
    {
        class TimeData
        {
            public TimeSpan Interval;
            public DateTime ExecuteTime;
        }

        readonly ConcurrentDictionary<IRunnable, TimeData> mapping
            = new ConcurrentDictionary<IRunnable, TimeData>();

        bool isStoped;

        public RunnableExecuter()
        {
            Task.Run(() =>
            {
                while (!this.isStoped)
                {                    
                    DateTime now = DateTime.Now;
                    foreach (var item in this.mapping.ToArray())
                    {
                        if (now - item.Value.ExecuteTime > item.Value.Interval)
                        {
                            item.Value.ExecuteTime = now;
                            Task.Run(() => item.Key.run());
                        }
                    }
                    Thread.Sleep(10);
                }
            });
        }

        public void scheduleAtFixedRate(IRunnable runnable, int delay, int interval)
        {
            if (runnable == null)
            {
                Debug.Fail("");
            }
            else
            {
                if (this.mapping.ContainsKey(runnable))
                {
                    Debug.Fail("");
                    return;
                }
                else
                {
                    if (this.mapping.TryAdd(runnable,
                         new TimeData()
                         {
                             ExecuteTime = DateTime.Now.AddMilliseconds(delay - interval),
                             Interval = TimeSpan.FromMilliseconds(interval)
                         }))
                    {
                        if (runnable is ICancel cancel)
                        {
                            cancel.Cancel += Cancel_Cancel;
                        }
                    }
                    else
                    {
                        Debug.Fail("");
                    }
                }
            }
        }

        private void Cancel_Cancel(ICancel cancel)
        {
            if (this.mapping.TryRemove(cancel, out _))
            {
                cancel.Cancel -= Cancel_Cancel;
            }
            else
            {
                Debug.Fail("");
            }
        }

        public void Close()
        {
            this.isStoped = true;
        }
    }
}
