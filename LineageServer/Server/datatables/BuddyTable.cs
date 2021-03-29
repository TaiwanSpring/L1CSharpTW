using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
    class BuddyTable
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.CharacterBuddys);

        private static BuddyTable _instance;

        private readonly IDictionary<int, L1Buddy> _buddys = MapFactory.NewMap<int, L1Buddy>();

        public static BuddyTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BuddyTable();
                }
                return _instance;
            }
        }

        private BuddyTable()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];

                int charId = dataSourceRow.getInt(CharacterBuddys.Column_char_id);

                L1Buddy l1Buddy;

                if (_buddys.ContainsKey(charId))
                {
                    l1Buddy = new L1Buddy(charId);
                    _buddys.Add(charId, l1Buddy);
                }
                else
                {
                    l1Buddy = _buddys[charId];
                }

                l1Buddy.add(dataSourceRow.getInt(CharacterBuddys.Column_buddy_id),
                    dataSourceRow.getString(CharacterBuddys.Column_buddy_name));
            }
        }

        public virtual L1Buddy getBuddyTable(int charId)
        {
            L1Buddy buddy = _buddys[charId];
            if (buddy == null)
            {
                buddy = new L1Buddy(charId);
                _buddys[charId] = buddy;
            }
            return buddy;
        }

        public virtual void addBuddy(int charId, int objId, string name)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Insert()
            .Set(CharacterBuddys.Column_char_id, charId)
            .Set(CharacterBuddys.Column_buddy_id, objId)
            .Set(CharacterBuddys.Column_buddy_name, name)
            .Execute();
        }

        public virtual void removeBuddy(int charId, string buddyName)
        {
            L1Buddy buddy = getBuddyTable(charId);

            if (buddy.containsName(buddyName))
            {
                IDataSourceRow dataSourceRow = dataSource.NewRow();

                dataSourceRow.Delete()
                .Where(CharacterBuddys.Column_char_id, charId)
                .Where(CharacterBuddys.Column_buddy_id, buddyName)
                .Execute();

                buddy.remove(buddyName);
            }
        }
    }

}