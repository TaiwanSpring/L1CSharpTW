using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.Interfaces
{
    interface IDatabaseFactory
    {
        IDataBaseConnection Connection { get; }

        bool Initialize(string hostName, string user, string password, string dbName);
    }
}
