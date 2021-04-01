using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;

namespace LineageServer.Server.DataTables
{
	class SpawnTimeTable
	{
		private readonly static IDataSource dataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.SpawnlistTime);

		private static SpawnTimeTable _instance;

		private readonly IDictionary<int, L1SpawnTime> _times = MapFactory.NewMap<int, L1SpawnTime>();

		public static SpawnTimeTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new SpawnTimeTable();
				}
				return _instance;
			}
		}

		private SpawnTimeTable()
		{
			load();
		}

		public virtual L1SpawnTime get(int id)
		{
			return _times[id];
		}

		private void load()
		{
			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];

				int id = dataSourceRow.getInt(SpawnlistTime.Column_spawn_id);
				L1SpawnTimeBuilder builder = new L1SpawnTimeBuilder(id);
				builder.TimeStart = dataSourceRow.getTimestamp(SpawnlistTime.Column_time_start);
				builder.TimeEnd = dataSourceRow.getTimestamp(SpawnlistTime.Column_time_end);
				// builder.setPeriodStart(dataSourceRow.getTimestamp("period_start"));
				// builder.setPeriodEnd(dataSourceRow.getTimestamp("period_end"));
				builder.IsDeleteAtEndTime = dataSourceRow.getBoolean(SpawnlistTime.Column_delete_at_endtime);
				_times[id] = builder.build();
			}
		}
	}
}