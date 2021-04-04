using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

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
            new ColumnInfo() { Column = Column_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_item_obj_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_objid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_npcid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_lvl, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_exp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_lawful, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_food, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
        };
        public Pets() : base(TableName)
        {
            
        }
    }
}
