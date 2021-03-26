using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class UbSettings : DataSource
    {
        public const string TableName = "ub_settings";
        public const string Column_ub_name = "ub_name";
        public const string Column_ub_id = "ub_id";
        public const string Column_ub_mapid = "ub_mapid";
        public const string Column_ub_area_x1 = "ub_area_x1";
        public const string Column_ub_area_y1 = "ub_area_y1";
        public const string Column_ub_area_x2 = "ub_area_x2";
        public const string Column_ub_area_y2 = "ub_area_y2";
        public const string Column_min_lvl = "min_lvl";
        public const string Column_max_lvl = "max_lvl";
        public const string Column_max_player = "max_player";
        public const string Column_hpr_bonus = "hpr_bonus";
        public const string Column_mpr_bonus = "mpr_bonus";
        public const string Column_enter_royal = "enter_royal";
        public const string Column_enter_knight = "enter_knight";
        public const string Column_enter_mage = "enter_mage";
        public const string Column_enter_elf = "enter_elf";
        public const string Column_enter_darkelf = "enter_darkelf";
        public const string Column_enter_dragonknight = "enter_dragonknight";
        public const string Column_enter_illusionist = "enter_illusionist";
        public const string Column_enter_male = "enter_male";
        public const string Column_enter_female = "enter_female";
        public const string Column_use_pot = "use_pot";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.UbSettings; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_ub_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_ub_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_ub_mapid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ub_area_x1, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ub_area_y1, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ub_area_x2, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ub_area_y2, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_min_lvl, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_max_lvl, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_max_player, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hpr_bonus, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mpr_bonus, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_enter_royal, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_enter_knight, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_enter_mage, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_enter_elf, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_enter_darkelf, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_enter_dragonknight, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_enter_illusionist, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_enter_male, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_enter_female, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_use_pot, DbType = DbType.Boolean, IsPKey = false},
        };
        public UbSettings() : base(TableName)
        {
            
        }
    }
}
