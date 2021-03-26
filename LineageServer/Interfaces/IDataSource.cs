using LineageServer.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.Interfaces
{
    interface IDataSource
    {
        DataSourceTypeEnum DataSourceType { get; }
        IDataSourceQuery Select();
        IDataSourceRow NewRow();
    }
}
