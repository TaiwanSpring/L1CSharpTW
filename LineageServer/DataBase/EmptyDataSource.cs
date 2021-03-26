using LineageServer.Enum;
using LineageServer.Interfaces;
using System.Collections.Generic;

namespace LineageServer.DataBase
{
    class EmptyDataSource : IDataSource
    {
        private static readonly EmptyDataSourceRow emptyDataSourceRow = new EmptyDataSourceRow();
        public static IDataSourceRow NullDataSourceRow { get; } = emptyDataSourceRow;
        public static IDataSourceQuery NullDataSourceQuery { get; } = emptyDataSourceRow;

        public DataSourceTypeEnum DataSourceType => DataSourceTypeEnum.Unknow;

        public IDataSourceRow NewRow()
        {
            return NullDataSourceRow;
        }

        public IDataSourceQuery Select()
        {
            return NullDataSourceQuery;
        }
    }
}
