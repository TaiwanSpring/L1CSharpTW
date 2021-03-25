using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class DropItem : DataSourceTable
    {
        public const string TableName = "drop_item";
        public const string Column_note = "note";
        public const string Column_item_id = "item_id";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_note, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_item_id, DbType = DbType.Int32, IsPKey = true},
        };
        public DropItem() : base(TableName)
        {
            
        }
    }
}
