﻿using System.Collections.Generic;

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
namespace LineageServer.Server.Model.trap
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using IdFactory = LineageServer.Server.IdFactory;
	using TrapTable = LineageServer.Server.DataSources.TrapTable;
	using L1Location = LineageServer.Server.Model.L1Location;
	using L1World = LineageServer.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using L1TrapInstance = LineageServer.Server.Model.Instance.L1TrapInstance;
	using Point = LineageServer.Server.Types.Point;
	using SQLUtil = LineageServer.Utils.SQLUtil;
	using ListFactory = LineageServer.Utils.ListFactory;

	public class L1WorldTraps
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(L1WorldTraps).FullName);

		private IList<L1TrapInstance> _allTraps = ListFactory.newList();

		private IList<L1TrapInstance> _allBases = ListFactory.newList();

		private Timer _timer = new Timer();

		private static L1WorldTraps _instance;

		private L1WorldTraps()
		{
			initialize();
		}

		public static L1WorldTraps Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new L1WorldTraps();
				}
				return _instance;
			}
		}

		private void initialize()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;

			try
			{
				con = L1DatabaseFactory.Instance.Connection;

				pstm = con.prepareStatement("SELECT * FROM spawnlist_trap");

				rs = pstm.executeQuery();

				while (rs.next())
				{
					int trapId = dataSourceRow.getInt("trapId");
					L1Trap trapTemp = TrapTable.Instance.getTemplate(trapId);
					L1Location loc = new L1Location();
					loc.setMap(dataSourceRow.getInt("mapId"));
					loc.X = dataSourceRow.getInt("locX");
					loc.Y = dataSourceRow.getInt("locY");
					Point rndPt = new Point();
					rndPt.X = dataSourceRow.getInt("locRndX");
					rndPt.Y = dataSourceRow.getInt("locRndY");
					int count = dataSourceRow.getInt("count");
					int span = dataSourceRow.getInt("span");

					for (int i = 0; i < count; i++)
					{
						L1TrapInstance trap = new L1TrapInstance(IdFactory.Instance.nextId(), trapTemp, loc, rndPt, span);
						L1World.Instance.addVisibleObject(trap);
						_allTraps.Add(trap);
					}
					L1TrapInstance @base = new L1TrapInstance(IdFactory.Instance.nextId(), loc);
					L1World.Instance.addVisibleObject(@base);
					_allBases.Add(@base);
				}

			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		public static void reloadTraps()
		{
			TrapTable.reload();
			L1WorldTraps oldInstance = _instance;
			_instance = new L1WorldTraps();
			oldInstance.resetTimer();
			removeTraps(oldInstance._allTraps);
			removeTraps(oldInstance._allBases);
		}

		private static void removeTraps(IList<L1TrapInstance> traps)
		{
			foreach (L1TrapInstance trap in traps)
			{
				trap.disableTrap();
				L1World.Instance.removeVisibleObject(trap);
			}
		}

		private void resetTimer()
		{
			lock (this)
			{
				_timer.cancel();
				_timer = new Timer();
			}
		}

		private void disableTrap(L1TrapInstance trap)
		{
			trap.disableTrap();

			lock (this)
			{
				_timer.schedule(new TrapSpawnTimer(this, trap), trap.Span);
			}
		}

		public virtual void resetAllTraps()
		{
			foreach (L1TrapInstance trap in _allTraps)
			{
				trap.resetLocation();
				trap.enableTrap();
			}
		}

		public virtual void onPlayerMoved(L1PcInstance player)
		{
			L1Location loc = player.Location;

			foreach (L1TrapInstance trap in _allTraps)
			{
				if (trap.Enable && loc.Equals(trap.Location))
				{
					trap.onTrod(player);
					disableTrap(trap);
				}
			}
		}

		public virtual void onDetection(L1PcInstance caster)
		{
			L1Location loc = caster.Location;

			foreach (L1TrapInstance trap in _allTraps)
			{
				if (trap.Enable && loc.isInScreen(trap.Location))
				{
					trap.onDetection(caster);
					disableTrap(trap);
				}
			}
		}

		private class TrapSpawnTimer : TimerTask
		{
			private readonly L1WorldTraps outerInstance;

			internal readonly L1TrapInstance _targetTrap;

			public TrapSpawnTimer(L1WorldTraps outerInstance, L1TrapInstance trap)
			{
				this.outerInstance = outerInstance;
				_targetTrap = trap;
			}

			public override void run()
			{
				_targetTrap.resetLocation();
				_targetTrap.enableTrap();
			}
		}
	}

}