using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System.Collections.Generic;

namespace LineageServer.Interfaces
{
    interface IGameWorld
    {
        ICollection<GameObject> Object { get; }
        ICollection<L1PcInstance> AllPlayers { get; }
        ICollection<L1PetInstance> AllPets { get; }
        ICollection<L1SummonInstance> AllSummons { get; }

        IList<L1War> WarList { get; }

        ICollection<L1Clan> AllClans { get; }
        bool WorldChatElabled { get; set; }
        bool ProcessingContributionTotal { get; set; }
        int Weather { get; set; }
        void storeObject(GameObject l1Object);
        void removeObject(GameObject gameObject);
        GameObject findObject(int gameObjectId);
        L1GroundInventory getInventory(int x, int y, short mapId);

        L1GroundInventory getInventory(L1Location loc);

        void addVisibleObject(GameObject gameObject);
        void removeVisibleObject(GameObject gameObject);

        void moveVisibleObject(GameObject gameObject, int newMap);

        IList<GameObject> getVisibleLineObjects(GameObject src, GameObject target);

        IList<GameObject> getVisibleBoxObjects(GameObject gameObject, int heading, int width, int height);
        IList<GameObject> getVisibleObjects(GameObject gameObject);

        IList<GameObject> getVisibleObjects(GameObject gameObject, int radius);
        IList<GameObject> getVisiblePoint(L1Location loc, int radius);

        IList<L1PcInstance> getVisiblePlayer(GameObject gameObject);
        IList<L1PcInstance> getVisiblePlayer(GameObject gameObject, int radius);

        IList<L1PcInstance> getVisiblePlayerExceptTargetSight(GameObject gameObject, GameObject target);
        IList<L1PcInstance> getRecognizePlayer(GameObject gameObject);

        L1PcInstance getPlayer(string name);
        IDictionary<int, GameObject> getVisibleObjects(int mapId);
        void addWar(L1War war);
        void removeWar(L1War war);
        void storeClan(L1Clan clan);
        void removeClan(L1Clan clan);
        L1Clan getClan(string clan_name);
        void broadcastPacketToAll(ServerBasePacket packet);
        void broadcastServerMessage(string message);
    }
}
