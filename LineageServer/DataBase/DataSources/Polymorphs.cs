using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
    class Polymorphs : DataSource
    {
        public const string TableName = "polymorphs";
        public const string Column_name = "name";
        public const string Column_id = "id";
        public const string Column_polyid = "polyid";
        public const string Column_minlevel = "minlevel";
        public const string Column_weaponequip = "weaponequip";
        public const string Column_armorequip = "armorequip";
        public const string Column_isSkillUse = "isSkillUse";
        public const string Column_cause = "cause";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Polymorphs; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_polyid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_minlevel, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_weaponequip, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_armorequip, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_isSkillUse, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_cause, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public Polymorphs() : base(TableName)
        {
            
        }
    }
}
