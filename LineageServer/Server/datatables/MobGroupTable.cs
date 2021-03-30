using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
    class MobGroupTable
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.Mobgroup);
        private static MobGroupTable _instance;

        private readonly IDictionary<int, L1MobGroup> _mobGroupIndex = MapFactory.NewMap<int, L1MobGroup>();

        public static MobGroupTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MobGroupTable();
                }
                return _instance;
            }
        }

        private MobGroupTable()
        {
            loadMobGroup();
        }

        private void loadMobGroup()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                int mobGroupId = dataSourceRow.getInt(Mobgroup.Column_id);
                bool isRemoveGroup = (dataSourceRow.getBoolean(Mobgroup.Column_remove_group_if_leader_die));
                int leaderId = dataSourceRow.getInt(Mobgroup.Column_leader_id);
                IList<L1NpcCount> minions = ListFactory.NewList<L1NpcCount>();
                for (int j = 1; j <= 7; j++)
                {
                    int id = dataSourceRow.getInt("minion" + j + "_id");
                    int count = dataSourceRow.getInt("minion" + j + "_count");
                    minions.Add(new L1NpcCount(id, count));
                }
                L1MobGroup mobGroup = new L1MobGroup(mobGroupId, leaderId, minions, isRemoveGroup);
                _mobGroupIndex[mobGroupId] = mobGroup;
            }
        }

        public virtual L1MobGroup getTemplate(int mobGroupId)
        {
            return _mobGroupIndex[mobGroupId];
        }

    }

}