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
namespace LineageServer.Server.Server.command.executor
{

	using L1Commands = LineageServer.Server.Server.command.L1Commands;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;
	using L1Command = LineageServer.Server.Server.Templates.L1Command;

	/// <summary>
	/// GM指令：取得所有指令
	/// </summary>
	public class L1CommandHelp : L1CommandExecutor
	{
		private L1CommandHelp()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1CommandHelp();
			}
		}

		private string join(IList<L1Command> list, string with)
		{
			StringBuilder result = new StringBuilder();
			foreach (L1Command cmd in list)
			{
				if (result.Length > 0)
				{
					result.Append(with);
				}
				result.Append(cmd.Name);
			}
			return result.ToString();
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			IList<L1Command> list = L1Commands.availableCommandList(pc.AccessLevel);
			pc.sendPackets(new S_SystemMessage(join(list, ", ")));
		}
	}

}