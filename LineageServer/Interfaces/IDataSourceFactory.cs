using LineageServer.Enum;

namespace LineageServer.Interfaces
{
    interface IDataSourceFactory
    {
        IDataSource NullDataSource { get; }
        IDataSource Factory(DataSourceTypeEnum dataSourceType);
    }
}
