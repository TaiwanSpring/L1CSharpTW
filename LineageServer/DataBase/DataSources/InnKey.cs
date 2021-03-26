using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class InnKey : DataSource
    {
        public const string TableName = "inn_key";
        public const string Column_due_time = "due_time";
        public const string Column_item_obj_id = "item_obj_id";
        public const string Column_key_id = "key_id";
        public const string Column_npc_id = "npc_id";
        public const string Column_hall = "hall";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.InnKey; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_due_time, DbType = DbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_item_obj_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_key_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_npc_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hall, DbType = DbType.Boolean, IsPKey = false},
        };
        public InnKey() : base(TableName)
        {
            
        }
    }
}
