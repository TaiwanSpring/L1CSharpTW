using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
    class IpTable : IGameComponent
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.BanIp);

        private static IpTable _instance;
        public static IpTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new IpTable();
                }
                return _instance;
            }
        }
        private static IList<string> _banip = ListFactory.NewList<string>();
        private IpTable()
        {

        }
        public void Initialize()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                _banip.Add(dataSourceRow.getString(BanIp.Column_ip));
            }
        }
        public virtual void banIp(string ip)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Insert()
            .Set(BanIp.Column_ip, ip)
            .Execute();
            _banip.Add(ip);
        }

        public virtual bool isBannedIp(string s)
        {
            foreach (string BanIpAddress in _banip)
            { //被封鎖的IP
              // 判斷如果使用*結尾
                if (BanIpAddress.EndsWith("*", StringComparison.Ordinal))
                {
                    // 取回第一次出現*的index
                    int fStarindex = BanIpAddress.IndexOf("*", StringComparison.Ordinal);
                    // 取得0~fStar長度
                    string reip = BanIpAddress.Substring(0, fStarindex);
                    // 抓得Banip表單內ip在*號前的子字串 xxx.xxx||xxx.xxx.xxx
                    string newaddress = s.Substring(0, fStarindex);
                    if (newaddress == reip)
                    {
                        return true;
                    }
                }
                else
                {
                    if (s == BanIpAddress)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual bool liftBanIp(string ip)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Delete()
            .Where(BanIp.Column_ip, ip)
            .Execute();
            return true;
        }
    }

}