using LineageServer.Interfaces;
using LineageServer.Server.Server.model;
using LineageServer.Server.Server.Model.monitor;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LineageServer.Server.Server
{
    class GeneralThreadPool
    {
        private static GeneralThreadPool _instance;

        private const int SCHEDULED_CORE_POOL_SIZE = 10;

        private RunnableExecuter _scheduler; // 通用的ScheduledExecutorService

        private RunnableExecuter _pcScheduler; // 監測玩家專用的ScheduledExecutorService

        // (AutoUpdate:約6ms,ExpMonitor:極小)
        private readonly int _pcSchedulerPoolSize = 1 + Config.MAX_ONLINE_USERS / 20; // 每
                                                                                      // 20
                                                                                      // 人增加一個
                                                                                      // PoolSize

        public static GeneralThreadPool Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GeneralThreadPool();
                }
                return _instance;
            }
        }

        private GeneralThreadPool()
        {
            if (Config.THREAD_P_TYPE_GENERAL == 1)
            {
                ThreadPool.SetMaxThreads(Config.THREAD_P_SIZE_GENERAL, Config.THREAD_P_SIZE_GENERAL);
            }
            else if (Config.THREAD_P_TYPE_GENERAL == 2)
            {

            }
            else
            {

            }
        }

        public void Execute(IRunnable r)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((state) => r.run()));
        }

        //JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
        //ORIGINAL LINE: public java.util.concurrent.ScheduledFuture<?> schedule(Runnable r, long delay)
        public virtual ScheduledFuture<object> schedule(ThreadStart r, long delay)
        {
            try
            {
                if (delay <= 0)
                {
                    _executor.execute(r);
                    return null;
                }
                return _scheduler.schedule(r, delay, TimeUnit.MILLISECONDS);
            }
            catch (RejectedExecutionException)
            {
                return null;
            }
        }

        //JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
        //ORIGINAL LINE: public java.util.concurrent.ScheduledFuture<?> scheduleAtFixedRate(Runnable r, long initialDelay, long period)
        public virtual ScheduledFuture<object> scheduleAtFixedRate(ThreadStart r, long initialDelay, long period)
        {
            return _scheduler.scheduleAtFixedRate(r, initialDelay, period, TimeUnit.MILLISECONDS);
        }

        //JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
        //ORIGINAL LINE: public java.util.concurrent.ScheduledFuture<?> pcSchedule(l1j.server.server.model.monitor.L1PcMonitor r, long delay)
        public virtual CancellationTokenSource pcSchedule(L1PcMonitor r, int delay)
        {

            try
            {
                if (delay <= 0)
                {
                    execute(r);
                    return null;
                }
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                try
                {
                    Task.Delay(delay, cancellationTokenSource.Token).ContinueWith(task => r.run());
                }
                catch (TaskCanceledException)
                { }

                return cancellationTokenSource;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public virtual ICancel pcScheduleAtFixedRate(ICancel r, int initialDelay, int period)
        {
            _pcScheduler.scheduleAtFixedRate(r, initialDelay, period);

            return r;
        }
    }

}