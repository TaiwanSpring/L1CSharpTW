using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LineageServer.DataBase
{
	struct ColumnInfo
	{
		public bool IsPKey { get; set; }

		public string Column { get; set; }

		public DbType DbType { get; set; }
	}
}
