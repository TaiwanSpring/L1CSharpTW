using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model.shop;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
	class ShopTable
	{
		private readonly static IDataSource dataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.Shop);
		private static ShopTable _instance;

		private readonly IDictionary<int, L1Shop> _allShops = MapFactory.NewMap<int, L1Shop>();

		public static ShopTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ShopTable();
				}
				return _instance;
			}
		}

		private ShopTable()
		{
			loadShops();
		}

		private HashSet<int> enumNpcIds()
		{
			HashSet<int> ids = new HashSet<int>();

			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];
				int npcId = dataSourceRow.getInt(Shop.Column_npc_id);
				if (!ids.Contains(npcId))
				{
					ids.Add(npcId);
				}
			}
			return ids;
		}

		private L1Shop loadShop(int npcId, IList<IDataSourceRow> dataSourceRows)
		{
			IList<L1ShopItem> sellingList = ListFactory.NewList<L1ShopItem>();
			IList<L1ShopItem> purchasingList = ListFactory.NewList<L1ShopItem>();

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];
				int itemId = dataSourceRow.getInt(Shop.Column_item_id);
				int sellingPrice = dataSourceRow.getInt(Shop.Column_selling_price);
				int purchasingPrice = dataSourceRow.getInt(Shop.Column_purchasing_price);
				int packCount = dataSourceRow.getInt(Shop.Column_pack_count);
				packCount = packCount == 0 ? 1 : packCount;
				if (sellingPrice >= 0)
				{
					L1ShopItem item = new L1ShopItem(itemId, sellingPrice, packCount);
					sellingList.Add(item);
				}
				if (purchasingPrice >= 0)
				{
					L1ShopItem item = new L1ShopItem(itemId, purchasingPrice, packCount);
					purchasingList.Add(item);
				}
			}

			return new L1Shop(npcId, sellingList, purchasingList);
		}

		private void loadShops()
		{


			foreach (int npcId in enumNpcIds())
			{
				IList<IDataSourceRow> dataSourceRows =
					dataSource.Select()
					.Where(Shop.Column_npc_id, npcId)
					.OrderBy(Shop.Column_order_id).Query();

				L1Shop shop = loadShop(npcId, dataSourceRows);
				_allShops[npcId] = shop;
			}
		}

		public virtual L1Shop get(int npcId)
		{
			return _allShops[npcId];
		}
	}
}