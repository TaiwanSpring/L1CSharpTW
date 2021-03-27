using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Models;
namespace LineageServer.Server
{
    public class Logins
    {
        private static ILogger _log = Logger.GetLogger(nameof(Logins));

        private readonly static IDataSource accountDataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Accounts);
        public static bool loginValid(string account, string password, string ip, string host)
        {
            string encodPassword = Container.Instance.Resolve<IEncode>().Encode(password);

            IDataSourceRow dataSourceRow = accountDataSource.NewRow().Select().Where(Accounts.Column_login, account);

            dataSourceRow.Execute();

            if (dataSourceRow.getString(Accounts.Column_password) == encodPassword)
            {
                return true;
            }
            else if (string.IsNullOrEmpty(dataSourceRow.getString(Accounts.Column_login)))
            {
                if (Config.AUTO_CREATE_ACCOUNTS)
                {
                    dataSourceRow = accountDataSource.NewRow();
                    dataSourceRow.Insert()
                    .Set(Accounts.Column_login, account)
                    .Set(Accounts.Column_password, encodPassword)
                    .Set(Accounts.Column_ip, ip)
                    .Set(Accounts.Column_host, host)
                    .Execute();
                    return true;
                }
                else
                {
                    _log.Warning("account missing for user " + account);
                }
            }
            else
            {
                _log.Warning("account missing for user " + account);
            }
            return false;
        }
    }
}