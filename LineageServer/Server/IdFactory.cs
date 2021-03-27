using System.Data;
using System.IO;

namespace LineageServer.Server
{
    public class IdFactory
    {
        private int _curId;

        private object _monitor = new object();

        private const int FIRST_ID = 0x10000000;

        private static IdFactory _instance = new IdFactory();

        private IdFactory()
        {
            loadState();
        }

        public static IdFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        public virtual int nextId()
        {
            lock (_monitor)
            {
                return _curId++;
            }
        }

        private void loadState()
        {
            // 取得資料庫中最大的ID+1
            IDbConnection dbConnection = Container.Instance.Resolve<IDbConnection>();
            IDbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = "select max(id)+1 as nextid from (select id from character_items union all select id from character_teleport union all select id from character_warehouse union all select id from character_elf_warehouse union all select objid as id from characters union all select clan_id as id from clan_data union all select index_id as id from clan_members union all select id from clan_warehouse union all select objid as id from pets ) t";
            IDataReader dataReader = dbCommand.ExecuteReader();

            if (dataReader.Read())
            {
                _curId = dataReader.GetInt32(0);

                if (_curId < FIRST_ID)
                {
                    _curId = FIRST_ID;
                }
            }
            else
            {
                throw new IOException("Read object id fail.");
            }
        }
    }

}