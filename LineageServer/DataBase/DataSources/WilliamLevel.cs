using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class WilliamLevel : DataSource
    {
        public const string TableName = "william_level";
        public const string Column_註解 = "註解";
        public const string Column_getItem = "getItem";
        public const string Column_count = "count";
        public const string Column_enchantlvl = "enchantlvl";
        public const string Column_message = "message";
        public const string Column_id = "id";
        public const string Column_level = "level";
        public const string Column_give_royal = "give_royal";
        public const string Column_give_knight = "give_knight";
        public const string Column_give_mage = "give_mage";
        public const string Column_give_elf = "give_elf";
        public const string Column_give_darkelf = "give_darkelf";
        public const string Column_give_dragonknight = "give_dragonknight";
        public const string Column_give_illusionist = "give_illusionist";
        public const string Column_quest_id = "quest_id";
        public const string Column_quest_step = "quest_step";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.WilliamLevel; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_註解, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_getItem, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_count, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_enchantlvl, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_message, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_level, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_give_royal, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_give_knight, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_give_mage, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_give_elf, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_give_darkelf, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_give_dragonknight, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_give_illusionist, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_quest_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_quest_step, DbType = DbType.Int32, IsPKey = false},
        };
        public WilliamLevel() : base(TableName)
        {
            
        }
    }
}
