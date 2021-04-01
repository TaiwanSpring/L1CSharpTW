using LineageServer.Server.Model.Instance;
using LineageServer.Server.Storage;

namespace LineageServer.Server.Model.trap
{
	class L1TeleportTrap : L1Trap
	{
		private readonly L1Location _loc;

		public L1TeleportTrap(ITrapStorage storage) : base(storage)
		{
			int x = storage.getInt("teleportX");
			int y = storage.getInt("teleportY");
			int mapId = storage.getInt("teleportMapId");
			_loc = new L1Location(x, y, mapId);
		}

		public override void onTrod(L1PcInstance trodFrom, GameObject trapObj)
		{
			sendEffect(trapObj);

			L1Teleport.teleport(trodFrom, _loc.X, _loc.Y, (short)_loc.MapId, 5, true);
		}
	}
}