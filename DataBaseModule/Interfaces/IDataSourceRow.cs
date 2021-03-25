using System.Data;

namespace DataBaseModule.Interfaces
{
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
    }
}
