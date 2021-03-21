using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.Interfaces
{
    interface PreparedStatement
    {
        void setString(int index, string str);
        void setInt(int index, int str);
        void setTimestamp(int index, DateTime dateTime);
        void execute();

        ResultSet executeQuery();
    }
}
