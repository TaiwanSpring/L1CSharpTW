using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.Interfaces
{
	interface IDataSourceTable
	{
		IDataSourceRow[] Select();

		IDataSourceRow NewRow();
	}
}
