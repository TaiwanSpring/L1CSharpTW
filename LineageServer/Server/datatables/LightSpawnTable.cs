using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using System;
using System.Collections.Generic;

namespace LineageServer.Server.DataTables
{
    class LightSpawnTable
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.SpawnlistLight);
        private static LightSpawnTable _instance;

        public static LightSpawnTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LightSpawnTable();
                }
                return _instance;
            }
        }

        private LightSpawnTable()
        {
            FillLightSpawnTable();
        }

        private void FillLightSpawnTable()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];

                L1Npc l1npc = NpcTable.Instance.getTemplate(dataSourceRow.getInt(SpawnlistLight.Column_npcid));

                if (L1NpcInstance.Factory(l1npc) is L1FieldObjectInstance l1FieldObjectInstance)
                {
                    l1FieldObjectInstance.Id = IdFactory.Instance.nextId();
                    l1FieldObjectInstance.X = dataSourceRow.getInt(SpawnlistLight.Column_locx);
                    l1FieldObjectInstance.Y = dataSourceRow.getInt(SpawnlistLight.Column_locy);
                    l1FieldObjectInstance.MapId = (short)dataSourceRow.getInt(SpawnlistLight.Column_mapid);
                    l1FieldObjectInstance.HomeX = l1FieldObjectInstance.X;
                    l1FieldObjectInstance.HomeY = l1FieldObjectInstance.Y;
                    l1FieldObjectInstance.Heading = 0;
                    l1FieldObjectInstance.LightSize = l1npc.LightSize;

                    L1World.Instance.storeObject(l1FieldObjectInstance);
                    L1World.Instance.addVisibleObject(l1FieldObjectInstance);
                }
            }
        }
    }
}