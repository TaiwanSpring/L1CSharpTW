using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class FurnitureItem : DataSource
    {
        public const string TableName = "furniture_item";
        public const string Column_note = "note";
        public const string Column_item_id = "item_id";
        public const string Column_npc_id = "npc_id";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.FurnitureItem; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_item_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_npc_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public FurnitureItem() : base(TableName)
        {
            
        }
    }
}
