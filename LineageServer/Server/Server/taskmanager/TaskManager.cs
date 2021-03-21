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
namespace LineageServer.Server.Server.taskmanager
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using TaskRestart = LineageServer.Server.Server.taskmanager.tasks.TaskRestart;
	using TaskShutdown = LineageServer.Server.Server.taskmanager.tasks.TaskShutdown;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	/// <summary>
	/// @author Layane
	/// 
	/// </summary>
	public sealed class TaskManager
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		protected internal static readonly Logger _log = Logger.getLogger(typeof(TaskManager).FullName);

		private static TaskManager _instance;

		protected internal static readonly string[] SQL_STATEMENTS = new string[] {"SELECT id,task,type,last_activation,param1,param2,param3 FROM global_tasks", "UPDATE global_tasks SET last_activation=? WHERE id=?", "SELECT id FROM global_tasks WHERE task=?", "INSERT INTO global_tasks (task,type,last_activation,param1,param2,param3) VALUES(?,?,?,?,?,?)"};

		private readonly IDictionary<int, Task> _tasks = Maps.newMap();

		protected internal readonly IList<ExecutedTask> _currentTasks = Lists.newList();

		public class ExecutedTask : IRunnableStart
		{
			private readonly TaskManager outerInstance;

			internal int _id;

			internal long _lastActivation;

			internal Task _task;

			internal TaskTypes _type;

			internal string[] _params;

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: java.util.concurrent.ScheduledFuture<?> _scheduled;
			internal ScheduledFuture<object> _scheduled;

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public ExecutedTask(Task task, TaskTypes type, java.sql.ResultSet rset) throws java.sql.SQLException
			public ExecutedTask(TaskManager outerInstance, Task task, TaskTypes type, ResultSet rset)
			{
				this.outerInstance = outerInstance;
				_task = task;
				_type = type;
				_id = rset.getInt("id");
				_lastActivation = rset.getLong("last_activation");
				_params = new string[] {rset.getString("param1"), rset.getString("param2"), rset.getString("param3")};
			}

			public override void run()
			{
				_task.onTimeElapsed(this);

				_lastActivation = DateTimeHelper.CurrentUnixTimeMillis();

				java.sql.IDataBaseConnection con = null;
				PreparedStatement pstm = null;
				try
				{
					con = L1DatabaseFactory.Instance.Connection;
					pstm = con.prepareStatement(SQL_STATEMENTS[1]);
					pstm.setLong(1, _lastActivation);
					pstm.setInt(2, _id);
					pstm.executeUpdate();
				}
				catch (SQLException e)
				{
					_log.warning("cannot updated the Global Task " + _id + ": " + e.Message);
				}
				finally
				{
					SQLUtil.close(pstm);
					SQLUtil.close(con);
				}

			}

			public override bool Equals(object @object)
			{
				return _id == ((ExecutedTask) @object)._id;
			}

			public virtual Task Task
			{
				get
				{
					return _task;
				}
			}

			public virtual TaskTypes Type
			{
				get
				{
					return _type;
				}
			}

			public virtual int Id
			{
				get
				{
					return _id;
				}
			}

			public virtual string[] Params
			{
				get
				{
					return _params;
				}
			}

			public virtual long LastActivation
			{
				get
				{
					return _lastActivation;
				}
			}

			public virtual void stopTask()
			{
				_task.onDestroy();

				if (_scheduled != null)
				{
					_scheduled.cancel(true);
				}

				outerInstance._currentTasks.Remove(this);
			}

		}

		public static TaskManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new TaskManager();
				}
				return _instance;
			}
		}

		public TaskManager()
		{
			initializate();
			startAllTasks();
		}

		private void initializate()
		{
			registerTask(new TaskRestart());
			registerTask(new TaskShutdown());
		}

		public void registerTask(Task task)
		{
			int key = task.Name.GetHashCode();
			if (!_tasks.ContainsKey(key))
			{
				_tasks[key] = task;
				task.initializate();
			}
		}

		private void startAllTasks()
		{
			java.sql.IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement(SQL_STATEMENTS[0]);
				rs = pstm.executeQuery();

				while (rs.next())
				{
					Task task = _tasks[rs.getString("task").Trim().ToLower().GetHashCode()];

					if (task == null)
					{
						continue;
					}

				}

			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, "error while loading Global Task table", e);
			}
			finally
			{
				if (null != rs)
				{
					try
					{
						rs.close();
					}
					catch (SQLException)
					{
						// ignore
					}
					rs = null;
				}

				if (null != pstm)
				{
					try
					{
						pstm.close();
					}
					catch (SQLException)
					{
						// ignore
					}
					pstm = null;
				}

				if (null != con)
				{
					try
					{
						con.close();
					}
					catch (SQLException)
					{
						// ignore
					}
					con = null;
				}
			}

		}

		public static bool addUniqueTask(string task, TaskTypes type, string param1, string param2, string param3)
		{
			return addUniqueTask(task, type, param1, param2, param3, 0);
		}

		public static bool addUniqueTask(string task, TaskTypes type, string param1, string param2, string param3, long lastActivation)
		{
			java.sql.IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;

			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement(SQL_STATEMENTS[2]);
				pstm.setString(1, task);
				rs = pstm.executeQuery();
				pstm.close();
				if (!rs.next())
				{
					pstm = con.prepareStatement(SQL_STATEMENTS[3]);
					pstm.setString(1, task);
					pstm.setString(2, type.ToString());
					pstm.setLong(3, lastActivation);
					pstm.setString(4, param1);
					pstm.setString(5, param2);
					pstm.setString(6, param3);
					pstm.execute();
				}

				return true;
			}
			catch (SQLException e)
			{
				_log.warning("cannot add the unique task: " + e.Message);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}

			return false;
		}

		public static bool addTask(string task, TaskTypes type, string param1, string param2, string param3)
		{
			return addTask(task, type, param1, param2, param3, 0);
		}

		public static bool addTask(string task, TaskTypes type, string param1, string param2, string param3, long lastActivation)
		{
			java.sql.IDataBaseConnection con = null;
			PreparedStatement pstm = null;

			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement(SQL_STATEMENTS[3]);
				pstm.setString(1, task);
				pstm.setString(2, type.ToString());
				pstm.setLong(3, lastActivation);
				pstm.setString(4, param1);
				pstm.setString(5, param2);
				pstm.setString(6, param3);
				pstm.execute();

				return true;
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, "cannot add the task", e);
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}

			return false;
		}

	}

}