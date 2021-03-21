using System.Threading;

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
namespace LineageServer.Server.telnet
{

	using Config = LineageServer.Server.Config;

	public class TelnetServer
	{
		private static TelnetServer _instance;

		private class ServerThread : IRunnable
		{
			private readonly TelnetServer outerInstance;

			public ServerThread(TelnetServer outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			internal ServerSocket _sock;

			public override void run()
			{
				try
				{
					_sock = new ServerSocket(Config.TELNET_SERVER_PORT);

					while (true)
					{
						Socket sock = _sock.accept();
						new TelnetConnection(sock);
					}
				}
				catch (IOException)
				{
				}
				try
				{
					_sock.close();
				}
				catch (IOException)
				{
				}
			}
		}

		private TelnetServer()
		{
		}

		public virtual void start()
		{
			(new ServerThread(this)).Start();
		}

		public static TelnetServer Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new TelnetServer();
				}
				return _instance;
			}
		}
	}

}