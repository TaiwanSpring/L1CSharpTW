using LineageServer.Interfaces;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Model.map;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Types;
using LineageServer.Server.Server.Utils.collections;
using System;
using System.Collections.Generic;
using System.Linq;
namespace LineageServer.Server.Server.Model
{
    class L1World
    {
        private static ILogger _log = Logger.getLogger(nameof(L1World));

        private readonly IDictionary<string, L1PcInstance> _allPlayers;

        private readonly IDictionary<int, L1PetInstance> _allPets;

        private readonly IDictionary<int, L1SummonInstance> _allSummons;

        private readonly IDictionary<int, L1Object> _allObjects;

        private readonly IDictionary<int, L1Object>[] _visibleObjects;

        private readonly IList<L1War> _allWars;

        private readonly IDictionary<string, L1Clan> _allClans;

        private int _weather = 4;

        private bool _worldChatEnabled = true;

        private bool _processingContributionTotal = false;

        private const int MAX_MAP_ID = 10000;

        private static L1World _instance;

        //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @SuppressWarnings("unchecked") private L1World()
        private L1World()
        {
            _allPlayers = Maps.newConcurrentMap<string, L1PcInstance>(); // 全てのプレイヤー
            _allPets = Maps.newConcurrentMap<int, L1PetInstance>(); // 全てのペット
            _allSummons = Maps.newConcurrentMap<int, L1SummonInstance>(); // 全てのサモンモンスター
            _allObjects = Maps.newConcurrentMap<int, L1Object>(); // 全てのオブジェクト(L1ItemInstance入り、L1Inventoryはなし)
            _visibleObjects = new IDictionary<int, L1Object>[MAX_MAP_ID + 1]; // マップ毎のオブジェクト(L1Inventory入り、L1ItemInstanceはなし)
            _allWars = Lists.newConcurrentList<L1War>(); // 全ての戦争
            _allClans = Maps.newConcurrentMap<string, L1Clan>(); // 所有的血盟物件 (Online/Offline)

            for (int i = 0; i <= MAX_MAP_ID; i++)
            {
                _visibleObjects[i] = Maps.newConcurrentMap<int, L1Object>();
            }
        }

        public static L1World Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new L1World();
                }
                return _instance;
            }
        }

        /// <summary>
        /// 全ての状態をクリアする。<br>
        /// デバッグ、テストなどの特殊な目的以外で呼び出してはならない。
        /// </summary>
        public virtual void clear()
        {
            _instance = new L1World();
        }

        public virtual void storeObject(L1Object l1Object)
        {
            if (l1Object == null)
            {
                throw new System.NullReferenceException();
            }

            _allObjects[l1Object.Id] = l1Object;
            if (l1Object is L1PcInstance)
            {
                _allPlayers[((L1PcInstance)l1Object).Name] = (L1PcInstance)l1Object;
            }
            if (l1Object is L1PetInstance)
            {
                _allPets[l1Object.Id] = (L1PetInstance)l1Object;
            }
            if (l1Object is L1SummonInstance)
            {
                _allSummons[l1Object.Id] = (L1SummonInstance)l1Object;
            }
        }

        public virtual void removeObject(L1Object l1Object)
        {
            if (l1Object == null)
            {
                throw new System.NullReferenceException();
            }

            _allObjects.Remove(l1Object.Id);
            if (l1Object is L1PcInstance)
            {
                _allPlayers.Remove(((L1PcInstance)l1Object).Name);
            }
            if (l1Object is L1PetInstance)
            {
                _allPets.Remove(l1Object.Id);
            }
            if (l1Object is L1SummonInstance)
            {
                _allSummons.Remove(l1Object.Id);
            }
        }

        public virtual L1Object findObject(int oID)
        {
            return _allObjects[oID];
        }

        // _allObjectsのビュー
        private ICollection<L1Object> _allValues;

        public virtual ICollection<L1Object> Object
        {
            get
            {
                ICollection<L1Object> vs = _allValues;
                return (vs != null) ? vs : (_allValues = _allObjects.Values.ToHashSet());
            }
        }

        public virtual L1GroundInventory getInventory(int x, int y, short map)
        {
            int inventoryKey = ((x - 30000) * 10000 + (y - 30000)) * -1; // xyのマイナス値をインベントリキーとして使用

            object l1Object = _visibleObjects[map][inventoryKey];
            if (l1Object == null)
            {
                return new L1GroundInventory(inventoryKey, x, y, map);
            }
            else
            {
                return (L1GroundInventory)l1Object;
            }
        }

        public virtual L1GroundInventory getInventory(L1Location loc)
        {
            return getInventory(loc.X, loc.Y, (short)loc.getMap().Id);
        }

        public virtual void addVisibleObject(L1Object l1Object)
        {
            if (l1Object.MapId <= MAX_MAP_ID)
            {
                _visibleObjects[l1Object.MapId][l1Object.Id] = l1Object;
            }
        }

        public virtual void removeVisibleObject(L1Object l1Object)
        {
            if (l1Object.MapId <= MAX_MAP_ID)
            {
                _visibleObjects[l1Object.MapId].Remove(l1Object.Id);
            }
        }

        public virtual void moveVisibleObject(L1Object l1Object, int newMap) // set_Mapで新しいMapにするまえに呼ぶこと
        {
            if (l1Object.MapId != newMap)
            {
                if (l1Object.MapId <= MAX_MAP_ID)
                {
                    _visibleObjects[l1Object.MapId].Remove(l1Object.Id);
                }
                if (newMap <= MAX_MAP_ID)
                {
                    _visibleObjects[newMap][l1Object.Id] = l1Object;
                }
            }
        }

        private IDictionary<int, int> createLineMap(Point src, Point target)
        {
            IDictionary<int, int> lineMap = Maps.newConcurrentMap<int, int>();

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

        public virtual IList<L1Object> getVisibleLineObjects(L1Object src, L1Object target)
        {
            IDictionary<int, int> lineMap = createLineMap(src.Location, target.Location);

            int map = target.MapId;
            IList<L1Object> result = Lists.newList<L1Object>();

            if (map <= MAX_MAP_ID)
            {
                foreach (L1Object element in _visibleObjects[map].Values)
                {
                    if (element.Equals(src))
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

        public virtual IList<L1Object> getVisibleBoxObjects(L1Object l1Object, int heading, int width, int height)
        {
            int x = l1Object.X;
            int y = l1Object.Y;
            int map = l1Object.MapId;
            L1Location location = l1Object.Location;
            IList<L1Object> result = Lists.newList<L1Object>();
            int[] headingRotate = new int[] { 6, 7, 0, 1, 2, 3, 4, 5 };
            double cosSita = Math.Cos(headingRotate[heading] * Math.PI / 4);
            double sinSita = Math.Sin(headingRotate[heading] * Math.PI / 4);

            if (map <= MAX_MAP_ID)
            {
                foreach (L1Object element in _visibleObjects[map].Values)
                {
                    if (element.Equals(l1Object))
                    {
                        continue;
                    }
                    if (map != element.MapId)
                    {
                        continue;
                    }

                    // 同じ座標に重なっている場合は範囲内とする
                    if (location.isSamePoint(element.Location))
                    {
                        result.Add(element);
                        continue;
                    }

                    int distance = location.getTileLineDistance(element.Location);
                    // 直線距離が高さ、幅どちらよりも大きい場合、計算するまでもなく範囲外
                    if ((distance > height) && (distance > width))
                    {
                        continue;
                    }

                    // objectの位置を原点とするための座標補正
                    int x1 = element.X - x;
                    int y1 = element.Y - y;

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
                        result.Add(element);
                    }
                }
            }

            return result;
        }

        public virtual IList<L1Object> getVisibleObjects(L1Object l1Object)
        {
            return getVisibleObjects(l1Object, -1);
        }

        public virtual IList<L1Object> getVisibleObjects(L1Object l1Object, int radius)
        {
            L1Map map = l1Object.getMap();
            Point pt = l1Object.Location;
            IList<L1Object> result = Lists.newList<L1Object>();
            if (map.Id <= MAX_MAP_ID)
            {
                foreach (L1Object element in _visibleObjects[map.Id].Values)
                {
                    if (element.Equals(l1Object))
                    {
                        continue;
                    }
                    if (map != element.getMap())
                    {
                        continue;
                    }

                    if (radius == -1)
                    {
                        if (pt.isInScreen(element.Location))
                        {
                            result.Add(element);
                        }
                    }
                    else if (radius == 0)
                    {
                        if (pt.isSamePoint(element.Location))
                        {
                            result.Add(element);
                        }
                    }
                    else
                    {
                        if (pt.getTileLineDistance(element.Location) <= radius)
                        {
                            result.Add(element);
                        }
                    }
                }
            }

            return result;
        }

        public virtual IList<L1Object> getVisiblePoint(L1Location loc, int radius)
        {
            IList<L1Object> result = Lists.newList<L1Object>();
            int mapId = loc.MapId; // ループ内で呼ぶと重いため

            if (mapId <= MAX_MAP_ID)
            {
                foreach (L1Object element in _visibleObjects[mapId].Values)
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

        public virtual IList<L1PcInstance> getVisiblePlayer(L1Object l1Object)
        {
            return getVisiblePlayer(l1Object, -1);
        }

        public virtual IList<L1PcInstance> getVisiblePlayer(L1Object l1Object, int radius)
        {
            int map = l1Object.MapId;
            Point pt = l1Object.Location;
            IList<L1PcInstance> result = Lists.newList<L1PcInstance>();

            foreach (L1PcInstance element in _allPlayers.Values)
            {
                if (element.Equals(l1Object))
                {
                    continue;
                }

                if (map != element.MapId)
                {
                    continue;
                }

                if (radius == -1)
                {
                    if (pt.isInScreen(element.Location))
                    {
                        result.Add(element);
                    }
                }
                else if (radius == 0)
                {
                    if (pt.isSamePoint(element.Location))
                    {
                        result.Add(element);
                    }
                }
                else
                {
                    if (pt.getTileLineDistance(element.Location) <= radius)
                    {
                        result.Add(element);
                    }
                }
            }
            return result;
        }

        public virtual IList<L1PcInstance> getVisiblePlayerExceptTargetSight(L1Object l1Object, L1Object target)
        {
            int map = l1Object.MapId;
            Point objectPt = l1Object.Location;
            Point targetPt = target.Location;
            IList<L1PcInstance> result = Lists.newList<L1PcInstance>();

            foreach (L1PcInstance element in _allPlayers.Values)
            {
                if (element.Equals(l1Object))
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
        public virtual IList<L1PcInstance> getRecognizePlayer(L1Object l1Object)
        {
            return getVisiblePlayer(l1Object, Config.PC_RECOGNIZE_RANGE);
        }

        // _allPlayersのビュー
        private ICollection<L1PcInstance> _allPlayerValues;

        public virtual ICollection<L1PcInstance> AllPlayers
        {
            get
            {
                ICollection<L1PcInstance> vs = _allPlayerValues;
                return (vs != null) ? vs : (_allPlayerValues = _allPlayers.Values.ToHashSet());
            }
        }

        /// <summary>
        /// ワールド内にいる指定された名前のプレイヤーを取得する。
        /// </summary>
        /// <param name="name">
        ///            - プレイヤー名(小文字・大文字は無視される) </param>
        /// <returns> 指定された名前のL1PcInstance。該当プレイヤーが存在しない場合はnullを返す。 </returns>
        public virtual L1PcInstance getPlayer(string name)
        {
            if (_allPlayers.ContainsKey(name))
            {
                return _allPlayers[name];
            }
            foreach (L1PcInstance each in AllPlayers)
            {
                if (each.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return each;
                }
            }
            return null;
        }

        // _allPetsのビュー
        private ICollection<L1PetInstance> _allPetValues;

        public virtual ICollection<L1PetInstance> AllPets
        {
            get
            {
                ICollection<L1PetInstance> vs = _allPetValues;
                return (vs != null) ? vs : (_allPetValues = _allPets.Values.ToHashSet());
            }
        }

        // _allSummonsのビュー
        private ICollection<L1SummonInstance> _allSummonValues;

        public virtual ICollection<L1SummonInstance> AllSummons
        {
            get
            {
                ICollection<L1SummonInstance> vs = _allSummonValues;
                return (vs != null) ? vs : (_allSummonValues = _allSummons.Values.ToHashSet());
            }
        }

        public IDictionary<int, L1Object> AllVisibleObjects
        {
            get
            {
                return _allObjects;
            }
        }

        public IDictionary<int, L1Object>[] VisibleObjects
        {
            get
            {
                return _visibleObjects;
            }
        }

        public IDictionary<int, L1Object> getVisibleObjects(int mapId)
        {
            return _visibleObjects[mapId];
        }

        public virtual object getRegion(object l1Object)
        {
            return null;
        }

        public virtual void addWar(L1War war)
        {
            if (!_allWars.Contains(war))
            {
                _allWars.Add(war);
            }
        }

        public virtual void removeWar(L1War war)
        {
            if (_allWars.Contains(war))
            {
                _allWars.Remove(war);
            }
        }

        // _allWarsのビュー
        private IList<L1War> _allWarList;

        public virtual IList<L1War> WarList
        {
            get
            {
                IList<L1War> vs = _allWarList;
                return vs == null ? (_allWarList = _allWars.ToArray()) : vs;
            }
        }

        public virtual void storeClan(L1Clan clan)
        {
            L1Clan temp = getClan(clan.ClanName);
            if (temp == null)
            {
                _allClans[clan.ClanName] = clan;
            }
        }

        public virtual void removeClan(L1Clan clan)
        {
            L1Clan temp = getClan(clan.ClanName);
            if (temp != null)
            {
                _allClans.Remove(clan.ClanName);
            }
        }

        public virtual L1Clan getClan(string clan_name)
        {
            return _allClans[clan_name];
        }

        // _allClansのビュー
        private ICollection<L1Clan> _allClanValues;

        public virtual ICollection<L1Clan> AllClans
        {
            get
            {
                ICollection<L1Clan> vs = _allClanValues;
                return (vs != null) ? vs : (_allClanValues = _allClans.Values.ToHashSet());
            }
        }

        public virtual int Weather
        {
            set
            {
                _weather = value;
            }
            get
            {
                return _weather;
            }
        }


        public virtual void set_worldChatElabled(bool flag)
        {
            _worldChatEnabled = flag;
        }

        public virtual bool WorldChatElabled
        {
            get
            {
                return _worldChatEnabled;
            }
        }

        public virtual bool ProcessingContributionTotal
        {
            set
            {
                _processingContributionTotal = value;
            }
            get
            {
                return _processingContributionTotal;
            }
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
    }
}