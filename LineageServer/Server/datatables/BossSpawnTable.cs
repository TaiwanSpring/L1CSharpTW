using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Templates;
using System.Collections.Generic;

namespace LineageServer.Server.DataTables
{
    class BossSpawnTable
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.SpawnlistBoss);
        private BossSpawnTable()
        {
        }

        public static void fillSpawnTable()
        {

            int spawnCount = 0;

            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            // L1BossSpawn spawnDat;
            L1Spawn spawnDat;
            L1Npc template1;

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];

                int npcTemplateId = dataSourceRow.getInt(SpawnlistBoss.Column_npc_id);
                template1 = Container.Instance.Resolve<INpcController>().getTemplate(npcTemplateId);

                if (template1 != null)
                {
                    spawnDat = new L1Spawn(template1);
                    spawnDat.Id = dataSourceRow.getInt(SpawnlistBoss.Column_id);
                    spawnDat.Npcid = npcTemplateId;
                    spawnDat.Location = dataSourceRow.getString(SpawnlistBoss.Column_location);
                    // spawnDat.CycleType = dataSourceRow.getString(SpawnlistBoss.Column_cycle_type);
                    spawnDat.Amount = dataSourceRow.getInt(SpawnlistBoss.Column_count);
                    spawnDat.GroupId = dataSourceRow.getInt(SpawnlistBoss.Column_group_id);
                    spawnDat.LocX = dataSourceRow.getInt(SpawnlistBoss.Column_locx);
                    spawnDat.LocY = dataSourceRow.getInt(SpawnlistBoss.Column_locy);
                    spawnDat.Randomx = dataSourceRow.getInt(SpawnlistBoss.Column_randomx);
                    spawnDat.Randomy = dataSourceRow.getInt(SpawnlistBoss.Column_randomy);
                    spawnDat.LocX1 = dataSourceRow.getInt(SpawnlistBoss.Column_locx1);
                    spawnDat.LocY1 = dataSourceRow.getInt(SpawnlistBoss.Column_locy1);
                    spawnDat.LocX2 = dataSourceRow.getInt(SpawnlistBoss.Column_locx2);
                    spawnDat.LocY2 = dataSourceRow.getInt(SpawnlistBoss.Column_locy2);
                    spawnDat.Heading = dataSourceRow.getInt(SpawnlistBoss.Column_heading);
                    spawnDat.MapId = dataSourceRow.getShort(SpawnlistBoss.Column_mapid);
                    spawnDat.RespawnScreen = dataSourceRow.getBoolean(SpawnlistBoss.Column_respawn_screen);
                    spawnDat.MovementDistance = dataSourceRow.getInt(SpawnlistBoss.Column_movement_distance);
                    spawnDat.Rest = dataSourceRow.getBoolean(SpawnlistBoss.Column_rest);
                    spawnDat.SpawnType = dataSourceRow.getShort(SpawnlistBoss.Column_spawn_type);
                    //spawnDat.Percentage = dataSourceRow.getInt(SpawnlistBoss.Column_percentage);

                    spawnDat.Name = template1.get_name();

                    // start the spawning
                    spawnDat.init();
                    spawnCount += spawnDat.Amount;
                }
            }
        }
    }

}