using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

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
            new ColumnInfo() { Column = Column_ub_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_ub_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_ub_mapid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ub_area_x1, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ub_area_y1, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ub_area_x2, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ub_area_y2, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_min_lvl, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_max_lvl, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_max_player, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hpr_bonus, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mpr_bonus, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_enter_royal, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_enter_knight, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_enter_mage, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_enter_elf, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_enter_darkelf, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_enter_dragonknight, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_enter_illusionist, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_enter_male, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_enter_female, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_use_pot, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
        };
        public UbSettings() : base(TableName)
        {
            
        }
    }
}
