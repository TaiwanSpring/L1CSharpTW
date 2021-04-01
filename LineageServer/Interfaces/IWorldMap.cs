using LineageServer.Server.Model.Map;

namespace LineageServer.Interfaces
{
	public interface IWorldMap
	{
		L1Map getMap(short mapId);
	}
}
