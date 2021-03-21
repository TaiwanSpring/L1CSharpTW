using System;
using System.Collections.Generic;
using System.Text;

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

	using L1Commands = LineageServer.Server.Server.command.L1Commands;
	using L1CommandExecutor = LineageServer.Server.Server.command.executor.L1CommandExecutor;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;
	using L1Command = LineageServer.Server.Server.Templates.L1Command;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	// Referenced classes of package l1j.server.server:
	// ClientThread, Shutdown, IpTable, MobTable,
	// PolyTable, IdFactory
	//

	public class GMCommands
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(GMCommands).FullName);

		private static GMCommands _instance;

		private GMCommands()
		{
		}

		public static GMCommands Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new GMCommands();
				}
				return _instance;
			}
		}

		private string complementClassName(string className)
		{
			// 如果包涵 . 則認為他已經有完整路徑，所以直接丟回去
			if (className.Contains("."))
			{
				return className;
			}

			// 如果沒有點的話則自動幫他補完前面的路徑
			return "l1j.server.server.command.executor." + className;
		}

		private bool executeDatabaseCommand(L1PcInstance pc, string name, string arg)
		{
			try
			{
				L1Command command = L1Commands.get(name);
				if (command == null)
				{
					return false;
				}
				if (pc.AccessLevel < command.Level)
				{
					pc.sendPackets(new S_ServerMessage(74, "指令" + name)); // \f1%0は使用できません。
					return true;
				}

				Type cls = Type.GetType(complementClassName(command.ExecutorClassName));
				L1CommandExecutor exe = (L1CommandExecutor) cls.GetMethod("getInstance").invoke(null);
				exe.execute(pc, name, arg);
				_log.info(pc.Name + "使用 ." + name + " " + arg + "的指令。");
				return true;
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, "error gm command", e);
			}
			return false;
		}

		public virtual void handleCommands(L1PcInstance gm, string cmdLine)
		{
			StringTokenizer token = new StringTokenizer(cmdLine);
			// 命令，直到第一個空白，並在其後當作參數空格隔開
			string cmd = token.nextToken();
			string param = "";
			while (token.hasMoreTokens())
			{
				param = (new StringBuilder(param)).Append(token.nextToken()).Append(' ').ToString();
			}
			param = param.Trim();

			// 將使用過的指令存起來
			if (executeDatabaseCommand(gm, cmd, param))
			{
				if (!cmd.Equals("r", StringComparison.OrdinalIgnoreCase))
				{
					_lastCommands[gm.Id] = cmdLine;
				}
				return;
			}
			if (cmd.Equals("r", StringComparison.OrdinalIgnoreCase))
			{
				if (!_lastCommands.ContainsKey(gm.Id))
				{
					gm.sendPackets(new S_ServerMessage(74, "指令" + cmd)); // \f1%0は使用できません。
					return;
				}
				redo(gm, param);
				return;
			}
			gm.sendPackets(new S_SystemMessage("指令 " + cmd + " 不存在。"));
		}

		private static IDictionary<int, string> _lastCommands = Maps.newMap();

		private void redo(L1PcInstance pc, string arg)
		{
			try
			{
				string lastCmd = _lastCommands[pc.Id];
				if (arg.Length == 0)
				{
					pc.sendPackets(new S_SystemMessage("指令 " + lastCmd + " 重新執行。"));
					handleCommands(pc, lastCmd);
				}
				else
				{
					// 引数を変えて実行
					StringTokenizer token = new StringTokenizer(lastCmd);
					string cmd = token.nextToken() + " " + arg;
					pc.sendPackets(new S_SystemMessage("指令 " + cmd + " 執行。"));
					handleCommands(pc, cmd);
				}
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
				pc.sendPackets(new S_SystemMessage(".r 指令錯誤。"));
			}
		}
	}

}