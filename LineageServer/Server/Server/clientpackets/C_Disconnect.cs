using LineageServer.Interfaces;
using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來斷線的封包
	/// </summary>
	class C_Disconnect : ClientBasePacket
	{
		private const string C_DISCONNECT = "[C] C_Disconnect";

		private static ILogger _log = Logger.getLogger(nameof(C_Disconnect));

		public C_Disconnect(byte[] decrypt, ClientThread client) : base(decrypt)
		{
			client.CharReStart(true);
			L1PcInstance pc = client.ActiveChar;
			if (pc != null)
			{
				if (client.Account != null)
				{
					Account.SetOnline(client.Account, false);
				}

				ClientThread.quitGame(pc);

				lock (pc)
				{
					pc.logout();
					client.ActiveChar = null;
				}
			}
			else
			{
				_log.info("Disconnect Request from Account : " + client.AccountName);
			}
		}

		public override string Type
		{
			get
			{
				return C_DISCONNECT;
			}
		}
	}

}