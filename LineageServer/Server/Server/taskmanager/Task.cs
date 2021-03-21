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

	using Config = LineageServer.Server.Config;
	using ExecutedTask = LineageServer.Server.Server.taskmanager.TaskManager.ExecutedTask;

	/// <summary>
	/// @author Layane
	/// 
	/// </summary>
	public abstract class Task
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(Task).FullName);

		public virtual void initializate()
		{
			if (Config.DEBUG)
			{
				_log.info("Task" + Name + " inializate");
			}
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: public java.util.concurrent.ScheduledFuture<?> launchSpecial(l1j.server.server.taskmanager.TaskManager.ExecutedTask instance)
		public virtual ScheduledFuture<object> launchSpecial(ExecutedTask instance)
		{
			return null;
		}

		public abstract string Name {get;}

		public abstract void onTimeElapsed(ExecutedTask task);

		public virtual void onDestroy()
		{
		}
	}

}