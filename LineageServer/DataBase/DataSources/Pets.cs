using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class Pets : DataSource
    {
        public const string TableName = "pets";
        public const string Column_name = "name";
        public const string Column_item_obj_id = "item_obj_id";
        public const string Column_objid = "objid";
        public const string Column_npcid = "npcid";
        public const string Column_lvl = "lvl";
        public const string Column_hp = "hp";
        public const string Column_mp = "mp";
        public const string Column_exp = "exp";
        public const string Column_lawful = "lawful";
        public const string Column_food = "food";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Pets; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_item_obj_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_objid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_npcid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_lvl, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_exp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_lawful, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_food, DbType = DbType.Int32, IsPKey = false},
        };
        public Pets() : base(TableName)
        {
            
        }
    }
}
