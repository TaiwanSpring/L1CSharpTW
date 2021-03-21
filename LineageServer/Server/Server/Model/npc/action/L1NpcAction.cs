
using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.Model.npc.action
{
    interface INpcAction
    {

        bool acceptsRequest(string actionName, L1PcInstance pc, L1Object obj);

        L1NpcHtml execute(string actionName, L1PcInstance pc, L1Object obj, sbyte[] args);

        L1NpcHtml executeWithAmount(string actionName, L1PcInstance pc, L1Object obj, int amount);

    }
}