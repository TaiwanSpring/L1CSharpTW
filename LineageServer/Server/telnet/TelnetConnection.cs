using System.IO;
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

	using StreamUtil = LineageServer.Server.Server.utils.StreamUtil;
	using TelnetCommandExecutor = LineageServer.Server.telnet.command.TelnetCommandExecutor;
	using TelnetCommandResult = LineageServer.Server.telnet.command.TelnetCommandResult;

	public class TelnetConnection
	{
		private class ConnectionThread : IRunnable
		{
			private readonly TelnetConnection outerInstance;

			internal Socket _socket;

			public ConnectionThread(TelnetConnection outerInstance, Socket sock)
			{
				this.outerInstance = outerInstance;
				_socket = sock;
			}

			public override void run()
			{
				StreamReader isr = null;
				StreamReader @in = null;
				StreamWriter osw = null;
				StreamWriter @out = null;
				try
				{
					isr = new StreamReader(_socket.InputStream);
					@in = new StreamReader(isr);
					osw = new StreamWriter(_socket.OutputStream);
					@out = new StreamWriter(osw);

					string cmd = null;
					while (null != (cmd = @in.ReadLine()))
					{
						TelnetCommandResult result = TelnetCommandExecutor.Instance.execute(cmd);
						@out.Write(result.Code + " " + result.CodeMessage + "\r\n");
						@out.Write(result.Result + "\r\n");
						@out.Flush();
						// // for debug
						// System.out.println(result.getCode() + " " +
						// result.getCodeMessage());
						// System.out.println(result.getResult());
					}
				}
				catch (IOException)
				{
					StreamUtil.close(isr, @in);
					StreamUtil.close(osw, @out);
				}
				try
				{
					_socket.close();
				}
				catch (IOException)
				{
				}
			}
		}

		public TelnetConnection(Socket sock)
		{
			(new ConnectionThread(this, sock)).Start();
		}
	}

}