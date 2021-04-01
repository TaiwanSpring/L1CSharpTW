using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Interfaces
{
    interface IDoorController
    {
        L1DoorInstance[] DoorList { get; }
        void deleteDoorByLocation(L1Location loc);
        int getDoorDirection(L1Location loc);
        L1DoorInstance findByDoorId(int doorId);
    }
}
