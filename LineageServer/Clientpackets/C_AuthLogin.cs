using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理 beanfun相關的帳號密碼取得登入與登出
    /// </summary>
    class C_AuthLogin : ClientBasePacket
    {

        private const string C_AUTH_LOGIN = "[C] C_AuthLogin";

        private static ILogger _log = Logger.GetLogger(nameof(C_AuthLogin));

        public C_AuthLogin(byte[] decrypt, ClientThread client) : base(decrypt)
        {
            int action = ReadC();

            switch (action)
            {
                case 0x06: // 登入請求
                    string accountName = ReadS().ToLower();
                    string password = ReadS();
                    string ip = client.Ip;
                    string host = client.Hostname;

                    //_log.finest("Request AuthLogin from user : " + accountName);
                    if (!Config.ALLOW_2PC)
                    {
                        foreach (ClientThread tempClient in LoginController.Instance.AllAccounts)
                        {
                            if (ip.Equals(tempClient.Ip, StringComparison.OrdinalIgnoreCase))
                            {
                                _log.Info("拒絕 2P 登入。account=" + accountName + " host=" + host);
                                client.SendPacket(new S_LoginResult(S_LoginResult.REASON_USER_OR_PASS_WRONG));
                                return;
                            }
                        }
                    }

                    Account account = Account.Load(accountName);
                    if (account == null)
                    {
                        if (Config.AUTO_CREATE_ACCOUNTS)
                        {
                            account = Account.Create(accountName, password, ip, host);
                        }
                        else
                        {
                            _log.Warning("account missing for user " + accountName);
                        }
                    }
                    if (account == null || !account.ValidatePassword(password))
                    {
                        client.SendPacket(new S_LoginResult(S_LoginResult.REASON_USER_OR_PASS_WRONG));
                        return;
                    }
                    if (account.Onlined)
                    {
                        client.SendPacket(new S_LoginResult(S_LoginResult.REASON_ACCOUNT_ALREADY_EXISTS)); //原碼 REASON_ACCOUNT_IN_USE
                        return;
                    }
                    if (account.Banned)
                    { // BANアカウント
                        _log.Info("禁止登入的帳號嘗試登入。account=" + accountName + " host=" + host);
                        client.SendPacket(new S_LoginResult(S_LoginResult.REASON_USER_OR_PASS_WRONG));
                        return;
                    }

                    try
                    {
                        LoginController.Instance.login(client, account);
                        account.UpdateLastActive(ip); // 更新最後一次登入的時間與IP
                        client.Account = account;
                        client.SendPacket(new S_LoginResult(S_LoginResult.REASON_LOGIN_OK));
                        //client.SendPacket(new S_CommonNews());
                        new L1CharList(client);

                        Account.SetOnline(account, true);
                    }
                    catch (GameServerFullException)
                    {
                        client.kick();
                        _log.Info("線上人數已經飽和，切斷 (" + client.Hostname + ") 的連線。");
                        return;
                    }
                    catch (AccountAlreadyLoginException)
                    {
                        client.kick();
                        _log.Info("同個帳號已經登入，切斷 (" + client.Hostname + ") 的連線。");
                        return;
                    }
                    break;
                case 0x0b: // 重新選擇角色
                    break;
                case 0x1c: // 登出請求
                    break;
            }
        }

        public override string Type
        {
            get
            {
                return C_AUTH_LOGIN;
            }
        }

    }
}