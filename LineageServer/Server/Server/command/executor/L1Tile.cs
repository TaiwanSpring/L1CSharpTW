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

	public class L1Tile : L1CommandExecutor
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1Tile).FullName);

		private L1Tile()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1Tile();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				int locX = pc.X;
				int locY = pc.Y;
				short mapId = pc.MapId;
				int tile0 = L1WorldMap.Instance.getMap(mapId).getOriginalTile(locX, locY - 1);
				int tile1 = L1WorldMap.Instance.getMap(mapId).getOriginalTile(locX + 1, locY - 1);
				int tile2 = L1WorldMap.Instance.getMap(mapId).getOriginalTile(locX + 1, locY);
				int tile3 = L1WorldMap.Instance.getMap(mapId).getOriginalTile(locX + 1, locY + 1);
				int tile4 = L1WorldMap.Instance.getMap(mapId).getOriginalTile(locX, locY + 1);
				int tile5 = L1WorldMap.Instance.getMap(mapId).getOriginalTile(locX - 1, locY + 1);
				int tile6 = L1WorldMap.Instance.getMap(mapId).getOriginalTile(locX - 1, locY);
				int tile7 = L1WorldMap.Instance.getMap(mapId).getOriginalTile(locX - 1, locY - 1);
				string msg = string.Format("0:{0:D} 1:{1:D} 2:{2:D} 3:{3:D} 4:{4:D} 5:{5:D} 6:{6:D} 7:{7:D}", tile0, tile1, tile2, tile3, tile4, tile5, tile6, tile7);
				pc.sendPackets(new S_SystemMessage(msg));
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
		}
	}

}