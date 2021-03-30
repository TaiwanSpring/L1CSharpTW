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
        bool HaveData { get; }
        int getInt(string column);
        byte getByte(string column);
        long getLong(string column);
        short getShort(string column);
        bool getBoolean(string column);
        string getString(string column);
        byte[] getBlob(string column);

        double getDouble(string column);
        DateTime getTimestamp(string column);
    }
}
