using LineageServer.Interfaces;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.serverpackets;
using System;
namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理 beanfun相關的帳號密碼取得登入與登出
	/// </summary>
	class C_AuthLogin : ClientBasePacket
	{

		private const string C_AUTH_LOGIN = "[C] C_AuthLogin";

		private static ILogger _log = Logger.getLogger(nameof(C_AuthLogin));

		public C_AuthLogin(sbyte[] decrypt, ClientThread client) : base(decrypt)
		{
			int action = readC();

			switch (action)
			{
			case 0x06: // 登入請求
				string accountName = readS().ToLower();
				string password = readS();
				string ip = client.Ip;
				string host = client.Hostname;

				//_log.finest("Request AuthLogin from user : " + accountName);
				if (!Config.ALLOW_2PC)
				{
					foreach (ClientThread tempClient in LoginController.Instance.AllAccounts)
					{
						if (ip.Equals(tempClient.Ip, StringComparison.OrdinalIgnoreCase))
						{
							_log.info("拒絕 2P 登入。account=" + accountName + " host=" + host);
							client.SendPacket(new S_LoginResult(S_LoginResult.REASON_USER_OR_PASS_WRONG));
							return;
						}
					}
				}

				Account account = Account.load(accountName);
				if (account == null)
				{
					if (Config.AUTO_CREATE_ACCOUNTS)
					{
						account = Account.create(accountName, password, ip, host);
					}
					else
					{
						_log.warning("account missing for user " + accountName);
					}
				}
				if (account == null || !account.validatePassword(password))
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
					_log.info("禁止登入的帳號嘗試登入。account=" + accountName + " host=" + host);
					client.SendPacket(new S_LoginResult(S_LoginResult.REASON_USER_OR_PASS_WRONG));
					return;
				}

				try
				{
					LoginController.Instance.login(client, account);
					Account.updateLastActive(account, ip); // 更新最後一次登入的時間與IP
					client.Account = account;
					client.SendPacket(new S_LoginResult(S_LoginResult.REASON_LOGIN_OK));
					//client.SendPacket(new S_CommonNews());
					new L1CharList(client);

					Account.online(account, true);
				}
				catch (GameServerFullException)
				{
					client.kick();
					_log.info("線上人數已經飽和，切斷 (" + client.Hostname + ") 的連線。");
					return;
				}
				catch (AccountAlreadyLoginException)
				{
					client.kick();
					_log.info("同個帳號已經登入，切斷 (" + client.Hostname + ") 的連線。");
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