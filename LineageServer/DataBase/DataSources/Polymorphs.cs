using System.Data;
using LineageServer.Enum;
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
            new ColumnInfo() { Column = Column_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_polyid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_minlevel, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_weaponequip, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_armorequip, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_isSkillUse, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_cause, DbType = DbType.Int32, IsPKey = false},
        };
        public Polymorphs() : base(TableName)
        {
            
        }
    }
}
