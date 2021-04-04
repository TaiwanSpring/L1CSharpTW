using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class CharacterConfig : DataSource
    {
        public const string TableName = "character_config";
        public const string Column_object_id = "object_id";
        public const string Column_length = "length";
        public const string Column_data = "data";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.CharacterConfig; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_object_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_length, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_data, MySqlDbType = MySqlDbType.Binary, IsPKey = false},
        };
        public CharacterConfig() : base(TableName)
        {
            
        }
    }
}
