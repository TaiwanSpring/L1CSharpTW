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
namespace LineageServer.Server.Server.command.executor
{

	using ExpTable = LineageServer.Server.Server.datatables.ExpTable;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;
	using IntRange = LineageServer.Server.Server.utils.IntRange;

	public class L1Level : L1CommandExecutor
	{
		private L1Level()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1Level();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				StringTokenizer tok = new StringTokenizer(arg);
				int level = int.Parse(tok.nextToken());
				if (level == pc.Level)
				{
					return;
				}
				if (!IntRange.includes(level, 1, 110))
				{
					pc.sendPackets(new S_SystemMessage("請在1-110範圍內指定"));
					return;
				}
				pc.Exp = ExpTable.getExpByLevel(level);
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage("請輸入 : " + cmdName + " lv "));
			}
		}
	}

}