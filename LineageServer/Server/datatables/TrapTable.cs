using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model.trap;
using LineageServer.Server.Storage;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
	class TrapTable
	{
		private readonly static IDataSource dataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.Trap);

		private static TrapTable _instance;

		private IDictionary<int, L1Trap> _traps = MapFactory.NewMap<int, L1Trap>();

		private TrapTable()
		{
			initialize();
		}

		private L1Trap createTrapInstance(string name, ITrapStorage storage)
		{
			switch (name)
			{
				case nameof(L1DamageTrap):
				{
					return new L1DamageTrap(storage);
				}
				case nameof(L1HealingTrap):
				{
					return new L1HealingTrap(storage);
				}
				case nameof(L1MonsterTrap):
				{
					return new L1MonsterTrap(storage);
				}
				case nameof(L1PoisonTrap):
				{
					return new L1PoisonTrap(storage);
				}
				case nameof(L1SkillTrap):
				{
					return new L1SkillTrap(storage);
				}
				case nameof(L1TeleportTrap):
				{
					return new L1TeleportTrap(storage);
				}
				default:
					return L1Trap.newNull();
			}
		}

		private void initialize()
		{
			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];

				string typeName = dataSourceRow.getString(Trap.Column_type);

				L1Trap trap = createTrapInstance(typeName, new SqlTrapStorage(dataSourceRow));

				_traps[trap.Id] = trap;
			}
		}

		public static TrapTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new TrapTable();
				}
				return _instance;
			}
		}

		public static void reload()
		{
			_instance._traps.Clear();
			_instance = new TrapTable();
		}

		public virtual L1Trap getTemplate(int id)
		{
			return _traps[id];
		}

		class SqlTrapStorage : ITrapStorage
		{
			private readonly IDataSourceRow dataSourceRow;

			public SqlTrapStorage(IDataSourceRow dataSourceRow)
			{
				this.dataSourceRow = dataSourceRow;
			}

			public virtual string getString(string name)
			{
				return this.dataSourceRow.getString(name);
			}

			public virtual int getInt(string name)
			{
				return this.dataSourceRow.getInt(name);
			}

			public virtual bool getBoolean(string name)
			{
				return this.dataSourceRow.getBoolean(name);
			}
		}
	}

}