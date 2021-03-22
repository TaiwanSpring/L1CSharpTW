using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.Interfaces
{
    interface IDatabaseFactory : IDisposable
    {
        IDataBaseConnection Connection { get; }
    }
}
