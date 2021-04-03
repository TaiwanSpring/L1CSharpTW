using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Types;
using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Server.Model.trap
{
	class L1WorldTraps
	{
		private readonly static IDataSource dataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.SpawnlistTrap);

		private IList<L1TrapInstance> _allTraps = ListFactory.NewList<L1TrapInstance>();

		private IList<L1TrapInstance> _allBases = ListFactory.NewList<L1TrapInstance>();

		private static L1WorldTraps _instance;

		private HashSet<ITimerTask> activeTrapSet = new HashSet<ITimerTask>();

		private L1WorldTraps()
		{
			
		}
		public void Initialize()
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
			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];
				int trapId = dataSourceRow.getInt(SpawnlistTrap.Column_trapId);
				L1Trap trapTemp = TrapTable.Instance.getTemplate(trapId);
				L1Location loc = new L1Location();
				loc.setMap(dataSourceRow.getInt(SpawnlistTrap.Column_mapId));
				loc.X = dataSourceRow.getInt(SpawnlistTrap.Column_locX);
				loc.Y = dataSourceRow.getInt(SpawnlistTrap.Column_locY);
				Point rndPt = new Point();
				rndPt.X = dataSourceRow.getInt(SpawnlistTrap.Column_locRndX);
				rndPt.Y = dataSourceRow.getInt(SpawnlistTrap.Column_locRndY);
				int count = dataSourceRow.getInt(SpawnlistTrap.Column_count);
				int span = dataSourceRow.getInt(SpawnlistTrap.Column_span);

				for (int j = 0; j < count; j++)
				{
					L1TrapInstance trap = new L1TrapInstance(Container.Instance.Resolve<IIdFactory>().nextId(), trapTemp, loc, rndPt, span);
					Container.Instance.Resolve<IGameWorld>().addVisibleObject(trap);
					_allTraps.Add(trap);
				}

				L1TrapInstance trapBase = new L1TrapInstance(Container.Instance.Resolve<IIdFactory>().nextId(), loc);
				Container.Instance.Resolve<IGameWorld>().addVisibleObject(trapBase);
				_allBases.Add(trapBase);
			}
		}

		public static void reloadTraps()
		{
			TrapTable.reload();

			_instance.resetTimer();

			removeTraps(_instance._allTraps);

			removeTraps(_instance._allBases);

			_instance = new L1WorldTraps();
		}

		private static void removeTraps(IList<L1TrapInstance> traps)
		{
			foreach (L1TrapInstance trap in traps)
			{
				trap.disableTrap();
				Container.Instance.Resolve<IGameWorld>().removeVisibleObject(trap);
			}
		}

		private void resetTimer()
		{
			lock (this)
			{
				foreach (ITimerTask item in this.activeTrapSet)
				{
					item.cancel();
				}
				this.activeTrapSet.Clear();
			}
		}

		private void disableTrap(L1TrapInstance trap)
		{
			trap.disableTrap();

			lock (this)
			{
				ITimerTask timerTask = new TrapSpawnTimer(trap);
				this.activeTrapSet.Add(timerTask);
				Container.Instance.Resolve<ITaskController>().execute((IRunnable)timerTask, trap.Span);
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
				if (trap.Enable && loc == trap.Location)
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
			internal readonly L1TrapInstance _targetTrap;

			public TrapSpawnTimer(L1TrapInstance trap)
			{
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