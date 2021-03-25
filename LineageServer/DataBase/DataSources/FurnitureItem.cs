using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class FurnitureItem : DataSourceTable
    {
        public const string TableName = "furniture_item";
        public const string Column_note = "note";
        public const string Column_item_id = "item_id";
        public const string Column_npc_id = "npc_id";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_item_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_npc_id, DbType = DbType.Int32, IsPKey = false},
        };
        public FurnitureItem() : base(TableName)
        {
            
        }
    }
}
