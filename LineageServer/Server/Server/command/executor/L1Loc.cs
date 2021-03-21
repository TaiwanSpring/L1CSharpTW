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

	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1WorldMap = LineageServer.Server.Server.Model.map.L1WorldMap;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;

	public class L1Loc : L1CommandExecutor
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1Loc).FullName);

		private L1Loc()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1Loc();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				int locx = pc.X;
				int locy = pc.Y;
				short mapid = pc.MapId;
				int gab = L1WorldMap.Instance.getMap(mapid).getOriginalTile(locx, locy);
				string msg = string.Format("座標 ({0:D}, {1:D}, {2:D}) {3:D}", locx, locy, mapid, gab);
				pc.sendPackets(new S_SystemMessage(msg));
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
		}
	}

}