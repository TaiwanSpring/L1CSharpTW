using LineageServer.Interfaces;
using System;

namespace LineageServer.DataBase.DataSources
{
	class EmptyDataSourceTable : IDataSourceTable
	{
		public IDataSourceRow[] Select()
		{
			throw new NotImplementedException();
		}
	}
}
