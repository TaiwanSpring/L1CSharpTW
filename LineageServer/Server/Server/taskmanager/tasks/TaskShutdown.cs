using System;

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
namespace LineageServer.Server.Server.taskmanager.tasks
{
	using Shutdown = LineageServer.Server.Server.Shutdown;
	using Task = LineageServer.Server.Server.taskmanager.Task;
	using ExecutedTask = LineageServer.Server.Server.taskmanager.TaskManager.ExecutedTask;

	/// <summary>
	/// @author Layane
	/// 
	/// </summary>
	public class TaskShutdown : Task
	{
		public static string NAME = "shutdown";

		/*
		 * (non-Javadoc)
		 * 
		 * @see l1j.server.server.taskmanager.Task#getName()
		 */
		public override string Name
		{
			get
			{
				return NAME;
			}
		}

		/*
		 * (non-Javadoc)
		 * 
		 * @see l1j.server.server.taskmanager.Task#onTimeElapsed(l1j.server.server.taskmanager.TaskManager.ExecutedTask)
		 */
		public override void onTimeElapsed(ExecutedTask task)
		{
			Shutdown handler = new Shutdown(Convert.ToInt32(task.Params[2]), false);
			handler.Start();
		}

	}

}