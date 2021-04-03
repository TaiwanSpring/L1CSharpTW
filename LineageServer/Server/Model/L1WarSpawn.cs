using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using System;
using System.Text;
namespace LineageServer.Server.Model
{
    class L1WarSpawn
    {
        private static L1WarSpawn _instance;

        public L1WarSpawn()
        {
        }

        public static L1WarSpawn Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new L1WarSpawn();
                }
                return _instance;
            }
        }

        public virtual void SpawnTower(int castleId)
        {
            int npcId = 81111;
            if (castleId == L1CastleLocation.ADEN_CASTLE_ID)
            {
                npcId = 81189;
            }
            L1Npc l1npc = Container.Instance.Resolve<INpcController>().getTemplate(npcId); // ガーディアンタワー
            int[] loc = new int[3];
            loc = L1CastleLocation.getTowerLoc(castleId);
            SpawnWarObject(l1npc, loc[0], loc[1], (short)(loc[2]));
            if (castleId == L1CastleLocation.ADEN_CASTLE_ID)
            {
                spawnSubTower();
            }
        }

        private void spawnSubTower()
        {
            L1Npc l1npc;
            int[] loc = new int[3];
            for (int i = 1; i <= 4; i++)
            {
                l1npc = Container.Instance.Resolve<INpcController>().getTemplate(81189 + i); // サブタワー
                loc = L1CastleLocation.getSubTowerLoc(i);
                SpawnWarObject(l1npc, loc[0], loc[1], (short)(loc[2]));
            }
        }

        public virtual void SpawnCrown(int castleId)
        {
            L1Npc l1npc = Container.Instance.Resolve<INpcController>().getTemplate(81125); // クラウン
            int[] loc = new int[3];
            loc = L1CastleLocation.getTowerLoc(castleId);
            SpawnWarObject(l1npc, loc[0], loc[1], (short)(loc[2]));
        }

        public virtual void SpawnFlag(int castleId)
        {
            L1Npc l1npc = Container.Instance.Resolve<INpcController>().getTemplate(81122); // 旗
            int[] loc = new int[5];
            loc = L1CastleLocation.getWarArea(castleId);
            int x = 0;
            int y = 0;
            int locx1 = loc[0];
            int locx2 = loc[1];
            int locy1 = loc[2];
            int locy2 = loc[3];
            short mapid = (short)loc[4];

            for (x = locx1, y = locy1; x <= locx2; x += 8)
            {
                SpawnWarObject(l1npc, x, y, mapid);
            }
            for (x = locx2, y = locy1; y <= locy2; y += 8)
            {
                SpawnWarObject(l1npc, x, y, mapid);
            }
            for (x = locx2, y = locy2; x >= locx1; x -= 8)
            {
                SpawnWarObject(l1npc, x, y, mapid);
            }
            for (x = locx1, y = locy2; y >= locy1; y -= 8)
            {
                SpawnWarObject(l1npc, x, y, mapid);
            }
        }

        private void SpawnWarObject(L1Npc l1npc, int locx, int locy, short mapid)
        {
            try
            {
                if (l1npc != null)
                {
                    L1NpcInstance npc = L1NpcInstance.Factory(l1npc);
                    npc.Id = Container.Instance.Resolve<IIdFactory>().nextId();
                    npc.X = locx;
                    npc.Y = locy;
                    npc.HomeX = locx;
                    npc.HomeY = locy;
                    npc.Heading = 0;
                    npc.MapId = mapid;
                    Container.Instance.Resolve<IGameWorld>().storeObject(npc);
                    Container.Instance.Resolve<IGameWorld>().addVisibleObject(npc);

                    foreach (L1PcInstance pc in Container.Instance.Resolve<IGameWorld>().AllPlayers)
                    {
                        npc.addKnownObject(pc);
                        pc.addKnownObject(npc);
                        pc.sendPackets(new S_NPCPack(npc));
                        pc.broadcastPacket(new S_NPCPack(npc));
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }

}