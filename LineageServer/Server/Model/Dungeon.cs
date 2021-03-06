using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Gametime;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;
namespace LineageServer.Server.Model
{
    class DungeonController : IGameComponent
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.Dungeon);
        private static DungeonController _instance = null;

        private static IDictionary<string, NewDungeon> _dungeonMap = MapFactory.NewMap<string, NewDungeon>();

        private enum DungeonType
        {
            NONE,
            SHIP_FOR_FI,
            SHIP_FOR_HEINE,
            SHIP_FOR_PI,
            SHIP_FOR_HIDDENDOCK,
            SHIP_FOR_GLUDIN,
            SHIP_FOR_TI,
            TALKING_ISLAND_HOTEL,
            GLUDIO_HOTEL,
            SILVER_KNIGHT_HOTEL,
            WINDAWOOD_HOTEL,
            HEINE_HOTEL,
            GIRAN_HOTEL,
            OREN_HOTEL
        }

        public static DungeonController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DungeonController();
                }
                return _instance;
            }
        }
        public void Initialize()
        {
            IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                int srcMapId = dataSourceRow.getInt(Dungeon.Column_src_mapid);
                int srcX = dataSourceRow.getInt(Dungeon.Column_src_x);
                int srcY = dataSourceRow.getInt(Dungeon.Column_src_y);
                string key = (new StringBuilder()).Append(srcMapId).Append(srcX).Append(srcY).ToString();
                int newX = dataSourceRow.getInt(Dungeon.Column_new_x);
                int newY = dataSourceRow.getInt(Dungeon.Column_new_y);
                int NewMapId = dataSourceRow.getInt(Dungeon.Column_new_mapid);
                int heading = dataSourceRow.getInt(Dungeon.Column_new_heading);
                DungeonType dungeonType = DungeonType.NONE;
                if ((((srcX == 33423) || (srcX == 33424) || (srcX == 33425) || (srcX == 33426)) && (srcY == 33502) && (srcMapId == 4)) || (((srcX == 32733) || (srcX == 32734) || (srcX == 32735) || (srcX == 32736)) && (srcY == 32794) && (srcMapId == 83)))
                { // FI行きの船->ハイネ船着場
                    dungeonType = DungeonType.SHIP_FOR_FI;
                }
                else if ((((srcX == 32935) || (srcX == 32936) || (srcX == 32937)) && (srcY == 33058) && (srcMapId == 70)) || (((srcX == 32732) || (srcX == 32733) || (srcX == 32734) || (srcX == 32735)) && (srcY == 32796) && (srcMapId == 84)))
                { // ハイネ行きの船->FI船着場
                    dungeonType = DungeonType.SHIP_FOR_HEINE;
                }
                else if ((((srcX == 32750) || (srcX == 32751) || (srcX == 32752)) && (srcY == 32874) && (srcMapId == 445)) || (((srcX == 32731) || (srcX == 32732) || (srcX == 32733)) && (srcY == 32796) && (srcMapId == 447)))
                { // 海賊島行きの船->隠された船着場
                    dungeonType = DungeonType.SHIP_FOR_PI;
                }
                else if ((((srcX == 32296) || (srcX == 32297) || (srcX == 32298)) && (srcY == 33087) && (srcMapId == 440)) || (((srcX == 32735) || (srcX == 32736) || (srcX == 32737)) && (srcY == 32794) && (srcMapId == 446)))
                { // 隠された船着場行きの船->海賊島船着場
                    dungeonType = DungeonType.SHIP_FOR_HIDDENDOCK;
                }
                else if ((((srcX == 32630) || (srcX == 32631) || (srcX == 32632)) && (srcY == 32983) && (srcMapId == 0)) || (((srcX == 32733) || (srcX == 32734) || (srcX == 32735)) && (srcY == 32796) && (srcMapId == 5)))
                { // TalkingIslandShiptoAdenMainland->TalkingIsland
                    dungeonType = DungeonType.SHIP_FOR_GLUDIN;
                }
                else if ((((srcX == 32540) || (srcX == 32542) || (srcX == 32543) || (srcX == 32544) || (srcX == 32545)) && (srcY == 32728) && (srcMapId == 4)) || (((srcX == 32734) || (srcX == 32735) || (srcX == 32736) || (srcX == 32737)) && (srcY == 32794) && (srcMapId == 6)))
                { // AdenMainlandShiptoTalkingIsland->AdenMainland
                    dungeonType = DungeonType.SHIP_FOR_TI;
                }
                else if ((srcX == 32600) && (srcY == 32931) && (srcMapId == 0))
                { // 說話之島旅館
                    dungeonType = DungeonType.TALKING_ISLAND_HOTEL;
                }
                else if ((srcX == 32632) && (srcY == 32761) && (srcMapId == 4))
                { // 古魯丁旅館
                    dungeonType = DungeonType.GLUDIO_HOTEL;
                }
                else if ((srcX == 33116) && (srcY == 33379) && (srcMapId == 4))
                { // 銀騎士旅館
                    dungeonType = DungeonType.SILVER_KNIGHT_HOTEL;
                }
                else if ((srcX == 32628) && (srcY == 33167) && (srcMapId == 4))
                { // 風木旅館
                    dungeonType = DungeonType.WINDAWOOD_HOTEL;
                }
                else if ((srcX == 33605) && (srcY == 33275) && (srcMapId == 4))
                { // 海音旅館
                    dungeonType = DungeonType.HEINE_HOTEL;
                }
                else if ((srcX == 33437) && (srcY == 32789) && (srcMapId == 4))
                { // 奇岩旅館
                    dungeonType = DungeonType.GIRAN_HOTEL;
                }
                else if ((srcX == 34068) && (srcY == 32254) && (srcMapId == 4))
                { // 歐瑞旅館
                    dungeonType = DungeonType.OREN_HOTEL;
                }
                NewDungeon newDungeon = new NewDungeon(newX, newY, (short)NewMapId, heading, dungeonType);

                _dungeonMap[key] = newDungeon;
            }
        }
        private DungeonController()
        {

        }

        private class NewDungeon
        {
            internal int _newX;

            internal int _newY;

            internal short _NewMapId;

            internal int _heading;

            internal DungeonType _dungeonType;

            internal NewDungeon(int newX, int newY, short NewMapId, int heading, DungeonType dungeonType)
            {
                _newX = newX;
                _newY = newY;
                _NewMapId = NewMapId;
                _heading = heading;
                _dungeonType = dungeonType;

            }
        }

        public virtual bool dg(int locX, int locY, int mapId, L1PcInstance pc)
        {
            int servertime = Container.Instance.Resolve<IGameTimeClock>().CurrentTime().Seconds;
            int nowtime = servertime % 86400;
            string key = (new StringBuilder()).Append(mapId).Append(locX).Append(locY).ToString();
            if (_dungeonMap.ContainsKey(key))
            {
                NewDungeon newDungeon = _dungeonMap[key];
                short NewMap = newDungeon._NewMapId;
                int newX = newDungeon._newX;
                int newY = newDungeon._newY;
                int heading = newDungeon._heading;
                DungeonType dungeonType = newDungeon._dungeonType;
                bool teleportable = false;

                if (dungeonType == DungeonType.NONE)
                {
                    teleportable = true;
                }
                else
                {
                    if (dungeonType == DungeonType.TALKING_ISLAND_HOTEL || dungeonType == DungeonType.GLUDIO_HOTEL || dungeonType == DungeonType.WINDAWOOD_HOTEL || dungeonType == DungeonType.SILVER_KNIGHT_HOTEL || dungeonType == DungeonType.HEINE_HOTEL || dungeonType == DungeonType.GIRAN_HOTEL || dungeonType == DungeonType.OREN_HOTEL)
                    {
                        int npcid = 0;
                        int[] data = null;
                        if (dungeonType == DungeonType.TALKING_ISLAND_HOTEL)
                        {
                            npcid = 70012; // 說話之島 - 瑟琳娜
                            data = new int[] { 32745, 32803, 16384, 32743, 32808, 16896 };
                        }
                        else if (dungeonType == DungeonType.GLUDIO_HOTEL)
                        {
                            npcid = 70019; // 古魯丁 - 羅利雅
                            data = new int[] { 32743, 32803, 17408, 32744, 32807, 17920 };
                        }
                        else if (dungeonType == DungeonType.GIRAN_HOTEL)
                        {
                            npcid = 70031; // 奇岩 - 瑪理
                            data = new int[] { 32744, 32803, 18432, 32744, 32807, 18944 };
                        }
                        else if (dungeonType == DungeonType.OREN_HOTEL)
                        {
                            npcid = 70065; // 歐瑞 - 小安安
                            data = new int[] { 32744, 32803, 19456, 32744, 32807, 19968 };
                        }
                        else if (dungeonType == DungeonType.WINDAWOOD_HOTEL)
                        {
                            npcid = 70070; // 風木 - 維萊莎
                            data = new int[] { 32744, 32803, 20480, 32744, 32807, 20992 };
                        }
                        else if (dungeonType == DungeonType.SILVER_KNIGHT_HOTEL)
                        {
                            npcid = 70075; // 銀騎士 - 米蘭德
                            data = new int[] { 32744, 32803, 21504, 32744, 32807, 22016 };
                        }
                        else if (dungeonType == DungeonType.HEINE_HOTEL)
                        {
                            npcid = 70084; // 海音 - 伊莉
                            data = new int[] { 32744, 32803, 22528, 32744, 32807, 23040 };
                        }

                        int type = checkInnKey(pc, npcid);

                        if (type == 1)
                        { // 房間
                            newX = data[0];
                            newY = data[1];
                            NewMap = (short)data[2];
                            heading = 6;
                            teleportable = true;
                        }
                        else if (type == 2)
                        { // 會議室
                            newX = data[3];
                            newY = data[4];
                            NewMap = (short)data[5];
                            heading = 6;
                            teleportable = true;
                        }
                    }
                    else if (((nowtime >= 15 * 360) && (nowtime < 25 * 360)) || ((nowtime >= 45 * 360) && (nowtime < 55 * 360)) || ((nowtime >= 75 * 360) && (nowtime < 85 * 360)) || ((nowtime >= 105 * 360) && (nowtime < 115 * 360)) || ((nowtime >= 135 * 360) && (nowtime < 145 * 360)) || ((nowtime >= 165 * 360) && (nowtime < 175 * 360)) || ((nowtime >= 195 * 360) && (nowtime < 205 * 360)) || ((nowtime >= 225 * 360) && (nowtime < 235 * 360)))
                    {
                        if ((pc.Inventory.checkItem(40299, 1) && (dungeonType == DungeonType.SHIP_FOR_GLUDIN)) || (pc.Inventory.checkItem(40301, 1) && (dungeonType == DungeonType.SHIP_FOR_HEINE)) || (pc.Inventory.checkItem(40302, 1) && (dungeonType == DungeonType.SHIP_FOR_PI)))
                        { // ShipPirateislandtoHiddendock
                            teleportable = true;
                        }
                    }
                    else if (((nowtime >= 0) && (nowtime < 360)) || ((nowtime >= 30 * 360) && (nowtime < 40 * 360)) || ((nowtime >= 60 * 360) && (nowtime < 70 * 360)) || ((nowtime >= 90 * 360) && (nowtime < 100 * 360)) || ((nowtime >= 120 * 360) && (nowtime < 130 * 360)) || ((nowtime >= 150 * 360) && (nowtime < 160 * 360)) || ((nowtime >= 180 * 360) && (nowtime < 190 * 360)) || ((nowtime >= 210 * 360) && (nowtime < 220 * 360)))
                    {
                        if ((pc.Inventory.checkItem(40298, 1) && (dungeonType == DungeonType.SHIP_FOR_TI)) || (pc.Inventory.checkItem(40300, 1) && (dungeonType == DungeonType.SHIP_FOR_FI)) || (pc.Inventory.checkItem(40303, 1) && (dungeonType == DungeonType.SHIP_FOR_HIDDENDOCK)))
                        { // ShipHiddendocktoPirateisland
                            teleportable = true;
                        }
                    }
                }

                if (teleportable)
                {
                    // 2秒無敵狀態。
                    pc.setSkillEffect(L1SkillId.ABSOLUTE_BARRIER, 2000);
                    pc.stopHpRegeneration();
                    pc.stopMpRegeneration();
                    pc.stopHpRegenerationByDoll();
                    pc.stopMpRegenerationByDoll();
                    L1Teleport.teleport(pc, newX, newY, NewMap, heading, false);
                    return true;
                }
            }
            return false;
        }

        // 檢查身上的鑰匙
        private int checkInnKey(L1PcInstance pc, int npcid)
        {
            foreach (L1ItemInstance item in pc.Inventory.Items)
            {
                if (item.InnNpcId == npcid)
                { // 鑰匙與旅館NPC相符
                    for (int i = 0; i < 16; i++)
                    {
                        L1Inn inn = InnTable.Instance.getTemplate(npcid, i);
                        if (inn.KeyId == item.KeyId)
                        {
                            DateTime dueTime = item.DueTime;
                            if (dueTime != default(DateTime))
                            {
                                // 時間不為空值
                                if (((DateTime.Now - dueTime).TotalMilliseconds / 1000) < 0)
                                { // 租用時間未到
                                    pc.InnKeyId = item.KeyId; // 登入此鑰匙
                                    return item.checkRoomOrHall() ? 2 : 1; // 1:房間 2:會議室
                                }
                            }
                        }
                    }
                }
            }
            return 0;
        }

    }

}