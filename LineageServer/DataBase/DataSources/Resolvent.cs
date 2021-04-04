using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Resolvent : DataSource
    {
        public const string TableName = "resolvent";
        public const string Column_note = "note";
        public const string Column_item_id = "item_id";
        public const string Column_crystal_count = "crystal_count";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Resolvent; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_item_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_crystal_count, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public Resolvent() : base(TableName)
        {
            
        }
    }
}
