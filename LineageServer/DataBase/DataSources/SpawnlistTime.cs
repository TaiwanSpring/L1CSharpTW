using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
	class SpawnlistTime : DataSource
	{
		public const string TableName = "spawnlist_time";
		public const string Column_spawn_id = "spawn_id";
		public const string Column_time_start = "time_start";
		public const string Column_time_end = "time_end";
		public const string Column_period_start = "period_start";
		public const string Column_period_end = "period_end";
		public const string Column_delete_at_endtime = "delete_at_endtime";
		public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.SpawnlistTime; } }
		protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
		private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
		{
			new ColumnInfo() { Column = Column_spawn_id, DbType = DbType.Int32, IsPKey = true},
			new ColumnInfo() { Column = Column_time_start, DbType = DbType.DateTime, IsPKey = false},
			new ColumnInfo() { Column = Column_time_end, DbType = DbType.DateTime, IsPKey = false},
			new ColumnInfo() { Column = Column_period_start, DbType = DbType.DateTime, IsPKey = false},
			new ColumnInfo() { Column = Column_period_end, DbType = DbType.DateTime, IsPKey = false},
			new ColumnInfo() { Column = Column_delete_at_endtime, DbType = DbType.Boolean, IsPKey = false},
		};
		public SpawnlistTime() : base(TableName)
		{

		}
	}
}
