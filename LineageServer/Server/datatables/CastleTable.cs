using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;
using System.Linq;
namespace LineageServer.Server.DataTables
{
    class CastleTable
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.Castle);
        private static CastleTable _instance;

        private readonly IDictionary<int, L1Castle> _castles = MapFactory.NewConcurrentMap<int, L1Castle>();

        public static CastleTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CastleTable();
                }
                return _instance;
            }
        }

        private CastleTable()
        {
            load();
        }

        private void load()
        {
            IDataSource clanData =
           Container.Instance.Resolve<IDataSourceFactory>()
           .Factory(Enum.DataSourceTypeEnum.ClanData);
            IList<IDataSourceRow> clanDatas = clanData.Select().Query();
            IDictionary<int, int> clanDatasMapping = new Dictionary<int, int>();
            for (int i = 0; i < clanDatas.Count; i++)
            {
                clanDatasMapping.Add(clanDatas[i].getInt(ClanData.Column_hascastle),
                    clanDatas[i].getInt(ClanData.Column_clan_id));
            }

            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];

                L1Castle castle = new L1Castle(dataSourceRow.getInt(Castle.Column_castle_id),
                    dataSourceRow.getString(Castle.Column_name));
                castle.WarTime = dataSourceRow.getTimestamp(Castle.Column_war_time);
                castle.TaxRate = dataSourceRow.getInt(Castle.Column_tax_rate);
                castle.PublicMoney = dataSourceRow.getInt(Castle.Column_public_money);
                if (clanDatasMapping.ContainsKey(castle.Id))
                {
                    castle.HeldClan = clanDatasMapping[castle.Id];
                }
                _castles[castle.Id] = castle;
            }
        }

        public virtual L1Castle[] CastleTableList
        {
            get
            {
                return _castles.Values.ToArray();
            }
        }

        public virtual L1Castle getCastleTable(int id)
        {
            return _castles[id];
        }

        public virtual void updateCastle(L1Castle castle)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Update()
            .Where(Castle.Column_castle_id, castle.Id)
            .Set(Castle.Column_name, castle.Name)
            .Set(Castle.Column_war_time, castle.WarTime)
            .Set(Castle.Column_tax_rate, castle.TaxRate)
            .Set(Castle.Column_public_money, castle.PublicMoney)
            .Execute();
            _castles[castle.Id] = castle;
        }
    }
}