using System;

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
namespace LineageServer.Server.telnet.command
{
	using static LineageServer.Server.telnet.command.TelnetCommandResult;

	public class TelnetCommandExecutor
	{
		private static TelnetCommandExecutor _instance = new TelnetCommandExecutor();

		public static TelnetCommandExecutor Instance
		{
			get
			{
				return _instance;
			}
		}

		public virtual TelnetCommandResult execute(string cmd)
		{
			try
			{
				StringTokenizer tok = new StringTokenizer(cmd, " ");
				string name = tok.nextToken();

				TelnetCommand command = TelnetCommandList.get(name);
				if (command == null)
				{
					return new TelnetCommandResult(CMD_NOT_FOUND, cmd + " not found");
				}

				string args = "";
				if (name.Length + 1 < cmd.Length)
				{
					args = cmd.Substring(name.Length + 1);
				}
				return command.execute(args);
			}
			catch (Exception e)
			{
				return new TelnetCommandResult(CMD_INTERNAL_ERROR, e.Message);
			}
		}
	}

}