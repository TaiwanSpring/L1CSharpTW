using LineageServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LineageServer.DataBase
{
    class EmptyDataSourceRow : IDataSourceRow, IDataSourceQuery
    {
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

        public int getInt(string column)
        {
            return 0;
        }

        public string getString(string column)
        {
            return string.Empty;
        }

        public DateTime getTimestamp(string column)
        {
            throw new NotImplementedException();
        }

        public IDataSourceRow Insert()
        {
            return this;
        }

        public IList<IDataSourceRow> Query()
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
            throw new NotImplementedException();
        }
    }
}
