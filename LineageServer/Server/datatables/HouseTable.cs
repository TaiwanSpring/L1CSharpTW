using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
namespace LineageServer.Server.DataTables
{
    class HouseTable
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.House);
        private static HouseTable _instance;

        private readonly IDictionary<int, L1House> _house = MapFactory.NewConcurrentMap<int, L1House>();

        public static HouseTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HouseTable();
                }
                return _instance;
            }
        }

        public HouseTable()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                L1House house = new L1House();
                house.HouseId = dataSourceRow.getInt(House.Column_house_id);
                house.HouseName = dataSourceRow.getString(House.Column_house_name);
                house.HouseArea = dataSourceRow.getInt(House.Column_house_area);
                house.Location = dataSourceRow.getString(House.Column_location);
                house.KeeperId = dataSourceRow.getInt(House.Column_keeper_id);
                house.OnSale = dataSourceRow.getInt(House.Column_is_on_sale) == 1 ? true : false;
                house.PurchaseBasement = dataSourceRow.getInt(House.Column_is_purchase_basement) == 1 ? true : false;
                house.TaxDeadline = dataSourceRow.getTimestamp(House.Column_tax_deadline);
                _house[house.HouseId] = house;
            }
        }

        public virtual L1House[] HouseTableList
        {
            get
            {
                return _house.Values.ToArray();
            }
        }

        public virtual L1House getHouseTable(int houseId)
        {
            return _house[houseId];
        }

        public virtual void updateHouse(L1House house)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Update()
            .Where(House.Column_house_id, house.HouseId)
            .Set(House.Column_house_name, house.HouseName)
            .Set(House.Column_house_area, house.HouseArea)
            .Set(House.Column_location, house.Location)
            .Set(House.Column_keeper_id, house.KeeperId)
            .Set(House.Column_is_on_sale, house.OnSale == true ? 1 : 0)
            .Set(House.Column_is_purchase_basement, house.PurchaseBasement == true ? 1 : 0)
            .Set(House.Column_tax_deadline, house.TaxDeadline)
            .Execute();
        }

        public static IList<int> HouseIdList
        {
            get
            {
                IList<int> houseIdList = ListFactory.NewList<int>();
                IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();
                for (int i = 0; i < dataSourceRows.Count; i++)
                {
                    IDataSourceRow dataSourceRow = dataSourceRows[i];
                    houseIdList.Add(dataSourceRow.getInt(House.Column_house_id));
                }
                return houseIdList;
            }
        }
    }

}