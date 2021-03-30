using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
	sealed class DropItemTable
	{
		private readonly static IDataSource dataSource =
			 Container.Instance.Resolve<IDataSourceFactory>()
			 .Factory(Enum.DataSourceTypeEnum.DropItem);
		private static DropItemTable _instance;

		private readonly IDictionary<int, dropItemData> _dropItem = MapFactory.NewMap<int, dropItemData>();

		public static DropItemTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new DropItemTable();
				}
				return _instance;
			}
		}

		private DropItemTable()
		{
			loadMapsFromDatabase();
		}

		private void loadMapsFromDatabase()
		{
			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];
				dropItemData data = new dropItemData();
				int itemId = dataSourceRow.getInt(DropItem.Column_item_id);
				data.dropRate = dataSourceRow.getDouble(DropItem.Column_drop_rate);
				data.dropAmount = dataSourceRow.getDouble(DropItem.Column_drop_amount);
				_dropItem[itemId] = data;
			}
		}

		public double getDropRate(int itemId)
		{
			if (_dropItem.ContainsKey(itemId))
			{
				return _dropItem[itemId].dropRate;
			}
			else
			{
				return 0;
			}
		}

		public double getDropAmount(int itemId)
		{
			if (_dropItem.ContainsKey(itemId))
			{
				return _dropItem[itemId].dropAmount;
			}
			else
			{
				return 0;
			}
		}
		struct dropItemData
		{
			public double dropRate;

			public double dropAmount;
		}

	}
}