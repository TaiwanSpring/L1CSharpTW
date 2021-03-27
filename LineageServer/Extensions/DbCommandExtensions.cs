using System.Data;

namespace LineageServer.Extensions
{
    static class DbCommandExtensions
    {
        public static void AddParameter(this IDbCommand dbCommand, string parameterName, object value, DbType dbType)
        {
            IDbDataParameter dbDataParameter = dbCommand.CreateParameter();
            dbDataParameter.ParameterName = parameterName;
            dbDataParameter.Value = value;
            dbDataParameter.DbType = dbType;
            dbCommand.Parameters.Add(dbDataParameter);
        }
    }
}
