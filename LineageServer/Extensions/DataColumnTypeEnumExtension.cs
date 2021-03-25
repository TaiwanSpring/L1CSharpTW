using LineageServer.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.Extensions
{
    static class DataColumnTypeEnumExtension
    {
        public static string ToName(this DataColumnTypeEnum dataSourceType)
        {
            //Column_
            return dataSourceType.ToString().Substring(7);
        }
    }
}