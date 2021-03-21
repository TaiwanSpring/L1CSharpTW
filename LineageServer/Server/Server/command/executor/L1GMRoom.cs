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
	using GMCommandsConfig = LineageServer.Server.Server.GMCommandsConfig;
	using L1Location = LineageServer.Server.Server.Model.L1Location;
	using L1Teleport = LineageServer.Server.Server.Model.L1Teleport;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;

	public class L1GMRoom : L1CommandExecutor
	{
		private L1GMRoom()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1GMRoom();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				int i = 0;
				try
				{
					i = int.Parse(arg);
				}
				catch (System.FormatException)
				{
				}

				if (i == 1)
				{
					L1Teleport.teleport(pc, 32737, 32796, (short) 99, 5, false);
				}
				else if (i == 2)
				{
					L1Teleport.teleport(pc, 32734, 32799, (short) 17100, 5, false); // 17100!?
				}
				else if (i == 3)
				{
					L1Teleport.teleport(pc, 32644, 32955, (short) 0, 5, false);
				}
				else if (i == 4)
				{
					L1Teleport.teleport(pc, 33429, 32814, (short) 4, 5, false);
				}
				else if (i == 5)
				{
					L1Teleport.teleport(pc, 32894, 32535, (short) 300, 5, false);
				}
				else
				{
					L1Location loc = GMCommandsConfig.ROOMS[arg.ToLower()];
					if (loc == null)
					{
						pc.sendPackets(new S_SystemMessage(arg + " 未定義的Room～"));
						return;
					}
					L1Teleport.teleport(pc, loc.X, loc.Y, (short) loc.MapId, 5, false);
				}
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage("請輸入 .gmroom1～.gmroom5 or .gmroom name 。"));
			}
		}
	}

}