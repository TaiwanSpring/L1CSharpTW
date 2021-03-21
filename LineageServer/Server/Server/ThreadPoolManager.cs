using System;
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
namespace LineageServer.Server.Server
{

	using TextBuilder = javolution.text.TextBuilder;
	using Config = LineageServer.Server.Config;

	// import l1j.server.server.network.L2GameClient;

	public class ThreadPoolManager
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(ThreadPoolManager).FullName);

		private static ThreadPoolManager _instance;

		private readonly ScheduledThreadPoolExecutor _effectsScheduledThreadPool;

		private readonly ScheduledThreadPoolExecutor _generalScheduledThreadPool;

		private readonly ThreadPoolExecutor _generalPacketsThreadPool;

		private readonly ThreadPoolExecutor _ioPacketsThreadPool;

		// will be really used in the next AI implementation.
		private readonly ThreadPoolExecutor _aiThreadPool;

		private readonly ThreadPoolExecutor _generalThreadPool;

		// temp
		private readonly ScheduledThreadPoolExecutor _aiScheduledThreadPool;

		private bool _shutdown;

		public static ThreadPoolManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ThreadPoolManager();
				}
				return _instance;
			}
		}

		private ThreadPoolManager()
		{
			_effectsScheduledThreadPool = new ScheduledThreadPoolExecutor(Config.THREAD_P_EFFECTS, new PriorityThreadFactory(this, "EffectsSTPool", Thread.MIN_PRIORITY));
			_generalScheduledThreadPool = new ScheduledThreadPoolExecutor(Config.THREAD_P_GENERAL, new PriorityThreadFactory(this, "GerenalSTPool", Thread.NORM_PRIORITY));

			_ioPacketsThreadPool = new ThreadPoolExecutor(2, int.MaxValue, 5L, TimeUnit.SECONDS, new LinkedBlockingQueue<ThreadStart>(), new PriorityThreadFactory(this, "I/O Packet Pool", Thread.NORM_PRIORITY + 1));

			_generalPacketsThreadPool = new ThreadPoolExecutor(4, 6, 15L, TimeUnit.SECONDS, new LinkedBlockingQueue<ThreadStart>(), new PriorityThreadFactory(this, "Normal Packet Pool", Thread.NORM_PRIORITY + 1));

			_generalThreadPool = new ThreadPoolExecutor(2, 4, 5L, TimeUnit.SECONDS, new LinkedBlockingQueue<ThreadStart>(), new PriorityThreadFactory(this, "General Pool", Thread.NORM_PRIORITY));

			// will be really used in the next AI implementation.
			_aiThreadPool = new ThreadPoolExecutor(1, Config.AI_MAX_THREAD, 10L, TimeUnit.SECONDS, new LinkedBlockingQueue<ThreadStart>());

			_aiScheduledThreadPool = new ScheduledThreadPoolExecutor(Config.AI_MAX_THREAD, new PriorityThreadFactory(this, "AISTPool", Thread.NORM_PRIORITY));
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: public java.util.concurrent.ScheduledFuture<?> scheduleEffect(Runnable r, long delay)
		public virtual ScheduledFuture<object> scheduleEffect(ThreadStart r, long delay)
		{
			try
			{
				if (delay < 0)
				{
					delay = 0;
				}
				return _effectsScheduledThreadPool.schedule(r, delay, TimeUnit.MILLISECONDS);
			}
			catch (RejectedExecutionException)
			{
				return null; // shutdown, ignore
			}
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: public java.util.concurrent.ScheduledFuture<?> scheduleEffectAtFixedRate(Runnable r, long initial, long delay)
		public virtual ScheduledFuture<object> scheduleEffectAtFixedRate(ThreadStart r, long initial, long delay)
		{
			try
			{
				if (delay < 0)
				{
					delay = 0;
				}
				if (initial < 0)
				{
					initial = 0;
				}
				return _effectsScheduledThreadPool.scheduleAtFixedRate(r, initial, delay, TimeUnit.MILLISECONDS);
			}
			catch (RejectedExecutionException)
			{
				return null; // shutdown, ignore
			}
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: public java.util.concurrent.ScheduledFuture<?> scheduleGeneral(Runnable r, long delay)
		public virtual ScheduledFuture<object> scheduleGeneral(ThreadStart r, long delay)
		{
			try
			{
				if (delay < 0)
				{
					delay = 0;
				}
				return _generalScheduledThreadPool.schedule(r, delay, TimeUnit.MILLISECONDS);
			}
			catch (RejectedExecutionException)
			{
				return null; // shutdown, ignore
			}
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: public java.util.concurrent.ScheduledFuture<?> scheduleGeneralAtFixedRate(Runnable r, long initial, long delay)
		public virtual ScheduledFuture<object> scheduleGeneralAtFixedRate(ThreadStart r, long initial, long delay)
		{
			try
			{
				if (delay < 0)
				{
					delay = 0;
				}
				if (initial < 0)
				{
					initial = 0;
				}
				return _generalScheduledThreadPool.scheduleAtFixedRate(r, initial, delay, TimeUnit.MILLISECONDS);
			}
			catch (RejectedExecutionException)
			{
				return null; // shutdown, ignore
			}
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: public java.util.concurrent.ScheduledFuture<?> scheduleAi(Runnable r, long delay)
		public virtual ScheduledFuture<object> scheduleAi(ThreadStart r, long delay)
		{
			try
			{
				if (delay < 0)
				{
					delay = 0;
				}
				return _aiScheduledThreadPool.schedule(r, delay, TimeUnit.MILLISECONDS);
			}
			catch (RejectedExecutionException)
			{
				return null; // shutdown, ignore
			}
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: public java.util.concurrent.ScheduledFuture<?> scheduleAiAtFixedRate(Runnable r, long initial, long delay)
		public virtual ScheduledFuture<object> scheduleAiAtFixedRate(ThreadStart r, long initial, long delay)
		{
			try
			{
				if (delay < 0)
				{
					delay = 0;
				}
				if (initial < 0)
				{
					initial = 0;
				}
				return _aiScheduledThreadPool.scheduleAtFixedRate(r, initial, delay, TimeUnit.MILLISECONDS);
			}
			catch (RejectedExecutionException)
			{
				return null; // shutdown, ignore
			}
		}

		/*
		 * public void executePacket(ReceivablePacket<L2GameClient> pkt) {
		 * _generalPacketsThreadPool.execute(pkt); }
		 * 
		 * public void executeIOPacket(ReceivablePacket<L2GameClient> pkt) {
		 * _ioPacketsThreadPool.execute(pkt); }
		 */
		public void ExecuteTask(ThreadStart r)
		{
			_generalThreadPool.execute(r);
		}

		public void ExecuteAi(ThreadStart r)
		{
			_aiThreadPool.execute(r);
		}

		public virtual string[] Stats
		{
			get
			{
				return new string[] {"STP:", " + Effects:", " |- ActiveThreads:   " + _effectsScheduledThreadPool.ActiveCount, " |- getCorePoolSize: " + _effectsScheduledThreadPool.CorePoolSize, " |- PoolSize:        " + _effectsScheduledThreadPool.PoolSize, " |- MaximumPoolSize: " + _effectsScheduledThreadPool.MaximumPoolSize, " |- CompletedTasks:  " + _effectsScheduledThreadPool.CompletedTaskCount, " |- ScheduledTasks:  " + (_effectsScheduledThreadPool.TaskCount - _effectsScheduledThreadPool.CompletedTaskCount), " | -------", " + General:", " |- ActiveThreads:   " + _generalScheduledThreadPool.ActiveCount, " |- getCorePoolSize: " + _generalScheduledThreadPool.CorePoolSize, " |- PoolSize:        " + _generalScheduledThreadPool.PoolSize, " |- MaximumPoolSize: " + _generalScheduledThreadPool.MaximumPoolSize, " |- CompletedTasks:  " + _generalScheduledThreadPool.CompletedTaskCount, " |- ScheduledTasks:  " + (_generalScheduledThreadPool.TaskCount - _generalScheduledThreadPool.CompletedTaskCount), " | -------", " + AI:", " |- ActiveThreads:   " + _aiScheduledThreadPool.ActiveCount, " |- getCorePoolSize: " + _aiScheduledThreadPool.CorePoolSize, " |- PoolSize:        " + _aiScheduledThreadPool.PoolSize, " |- MaximumPoolSize: " + _aiScheduledThreadPool.MaximumPoolSize, " |- CompletedTasks:  " + _aiScheduledThreadPool.CompletedTaskCount, " |- ScheduledTasks:  " + (_aiScheduledThreadPool.TaskCount - _aiScheduledThreadPool.CompletedTaskCount), "TP:", " + Packets:", " |- ActiveThreads:   " + _generalPacketsThreadPool.ActiveCount, " |- getCorePoolSize: " + _generalPacketsThreadPool.CorePoolSize, " |- MaximumPoolSize: " + _generalPacketsThreadPool.MaximumPoolSize, " |- LargestPoolSize: " + _generalPacketsThreadPool.LargestPoolSize, " |- PoolSize:        " + _generalPacketsThreadPool.PoolSize, " |- CompletedTasks:  " + _generalPacketsThreadPool.CompletedTaskCount, " |- QueuedTasks:     " + _generalPacketsThreadPool.Queue.size(), " | -------", " + I/O Packets:", " |- ActiveThreads:   " + _ioPacketsThreadPool.ActiveCount, " |- getCorePoolSize: " + _ioPacketsThreadPool.CorePoolSize, " |- MaximumPoolSize: " + _ioPacketsThreadPool.MaximumPoolSize, " |- LargestPoolSize: " + _ioPacketsThreadPool.LargestPoolSize, " |- PoolSize:        " + _ioPacketsThreadPool.PoolSize, " |- CompletedTasks:  " + _ioPacketsThreadPool.CompletedTaskCount, " |- QueuedTasks:     " + _ioPacketsThreadPool.Queue.size(), " | -------", " + General Tasks:", " |- ActiveThreads:   " + _generalThreadPool.ActiveCount, " |- getCorePoolSize: " + _generalThreadPool.CorePoolSize, " |- MaximumPoolSize: " + _generalThreadPool.MaximumPoolSize, " |- LargestPoolSize: " + _generalThreadPool.LargestPoolSize, " |- PoolSize:        " + _generalThreadPool.PoolSize, " |- CompletedTasks:  " + _generalThreadPool.CompletedTaskCount, " |- QueuedTasks:     " + _generalThreadPool.Queue.size(), " | -------", " + AI:", " |- Not Done"};
			}
		}

		private class PriorityThreadFactory : IRunnableFactory
		{
			private readonly ThreadPoolManager outerInstance;

			internal readonly int _prio;

			internal readonly string _name;

			internal readonly AtomicInteger _threadNumber = new AtomicInteger(1);

			internal readonly ThreadGroup _group;

			public PriorityThreadFactory(ThreadPoolManager outerInstance, string name, int prio)
			{
				this.outerInstance = outerInstance;
				_prio = prio;
				_name = name;
				_group = new ThreadGroup(_name);
			}

			/*
			 * (non-Javadoc)
			 * 
			 * @see java.util.concurrent.ThreadFactory#newThread(java.lang.Runnable)
			 */
			public override Thread newThread(ThreadStart r)
			{
				Thread t = new Thread(_group, r);
				t.Name = _name + "-" + _threadNumber.AndIncrement;
				t.Priority = _prio;
				return t;
			}

			public virtual ThreadGroup Group
			{
				get
				{
					return _group;
				}
			}
		}

		/// 
		public virtual void shutdown()
		{
			_shutdown = true;
			try
			{
				_effectsScheduledThreadPool.awaitTermination(1, TimeUnit.SECONDS);
				_generalScheduledThreadPool.awaitTermination(1, TimeUnit.SECONDS);
				_generalPacketsThreadPool.awaitTermination(1, TimeUnit.SECONDS);
				_ioPacketsThreadPool.awaitTermination(1, TimeUnit.SECONDS);
				_generalThreadPool.awaitTermination(1, TimeUnit.SECONDS);
				_aiThreadPool.awaitTermination(1, TimeUnit.SECONDS);
				_effectsScheduledThreadPool.shutdown();
				_generalScheduledThreadPool.shutdown();
				_generalPacketsThreadPool.shutdown();
				_ioPacketsThreadPool.shutdown();
				_generalThreadPool.shutdown();
				_aiThreadPool.shutdown();
                System.Console.WriteLine("All ThreadPools are now stoped");

			}
			catch (InterruptedException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);

			}
		}

		public virtual bool Shutdown
		{
			get
			{
				return _shutdown;
			}
		}

		/// 
		public virtual void purge()
		{
			_effectsScheduledThreadPool.purge();
			_generalScheduledThreadPool.purge();
			_aiScheduledThreadPool.purge();
			_ioPacketsThreadPool.purge();
			_generalPacketsThreadPool.purge();
			_generalThreadPool.purge();
			_aiThreadPool.purge();
		}

		/// 
		public virtual string PacketStats
		{
			get
			{
				TextBuilder tb = new TextBuilder();
				ThreadFactory tf = _generalPacketsThreadPool.ThreadFactory;
				if (tf is PriorityThreadFactory)
				{
					tb.append("General Packet Thread Pool:\r\n");
					tb.append("Tasks in the queue: " + _generalPacketsThreadPool.Queue.size() + "\r\n");
					tb.append("Showing threads stack trace:\r\n");
					PriorityThreadFactory ptf = (PriorityThreadFactory) tf;
					int count = ptf.Group.activeCount();
					Thread[] threads = new Thread[count + 2];
					ptf.Group.enumerate(threads);
					tb.append("There should be " + count + " Threads\r\n");
					foreach (Thread t in threads)
					{
						if (t == null)
						{
							continue;
						}
						tb.append(t.Name + "\r\n");
						foreach (StackTraceElement ste in t.StackTrace)
						{
							tb.append(ste.ToString());
							tb.append("\r\n");
						}
					}
				}
				tb.append("Packet Tp stack traces printed.\r\n");
				return tb.ToString();
			}
		}

		public virtual string IOPacketStats
		{
			get
			{
				TextBuilder tb = new TextBuilder();
				ThreadFactory tf = _ioPacketsThreadPool.ThreadFactory;
				if (tf is PriorityThreadFactory)
				{
					tb.append("I/O Packet Thread Pool:\r\n");
					tb.append("Tasks in the queue: " + _ioPacketsThreadPool.Queue.size() + "\r\n");
					tb.append("Showing threads stack trace:\r\n");
					PriorityThreadFactory ptf = (PriorityThreadFactory) tf;
					int count = ptf.Group.activeCount();
					Thread[] threads = new Thread[count + 2];
					ptf.Group.enumerate(threads);
					tb.append("There should be " + count + " Threads\r\n");
					foreach (Thread t in threads)
					{
						if (t == null)
						{
							continue;
						}
						tb.append(t.Name + "\r\n");
						foreach (StackTraceElement ste in t.StackTrace)
						{
							tb.append(ste.ToString());
							tb.append("\r\n");
						}
					}
				}
				tb.append("Packet Tp stack traces printed.\r\n");
				return tb.ToString();
			}
		}

		public virtual string GeneralStats
		{
			get
			{
				TextBuilder tb = new TextBuilder();
				ThreadFactory tf = _generalThreadPool.ThreadFactory;
				if (tf is PriorityThreadFactory)
				{
					tb.append("General Thread Pool:\r\n");
					tb.append("Tasks in the queue: " + _generalThreadPool.Queue.size() + "\r\n");
					tb.append("Showing threads stack trace:\r\n");
					PriorityThreadFactory ptf = (PriorityThreadFactory) tf;
					int count = ptf.Group.activeCount();
					Thread[] threads = new Thread[count + 2];
					ptf.Group.enumerate(threads);
					tb.append("There should be " + count + " Threads\r\n");
					foreach (Thread t in threads)
					{
						if (t == null)
						{
							continue;
						}
						tb.append(t.Name + "\r\n");
						foreach (StackTraceElement ste in t.StackTrace)
						{
							tb.append(ste.ToString());
							tb.append("\r\n");
						}
					}
				}
				tb.append("Packet Tp stack traces printed.\r\n");
				return tb.ToString();
			}
		}
	}
}