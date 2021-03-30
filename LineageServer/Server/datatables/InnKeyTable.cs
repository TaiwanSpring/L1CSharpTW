using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using System.Collections.Generic;

namespace LineageServer.Server.DataTables
{
    class InnKeyTable
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.InnKey);
        private InnKeyTable()
        {
        }

        public static void StoreKey(L1ItemInstance item)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Insert()
            .Set(InnKey.Column_item_obj_id, item.Id)
            .Set(InnKey.Column_key_id, item.KeyId)
            .Set(InnKey.Column_npc_id, item.InnNpcId)
            .Set(InnKey.Column_hall, item.checkRoomOrHall())
            .Set(InnKey.Column_due_time, item.DueTime)
            .Execute();
        }

        public static void DeleteKey(L1ItemInstance item)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Delete()
            .Where(InnKey.Column_item_obj_id, item.Id)
            .Execute();
        }

        public static bool checkey(L1ItemInstance item)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Select()
            .Where(InnKey.Column_item_obj_id, item.Id)
            .Execute();

            return dataSourceRow.HaveData;
        }
    }
}