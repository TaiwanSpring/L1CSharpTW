using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using System.Collections.Generic;

namespace LineageServer.Server.DataTables
{
    class CharacterConfigTable
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.CharacterConfig);
        private static CharacterConfigTable _instance;

        public CharacterConfigTable()
        {
        }

        public static CharacterConfigTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CharacterConfigTable();
                }
                return _instance;
            }
        }

        public virtual void storeCharacterConfig(int objectId, int length, byte[] data)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Insert()
            .Set(CharacterConfig.Column_object_id, objectId)
            .Set(CharacterConfig.Column_length, length)
            .Set(CharacterConfig.Column_data, data)
            .Execute();
        }

        public virtual void updateCharacterConfig(int objectId, int length, byte[] data)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Update()
            .Where(CharacterConfig.Column_object_id, objectId)
            .Set(CharacterConfig.Column_length, length)
            .Set(CharacterConfig.Column_data, data)
            .Execute();
        }

        public virtual void deleteCharacterConfig(int objectId)
        {

            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Delete()
            .Where(CharacterConfig.Column_object_id, objectId)
            .Execute();
        }

        public virtual int countCharacterConfig(int objectId)
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Where(CharacterConfig.Column_object_id, objectId).Query();
            return dataSourceRows.Count;
        }
    }

}