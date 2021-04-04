using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;

namespace LineageServer.Interfaces
{
	interface INpcController
	{
		L1Npc getTemplate(int id);
		L1NpcInstance newNpcInstance(int id);
		L1NpcInstance newNpcInstance(L1Npc template);
		int findNpcIdByNameWithoutSpace(string name);
	}
}
