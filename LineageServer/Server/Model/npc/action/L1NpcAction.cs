using LineageServer.Server.Model.Instance;

namespace LineageServer.Server.Model.Npc.Action
{
	interface INpcAction
	{
		bool AcceptsRequest(string actionName, L1PcInstance pc, GameObject obj);

		L1NpcHtml Execute(string actionName, L1PcInstance pc, GameObject obj, byte[] args);

		L1NpcHtml ExecuteWithAmount(string actionName, L1PcInstance pc, GameObject obj, int amount);
	}
}