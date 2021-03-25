using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataBaseModule
{
    public static class ModuleInfo
    {
        public static IDbConnection DbConnection { get; private set; }
        public static void SetDataBaseConnection(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;
        }
    }
}
