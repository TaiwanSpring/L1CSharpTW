using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

namespace LineageServer.DataBase.DataSources
{
	class DropItem : DataSource
	{
		public const string TableName = "drop_item";
		public const string Column_note = "note";
		public const string Column_item_id = "item_id";
		public const string Column_drop_rate = "drop_rate";
		public const string Column_drop_amount = "drop_amount";
		public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.DropItem; } }
		protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
		private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
		{
			new ColumnInfo() { Column = Column_note, MySqlDbType = MySqlDbType.Text, IsPKey = false},
			new ColumnInfo() { Column = Column_item_id, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
			new ColumnInfo() { Column = Column_drop_rate, MySqlDbType = MySqlDbType.Float, IsPKey = false},
			new ColumnInfo() { Column = Column_drop_amount, MySqlDbType = MySqlDbType.Float, IsPKey = false},
		};
		public DropItem() : base(TableName)
		{

		}
	}
}
