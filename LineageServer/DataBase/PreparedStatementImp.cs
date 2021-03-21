using LineageServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace LineageServer.DataBase
{
    class PreparedStatementImp : PreparedStatement
    {
        readonly IDbCommand dbCommand;
        public string Command
        {
            get { return this.dbCommand.CommandText; }
            set { this.dbCommand.CommandText = value; }
        }
        public PreparedStatementImp(IDbCommand dbCommand)
        {
            this.dbCommand = dbCommand;
        }
        public void execute()
        {
            this.dbCommand.ExecuteNonQuery();
        }

        public void setString(int index, string str)
        {
            IDbDataParameter dbDataParameter = this.dbCommand.CreateParameter();
            dbDataParameter.ParameterName = $"@{index}@";
            dbDataParameter.DbType = DbType.String;
            dbDataParameter.Value = str;
            this.dbCommand.Parameters.Add(dbDataParameter);
        }

        public void setInt(int index, int value)
        {
            IDbDataParameter dbDataParameter = this.dbCommand.CreateParameter();
            dbDataParameter.ParameterName = $"@{index}@";
            dbDataParameter.DbType = DbType.Int32;
            dbDataParameter.Value = value;
            this.dbCommand.Parameters.Add(dbDataParameter);
        }

        public void setTimestamp(int index, DateTime dateTime)
        {
            IDbDataParameter dbDataParameter = this.dbCommand.CreateParameter();
            dbDataParameter.ParameterName = $"@{index}@";
            dbDataParameter.DbType = DbType.DateTime;
            dbDataParameter.Value = dateTime;
            this.dbCommand.Parameters.Add(dbDataParameter);
        }

        public ResultSet executeQuery()
        {
            ResultSetImp resultSetImp = new ResultSetImp();
            IDataReader dataReader = this.dbCommand.ExecuteReader();
            while (dataReader.Read())
            {
                var dictionary = new Dictionary<string, object>();
                resultSetImp.Add(dictionary);
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    dictionary.Add(dataReader.GetName(i), dataReader.GetValue(i));
                }
            }
            return resultSetImp;
        }
    }
}
