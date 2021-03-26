using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class Droplist : DataSource
    {
        public const string TableName = "droplist";
        public const string Column_mobId = "mobId";
        public const string Column_itemId = "itemId";
        public const string Column_min = "min";
        public const string Column_max = "max";
        public const string Column_chance = "chance";
        public const string Column_enchantlvl = "enchantlvl";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Droplist; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_mobId, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_itemId, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_min, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_max, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_chance, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_enchantlvl, DbType = DbType.Int32, IsPKey = false},
        };
        public Droplist() : base(TableName)
        {
            
        }
    }
}
