using LineageServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LineageServer.DataBase
{
    class DataBaseConnectionImp : IDataBaseConnection
    {
        private IDbConnection connection;

        public DataBaseConnectionImp(IDbConnection connection)
        {
            this.connection = connection;
        }

        public PreparedStatement prepareStatement(string command)
        {
            return new PreparedStatementImp(this.connection.CreateCommand()) { Command = command };
        }

        public void Close()
        {
            this.connection.Close();
        }
    }
}
