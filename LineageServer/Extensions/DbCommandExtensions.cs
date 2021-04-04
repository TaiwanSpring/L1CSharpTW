using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;

namespace LineageServer.Extensions
{
    static class DbCommandExtensions
    {
        public static void AddParameter(this IDbCommand dbCommand, string parameterName, object value, MySqlDbType dbType)
        {
            MySqlParameter dataParameter = dbCommand.CreateParameter() as MySqlParameter;
            dataParameter.ParameterName = parameterName;
            dataParameter.Value = value;
            dataParameter.MySqlDbType = dbType;
            dbCommand.Parameters.Add(dataParameter);
        }
    }
}
