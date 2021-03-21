using System;
using System.Collections.Generic;
using System.Threading;

/*
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA
 * 02111-1307, USA.
 *
 * http://www.gnu.org/copyleft/gpl.html
 */
namespace LineageServer.william
{

	using Config = LineageServer.Server.Config;
	using ClientThread = LineageServer.Server.Server.ClientThread;
	using GameServer = LineageServer.Server.Server.GameServer;
	using GeneralThreadPool = LineageServer.Server.Server.GeneralThreadPool;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1GameTime = LineageServer.Server.Server.Model.gametime.L1GameTime;
	using L1GameTimeListener = LineageServer.Server.Server.Model.gametime.L1GameTimeListener;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unused") public class L1GameReStart
	public class L1GameReStart
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1GameReStart).FullName);

		private static L1GameReStart _instance;
		private volatile L1GameTime _currentTime = new L1GameTime();
		private L1GameTime _previousTime = null;

		private IList<L1GameTimeListener> _listeners = new CopyOnWriteArrayList<L1GameTimeListener>();

		private static int willRestartTime;
		public int _remnant;

		private class TimeUpdaterRestar : IRunnableStart
		{
			private readonly L1GameReStart outerInstance;

			public TimeUpdaterRestar(L1GameReStart outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void run()
			{
				while (true)
				{
					outerInstance._previousTime = outerInstance._currentTime;
					outerInstance._currentTime = new L1GameTime();
					outerInstance.notifyChanged();
					int remnant = outerInstance.GetRestartTime() * 60;
					  Console.WriteLine("【讀取】 【自動重啟】 【設定】【完成】【" + outerInstance.GetRestartTime() + "】【分鐘】。");
					while (remnant > 0)
					{
						for (int i = remnant ; i >= 0 ; i--)
						{
							outerInstance.SetRemnant(i);
							willRestartTime = i;
							if (i % 60 == 0 && i <= 300 && i != 0)
							{
								outerInstance.BroadCastToAll("伺服器將於 " + i / 60 + " 分鐘後自動重啟,請至安全區域準備登出。");
								Console.WriteLine("伺服器將於 " + i / 60 + " 分鐘後重新啟動");
							} //TODO if (五分鐘內 一分鐘一次)
							else if (i <= 30 && i != 0)
							{
								outerInstance.BroadCastToAll("伺服器將於 " + i + "秒後重新啟動,煩請儘速下線！");
								Console.WriteLine("伺服器將於 " + i + " 秒後重新啟動");
							} //TODO else if (30秒內 一秒一次)
							else if (i == 0)
							{
								outerInstance.BroadCastToAll("伺服器自動重啟。");
								Console.WriteLine("伺服器重新啟動。");
								GameServer.Instance.shutdown(); //TODO 修正自動重開角色資料會回溯
								outerInstance.disconnectAllCharacters();
								Environment.Exit(1);
							} //TODO if (1秒)
							try
							{
								Thread.Sleep(1000);
							}
							catch (InterruptedException e)
							{
								_log.log(Enum.Level.Server, e.Message, e);
							}
						}
					}
				}
			}
		}

		public virtual void disconnectAllCharacters()
		{
			ICollection<L1PcInstance> players = L1World.Instance.AllPlayers;
			foreach (L1PcInstance pc in players)
			{
				pc.NetConnection.ActiveChar = null;
				pc.NetConnection.kick();
			}
			// 全員Kickした後に保存処理をする
			foreach (L1PcInstance pc in players)
			{
				ClientThread.quitGame(pc);
				L1World.Instance.removeObject(pc);
			}
		}

		private int GetRestartTime()
		{
			return Config.REST_TIME;
		}

		private void BroadCastToAll(string @string)
		{
			ICollection<L1PcInstance> allpc = L1World.Instance.AllPlayers;
			foreach (L1PcInstance pc in allpc)
			{
				pc.sendPackets(new S_SystemMessage(@string));
			}
		}

		public virtual void SetRemnant(int remnant)
		{
			_remnant = remnant;
		}

		public static int WillRestartTime
		{
			get
			{
				return willRestartTime;
			}
		}

		public virtual int GetRemnant()
		{
			return _remnant;
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

		private L1GameReStart()
		{
			GeneralThreadPool.Instance.execute(new TimeUpdaterRestar(this));
		}

		public static void init()
		{
			_instance = new L1GameReStart();
		}

		public static L1GameReStart Instance
		{
			get
			{
				return _instance;
			}
		}

		public virtual L1GameTime GameTime
		{
			get
			{
				return _currentTime;
			}
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