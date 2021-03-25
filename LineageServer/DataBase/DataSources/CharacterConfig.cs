using System.Data;
namespace LineageServer.DataBase.DataSources
{
    class CharacterConfig : DataSourceTable
    {
        public const string TableName = "character_config";
        public const string Column_object_id = "object_id";
        public const string Column_length = "length";
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; }}
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_object_id, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_length, DbType = DbType.Int32, IsPKey = false},
        };
        public CharacterConfig() : base(TableName)
        {
            
        }
    }
}
