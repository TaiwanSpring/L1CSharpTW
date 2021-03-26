using System;
using System.Data;

namespace LineageServer.Interfaces
{
    /// <summary>
    /// 先做個簡易的
    /// </summary>
    public interface IDataSourceRow
    {
        IDataSourceRow Select();
        IDataSourceRow Insert();
        IDataSourceRow Update();
        IDataSourceRow Delete();
        IDataSourceRow Where(string column, object value);
        IDataSourceRow Set(string column, object value);
        void Execute();
        bool FillData(IDataReader dataReader);

        int getInt(string column);
        string getString(string column);
        DateTime getTimestamp(string column);
    }
}
