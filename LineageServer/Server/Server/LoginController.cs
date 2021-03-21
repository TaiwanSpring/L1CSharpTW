using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Server
{

	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1Trade = LineageServer.Server.Server.Model.L1Trade;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;


	public class LoginController
	{
		private static LoginController _instance;

		private IDictionary<string, ClientThread> _accounts = Maps.newConcurrentMap();

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


		private void kickClient(in ClientThread client)
		{
			if (client == null)
			{
				return;
			}

			GeneralThreadPool.Instance.execute(() =>
			{
			if (client.ActiveChar != null)
			{
				client.ActiveChar.sendPackets(new S_ServerMessage(357));
			}

			try
			{
				Thread.Sleep(1000);
			}
			catch (Exception)
			{
			}
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
					kickClient(_accounts.Remove(account.Name));
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