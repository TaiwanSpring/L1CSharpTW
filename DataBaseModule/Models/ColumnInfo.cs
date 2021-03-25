using System.Data;

namespace DataBaseModule.DataBase
{
	public struct ColumnInfo
	{
		public bool IsPKey { get; set; }

		public string Column { get; set; }

		public DbType DbType { get; set; }
	}
}
