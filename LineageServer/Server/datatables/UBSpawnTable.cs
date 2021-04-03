using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
    class UBSpawnTable : IGameComponent
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.SpawnlistUb);

        private static UBSpawnTable _instance;

        private IDictionary<int, L1UbSpawn> _spawnTable = MapFactory.NewMap<int, L1UbSpawn>();

        public static UBSpawnTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UBSpawnTable();
                }
                return _instance;
            }
        }

        private UBSpawnTable()
        {

        }
        public void Initialize()
        {
            loadSpawnTable();
        }
        private void loadSpawnTable()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];

                L1Npc npcTemp = Container.Instance.Resolve<INpcController>().getTemplate(dataSourceRow.getInt(SpawnlistUb.Column_npc_templateid));

                if (npcTemp == null)
                {
                    continue;
                }

                L1UbSpawn spawnDat = new L1UbSpawn();

                spawnDat.Id = dataSourceRow.getInt(SpawnlistUb.Column_id);
                spawnDat.UbId = dataSourceRow.getInt(SpawnlistUb.Column_ub_id);
                spawnDat.Pattern = dataSourceRow.getInt(SpawnlistUb.Column_pattern);
                spawnDat.Group = dataSourceRow.getInt(SpawnlistUb.Column_group_id);
                spawnDat.Name = npcTemp.get_name();
                spawnDat.NpcTemplateId = npcTemp.get_npcId();
                spawnDat.Amount = dataSourceRow.getInt(SpawnlistUb.Column_count);
                spawnDat.SpawnDelay = dataSourceRow.getInt(SpawnlistUb.Column_spawn_delay);
                spawnDat.SealCount = dataSourceRow.getInt(SpawnlistUb.Column_seal_count);

                _spawnTable[spawnDat.Id] = spawnDat;
            }
        }

        public virtual L1UbSpawn getSpawn(int spawnId)
        {
            return _spawnTable[spawnId];
        }

        /// <summary>
        /// 指定されたUBIDに対するパターンの最大数を返す。
        /// </summary>
        /// <param name="ubId">
        ///            調べるUBID。 </param>
        /// <returns> パターンの最大数。 </returns>
        public virtual int getMaxPattern(int ubId)
        {
            int n = 0;
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query($"SELECT MAX(pattern) as pattern FROM spawnlist_ub WHERE ub_id={ubId}");
            if (dataSourceRows.Count > 0)
            {
                return dataSourceRows[0].getInt(SpawnlistUb.Column_pattern);

            }
            else
            {
                return 0;
            }
        }

        public virtual L1UbPattern getPattern(int ubId, int patternNumer)
        {
            L1UbPattern pattern = new L1UbPattern();
            foreach (L1UbSpawn spawn in _spawnTable.Values)
            {
                if ((spawn.UbId == ubId) && (spawn.Pattern == patternNumer))
                {
                    pattern.addSpawn(spawn.Group, spawn);
                }
            }
            pattern.freeze();

            return pattern;
        }
    }

}