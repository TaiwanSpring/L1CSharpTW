using LineageServer.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace LineageServer.Server
{
    /// <summary>
    /// DBへのアクセスするための各種インターフェースを提供する.
    /// </summary>
    class L1DatabaseFactory
    {
        //差異很大就直接先小重構一下
        /// <summary>
        /// DataBaseFacotry
        /// </summary>
        public static IDatabaseFactory Instance { get; } = Container.Instance.Resolve<IDatabaseFactory>();

        private L1DatabaseFactory() { }
    }
}