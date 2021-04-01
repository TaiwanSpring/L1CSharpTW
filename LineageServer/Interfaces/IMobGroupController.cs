using LineageServer.Server.Model.Instance;

namespace LineageServer.Interfaces
{
    interface IMobGroupController
    {
        void doSpawn(L1NpcInstance leader, int groupId, bool isRespawnScreen, bool isInitSpawn);
    }
}
