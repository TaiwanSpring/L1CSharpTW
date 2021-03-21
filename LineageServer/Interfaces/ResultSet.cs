using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.Interfaces
{
    interface ResultSet
    {
        bool next();
        int getInt(string columnName);
        string getString(string columnName);
        DateTime getDateTime(string columnName);
    }
}
