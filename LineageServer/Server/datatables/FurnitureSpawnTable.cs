using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using System;
using System.Collections.Generic;

namespace LineageServer.Server.DataTables
{
    class FurnitureSpawnTable
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.SpawnlistFurniture);

        private static FurnitureSpawnTable _instance;

        public static FurnitureSpawnTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FurnitureSpawnTable();
                }
                return _instance;
            }
        }

        private FurnitureSpawnTable()
        {
            FillFurnitureSpawnTable();
        }

        private void FillFurnitureSpawnTable()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                L1Npc l1npc = NpcTable.Instance.getTemplate(dataSourceRow.getInt(SpawnlistFurniture.Column_npcid));
                if (l1npc != null)
                {
                    string s = l1npc.Impl;

                    if (L1NpcInstance.Factory(l1npc) is L1FurnitureInstance furniture)
                    {
                        furniture.Id = IdFactory.Instance.nextId();
                        furniture.ItemObjId = dataSourceRow.getInt(SpawnlistFurniture.Column_item_obj_id);
                        furniture.X = dataSourceRow.getInt(SpawnlistFurniture.Column_locx);
                        furniture.Y = dataSourceRow.getInt(SpawnlistFurniture.Column_locy);
                        furniture.MapId = (short)dataSourceRow.getInt(SpawnlistFurniture.Column_mapid);
                        furniture.HomeX = furniture.X;
                        furniture.HomeY = furniture.Y;
                        furniture.Heading = 0;
                        L1World.Instance.storeObject(furniture);
                        L1World.Instance.addVisibleObject(furniture);
                    }
                }
            }
        }

        public virtual void insertFurniture(L1FurnitureInstance furniture)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Insert()
            .Set(SpawnlistFurniture.Column_item_obj_id, furniture.ItemObjId)
            .Set(SpawnlistFurniture.Column_npcid, furniture.NpcTemplate.get_npcId())
            .Set(SpawnlistFurniture.Column_locx, furniture.X)
            .Set(SpawnlistFurniture.Column_locy, furniture.Y)
            .Set(SpawnlistFurniture.Column_mapid, furniture.MapId)
            .Execute();
        }

        public virtual void deleteFurniture(L1FurnitureInstance furniture)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Delete()
            .Where(SpawnlistFurniture.Column_item_obj_id, furniture.ItemObjId)
            .Execute();
        }
    }
}