using LineageServer.Server.Model.Instance;

namespace LineageServer.Server.Model.Npc.Action
{
    interface INpcAction
    {
        bool acceptsRequest(string actionName, L1PcInstance pc, GameObject obj);

        L1NpcHtml execute(string actionName, L1PcInstance pc, GameObject obj, byte[] args);

        L1NpcHtml executeWithAmount(string actionName, L1PcInstance pc, GameObject obj, int amount);
    }
}