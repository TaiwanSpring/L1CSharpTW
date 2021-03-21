using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.Interfaces
{
    interface IDataBaseConnection
    {
        PreparedStatement prepareStatement(string command);

        void Close();
    }
}
