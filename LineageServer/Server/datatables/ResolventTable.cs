using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
	sealed class ResolventTable
	{
		private readonly static IDataSource dataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.Resolvent);

		private static ResolventTable _instance;

		private readonly IDictionary<int, int> _resolvent = MapFactory.NewMap<int, int>();

		public static ResolventTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ResolventTable();
				}
				return _instance;
			}
		}

		private ResolventTable()
		{
			loadMapsFromDatabase();
		}

		private void loadMapsFromDatabase()
		{
			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];

				int itemId = dataSourceRow.getInt(Resolvent.Column_item_id);
				int crystalCount = dataSourceRow.getInt(Resolvent.Column_crystal_count);
				_resolvent[itemId] = crystalCount;
			}
		}

		public int getCrystalCount(int itemId)
		{
			if (_resolvent.ContainsKey(itemId))
			{
				return _resolvent[itemId];
			}
			else
			{
				return 0;
			}
		}
	}
}