using LineageServer.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.LineageServer.Extensions
{
    static class DataSourceTypeEnumExtension
    {

        public static string ToName(this DataSourceTypeEnum dataSourceType)
        {
            //Table_
            return dataSourceType.ToString().Substring(6);
        }
    }
}
