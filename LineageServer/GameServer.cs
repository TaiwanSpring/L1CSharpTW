using LineageServer.DataBase;
using LineageServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LineageServer
{
    class GameServer
    {
        readonly Container container = new Container();

        public GameServer(IDbConnection dbConnection)
        {
            InitializeContainer(dbConnection);
        }

        public void Initialize()
        {
            //Todo Start TCP Server
        }

        private void InitializeContainer(IDbConnection dbConnection)
        {
            this.container.RegisterInstance<IDbConnection>(dbConnection);
            this.container.RegisterInstance<IDataBaseConnection>(new DataBaseConnectionImp());
        }
    }
}
