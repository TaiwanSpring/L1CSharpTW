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

	using L1Teleport = LineageServer.Server.Server.Model.L1Teleport;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;

	public class L1Move : L1CommandExecutor
	{
		private L1Move()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1Move();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				StringTokenizer st = new StringTokenizer(arg);
				int locx = int.Parse(st.nextToken());
				int locy = int.Parse(st.nextToken());
				short mapid;
				if (st.hasMoreTokens())
				{
					mapid = short.Parse(st.nextToken());
				}
				else
				{
					mapid = pc.MapId;
				}
				L1Teleport.teleport(pc, locx, locy, mapid, 5, false);
				pc.sendPackets(new S_SystemMessage("座標 " + locx + ", " + locy + ", " + mapid + "已經到達。"));
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage(cmdName + "請輸入 X座標 Y座標 [地圖編號]。"));
			}
		}
	}

}