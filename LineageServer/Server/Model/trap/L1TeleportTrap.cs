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
namespace LineageServer.Server.Model.trap
{
	using L1Location = LineageServer.Server.Model.L1Location;
	using GameObject = LineageServer.Server.Model.GameObject;
	using L1Teleport = LineageServer.Server.Model.L1Teleport;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using TrapStorage = LineageServer.Server.storage.TrapStorage;

	public class L1TeleportTrap : L1Trap
	{
		private readonly L1Location _loc;

		public L1TeleportTrap(TrapStorage storage) : base(storage)
		{

			int x = storage.getInt("teleportX");
			int y = storage.getInt("teleportY");
			int mapId = storage.getInt("teleportMapId");
			_loc = new L1Location(x, y, mapId);
		}

		public override void onTrod(L1PcInstance trodFrom, GameObject trapObj)
		{
			sendEffect(trapObj);

			L1Teleport.teleport(trodFrom, _loc.X, _loc.Y, (short) _loc.MapId, 5, true);
		}
	}

}