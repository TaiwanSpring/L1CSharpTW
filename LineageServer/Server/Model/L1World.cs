using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.Map;
using LineageServer.Server.Types;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LineageServer.Server.Model
{
    class GameWorld : IGameComponent, IGameWorld
    {
        private static ILogger _log = Logger.GetLogger(nameof(GameWorld));
        public ICollection<GameObject> Object
        {
            get
            {
                return _allObjects.Values.ToHashSet();
            }
        }

        // _allPlayersのビュー

        public virtual ICollection<L1PcInstance> AllPlayers
        {
            get
            {
                return _allPlayers.Values.ToHashSet();
            }
        }
        public virtual ICollection<L1PetInstance> AllPets
        {
            get
            {
                return _allPets.Values.ToHashSet();
            }
        }

        // _allSummonsのビュー

        public virtual ICollection<L1SummonInstance> AllSummons
        {
            get
            {
                return _allSummons.Values.ToHashSet();
            }
        }
        public virtual IList<L1War> WarList
        {
            get
            {
                return _allWars.ToArray();
            }
        }
        // _allClansのビュー

        public virtual ICollection<L1Clan> AllClans
        {
            get
            {
                return _allClans.Values.ToHashSet();
            }
        }
        public int Weather { get; set; } = 4;

        public bool WorldChatElabled { get; set; } = true;

        public bool ProcessingContributionTotal { get; set; }

        private readonly IDictionary<string, L1PcInstance> _allPlayers = MapFactory.NewConcurrentMap<string, L1PcInstance>();

        private readonly IDictionary<int, L1PetInstance> _allPets = MapFactory.NewConcurrentMap<int, L1PetInstance>();

        private readonly IDictionary<int, L1SummonInstance> _allSummons = MapFactory.NewConcurrentMap<int, L1SummonInstance>();

        private readonly IDictionary<int, GameObject> _allObjects = MapFactory.NewConcurrentMap<int, GameObject>();

        private readonly IDictionary<int, GameObject>[] _visibleObjects = new IDictionary<int, GameObject>[MAX_MAP_ID + 1];

        private readonly IDictionary<string, L1Clan> _allClans = MapFactory.NewConcurrentMap<string, L1Clan>();

        private readonly IList<L1War> _allWars = ListFactory.NewList<L1War>();

        private const int MAX_MAP_ID = 10000;

        public void storeObject(GameObject gameObject)
        {
            if (gameObject is L1PcInstance pc)
            {
                _allPlayers[pc.Name] = pc;

                _allObjects[gameObject.Id] = gameObject;
            }
            else
            if (gameObject is L1PetInstance pet)
            {
                _allPets[gameObject.Id] = pet;

                _allObjects[gameObject.Id] = gameObject;
            }
            else
            if (gameObject is L1SummonInstance summon)
            {
                _allSummons[gameObject.Id] = summon;

                _allObjects[gameObject.Id] = gameObject;
            }
        }

        public void removeObject(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }

            if (_allObjects.ContainsKey(gameObject.Id))
            {
                _allObjects.Remove(gameObject.Id);

                if (gameObject is L1PcInstance pc)
                {
                    _allPlayers.Remove(pc.Name);
                }
                else if (gameObject is L1PetInstance)
                {
                    _allPets.Remove(gameObject.Id);
                }
                else if (gameObject is L1SummonInstance)
                {
                    _allSummons.Remove(gameObject.Id);
                }
            }
        }

        public GameObject findObject(int gameObjectId)
        {
            return _allObjects[gameObjectId];
        }

        public L1GroundInventory getInventory(int x, int y, short mapId)
        {
            if (mapId < _visibleObjects.Length)
            {
                // xyのマイナス値をインベントリキーとして使用
                int inventoryKey = (((x - 30000) * 10000) + (y - 30000)) * -1;


                if (_visibleObjects[mapId].ContainsKey(inventoryKey))
                {
                    GameObject gameObject = _visibleObjects[mapId][inventoryKey];

                    if (gameObject is L1GroundInventory groundInventory)
                    {
                        return groundInventory;
                    }
                    else
                    {
                        throw new Exception("");
                    }
                }
                else
                {
                    return new L1GroundInventory(inventoryKey, x, y, mapId);
                }
            }
            else
            {
                throw new Exception($"not map id = {mapId}");
            }
        }

        public L1GroundInventory getInventory(L1Location loc)
        {
            return getInventory(loc.X, loc.Y, (short)loc.getMap().Id);
        }

        public void addVisibleObject(GameObject gameObject)
        {
            if (gameObject.MapId < _visibleObjects.Length)
            {
                _visibleObjects[gameObject.MapId][gameObject.Id] = gameObject;
            }
        }

        public void removeVisibleObject(GameObject gameObject)
        {
            if (gameObject.MapId < _visibleObjects.Length)
            {
                _visibleObjects[gameObject.MapId].Remove(gameObject.Id);
            }
        }

        public void moveVisibleObject(GameObject gameObject, int newMap) // set_Mapで新しいMapにするまえに呼ぶこと
        {
            if (gameObject.MapId != newMap)
            {
                if (gameObject.MapId < _visibleObjects.Length &&
                   newMap < _visibleObjects.Length)
                {
                    if (_visibleObjects[gameObject.MapId].Remove(gameObject.Id))
                    {
                        _visibleObjects[newMap][gameObject.Id] = gameObject;
                    }
                }
            }
        }

        private IDictionary<int, int> createLineMap(Point src, Point target)
        {
            IDictionary<int, int> lineMap = MapFactory.NewConcurrentMap<int, int>();

            /*
			 * http://www2.starcat.ne.jp/~fussy/algo/algo1-1.htmより
			 */
            int E;
            int x;
            int y;
            int key;
            int i;
            int x0 = src.X;
            int y0 = src.Y;
            int x1 = target.X;
            int y1 = target.Y;
            int sx = (x1 > x0) ? 1 : -1;
            int dx = (x1 > x0) ? x1 - x0 : x0 - x1;
            int sy = (y1 > y0) ? 1 : -1;
            int dy = (y1 > y0) ? y1 - y0 : y0 - y1;

            x = x0;
            y = y0;
            /* 傾きが1以下の場合 */
            if (dx >= dy)
            {
                E = -dx;
                for (i = 0; i <= dx; i++)
                {
                    key = (x << 16) + y;
                    lineMap[key] = key;
                    x += sx;
                    E += 2 * dy;
                    if (E >= 0)
                    {
                        y += sy;
                        E -= 2 * dx;
                    }
                }
                /* 傾きが1より大きい場合 */
            }
            else
            {
                E = -dy;
                for (i = 0; i <= dy; i++)
                {
                    key = (x << 16) + y;
                    lineMap[key] = key;
                    y += sy;
                    E += 2 * dx;
                    if (E >= 0)
                    {
                        x += sx;
                        E -= 2 * dy;
                    }
                }
            }

            return lineMap;
        }

        public IList<GameObject> getVisibleLineObjects(GameObject src, GameObject target)
        {
            int map = target.MapId;

            IList<GameObject> result = ListFactory.NewList<GameObject>();

            if (map < _visibleObjects.Length)
            {
                IDictionary<int, int> lineMap = createLineMap(src.Location, target.Location);

                foreach (GameObject element in _visibleObjects[map].Values)
                {
                    if (element == src)
                    {
                        continue;
                    }

                    int key = (element.X << 16) + element.Y;

                    if (lineMap.ContainsKey(key))
                    {
                        result.Add(element);
                    }
                }
            }

            return result;
        }

        public IList<GameObject> getVisibleBoxObjects(GameObject gameObject, int heading, int width, int height)
        {
            IList<GameObject> result = ListFactory.NewList<GameObject>();
            int map = gameObject.MapId;
            if (map < _visibleObjects.Length)
            {
                int x = gameObject.X;
                int y = gameObject.Y;
                L1Location location = gameObject.Location;
                int[] headingRotate = new int[] { 6, 7, 0, 1, 2, 3, 4, 5 };
                double cosSita = Math.Cos(headingRotate[heading] * Math.PI / 4);
                double sinSita = Math.Sin(headingRotate[heading] * Math.PI / 4);
                foreach (GameObject item in _visibleObjects[map].Values)
                {
                    if (item == gameObject)
                    {
                        continue;
                    }

                    if (item.MapId != map)
                    {
                        continue;
                    }

                    // 同じ座標に重なっている場合は範囲内とする
                    if (location == item.Location)
                    {
                        result.Add(item);
                        continue;
                    }

                    int distance = location.getTileLineDistance(item.Location);
                    // 直線距離が高さ、幅どちらよりも大きい場合、計算するまでもなく範囲外
                    if ((distance > height) && (distance > width))
                    {
                        continue;
                    }

                    // objectの位置を原点とするための座標補正
                    int x1 = item.X - x;
                    int y1 = item.Y - y;

                    // Z軸回転させ角度を0度にする。
                    int rotX = (int)(long)Math.Round(x1 * cosSita + y1 * sinSita, MidpointRounding.AwayFromZero);
                    int rotY = (int)(long)Math.Round(-x1 * sinSita + y1 * cosSita, MidpointRounding.AwayFromZero);

                    int xmin = 0;
                    int xmax = height;
                    int ymin = -width;
                    int ymax = width;

                    // 奥行きが射程とかみ合わないので直線距離で判定するように変更。
                    // if (rotX > xmin && rotX <= xmax && rotY >= ymin && rotY <=
                    // ymax) {
                    if ((rotX > xmin) && (distance <= xmax) && (rotY >= ymin) && (rotY <= ymax))
                    {
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        public IList<GameObject> getVisibleObjects(GameObject gameObject)
        {
            return getVisibleObjects(gameObject, -1);
        }

        public virtual IList<GameObject> getVisibleObjects(GameObject gameObject, int radius)
        {
            L1Map map = gameObject.Map;
            L1Location location = gameObject.Location;
            IList<GameObject> result = ListFactory.NewList<GameObject>();
            if (map.Id <= MAX_MAP_ID)
            {
                foreach (GameObject element in _visibleObjects[map.Id].Values)
                {
                    if (element == gameObject)
                    {
                        continue;
                    }

                    if (map.Id != element.Map.Id)
                    {
                        continue;
                    }

                    if (radius == -1)
                    {
                        if (location.isInScreen(element.Location))
                        {
                            result.Add(element);
                        }
                    }
                    else if (radius == 0)
                    {
                        if (location == element.Location)
                        {
                            result.Add(element);
                        }
                    }
                    else
                    {
                        if (location.getTileLineDistance(element.Location) <= radius)
                        {
                            result.Add(element);
                        }
                    }
                }
            }

            return result;
        }

        public IList<GameObject> getVisiblePoint(L1Location loc, int radius)
        {
            IList<GameObject> result = ListFactory.NewList<GameObject>();
            int mapId = loc.MapId; // ループ内で呼ぶと重いため

            if (mapId < _visibleObjects.Length)
            {
                foreach (GameObject element in _visibleObjects[mapId].Values)
                {
                    if (mapId != element.MapId)
                    {
                        continue;
                    }

                    if (loc.getTileLineDistance(element.Location) <= radius)
                    {
                        result.Add(element);
                    }
                }
            }

            return result;
        }

        public IList<L1PcInstance> getVisiblePlayer(GameObject gameObject)
        {
            return getVisiblePlayer(gameObject, -1);
        }

        public IList<L1PcInstance> getVisiblePlayer(GameObject gameObject, int radius)
        {
            int map = gameObject.MapId;
            L1Location location = gameObject.Location;
            IList<L1PcInstance> result = ListFactory.NewList<L1PcInstance>();

            foreach (L1PcInstance element in _allPlayers.Values)
            {
                if (element == gameObject)
                {
                    continue;
                }

                if (map != element.MapId)
                {
                    continue;
                }

                if (radius == -1)
                {
                    if (location.isInScreen(element.Location))
                    {
                        result.Add(element);
                    }
                }
                else if (radius == 0)
                {
                    if (location == element.Location)
                    {
                        result.Add(element);
                    }
                }
                else
                {
                    if (location.getTileLineDistance(element.Location) <= radius)
                    {
                        result.Add(element);
                    }
                }
            }
            return result;
        }

        public IList<L1PcInstance> getVisiblePlayerExceptTargetSight(GameObject gameObject, GameObject target)
        {
            int map = gameObject.MapId;
            Point objectPt = gameObject.Location;
            Point targetPt = target.Location;
            IList<L1PcInstance> result = ListFactory.NewList<L1PcInstance>();

            foreach (L1PcInstance element in _allPlayers.Values)
            {
                if (element == gameObject)
                {
                    continue;
                }

                if (map != element.MapId)
                {
                    continue;
                }

                if (Config.PC_RECOGNIZE_RANGE == -1)
                {
                    if (objectPt.isInScreen(element.Location))
                    {
                        if (!targetPt.isInScreen(element.Location))
                        {
                            result.Add(element);
                        }
                    }
                }
                else
                {
                    if (objectPt.getTileLineDistance(element.Location) <= Config.PC_RECOGNIZE_RANGE)
                    {
                        if (targetPt.getTileLineDistance(element.Location) > Config.PC_RECOGNIZE_RANGE)
                        {
                            result.Add(element);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// objectを認識できる範囲にいるプレイヤーを取得する
        /// </summary>
        /// <param name="object">
        /// @return </param>
        public IList<L1PcInstance> getRecognizePlayer(GameObject gameObject)
        {
            return getVisiblePlayer(gameObject, Config.PC_RECOGNIZE_RANGE);
        }

        /// <summary>
        /// ワールド内にいる指定された名前のプレイヤーを取得する。
        /// </summary>
        /// <param name="name">
        ///            - プレイヤー名(小文字・大文字は無視される) </param>
        /// <returns> 指定された名前のL1PcInstance。該当プレイヤーが存在しない場合はnullを返す。 </returns>
        public L1PcInstance getPlayer(string name)
        {
            if (_allPlayers.ContainsKey(name))
            {
                return _allPlayers[name];
            }
            else
            {
                return null;
            }
        }

        public IDictionary<int, GameObject> getVisibleObjects(int mapId)
        {
            return _visibleObjects[mapId];
        }

        public void addWar(L1War war)
        {
            if (!_allWars.Contains(war))
            {
                _allWars.Add(war);
            }
        }

        public void removeWar(L1War war)
        {
            if (_allWars.Contains(war))
            {
                _allWars.Remove(war);
            }
        }

        public void storeClan(L1Clan clan)
        {
            if (!_allClans.ContainsKey(clan.ClanName))
            {
                _allClans[clan.ClanName] = clan;
            }
        }

        public void removeClan(L1Clan clan)
        {
            if (_allClans.ContainsKey(clan.ClanName))
            {
                _allClans.Remove(clan.ClanName);
            }
        }

        public L1Clan getClan(string clan_name)
        {
            return _allClans[clan_name];
        }

        /// <summary>
        /// ワールド上に存在する全てのプレイヤーへパケットを送信する。
        /// </summary>
        /// <param name="packet">
        ///            送信するパケットを表すServerBasePacketオブジェクト。 </param>
        public virtual void broadcastPacketToAll(ServerBasePacket packet)
        {
            foreach (L1PcInstance pc in AllPlayers)
            {
                pc.sendPackets(packet);
            }
        }

        /// <summary>
        /// ワールド上に存在する全てのプレイヤーへサーバーメッセージを送信する。
        /// </summary>
        /// <param name="message">
        ///            送信するメッセージ </param>
        public virtual void broadcastServerMessage(string message)
        {
            broadcastPacketToAll(new S_SystemMessage(message));
        }

        public void Initialize()
        {
            for (int i = 0; i < _visibleObjects.Length; i++)
            {
                _visibleObjects[i] = MapFactory.NewConcurrentMap<int, GameObject>();
            }
        }
    }
}