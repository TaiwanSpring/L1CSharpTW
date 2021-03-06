using LineageServer.Models;
using System;
using System.Extensions;
using System.IO;
namespace LineageServer
{
    static class Config
    {
        #region server.ConfigLoader
        /// <summary>
        /// Server listener ip, * = all ip
        /// </summary>
        public static string GAME_SERVER_HOST_NAME;
        /// <summary>
        /// Server listener port
        /// </summary>
        public static int GAME_SERVER_PORT;
        /// <summary>
        /// 
        /// </summary>
        public static string DB_Ip;
        /// <summary>
        /// 
        /// </summary>
        public static string DB_Name;
        /// <summary>
        /// 
        /// </summary>
        public static string DB_LOGIN;
        /// <summary>
        /// 
        /// </summary>
        public static string DB_PASSWORD;
        /// <summary>
        /// 客戶端語言 0.US 3.Taiwan 4.Janpan 5.China
        /// </summary>
        public static int CLIENT_LANGUAGE;

        public static string CLIENT_LANGUAGE_CODE;

        public static string[] LANGUAGE_CODE_ARRAY = new string[] { "UTF8", "EUCKR", "UTF8", "BIG5", "SJIS", "GBK" };
        /// <summary>
        /// 是否啟用 DNS 反向驗證
        /// </summary>
        public static bool HOSTNAME_LOOKUPS;
        /// <summary>
        /// 客戶端無動作時自動斷線時間設定 (單位: 分) 0 = 關閉
        /// </summary>
        public static int AUTOMATIC_KICK;
        /// <summary>
        /// 是否再登入畫面即可創建帳號 True=是 False=否
        /// </summary>
        public static bool AUTO_CREATE_ACCOUNTS;
        /// <summary>
        /// 定義允許多少數量的玩家同時在線上(每個玩家大約使用3KB上傳頻寬)
        /// </summary>
        public static short MAX_ONLINE_USERS;
        /// <summary>
        /// 是否自動生成地圖快取檔案
        /// </summary>
        public static bool CACHE_MAP_FILES;
        /// <summary>
        /// 使用 V2 地圖
        /// </summary>
        public static bool LOAD_V2_MAP_FILES;
        /// <summary>
        /// 加速器偵測功能
        /// </summary>
        public static bool CHECK_MOVE_INTERVAL;
        /// <summary>
        /// 加速器偵測功能
        /// </summary>
        public static bool CHECK_ATTACK_INTERVAL;
        /// <summary>
        /// 加速器偵測功能
        /// </summary>
        public static bool CHECK_SPELL_INTERVAL;
        /// <summary>
        /// 設定不正常封包數值,滿足條件則切斷連線
        /// </summary>
        public static short INJUSTICE_COUNT;
        /// <summary>
        /// 設定如果參雜正常封包在不正常封包中,數值滿足時 InjusticeCount歸 0
        /// </summary>
        public static int JUSTICE_COUNT;
        /// <summary>
        /// 加速器檢查嚴密度,為免除錯誤檢測設定數值允許幾%加速
        /// </summary>
        public static int CHECK_STRICTNESS;

        public static int ILLEGAL_SPEEDUP_PUNISHMENT;
        /// <summary>
        /// 伺服器自動存檔時間間隔 (單位: 秒)
        /// </summary>
        public static int AUTOSAVE_INTERVAL;
        /// <summary>
        /// 定時自動儲存角色裝備資料時間間隔 (單位: 秒)
        /// </summary>
        public static int AUTOSAVE_INTERVAL_INVENTORY;
        /// <summary>
        /// 發送到一個範圍的信息給客戶端對象, -1表示只傳送給屏幕上看到的對象
        /// </summary>
        public static int PC_RECOGNIZE_RANGE;
        /// <summary>
        /// 人物資訊(F5~12快捷鍵和人物血條位置等)是否於伺服器統一管理 true or false
        /// </summary>
        public static bool CHARACTER_CONFIG_IN_SERVER_SIDE;
        /// <summary>
        /// 是否允許雙開(同IP同時連線) true or false
        /// </summary>
        public static bool ALLOW_2PC;
        /// <summary>
        /// 允許降等的水平範圍（檢測死亡降等範圍）從現在最高等級和過去最低等級相減、如果數值超過設定數值則切斷連線。0 = 關閉
        /// </summary>
        public static int LEVEL_DOWN_RANGE;
        /// <summary>
        /// 定義是否瞬間移動之前等待客戶端通知 (true=開啟 false=關閉)
        /// </summary>
        public static bool SEND_PACKET_BEFORE_TELEPORT;
        /// <summary>
        /// 是否刪除DB無用的資料
        /// </summary>
        public static bool DETECT_DATABASE_LEAKS;
        /// <summary>
        /// 是否開啟cmd互動指令
        /// </summary>
        public static bool CONSOLE_COMMAND_ACTIVE;

        #endregion server.ConfigLoader

        /// <summary>
        /// Rate control </summary>
        public static double RATE_XP;

        public static double RATE_LA;

        public static double RATE_KARMA;

        public static double RATE_DROP_ADENA;

        public static double RATE_DROP_ITEMS;

        public static int ENCHANT_CHANCE_WEAPON;

        public static int ENCHANT_CHANCE_ARMOR;

        public static int ATTR_ENCHANT_CHANCE;

        public static double RATE_WEIGHT_LIMIT;

        public static double RATE_WEIGHT_LIMIT_PET;

        public static double RATE_SHOP_SELLING_PRICE;

        public static double RATE_SHOP_PURCHASING_PRICE;

        public static int CREATE_CHANCE_DIARY;

        public static int CREATE_CHANCE_RECOLLECTION;

        public static int CREATE_CHANCE_MYSTERIOUS;

        public static int CREATE_CHANCE_PROCESSING;

        public static int CREATE_CHANCE_PROCESSING_DIAMOND;

        public static int CREATE_CHANCE_DANTES;

        public static int CREATE_CHANCE_ANCIENT_AMULET;

        public static int CREATE_CHANCE_HISTORY_BOOK;

        public static int MAGIC_STONE_TYPE; // 附魔石類型

        public static int MAGIC_STONE_LEVEL; // 附魔石階級

        #region configLoader.ConfigLoader
        /// <summary>
        /// 全體聊天最低等級限制
        /// </summary>
        public static short GLOBAL_CHAT_LEVEL;
        /// <summary>
        /// 密語最低等級限制
        /// </summary>
        public static short WHISPER_CHAT_LEVEL;
        /// <summary>
        /// 設定自動取得道具的方式 0-掉落地上, 1-掉落寵物身上, 2-掉落角色身上
        /// </summary>
        public static int AUTO_LOOT;
        /// <summary>
        /// 設定道具掉落的範圍大小
        /// </summary>
        public static int LOOTING_RANGE;
        /// <summary>
        /// 設定PVP的模式 True=是, False=不是
        /// </summary>
        public static bool ALT_NONPVP;
        /// <summary>
        /// 設定GM是否顯示傷害訊息 True=顯示, False=不顯示
        /// </summary>
        public static bool ALT_ATKMSG;
        /// <summary>
        /// 是否允許自己更改稱號
        /// </summary>
        public static bool CHANGE_TITLE_BY_ONESELF;
        /// <summary>
        /// 血盟人數上限, 0的話，按照王的魅力計算
        /// </summary>
        public static int MAX_CLAN_MEMBER;
        /// <summary>
        /// 是否開啟血盟聯盟系統(加入另一個王子的血盟) True=啟用, False=停用
        /// </summary>
        public static bool CLAN_ALLIANCE;
        /// <summary>
        /// 組隊人數上限
        /// </summary>
        public static int MAX_PT;
        /// <summary>
        /// 組隊聊天人數上限
        /// </summary>
        public static int MAX_CHAT_PT;
        /// <summary>
        /// 設定攻城戰中紅人死亡後是否會受到處罰 True=是, False=否
        /// </summary>
        public static bool SIM_WAR_PENALTY;
        /// <summary>
        /// 設定重新登入時是否在出生地 True=是, False=否
        /// </summary>
        public static bool GET_BACK;
        /// <summary>
        /// 地圖上地面道具刪除設置 none|std|auto none - 不刪除 std - 指定道具每隔多長時間刪除 auto - 指定每隔多長時間地面道具一起刪除
        /// </summary>
        public static string ALT_ITEM_DELETION_TYPE;
        /// <summary>
        /// 設定物品在地面自動清除掉的時間（分）,(0=關閉自動清除地面物品)
        /// </summary>
        public static int ALT_ITEM_DELETION_TIME;
        /// <summary>
        /// 設定人物周圍不清除物品範圍大小 ItemDeletionType設定std的話，範圍大小無視 0-5（0指定，連玩家腳下的東西也刪除）
        /// </summary>
        public static int ALT_ITEM_DELETION_RANGE;
        /// <summary>
        /// 設定是否開啟GM商店... True=開啟, False=關閉
        /// </summary>
        public static bool ALT_GMSHOP;
        /// <summary>
        /// 設定GM商店編號最小值與最大值設定，可查看在spawnlist_npc內的編號進行設定
        /// </summary>
        public static int ALT_GMSHOP_MIN_ID;
        /// <summary>
        /// 設定GM商店編號最小值與最大值設定，可查看在spawnlist_npc內的編號進行設定
        /// </summary>
        public static int ALT_GMSHOP_MAX_ID;
        /// <summary>
        /// 南瓜怪任務開關設置... True=開, False=關
        /// </summary>
        public static bool ALT_HALLOWEENIVENT;
        /// <summary>
        /// 日本特典道具NPC開關設置... True=開, False=關
        /// </summary>
        public static bool ALT_JPPRIVILEGED;
        /// <summary>
        /// 說話卷軸任務開關設置... True=開, False=關
        /// </summary>
        public static bool ALT_TALKINGSCROLLQUEST;
        /// <summary>
        /// 設定 /who 指令是否可以使用 True=可以, False=不可以
        /// </summary>
        public static bool ALT_WHO_COMMAND;
        /// <summary>
        /// 
        /// </summary>
        public static int ALT_WHO_TYPE;
        /// <summary>
        /// 設定110級是否可以獲得返生藥水 True=可以, False=不可以
        /// </summary>
        public static bool ALT_REVIVAL_POTION;
        /// <summary>
        /// 設定 攻城戰時間(d:日 h:時 m:分)
        /// </summary>
        public static TimeSpan ALT_WAR_TIME;
        /// <summary>
        /// 設定 攻城日的間隔(d:日 h:時 m:分)
        /// </summary>
        public static TimeSpan ALT_WAR_INTERVAL;

        public static int ALT_RATE_OF_DUTY;
        /// <summary>
        /// 設定 是否使用範圍性怪物刷新...True=是, False=否
        /// </summary>
        public static bool SPAWN_HOME_POINT;
        /// <summary>
        /// 設定 怪物刷新的範圍大小。
        /// </summary>
        public static int SPAWN_HOME_POINT_RANGE;
        /// <summary>
        /// SpawnHomePointCount、SpawnHomePointDelay 設定迴避適用出生點 (不適用於首領怪等等) 
        /// 適用出生點設定最小 count (2的話，count 只有2以上 spawnlist 設定出生點)
        /// </summary>
        public static int SPAWN_HOME_POINT_COUNT;
        /// <summary>
        /// 適用出生點設定的最大 min_respawn_delay (100的話，min_respawn_delay 只有100以下 spawnlist 設定出生點)
        /// </summary>
        public static int SPAWN_HOME_POINT_DELAY;
        /// <summary>
        /// 服務器啟動時Boss是否出現。... True=是, False=否。 false的話，將依照BossCycle設置進行。
        /// </summary>
        public static bool INIT_BOSS_SPAWN;
        /// <summary>
        /// 妖精森林 元素石 的數量
        /// </summary>
        public static int ELEMENTAL_STONE_AMOUNT;
        /// <summary>
        /// 盟屋稅金的支付期限(日)
        /// </summary>
        public static int HOUSE_TAX_INTERVAL;
        /// <summary>
        /// 設定魔法娃娃召喚數量上限
        /// </summary>
        public static int MAX_DOLL_COUNT;
        /// <summary>
        /// 釋放元素技能的使用 True to On, False to Off
        /// </summary>
        public static bool RETURN_TO_NATURE;
        /// <summary>
        /// NPC(召喚, 寵物)身上可以持有的最大物品數量
        /// </summary>
        public static int MAX_NPC_ITEM;
        /// <summary>
        /// 個人倉庫物品上限數量
        /// </summary>
        public static int MAX_PERSONAL_WAREHOUSE_ITEM;
        /// <summary>
        /// 血盟倉庫物品上限數量
        /// </summary>
        public static int MAX_CLAN_WAREHOUSE_ITEM;
        /// <summary>
        /// 角色等級30以上，刪除角色是否要等待7天。... True=是, False=否
        /// </summary>
        public static bool DELETE_CHARACTER_AFTER_7DAYS;
        /// <summary>
        /// NPC死亡後屍體消失時間（秒）
        /// </summary>
        public static int NPC_DELETION_TIME;
        /// <summary>
        /// 預設角色數量
        /// </summary>
        public static int DEFAULT_CHARACTER_SLOT;
        /// <summary>
        /// 妖精森林NPC道具重置時間  #預設 10
        /// </summary>
        public static int GDROPITEM_TIME;
        /// <summary>
        /// 刪除丟棄的物品
        /// </summary>
        public static bool Drop_Item;
        /// <summary>
        /// 刪除丟棄的物品的最小等級
        /// </summary>
        public static int DropItemMinLv;
        /// <summary>
        /// 重新啟動伺服器 (分鐘、0 = 不重啟) by阿傑
        /// </summary>
        public static int REST_TIME;
        /// <summary>
        /// 設定是否使用循環公告?(\data\announcecycle.txt) True=是, False=否 by阿傑
        /// </summary>
        public static bool Use_Show_Announcecycle;
        /// <summary>
        /// 循環時間 by阿傑
        /// </summary>
        public static int Show_Announcecycle_Time;
        /// <summary>
        /// 設定ＧＭ使用公頻(&)顯示方式設定 True=顯示ID, False=顯示[******]
        /// </summary>
        public static bool GM_TALK;
        /// <summary>
        /// 是否開啟攻擊時，顯示怪物血條(單隻)? True=是, False=否
        /// </summary>
        public static bool Attack_Mob_HP_Bar;
        /// <summary>
        /// 全道具販賣 True=是, False=否
        /// </summary>
        public static bool ALL_ITEM_SELL;
        /// <summary>
        /// 是否顯示Npc編號  True=是, False=否
        /// </summary>
        public static bool SHOW_NPC_ID;
        /// <summary>
        /// 殷海薩的祝福 登入多少時間取得1% (單位分鐘)
        /// </summary>
        public static int RATE_AIN_TIME;
        /// <summary>
        /// 殷海薩的祝福 登出多少時間取得1% (單位分鐘)
        /// </summary>
        public static int RATE_AIN_OUTTIME;
        /// <summary>
        /// 殷海薩的祝福 最高百分比
        /// </summary>
        public static int RATE_MAX_CHARGE_PERCENT;
        /// <summary>
        /// 單項能力值上限 (力、敏、體、智、精、魅)
        /// </summary>
        public static int BONUS_STATS1; //能力值上限調整 by 丫傑 end
        /// <summary>
        /// 萬能藥上限
        /// </summary>
        public static int BONUS_STATS2; //能力值上限調整 by 丫傑 end
        /// <summary>
        /// 萬能藥上限 (能力值達到此上限後不可使用)
        /// </summary>
        public static int BONUS_STATS3; //能力值上限調整 by 丫傑 end
        /// <summary>
        /// 升級血魔滿
        /// </summary>
        public static bool MaxHPMP;
        /// <summary>
        /// 廣播不扣肉
        /// </summary>
        public static bool DeleteFood;
        /// <summary>
        /// 寵物經驗倍率
        /// </summary>
        public static double RATE_XP_PET;
        /// <summary>
        /// 寵物最高等級設定
        /// </summary>
        public static int Pet_Max_LV;
        /// <summary>
        /// 模擬器重開後 開啟延遲時間 (秒)
        /// </summary>
        public static int Gamesleep;
        /// <summary>
        /// GM偷聽密語對話 True=是, False=否
        /// </summary>
        public static bool GM_OVERHEARD;
        /// <summary>
        /// GM偷聽一般對話 True=是, False=否
        /// </summary>
        public static bool GM_OVERHEARD0;
        /// <summary>
        /// GM偷聽血盟對話 True=是, False=否
        /// </summary>
        public static bool GM_OVERHEARD4;
        /// <summary>
        /// GM偷聽組隊對話 True=是, False=否
        /// </summary>
        public static bool GM_OVERHEARD11;
        /// <summary>
        /// GM偷聽聯盟對話 True=是, False=否
        /// </summary>
        public static bool GM_OVERHEARD13;
        #endregion configLoader.ConfigLoader

        /// <summary>
        /// configLoader control 
        /// </summary>
        public static int PRINCE_MAX_HP;

        public static int PRINCE_MAX_MP;

        public static int KNIGHT_MAX_HP;

        public static int KNIGHT_MAX_MP;

        public static int ELF_MAX_HP;

        public static int ELF_MAX_MP;

        public static int WIZARD_MAX_HP;

        public static int WIZARD_MAX_MP;

        public static int DARKELF_MAX_HP;

        public static int DARKELF_MAX_MP;

        public static int DRAGONKNIGHT_MAX_HP;

        public static int DRAGONKNIGHT_MAX_MP;

        public static int ILLUSIONIST_MAX_HP;

        public static int ILLUSIONIST_MAX_MP;

        public static int LV50_EXP;

        public static int LV51_EXP;

        public static int LV52_EXP;

        public static int LV53_EXP;

        public static int LV54_EXP;

        public static int LV55_EXP;

        public static int LV56_EXP;

        public static int LV57_EXP;

        public static int LV58_EXP;

        public static int LV59_EXP;

        public static int LV60_EXP;

        public static int LV61_EXP;

        public static int LV62_EXP;

        public static int LV63_EXP;

        public static int LV64_EXP;

        public static int LV65_EXP;

        public static int LV66_EXP;

        public static int LV67_EXP;

        public static int LV68_EXP;

        public static int LV69_EXP;

        public static int LV70_EXP;

        public static int LV71_EXP;

        public static int LV72_EXP;

        public static int LV73_EXP;

        public static int LV74_EXP;

        public static int LV75_EXP;

        public static int LV76_EXP;

        public static int LV77_EXP;

        public static int LV78_EXP;

        public static int LV79_EXP;

        public static int LV80_EXP;

        public static int LV81_EXP;

        public static int LV82_EXP;

        public static int LV83_EXP;

        public static int LV84_EXP;

        public static int LV85_EXP;

        public static int LV86_EXP;

        public static int LV87_EXP;

        public static int LV88_EXP;

        public static int LV89_EXP;

        public static int LV90_EXP;

        public static int LV91_EXP;

        public static int LV92_EXP;

        public static int LV93_EXP;

        public static int LV94_EXP;

        public static int LV95_EXP;

        public static int LV96_EXP;

        public static int LV97_EXP;

        public static int LV98_EXP;

        public static int LV99_EXP;

        public static int LV100_EXP;

        public static int LV101_EXP;

        public static int LV102_EXP;

        public static int LV103_EXP;

        public static int LV104_EXP;

        public static int LV105_EXP;

        public static int LV106_EXP;

        public static int LV107_EXP;

        public static int LV108_EXP;

        public static int LV109_EXP;

        public static int LV110_EXP;

        /// <summary>
        /// configLoader control </summary>
        public static bool FIGHT_IS_ACTIVE;

        public static bool NOVICE_PROTECTION_IS_ACTIVE;

        public static int NOVICE_MAX_LEVEL;

        public static int NOVICE_PROTECTION_LEVEL_RANGE;

        /// <summary>
        ///Record Settings </summary>
        public static int LOGGING_WEAPON_ENCHANT;

        public static int LOGGING_ARMOR_ENCHANT;

        public static bool LOGGING_CHAT_NORMAL;

        public static bool LOGGING_CHAT_WHISPER;

        public static bool LOGGING_CHAT_SHOUT;

        public static bool LOGGING_CHAT_WORLD;

        public static bool LOGGING_CHAT_CLAN;

        public static bool LOGGING_CHAT_PARTY;

        public static bool LOGGING_CHAT_COMBINED;

        public static bool LOGGING_CHAT_CHAT_PARTY;

        public static bool writeTradeLog;

        public static bool writeRobotsLog;

        public static bool writeDropLog;

        public static int MysqlAutoBackup;

        public static bool CompressGzip;

        /// <summary>
        /// Configuration files </summary>
        public const string SERVER_CONFIG_FILE = "./config/server.properties";

        public const string RATES_CONFIG_FILE = "./config/rates.properties";

        public const string ALT_SETTINGS_FILE = "./config/altsettings.properties";

        public const string CHAR_SETTINGS_CONFIG_FILE = "./config/charsettings.properties";

        public const string FIGHT_SETTINGS_CONFIG_FILE = "./config/fights.properties";

        public const string RECORD_SETTINGS_CONFIG_FILE = "./config/record.properties";

        /// <summary>
        /// 其他設定 </summary>

        // 吸收每個 NPC 的 MP 上限
        public const int MANA_DRAIN_LIMIT_PER_NPC = 40;

        // 每一次攻擊吸收的 MP 上限(馬那、鋼鐵馬那）
        public const int MANA_DRAIN_LIMIT_PER_SOM_ATTACK = 9;

        public static void load()
        {
            // server.ConfigLoader
            try
            {
                ConfigLoader configLoader = new ConfigLoader(SERVER_CONFIG_FILE);
                GAME_SERVER_HOST_NAME = configLoader.GetProperty("GameserverHostname", "*");
                GAME_SERVER_PORT = int.Parse(configLoader.GetProperty("GameserverPort", "2000"));
                DB_LOGIN = configLoader.GetProperty("Login", "root");
                DB_PASSWORD = configLoader.GetProperty("Password", "");
                CLIENT_LANGUAGE = int.Parse(configLoader.GetProperty("ClientLanguage", "4"));
                CLIENT_LANGUAGE_CODE = LANGUAGE_CODE_ARRAY[CLIENT_LANGUAGE];
                HOSTNAME_LOOKUPS = bool.Parse(configLoader.GetProperty("HostnameLookups", "false"));
                AUTOMATIC_KICK = int.Parse(configLoader.GetProperty("AutomaticKick", "10"));
                AUTO_CREATE_ACCOUNTS = bool.Parse(configLoader.GetProperty("AutoCreateAccounts", "true"));
                MAX_ONLINE_USERS = short.Parse(configLoader.GetProperty("MaximumOnlineUsers", "30"));
                CACHE_MAP_FILES = bool.Parse(configLoader.GetProperty("CacheMapFiles", "false"));
                LOAD_V2_MAP_FILES = bool.Parse(configLoader.GetProperty("LoadV2MapFiles", "false"));
                CHECK_MOVE_INTERVAL = bool.Parse(configLoader.GetProperty("CheckMoveInterval", "false"));
                CHECK_ATTACK_INTERVAL = bool.Parse(configLoader.GetProperty("CheckAttackInterval", "false"));
                CHECK_SPELL_INTERVAL = bool.Parse(configLoader.GetProperty("CheckSpellInterval", "false"));
                INJUSTICE_COUNT = short.Parse(configLoader.GetProperty("InjusticeCount", "10"));
                JUSTICE_COUNT = int.Parse(configLoader.GetProperty("JusticeCount", "4"));
                CHECK_STRICTNESS = int.Parse(configLoader.GetProperty("CheckStrictness", "102"));
                ILLEGAL_SPEEDUP_PUNISHMENT = int.Parse(configLoader.GetProperty("Punishment", "0"));
                AUTOSAVE_INTERVAL = Convert.ToInt32(configLoader.GetProperty("AutosaveInterval", "1200"), 10);
                AUTOSAVE_INTERVAL_INVENTORY = Convert.ToInt32(configLoader.GetProperty("AutosaveIntervalOfInventory", "300"), 10);

                PC_RECOGNIZE_RANGE = int.Parse(configLoader.GetProperty("PcRecognizeRange", "20"));
                CHARACTER_CONFIG_IN_SERVER_SIDE = bool.Parse(configLoader.GetProperty("CharacterConfigInServerSide", "true"));
                ALLOW_2PC = bool.Parse(configLoader.GetProperty("Allow2PC", "true"));
                LEVEL_DOWN_RANGE = int.Parse(configLoader.GetProperty("LevelDownRange", "0"));
                SEND_PACKET_BEFORE_TELEPORT = bool.Parse(configLoader.GetProperty("SendPacketBeforeTeleport", "false"));
                DETECT_DATABASE_LEAKS = bool.Parse(configLoader.GetProperty("EnableDatabaseResourceLeaksDetection", "false"));
                CONSOLE_COMMAND_ACTIVE = bool.Parse(configLoader.GetProperty("CmdActive", "false"));
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Load " + SERVER_CONFIG_FILE + " File.");
            }

            // rates.ConfigLoader
            try
            {
                ConfigLoader configLoader = new ConfigLoader(RATES_CONFIG_FILE);

                RATE_XP = double.Parse(configLoader.GetProperty("RateXp", "1.0"));
                RATE_LA = double.Parse(configLoader.GetProperty("RateLawful", "1.0"));
                RATE_KARMA = double.Parse(configLoader.GetProperty("RateKarma", "1.0"));
                RATE_DROP_ADENA = double.Parse(configLoader.GetProperty("RateDropAdena", "1.0"));
                RATE_DROP_ITEMS = double.Parse(configLoader.GetProperty("RateDropItems", "1.0"));
                ENCHANT_CHANCE_WEAPON = int.Parse(configLoader.GetProperty("EnchantChanceWeapon", "68"));
                ENCHANT_CHANCE_ARMOR = int.Parse(configLoader.GetProperty("EnchantChanceArmor", "52"));
                ATTR_ENCHANT_CHANCE = int.Parse(configLoader.GetProperty("AttrEnchantChance", "10"));
                RATE_WEIGHT_LIMIT = double.Parse(configLoader.GetProperty("RateWeightLimit", "1"));
                RATE_WEIGHT_LIMIT_PET = double.Parse(configLoader.GetProperty("RateWeightLimitforPet", "1"));
                RATE_SHOP_SELLING_PRICE = double.Parse(configLoader.GetProperty("RateShopSellingPrice", "1.0"));
                RATE_SHOP_PURCHASING_PRICE = double.Parse(configLoader.GetProperty("RateShopPurchasingPrice", "1.0"));
                CREATE_CHANCE_DIARY = int.Parse(configLoader.GetProperty("CreateChanceDiary", "33"));
                CREATE_CHANCE_RECOLLECTION = int.Parse(configLoader.GetProperty("CreateChanceRecollection", "90"));
                CREATE_CHANCE_MYSTERIOUS = int.Parse(configLoader.GetProperty("CreateChanceMysterious", "90"));
                CREATE_CHANCE_PROCESSING = int.Parse(configLoader.GetProperty("CreateChanceProcessing", "90"));
                CREATE_CHANCE_PROCESSING_DIAMOND = int.Parse(configLoader.GetProperty("CreateChanceProcessingDiamond", "90"));
                CREATE_CHANCE_DANTES = int.Parse(configLoader.GetProperty("CreateChanceDantes", "50"));
                CREATE_CHANCE_ANCIENT_AMULET = int.Parse(configLoader.GetProperty("CreateChanceAncientAmulet", "90"));
                CREATE_CHANCE_HISTORY_BOOK = int.Parse(configLoader.GetProperty("CreateChanceHistoryBook", "50"));
                MAGIC_STONE_TYPE = int.Parse(configLoader.GetProperty("MagicStoneAttr", "50"));
                MAGIC_STONE_LEVEL = int.Parse(configLoader.GetProperty("MagicStoneLevel", "50"));
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Load " + RATES_CONFIG_FILE + " File.");
            }

            // configLoader.ConfigLoader
            try
            {
                ConfigLoader configLoader = new ConfigLoader(ALT_SETTINGS_FILE);

                GLOBAL_CHAT_LEVEL = short.Parse(configLoader.GetProperty("GlobalChatLevel", "30"));
                WHISPER_CHAT_LEVEL = short.Parse(configLoader.GetProperty("WhisperChatLevel", "5"));
                AUTO_LOOT = int.Parse(configLoader.GetProperty("AutoLoot", "2"));
                LOOTING_RANGE = int.Parse(configLoader.GetProperty("LootingRange", "3"));
                ALT_NONPVP = bool.Parse(configLoader.GetProperty("NonPvP", "true"));
                ALT_ATKMSG = bool.Parse(configLoader.GetProperty("AttackMessageOn", "true"));
                CHANGE_TITLE_BY_ONESELF = bool.Parse(configLoader.GetProperty("ChangeTitleByOneself", "false"));
                MAX_CLAN_MEMBER = int.Parse(configLoader.GetProperty("MaxClanMember", "0"));
                CLAN_ALLIANCE = bool.Parse(configLoader.GetProperty("ClanAlliance", "true"));
                MAX_PT = int.Parse(configLoader.GetProperty("MaxPT", "8"));
                MAX_CHAT_PT = int.Parse(configLoader.GetProperty("MaxChatPT", "8"));
                SIM_WAR_PENALTY = bool.Parse(configLoader.GetProperty("SimWarPenalty", "true"));
                GET_BACK = bool.Parse(configLoader.GetProperty("GetBack", "false"));
                ALT_ITEM_DELETION_TYPE = configLoader.GetProperty("ItemDeletionType", "auto");
                ALT_ITEM_DELETION_TIME = int.Parse(configLoader.GetProperty("ItemDeletionTime", "10"));
                ALT_ITEM_DELETION_RANGE = int.Parse(configLoader.GetProperty("ItemDeletionRange", "5"));
                ALT_GMSHOP = bool.Parse(configLoader.GetProperty("GMshop", "false"));
                ALT_GMSHOP_MIN_ID = int.Parse(configLoader.GetProperty("GMshopMinID", "0xffffffff")); // 設定錯誤時就取消GM商店
                ALT_GMSHOP_MAX_ID = int.Parse(configLoader.GetProperty("GMshopMaxID", "0xffffffff")); // 設定錯誤時就取消GM商店
                ALT_HALLOWEENIVENT = bool.Parse(configLoader.GetProperty("HalloweenIvent", "true"));
                ALT_JPPRIVILEGED = bool.Parse(configLoader.GetProperty("JpPrivileged", "false"));
                ALT_TALKINGSCROLLQUEST = bool.Parse(configLoader.GetProperty("TalkingScrollQuest", "false"));
                ALT_WHO_COMMAND = bool.Parse(configLoader.GetProperty("WhoCommand", "false"));
                ALT_REVIVAL_POTION = bool.Parse(configLoader.GetProperty("RevivalPotion", "false"));
                GDROPITEM_TIME = int.Parse(configLoader.GetProperty("GDropItemTime", "10"));
                REST_TIME = int.Parse(configLoader.GetProperty("RestartTime", "60"));
                Use_Show_Announcecycle = bool.Parse(configLoader.GetProperty("UseShowAnnouncecycle", "false")); // 循環公告 2/3by 丫傑
                Show_Announcecycle_Time = int.Parse(configLoader.GetProperty("ShowAnnouncecycleTime", "30")); // 循環時間 2/3 by 丫傑
                ALT_WHO_TYPE = int.Parse(configLoader.GetProperty("ALTWHOTYPE", "1"));
                GM_TALK = bool.Parse(configLoader.GetProperty("GMSpeakName", "false")); //TODO ＧＭ使用公頻(&)顯示方式 2/3
                Attack_Mob_HP_Bar = bool.Parse(configLoader.GetProperty("AttackMobHPBar", "false")); // 攻擊顯示怪物血條
                ALL_ITEM_SELL = bool.Parse(configLoader.GetProperty("AllItemSell", "false")); // 全道具販賣 by 丫傑 end
                SHOW_NPC_ID = bool.Parse(configLoader.GetProperty("SHOWNPCID", "true"));
                RATE_AIN_TIME = int.Parse(configLoader.GetProperty("RateAinTime", "30")); //TODO 殷海薩的祝福
                RATE_AIN_OUTTIME = int.Parse(configLoader.GetProperty("RateAinOutTime", "30")); //TODO 殷海薩的祝福
                RATE_MAX_CHARGE_PERCENT = int.Parse(configLoader.GetProperty("RateMaxChargePercent", "200")); //TODO 殷海薩的祝福
                BONUS_STATS1 = int.Parse(configLoader.GetProperty("BONUS_STATS1", "25")); //調整能力值上限 by 丫傑 end
                BONUS_STATS2 = int.Parse(configLoader.GetProperty("BONUS_STATS2", "5")); //調整能力值上限 by 丫傑 end
                BONUS_STATS3 = int.Parse(configLoader.GetProperty("BONUS_STATS3", "25")); //調整能力值上限 by 丫傑 end
                Drop_Item = bool.Parse(configLoader.GetProperty("DropItem", "false")); //TODO　刪除丟棄物品 by bill00148
                DropItemMinLv = byte.Parse(configLoader.GetProperty("DropItemMinLv", "200")); //TODO　刪除丟棄物品 by bill00148
                MaxHPMP = bool.Parse(configLoader.GetProperty("FullHPMP", "false")); //TODO 升級血魔滿
                DeleteFood = bool.Parse(configLoader.GetProperty("DeleteFood", "false")); //TODO 廣播扣飽食度
                Gamesleep = int.Parse(configLoader.GetProperty("Gamesleep", "30"));
                RATE_XP_PET = double.Parse(configLoader.GetProperty("PetRateXp", "1.0")); //TODO 寵物經驗倍率
                Pet_Max_LV = int.Parse(configLoader.GetProperty("PetMaxLV", "50")); //TODO 寵物最高等級設定
                GM_OVERHEARD = bool.Parse(configLoader.GetProperty("GM_OVERHEARD", "false")); // 密語頻道
                GM_OVERHEARD0 = bool.Parse(configLoader.GetProperty("GM_OVERHEARD0", "false")); // 一般頻道
                GM_OVERHEARD4 = bool.Parse(configLoader.GetProperty("GM_OVERHEARD4", "false")); // 血盟頻道
                GM_OVERHEARD11 = bool.Parse(configLoader.GetProperty("GM_OVERHEARD11", "false")); // 組隊頻道
                GM_OVERHEARD13 = bool.Parse(configLoader.GetProperty("GM_OVERHEARD13", "false")); // 聯盟頻道
                string strWar;
                strWar = configLoader.GetProperty("WarTime", "2h");
                int minute = 0;
                if (strWar.IndexOf("d", StringComparison.Ordinal) >= 0)
                {
                    minute = 24 * 60;
                    strWar = strWar.Replace("d", "");
                }
                else if (strWar.IndexOf("h", StringComparison.Ordinal) >= 0)
                {
                    minute = 60;
                    strWar = strWar.Replace("h", "");
                }
                else if (strWar.IndexOf("m", StringComparison.Ordinal) >= 0)
                {
                    minute = 1;
                    strWar = strWar.Replace("m", "");
                }
                ALT_WAR_TIME = TimeSpan.FromMinutes(int.Parse(strWar) * minute);
                strWar = configLoader.GetProperty("WarInterval", "4d");
                minute = 0;
                if (strWar.IndexOf("d", StringComparison.Ordinal) >= 0)
                {
                    minute = 24 * 60;
                    strWar = strWar.Replace("d", "");
                }
                else if (strWar.IndexOf("h", StringComparison.Ordinal) >= 0)
                {
                    minute = 60;
                    strWar = strWar.Replace("h", "");
                }
                else if (strWar.IndexOf("m", StringComparison.Ordinal) >= 0)
                {
                    minute = 1;
                    strWar = strWar.Replace("m", "");
                }
                ALT_WAR_INTERVAL = TimeSpan.FromMinutes(int.Parse(strWar) * minute);
                SPAWN_HOME_POINT = bool.Parse(configLoader.GetProperty("SpawnHomePoint", "true"));
                SPAWN_HOME_POINT_COUNT = int.Parse(configLoader.GetProperty("SpawnHomePointCount", "2"));
                SPAWN_HOME_POINT_DELAY = int.Parse(configLoader.GetProperty("SpawnHomePointDelay", "100"));
                SPAWN_HOME_POINT_RANGE = int.Parse(configLoader.GetProperty("SpawnHomePointRange", "8"));
                INIT_BOSS_SPAWN = bool.Parse(configLoader.GetProperty("InitBossSpawn", "true"));
                ELEMENTAL_STONE_AMOUNT = int.Parse(configLoader.GetProperty("ElementalStoneAmount", "300"));
                HOUSE_TAX_INTERVAL = int.Parse(configLoader.GetProperty("HouseTaxInterval", "10"));
                MAX_DOLL_COUNT = int.Parse(configLoader.GetProperty("MaxDollCount", "1"));
                RETURN_TO_NATURE = bool.Parse(configLoader.GetProperty("ReturnToNature", "false"));
                MAX_NPC_ITEM = int.Parse(configLoader.GetProperty("MaxNpcItem", "8"));
                MAX_PERSONAL_WAREHOUSE_ITEM = int.Parse(configLoader.GetProperty("MaxPersonalWarehouseItem", "100"));
                MAX_CLAN_WAREHOUSE_ITEM = int.Parse(configLoader.GetProperty("MaxClanWarehouseItem", "200"));
                DELETE_CHARACTER_AFTER_7DAYS = bool.Parse(configLoader.GetProperty("DeleteCharacterAfter7Days", "True"));
                NPC_DELETION_TIME = int.Parse(configLoader.GetProperty("NpcDeletionTime", "10"));
                DEFAULT_CHARACTER_SLOT = int.Parse(configLoader.GetProperty("DefaultCharacterSlot", "6"));
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Load " + ALT_SETTINGS_FILE + " File.");
            }

            // configLoader.ConfigLoader
            try
            {
                ConfigLoader configLoader = new ConfigLoader(CHAR_SETTINGS_CONFIG_FILE);

                PRINCE_MAX_HP = int.Parse(configLoader.GetProperty("PrinceMaxHP", "1000"));
                PRINCE_MAX_MP = int.Parse(configLoader.GetProperty("PrinceMaxMP", "800"));
                KNIGHT_MAX_HP = int.Parse(configLoader.GetProperty("KnightMaxHP", "1400"));
                KNIGHT_MAX_MP = int.Parse(configLoader.GetProperty("KnightMaxMP", "600"));
                ELF_MAX_HP = int.Parse(configLoader.GetProperty("ElfMaxHP", "1000"));
                ELF_MAX_MP = int.Parse(configLoader.GetProperty("ElfMaxMP", "900"));
                WIZARD_MAX_HP = int.Parse(configLoader.GetProperty("WizardMaxHP", "800"));
                WIZARD_MAX_MP = int.Parse(configLoader.GetProperty("WizardMaxMP", "1200"));
                DARKELF_MAX_HP = int.Parse(configLoader.GetProperty("DarkelfMaxHP", "1000"));
                DARKELF_MAX_MP = int.Parse(configLoader.GetProperty("DarkelfMaxMP", "900"));
                DRAGONKNIGHT_MAX_HP = int.Parse(configLoader.GetProperty("DragonKnightMaxHP", "1400"));
                DRAGONKNIGHT_MAX_MP = int.Parse(configLoader.GetProperty("DragonKnightMaxMP", "600"));
                ILLUSIONIST_MAX_HP = int.Parse(configLoader.GetProperty("IllusionistMaxHP", "900"));
                ILLUSIONIST_MAX_MP = int.Parse(configLoader.GetProperty("IllusionistMaxMP", "1100"));
                LV50_EXP = int.Parse(configLoader.GetProperty("Lv50Exp", "1"));
                LV51_EXP = int.Parse(configLoader.GetProperty("Lv51Exp", "1"));
                LV52_EXP = int.Parse(configLoader.GetProperty("Lv52Exp", "1"));
                LV53_EXP = int.Parse(configLoader.GetProperty("Lv53Exp", "1"));
                LV54_EXP = int.Parse(configLoader.GetProperty("Lv54Exp", "1"));
                LV55_EXP = int.Parse(configLoader.GetProperty("Lv55Exp", "1"));
                LV56_EXP = int.Parse(configLoader.GetProperty("Lv56Exp", "1"));
                LV57_EXP = int.Parse(configLoader.GetProperty("Lv57Exp", "1"));
                LV58_EXP = int.Parse(configLoader.GetProperty("Lv58Exp", "1"));
                LV59_EXP = int.Parse(configLoader.GetProperty("Lv59Exp", "1"));
                LV60_EXP = int.Parse(configLoader.GetProperty("Lv60Exp", "1"));
                LV61_EXP = int.Parse(configLoader.GetProperty("Lv61Exp", "1"));
                LV62_EXP = int.Parse(configLoader.GetProperty("Lv62Exp", "1"));
                LV63_EXP = int.Parse(configLoader.GetProperty("Lv63Exp", "1"));
                LV64_EXP = int.Parse(configLoader.GetProperty("Lv64Exp", "1"));
                LV65_EXP = int.Parse(configLoader.GetProperty("Lv65Exp", "2"));
                LV66_EXP = int.Parse(configLoader.GetProperty("Lv66Exp", "2"));
                LV67_EXP = int.Parse(configLoader.GetProperty("Lv67Exp", "2"));
                LV68_EXP = int.Parse(configLoader.GetProperty("Lv68Exp", "2"));
                LV69_EXP = int.Parse(configLoader.GetProperty("Lv69Exp", "2"));
                LV70_EXP = int.Parse(configLoader.GetProperty("Lv70Exp", "4"));
                LV71_EXP = int.Parse(configLoader.GetProperty("Lv71Exp", "4"));
                LV72_EXP = int.Parse(configLoader.GetProperty("Lv72Exp", "4"));
                LV73_EXP = int.Parse(configLoader.GetProperty("Lv73Exp", "4"));
                LV74_EXP = int.Parse(configLoader.GetProperty("Lv74Exp", "4"));
                LV75_EXP = int.Parse(configLoader.GetProperty("Lv75Exp", "8"));
                LV76_EXP = int.Parse(configLoader.GetProperty("Lv76Exp", "8"));
                LV77_EXP = int.Parse(configLoader.GetProperty("Lv77Exp", "8"));
                LV78_EXP = int.Parse(configLoader.GetProperty("Lv78Exp", "8"));
                LV79_EXP = int.Parse(configLoader.GetProperty("Lv79Exp", "16"));
                LV80_EXP = int.Parse(configLoader.GetProperty("Lv80Exp", "32"));
                LV81_EXP = int.Parse(configLoader.GetProperty("Lv81Exp", "64"));
                LV82_EXP = int.Parse(configLoader.GetProperty("Lv82Exp", "128"));
                LV83_EXP = int.Parse(configLoader.GetProperty("Lv83Exp", "256"));
                LV84_EXP = int.Parse(configLoader.GetProperty("Lv84Exp", "512"));
                LV85_EXP = int.Parse(configLoader.GetProperty("Lv85Exp", "1024"));
                LV86_EXP = int.Parse(configLoader.GetProperty("Lv86Exp", "2048"));
                LV87_EXP = int.Parse(configLoader.GetProperty("Lv87Exp", "4096"));
                LV88_EXP = int.Parse(configLoader.GetProperty("Lv88Exp", "8192"));
                LV89_EXP = int.Parse(configLoader.GetProperty("Lv89Exp", "16384"));
                LV90_EXP = int.Parse(configLoader.GetProperty("Lv90Exp", "32768"));
                LV91_EXP = int.Parse(configLoader.GetProperty("Lv91Exp", "65536"));
                LV92_EXP = int.Parse(configLoader.GetProperty("Lv92Exp", "131072"));
                LV93_EXP = int.Parse(configLoader.GetProperty("Lv93Exp", "262144"));
                LV94_EXP = int.Parse(configLoader.GetProperty("Lv94Exp", "524288"));
                LV95_EXP = int.Parse(configLoader.GetProperty("Lv95Exp", "1048576"));
                LV96_EXP = int.Parse(configLoader.GetProperty("Lv96Exp", "2097152"));
                LV97_EXP = int.Parse(configLoader.GetProperty("Lv97Exp", "4194304"));
                LV98_EXP = int.Parse(configLoader.GetProperty("Lv98Exp", "8388608"));
                LV99_EXP = int.Parse(configLoader.GetProperty("Lv99Exp", "16777216"));
                LV100_EXP = int.Parse(configLoader.GetProperty("Lv100Exp", "16777216"));
                LV101_EXP = int.Parse(configLoader.GetProperty("Lv101Exp", "65536"));
                LV102_EXP = int.Parse(configLoader.GetProperty("Lv102Exp", "131072"));
                LV103_EXP = int.Parse(configLoader.GetProperty("Lv103Exp", "262144"));
                LV104_EXP = int.Parse(configLoader.GetProperty("Lv104Exp", "524288"));
                LV105_EXP = int.Parse(configLoader.GetProperty("Lv105Exp", "1048576"));
                LV106_EXP = int.Parse(configLoader.GetProperty("Lv106Exp", "2097152"));
                LV107_EXP = int.Parse(configLoader.GetProperty("Lv107Exp", "4194304"));
                LV108_EXP = int.Parse(configLoader.GetProperty("Lv108Exp", "8388608"));
                LV109_EXP = int.Parse(configLoader.GetProperty("Lv109Exp", "16777216"));
                LV110_EXP = int.Parse(configLoader.GetProperty("Lv110Exp", "16777216"));
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Load " + CHAR_SETTINGS_CONFIG_FILE + " File.");
            }

            try
            {
                ConfigLoader configLoader = new ConfigLoader(FIGHT_SETTINGS_CONFIG_FILE);

                FIGHT_IS_ACTIVE = bool.Parse(configLoader.GetProperty("FightIsActive", "False"));
                NOVICE_PROTECTION_IS_ACTIVE = bool.Parse(configLoader.GetProperty("NoviceProtectionIsActive", "False"));
                NOVICE_MAX_LEVEL = int.Parse(configLoader.GetProperty("NoviceMaxLevel", "20"));
                NOVICE_PROTECTION_LEVEL_RANGE = int.Parse(configLoader.GetProperty("ProtectionLevelRange", "10"));
            }
            catch (Exception e)
            {
                throw new Exception("無法讀取設定檔: " + FIGHT_SETTINGS_CONFIG_FILE);
            }

            // record.ConfigLoader
            try
            {
                ConfigLoader configLoader = new ConfigLoader(RECORD_SETTINGS_CONFIG_FILE);

                LOGGING_WEAPON_ENCHANT = int.Parse(configLoader.GetProperty("LoggingWeaponEnchant", "0"));
                LOGGING_ARMOR_ENCHANT = int.Parse(configLoader.GetProperty("LoggingArmorEnchant", "0"));
                LOGGING_CHAT_NORMAL = bool.Parse(configLoader.GetProperty("LoggingChatNormal", "false"));
                LOGGING_CHAT_WHISPER = bool.Parse(configLoader.GetProperty("LoggingChatWhisper", "false"));
                LOGGING_CHAT_SHOUT = bool.Parse(configLoader.GetProperty("LoggingChatShout", "false"));
                LOGGING_CHAT_WORLD = bool.Parse(configLoader.GetProperty("LoggingChatWorld", "false"));
                LOGGING_CHAT_CLAN = bool.Parse(configLoader.GetProperty("LoggingChatClan", "false"));
                LOGGING_CHAT_PARTY = bool.Parse(configLoader.GetProperty("LoggingChatParty", "false"));
                LOGGING_CHAT_COMBINED = bool.Parse(configLoader.GetProperty("LoggingChatCombined", "false"));
                LOGGING_CHAT_CHAT_PARTY = bool.Parse(configLoader.GetProperty("LoggingChatChatParty", "false"));
                writeTradeLog = bool.Parse(configLoader.GetProperty("writeTradeLog", "false"));
                writeRobotsLog = bool.Parse(configLoader.GetProperty("writeRobotsLog", "false"));
                writeDropLog = bool.Parse(configLoader.GetProperty("writeDropLog", "false"));
                MysqlAutoBackup = int.Parse(configLoader.GetProperty("MysqlAutoBackup", "false"));
                CompressGzip = bool.Parse(configLoader.GetProperty("CompressGzip", "false"));

            }
            catch (Exception e)
            {
                throw new Exception("Failed to Load: " + RECORD_SETTINGS_CONFIG_FILE);
            }

            validate();
        }

        private static void validate()
        {
            if (!Config.ALT_ITEM_DELETION_RANGE.Includes(0, 5))
            {
                throw new System.InvalidOperationException("ItemDeletionRange 的設定值超出範圍。");
            }

            if (!Config.ALT_ITEM_DELETION_TIME.Includes(1, 35791))
            {
                throw new System.InvalidOperationException("ItemDeletionTime 的設定值超出範圍。");
            }
        }

        public static bool setParameterValue(string pName, string pValue)
        {
            // server.ConfigLoader
            if (pName == "GameserverHostname")
            {
                GAME_SERVER_HOST_NAME = pValue;
            }
            else if (pName == "GameserverPort")
            {
                GAME_SERVER_PORT = int.Parse(pValue);
            }
            else if (pName == "Login")
            {
                DB_LOGIN = pValue;
            }
            else if (pName == "Password")
            {
                DB_PASSWORD = pValue;
            }
            else if (pName == "ClientLanguage")
            {
                CLIENT_LANGUAGE = int.Parse(pValue);
            }
            else if (pName == "AutomaticKick")
            {
                AUTOMATIC_KICK = int.Parse(pValue);
            }
            else if (pName == "AutoCreateAccounts")
            {
                AUTO_CREATE_ACCOUNTS = bool.Parse(pValue);
            }
            else if (pName == "MaximumOnlineUsers")
            {
                MAX_ONLINE_USERS = short.Parse(pValue);
            }
            else if (pName == "CharacterConfigInServerSide")
            {
                CHARACTER_CONFIG_IN_SERVER_SIDE = bool.Parse(pValue);
            }
            else if (pName == "Allow2PC")
            {
                ALLOW_2PC = bool.Parse(pValue);
            }
            else if (pName == "LevelDownRange")
            {
                LEVEL_DOWN_RANGE = int.Parse(pValue);
            }
            else if (pName == "SendPacketBeforeTeleport")
            {
                SEND_PACKET_BEFORE_TELEPORT = bool.Parse(pValue);
            }
            else if (pName == "Punishment")
            {
                ILLEGAL_SPEEDUP_PUNISHMENT = int.Parse(pValue);
            }
            // rates.ConfigLoader
            else if (pName == "RateXp")
            {
                RATE_XP = double.Parse(pValue);
            }
            else if (pName == "RateLawful")
            {
                RATE_LA = double.Parse(pValue);
            }
            else if (pName == "RateKarma")
            {
                RATE_KARMA = double.Parse(pValue);
            }
            else if (pName == "RateDropAdena")
            {
                RATE_DROP_ADENA = double.Parse(pValue);
            }
            else if (pName == "RateDropItems")
            {
                RATE_DROP_ITEMS = double.Parse(pValue);
            }
            else if (pName == "EnchantChanceWeapon")
            {
                ENCHANT_CHANCE_WEAPON = int.Parse(pValue);
            }
            else if (pName == "EnchantChanceArmor")
            {
                ENCHANT_CHANCE_ARMOR = int.Parse(pValue);
            }
            else if (pName == "AttrEnchantChance")
            {
                ATTR_ENCHANT_CHANCE = int.Parse(pValue);
            }
            else if (pName == "Weightrate")
            {
                RATE_WEIGHT_LIMIT = int.Parse(pValue);
            }
            // configLoader.ConfigLoader
            else if (pName == "GlobalChatLevel")
            {
                GLOBAL_CHAT_LEVEL = short.Parse(pValue);
            }
            else if (pName == "WhisperChatLevel")
            {
                WHISPER_CHAT_LEVEL = short.Parse(pValue);
            }
            else if (pName == "AutoLoot")
            {
                AUTO_LOOT = int.Parse(pValue);
            }
            else if (pName == "LOOTING_RANGE")
            {
                LOOTING_RANGE = int.Parse(pValue);
            }
            else if (pName == "AltNonPvP")
            {
                ALT_NONPVP = Convert.ToBoolean(pValue);
            }
            else if (pName == "AttackMessageOn")
            {
                ALT_ATKMSG = Convert.ToBoolean(pValue);
            }
            else if (pName == "ChangeTitleByOneself")
            {
                CHANGE_TITLE_BY_ONESELF = Convert.ToBoolean(pValue);
            }
            else if (pName == "MaxClanMember")
            {
                MAX_CLAN_MEMBER = int.Parse(pValue);
            }
            else if (pName == "ClanAlliance")
            {
                CLAN_ALLIANCE = Convert.ToBoolean(pValue);
            }
            else if (pName == "MaxPT")
            {
                MAX_PT = int.Parse(pValue);
            }
            else if (pName == "MaxChatPT")
            {
                MAX_CHAT_PT = int.Parse(pValue);
            }
            else if (pName == "SimWarPenalty")
            {
                SIM_WAR_PENALTY = Convert.ToBoolean(pValue);
            }
            else if (pName == "GetBack")
            {
                GET_BACK = Convert.ToBoolean(pValue);
            }
            else if (pName == "AutomaticItemDeletionTime")
            {
                ALT_ITEM_DELETION_TIME = int.Parse(pValue);
            }
            else if (pName == "AutomaticItemDeletionRange")
            {
                ALT_ITEM_DELETION_RANGE = int.Parse(pValue);
            }
            else if (pName == "GMshop")
            {
                ALT_GMSHOP = Convert.ToBoolean(pValue);
            }
            else if (pName == "GMshopMinID")
            {
                ALT_GMSHOP_MIN_ID = int.Parse(pValue);
            }
            else if (pName == "GMshopMaxID")
            {
                ALT_GMSHOP_MAX_ID = int.Parse(pValue);
            }
            else if (pName == "HalloweenIvent")
            {
                ALT_HALLOWEENIVENT = Convert.ToBoolean(pValue);
            }
            else if (pName == "JpPrivileged")
            {
                ALT_JPPRIVILEGED = Convert.ToBoolean(pValue);
            }
            else if (pName == "TalkingScrollQuest")
            {
                ALT_TALKINGSCROLLQUEST = Convert.ToBoolean(pValue);
            }
            else if (pName == "HouseTaxInterval")
            {
                HOUSE_TAX_INTERVAL = Convert.ToInt32(pValue);
            }
            else if (pName == "MaxDollCount")
            {
                MAX_DOLL_COUNT = Convert.ToInt32(pValue);
            }
            else if (pName == "ReturnToNature")
            {
                RETURN_TO_NATURE = Convert.ToBoolean(pValue);
            }
            else if (pName == "MaxNpcItem")
            {
                MAX_NPC_ITEM = Convert.ToInt32(pValue);
            }
            else if (pName == "MaxPersonalWarehouseItem")
            {
                MAX_PERSONAL_WAREHOUSE_ITEM = Convert.ToInt32(pValue);
            }
            else if (pName == "MaxClanWarehouseItem")
            {
                MAX_CLAN_WAREHOUSE_ITEM = Convert.ToInt32(pValue);
            }
            else if (pName == "DeleteCharacterAfter7Days")
            {
                DELETE_CHARACTER_AFTER_7DAYS = Convert.ToBoolean(pValue);
            }
            else if (pName == "NpcDeletionTime")
            {
                NPC_DELETION_TIME = Convert.ToInt32(pValue);
            }
            else if (pName == "DefaultCharacterSlot")
            {
                DEFAULT_CHARACTER_SLOT = Convert.ToInt32(pValue);
            }
            else if (pName == "GDropItemTime")
            {
                GDROPITEM_TIME = int.Parse(pValue);
            }

            // configLoader.ConfigLoader
            else if (pName == "PrinceMaxHP")
            {
                PRINCE_MAX_HP = int.Parse(pValue);
            }
            else if (pName == "PrinceMaxMP")
            {
                PRINCE_MAX_MP = int.Parse(pValue);
            }
            else if (pName == "KnightMaxHP")
            {
                KNIGHT_MAX_HP = int.Parse(pValue);
            }
            else if (pName == "KnightMaxMP")
            {
                KNIGHT_MAX_MP = int.Parse(pValue);
            }
            else if (pName == "ElfMaxHP")
            {
                ELF_MAX_HP = int.Parse(pValue);
            }
            else if (pName == "ElfMaxMP")
            {
                ELF_MAX_MP = int.Parse(pValue);
            }
            else if (pName == "WizardMaxHP")
            {
                WIZARD_MAX_HP = int.Parse(pValue);
            }
            else if (pName == "WizardMaxMP")
            {
                WIZARD_MAX_MP = int.Parse(pValue);
            }
            else if (pName == "DarkelfMaxHP")
            {
                DARKELF_MAX_HP = int.Parse(pValue);
            }
            else if (pName == "DarkelfMaxMP")
            {
                DARKELF_MAX_MP = int.Parse(pValue);
            }
            else if (pName == "DragonKnightMaxHP")
            {
                DRAGONKNIGHT_MAX_HP = int.Parse(pValue);
            }
            else if (pName == "DragonKnightMaxMP")
            {
                DRAGONKNIGHT_MAX_MP = int.Parse(pValue);
            }
            else if (pName == "IllusionistMaxHP")
            {
                ILLUSIONIST_MAX_HP = int.Parse(pValue);
            }
            else if (pName == "IllusionistMaxMP")
            {
                ILLUSIONIST_MAX_MP = int.Parse(pValue);
            }
            else if (pName == "Lv50Exp")
            {
                LV50_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv51Exp")
            {
                LV51_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv52Exp")
            {
                LV52_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv53Exp")
            {
                LV53_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv54Exp")
            {
                LV54_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv55Exp")
            {
                LV55_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv56Exp")
            {
                LV56_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv57Exp")
            {
                LV57_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv58Exp")
            {
                LV58_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv59Exp")
            {
                LV59_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv60Exp")
            {
                LV60_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv61Exp")
            {
                LV61_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv62Exp")
            {
                LV62_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv63Exp")
            {
                LV63_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv64Exp")
            {
                LV64_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv65Exp")
            {
                LV65_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv66Exp")
            {
                LV66_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv67Exp")
            {
                LV67_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv68Exp")
            {
                LV68_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv69Exp")
            {
                LV69_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv70Exp")
            {
                LV70_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv71Exp")
            {
                LV71_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv72Exp")
            {
                LV72_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv73Exp")
            {
                LV73_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv74Exp")
            {
                LV74_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv75Exp")
            {
                LV75_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv76Exp")
            {
                LV76_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv77Exp")
            {
                LV77_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv78Exp")
            {
                LV78_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv79Exp")
            {
                LV79_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv80Exp")
            {
                LV80_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv81Exp")
            {
                LV81_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv82Exp")
            {
                LV82_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv83Exp")
            {
                LV83_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv84Exp")
            {
                LV84_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv85Exp")
            {
                LV85_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv86Exp")
            {
                LV86_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv87Exp")
            {
                LV87_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv88Exp")
            {
                LV88_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv89Exp")
            {
                LV89_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv90Exp")
            {
                LV90_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv91Exp")
            {
                LV91_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv92Exp")
            {
                LV92_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv93Exp")
            {
                LV93_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv94Exp")
            {
                LV94_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv95Exp")
            {
                LV95_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv96Exp")
            {
                LV96_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv97Exp")
            {
                LV97_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv98Exp")
            {
                LV98_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv99Exp")
            {
                LV99_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv100Exp")
            {
                LV100_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv101Exp")
            {
                LV101_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv102Exp")
            {
                LV102_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv103Exp")
            {
                LV103_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv104Exp")
            {
                LV104_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv105Exp")
            {
                LV105_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv106Exp")
            {
                LV106_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv107Exp")
            {
                LV107_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv108Exp")
            {
                LV108_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv109Exp")
            {
                LV109_EXP = int.Parse(pValue);
            }
            else if (pName == "Lv110Exp")
            {
                LV110_EXP = int.Parse(pValue);
            }
            //record.ConfigLoader
            else if (pName == "LoggingWeaponEnchant")
            {
                LOGGING_WEAPON_ENCHANT = int.Parse(pValue);
            }
            else if (pName == "LoggingArmorEnchant")
            {
                LOGGING_ARMOR_ENCHANT = int.Parse(pValue);
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}