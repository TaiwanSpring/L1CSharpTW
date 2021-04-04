using System.Collections.Generic;

namespace LineageServer.Interfaces
{
	public interface IDataSourceQuery
	{
		IDataSourceQuery Where(string column, object value);
		IDataSourceQuery WhereNot(string column, object value);
		IDataSourceQuery OrderBy(string column);
		IDataSourceQuery OrderByDesc(string column);
		IList<IDataSourceRow> Query();
		IList<IDataSourceRow> Query(string command);
	}
}
