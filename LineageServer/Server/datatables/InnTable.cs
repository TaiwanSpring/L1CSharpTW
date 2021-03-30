using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
    class InnTable
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.Inn);
        class InnWarpper
        {
            public readonly IDictionary<int, L1Inn> _inn = MapFactory.NewMap<int, L1Inn>();
        }

        private static readonly IDictionary<int, InnWarpper> _dataMap = MapFactory.NewMap<int, InnWarpper>();

        private static InnTable _instance;

        public static InnTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InnTable();
                }
                return _instance;
            }
        }

        private InnTable()
        {
            load();
        }

        private void load()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            InnWarpper inn;
            L1Inn l1inn;
            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];

                int npcId = dataSourceRow.getInt(Inn.Column_npcid);

                if (!_dataMap.ContainsKey(npcId))
                {
                    inn = new InnWarpper();
                    _dataMap[npcId] = inn;
                }
                else
                {
                    inn = _dataMap[npcId];
                }

                l1inn = new L1Inn();
                l1inn.InnNpcId = npcId;
                l1inn.RoomNumber = dataSourceRow.getInt(Inn.Column_room_number);
                l1inn.KeyId = dataSourceRow.getInt(Inn.Column_key_id);
                l1inn.LodgerId = dataSourceRow.getInt(Inn.Column_lodger_id);
                l1inn.Hall = dataSourceRow.getBoolean(Inn.Column_hall);
                l1inn.DueTime = dataSourceRow.getTimestamp(Inn.Column_due_time);

                inn._inn[Convert.ToInt32(l1inn.RoomNumber)] = l1inn;
            }
        }

        public virtual void updateInn(L1Inn inn)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Update()
            .Where(Inn.Column_npcid, inn.InnNpcId)
            .Where(Inn.Column_room_number, inn.RoomNumber)
            .Set(Inn.Column_key_id, inn.KeyId)
            .Set(Inn.Column_lodger_id, inn.LodgerId)
            .Set(Inn.Column_hall, inn.Hall)
            .Set(Inn.Column_due_time, inn.DueTime)
            .Execute();
        }

        public virtual L1Inn getTemplate(int npcid, int roomNumber)
        {
            if (_dataMap.ContainsKey(npcid))
            {
                return _dataMap[npcid]._inn[roomNumber];
            }
            return null;
        }
    }

}