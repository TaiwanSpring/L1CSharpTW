using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class CharacterSkills : DataSource
    {
        public const string TableName = "character_skills";
        public const string Column_skill_name = "skill_name";
        public const string Column_id = "id";
        public const string Column_char_obj_id = "char_obj_id";
        public const string Column_skill_id = "skill_id";
        public const string Column_is_active = "is_active";
        public const string Column_activetimeleft = "activetimeleft";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.CharacterSkills; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_skill_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_char_obj_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_skill_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_is_active, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_activetimeleft, DbType = DbType.Int32, IsPKey = false},
        };
        public CharacterSkills() : base(TableName)
        {
            
        }
    }
}
