using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using LineageServer.Utils;
using LineageServer.Models;
using LineageServer.Serverpackets;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model;

namespace LineageServer.Server
{
    class LoginController
    {
        private static LoginController _instance;

        private IDictionary<string, ClientThread> _accounts = MapFactory.NewConcurrentMap<string, ClientThread>();

        private int _maxAllowedOnlinePlayers;

        private LoginController()
        {
        }

        public static LoginController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LoginController();
                }
                return _instance;
            }
        }

        public virtual ClientThread[] AllAccounts
        {
            get
            {
                return _accounts.Values.ToArray();
            }
        }

        public virtual int OnlinePlayerCount
        {
            get
            {
                return _accounts.Count;
            }
        }

        public virtual int MaxAllowedOnlinePlayers
        {
            get
            {
                return _maxAllowedOnlinePlayers;
            }
            set
            {
                _maxAllowedOnlinePlayers = value;
            }
        }


        private void kickClient(ClientThread client)
        {
            if (client == null)
            {
                return;
            }

            RunnableExecuter.Instance.execute(() =>
            {
                if (client.ActiveChar != null)
                {
                    client.ActiveChar.sendPackets(new S_ServerMessage(357));
                }

                Thread.Sleep(1000);

                client.kick();
            });
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public synchronized void login(ClientThread client, Account account) throws GameServerFullException, AccountAlreadyLoginException
        public virtual void login(ClientThread client, Account account)
        {
            lock (this)
            {
                if (!account.Valid)
                {
                    // 密碼驗證未指定或不驗證帳戶。
                    // 此代碼只存在的錯誤檢測。
                    throw new System.ArgumentException("帳戶沒有通過認證");
                }
                if ((MaxAllowedOnlinePlayers <= OnlinePlayerCount) && !account.GameMaster)
                {
                    throw new GameServerFullException();
                }
                if (_accounts.ContainsKey(account.Name))
                {
                    if (_accounts.Remove(account.Name))
                    {
                        kickClient(client);
                    }
                    throw new AccountAlreadyLoginException();
                }

                _accounts[account.Name] = client;
            }
        }

        public virtual bool logout(ClientThread client)
        {
            lock (this)
            {
                if (string.ReferenceEquals(client.AccountName, null))
                {
                    return false;
                }

                //重登, 取消交易
                L1PcInstance player = client.ActiveChar;
                if (player != null)
                {
                    if (player.TradeID != 0)
                    {
                        //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                        //ORIGINAL LINE: final l1j.server.server.model.L1Trade trade = new l1j.server.server.model.L1Trade();
                        L1Trade trade = new L1Trade();
                        trade.TradeCancel(player);
                    }
                }

                return _accounts.Remove(client.AccountName) != null;
            }
        }
    }

}