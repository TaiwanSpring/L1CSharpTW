using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
	class FurnitureItemTable
	{
		private readonly static IDataSource dataSource =
		  Container.Instance.Resolve<IDataSourceFactory>()
		  .Factory(Enum.DataSourceTypeEnum.FurnitureItem);
		private static FurnitureItemTable _instance;

		private readonly IDictionary<int, L1FurnitureItem> _furnishings = MapFactory.NewMap<int, L1FurnitureItem>();

		public static FurnitureItemTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new FurnitureItemTable();
				}
				return _instance;
			}
		}

		private FurnitureItemTable()
		{
			load();
		}

		private void load()
		{
			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];
				L1FurnitureItem furniture = new L1FurnitureItem();
				int itemId = dataSourceRow.getInt(FurnitureItem.Column_item_id);
				furniture.FurnitureItemId = itemId;
				furniture.FurnitureNpcId = dataSourceRow.getInt(FurnitureItem.Column_npc_id);
				_furnishings[itemId] = furniture;
			}
		}

		public virtual L1FurnitureItem getTemplate(int itemId)
		{
			if (_furnishings.ContainsKey(itemId))
			{
				return _furnishings[itemId];
			}
			return null;
		}
	}
}