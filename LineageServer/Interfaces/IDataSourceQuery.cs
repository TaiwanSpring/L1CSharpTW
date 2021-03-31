using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.Interfaces
{
	public interface IDataSourceQuery
	{
		IDataSourceQuery Where(string column, object value);

		IList<IDataSourceRow> Query();
		IList<IDataSourceRow> Query(string command);
	}
}
