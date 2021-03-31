using LineageServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace LineageServer.DataBase
{
	class EmptyDataSourceRow : IDataSourceRow, IDataSourceQuery
	{
		public bool HaveData { get { return false; } }

		public IDataSourceRow Delete()
		{
			return this;
		}

		public void Execute()
		{

		}

		public bool FillData(IDataReader dataReader)
		{
			return false;
		}

		public byte[] getBlob(string column)
		{
			throw new NotImplementedException();
		}

		public bool getBoolean(string column)
		{
			return false;
		}

		public byte getByte(string column)
		{
			return 0;
		}

		public double getDouble(string column)
		{
			return 0d;
		}

		public int getInt(string column)
		{
			return 0;
		}

		public long getLong(string column)
		{
			return 0L;
		}

		public short getShort(string column)
		{
			return 0;
		}

		public string getString(string column)
		{
			return string.Empty;
		}

		public DateTime getTimestamp(string column)
		{
			return default(DateTime);
		}

		public IDataSourceRow Insert()
		{
			return this;
		}

		public IList<IDataSourceRow> Query()
		{
			return new List<IDataSourceRow>();
		}

		public IList<IDataSourceRow> Query(string command)
		{
			return new List<IDataSourceRow>();
		}

		public IDataSourceRow Select()
		{
			return this;
		}

		public IDataSourceRow Set(string column, object value)
		{
			return this;
		}

		public IDataSourceRow Update()
		{
			return this;
		}

		IDataSourceRow IDataSourceRow.Where(string column, object value)
		{
			return this;
		}

		IDataSourceQuery IDataSourceQuery.Where(string column, object value)
		{
			return this;
		}
	}
}
