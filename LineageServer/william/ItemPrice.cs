using LineageServer.Interfaces;
using LineageServer.Models;
using System.Collections.Generic;

namespace LineageServer.william
{
    class ItemPrice
    {
        private static ILogger _log = Logger.GetLogger(nameof(ItemPrice));

        private static ItemPrice _instance;

        private readonly Dictionary<int, L1WilliamItemPrice> _itemIdIndex = new Dictionary<int, L1WilliamItemPrice>();

        public static ItemPrice Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ItemPrice();
                }
                return _instance;
            }
        }

        private ItemPrice()
        {
            loadItemPrice();
        }

        private void loadItemPrice()
        {
            IList<IDataSourceRow> dataSourceRows =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.WilliamItemPrice)
            .Select()
            .Query();
            fillItemPrice(dataSourceRows);
        }
        private void fillItemPrice(IList<IDataSourceRow> dataSourceRows)
        {
            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];

                int itemId = dataSourceRow.getInt("item_id");

                int price = dataSourceRow.getInt("price");

                L1WilliamItemPrice item_price = new L1WilliamItemPrice(itemId, price);

                _itemIdIndex[itemId] = item_price;
            }
        }

        public virtual L1WilliamItemPrice getTemplate(int itemId)
        {
            return _itemIdIndex[itemId];
        }
    }

}