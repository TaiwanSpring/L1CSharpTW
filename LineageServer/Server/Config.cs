using System;
using System.IO;

/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server
{

	using IntRange = LineageServer.Server.Server.utils.IntRange;

	public sealed class Config
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static readonly Logger _log = Logger.getLogger(typeof(Config).FullName);

		/// <summary>
		/// Debug/release mode </summary>
		public const bool DEBUG = false;

		/// <summary>
		/// Thread pools size </summary>
		public static int THREAD_P_EFFECTS;

		public static int THREAD_P_GENERAL;

		public static int AI_MAX_THREAD;

		public static int THREAD_P_TYPE_GENERAL;

		public static int THREAD_P_SIZE_GENERAL;

		/// <summary>
		/// Server control </summary>
		public static string GAME_SERVER_HOST_NAME;

		public static int GAME_SERVER_PORT;

		public static string DB_DRIVER;

		public static string DB_URL;

		public static string DB_LOGIN;

		public static string DB_PASSWORD;

		public static string TIME_ZONE;

		public static int CLIENT_LANGUAGE;

		public static string CLIENT_LANGUAGE_CODE;

		public static string[] LANGUAGE_CODE_ARRAY = new string[] {"UTF8", "EUCKR", "UTF8", "BIG5", "SJIS", "GBK"};

		public static bool HOSTNAME_LOOKUPS;

		public static int AUTOMATIC_KICK;

		public static bool AUTO_CREATE_ACCOUNTS;

		public static short MAX_ONLINE_USERS;

		public static bool CACHE_MAP_FILES;

		public static bool LOAD_V2_MAP_FILES;

		public static bool CHECK_MOVE_INTERVAL;

		public static bool CHECK_ATTACK_INTERVAL;

		public static bool CHECK_SPELL_INTERVAL;

		public static short INJUSTICE_COUNT;

		public static int JUSTICE_COUNT;

		public static int CHECK_STRICTNESS;

		public static int ILLEGAL_SPEEDUP_PUNISHMENT;

		public static int AUTOSAVE_INTERVAL;

		public static int AUTOSAVE_INTERVAL_INVENTORY;

		public static int SKILLTIMER_IMPLTYPE;

		public static int NPCAI_IMPLTYPE;

		public static bool TELNET_SERVER;

		public static int TELNET_SERVER_PORT;

		public static int PC_RECOGNIZE_RANGE;

		public static bool CHARACTER_CONFIG_IN_SERVER_SIDE;

		public static bool ALLOW_2PC;

		public static int LEVEL_DOWN_RANGE;

		public static bool SEND_PACKET_BEFORE_TELEPORT;

		public static bool DETECT_DB_RESOURCE_LEAKS;

		public static bool CmdActive;

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


		/// <summary>
		/// AltSettings control </summary>
		public static short GLOBAL_CHAT_LEVEL;

		public static short WHISPER_CHAT_LEVEL;

		public static sbyte AUTO_LOOT;

		public static int LOOTING_RANGE;

		public static bool ALT_NONPVP;

		public static bool ALT_ATKMSG;

		public static bool CHANGE_TITLE_BY_ONESELF;

		public static int MAX_CLAN_MEMBER;

		public static bool CLAN_ALLIANCE;

		public static int MAX_PT;

		public static int MAX_CHAT_PT;

		public static bool SIM_WAR_PENALTY;

		public static bool GET_BACK;

		public static string ALT_ITEM_DELETION_TYPE;

		public static int ALT_ITEM_DELETION_TIME;

		public static int ALT_ITEM_DELETION_RANGE;

		public static bool ALT_GMSHOP;

		public static int ALT_GMSHOP_MIN_ID;

		public static int ALT_GMSHOP_MAX_ID;

		public static bool ALT_HALLOWEENIVENT;

		public static bool ALT_JPPRIVILEGED;

		public static bool ALT_TALKINGSCROLLQUEST;

		public static bool ALT_WHO_COMMAND;

		public static bool ALT_REVIVAL_POTION;

		public static int ALT_WAR_TIME;

		public static int ALT_WAR_TIME_UNIT;

		public static int ALT_WAR_INTERVAL;

		public static int ALT_WAR_INTERVAL_UNIT;

		public static int ALT_RATE_OF_DUTY;

		public static bool SPAWN_HOME_POINT;

		public static int SPAWN_HOME_POINT_RANGE;

		public static int SPAWN_HOME_POINT_COUNT;

		public static int SPAWN_HOME_POINT_DELAY;

		public static bool INIT_BOSS_SPAWN;

		public static int ELEMENTAL_STONE_AMOUNT;

		public static int HOUSE_TAX_INTERVAL;

		public static int MAX_DOLL_COUNT;

		public static bool RETURN_TO_NATURE;

		public static int MAX_NPC_ITEM;

		public static int MAX_PERSONAL_WAREHOUSE_ITEM;

		public static int MAX_CLAN_WAREHOUSE_ITEM;

		public static bool DELETE_CHARACTER_AFTER_7DAYS;

		public static int NPC_DELETION_TIME;

		public static int DEFAULT_CHARACTER_SLOT;
		public static int ALT_WHO_TYPE;
		public static int GDROPITEM_TIME;
		public static int REST_TIME; //伺服器重啟 by 丫傑
		public static bool Use_Show_Announcecycle; //TODO 循環公告 1/3 by阿傑
		public static int Show_Announcecycle_Time; //TODO 循環時間 1/3 by阿傑
		public static bool GM_TALK;
		public static bool Attack_Mob_HP_Bar; // 攻擊顯示怪物血條
		public static bool ALL_ITEM_SELL; // 全道具販賣 by 丫傑
		public static bool SHOW_NPC_ID;
		public static int RATE_AIN_TIME; //TODO 殷海薩的祝福
		public static int RATE_AIN_OUTTIME; //TODO 殷海薩的祝福
		public static int RATE_MAX_CHARGE_PERCENT; //TODO 殷海薩的祝福
		public static int BONUS_STATS1; //能力值上限調整 by 丫傑 end
		public static int BONUS_STATS2; //能力值上限調整 by 丫傑 end
		public static int BONUS_STATS3; //能力值上限調整 by 丫傑 end
		public static bool Drop_Item; //TODO 丟棄物品刪除道具
		public static sbyte DropItemMinLv; //TODO 丟棄物品刪除道具
		public static bool MaxHPMP; //TODO 升級血魔滿
		public static bool DeleteFood; //TODO 廣播扣飽食度
		public static double RATE_XP_PET; //TODO 寵物經驗倍率 1/3
		public static int Pet_Max_LV; //TODO 寵物最高等級設定 1/3
		public static int Gamesleep;
		public static bool GM_OVERHEARD; // GM偷聽一般頻道開關 by 丫傑
		public static bool GM_OVERHEARD0;
		public static bool GM_OVERHEARD4;
		public static bool GM_OVERHEARD11;
		public static bool GM_OVERHEARD13; // GM偷聽一般頻道開關 by 丫傑
		/// <summary>
		/// CharSettings control </summary>
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
		/// FightSettings control </summary>
		public static bool FIGHT_IS_ACTIVE;

		public static bool NOVICE_PROTECTION_IS_ACTIVE;

		public static int NOVICE_MAX_LEVEL;

		public static int NOVICE_PROTECTION_LEVEL_RANGE;

		/// <summary>
		///Record Settings </summary>
		public static sbyte LOGGING_WEAPON_ENCHANT;

		public static sbyte LOGGING_ARMOR_ENCHANT;

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
			_log.info("loading gameserver config");
			// server.properties
			try
			{
				Properties serverSettings = new Properties();
				Stream @is = new FileStream(SERVER_CONFIG_FILE, FileMode.Open, FileAccess.Read);
				serverSettings.load(@is);
				@is.Close();

				GAME_SERVER_HOST_NAME = serverSettings.getProperty("GameserverHostname", "*");
				GAME_SERVER_PORT = int.Parse(serverSettings.getProperty("GameserverPort", "2000"));
				DB_DRIVER = serverSettings.getProperty("Driver", "com.mysql.jdbc.Driver");
				DB_URL = serverSettings.getProperty("URL", "jdbc:mysql://localhost/l1jdb?useUnicode=true&characterEncoding=utf8");
				DB_LOGIN = serverSettings.getProperty("Login", "root");
				DB_PASSWORD = serverSettings.getProperty("Password", "");
				THREAD_P_TYPE_GENERAL = Convert.ToInt32(serverSettings.getProperty("RunnableExecuterType", "0"), 10);
				THREAD_P_SIZE_GENERAL = Convert.ToInt32(serverSettings.getProperty("RunnableExecuterSize", "0"), 10);
				CLIENT_LANGUAGE = int.Parse(serverSettings.getProperty("ClientLanguage", "4"));
				CLIENT_LANGUAGE_CODE = LANGUAGE_CODE_ARRAY[CLIENT_LANGUAGE];
				TIME_ZONE = serverSettings.getProperty("TimeZone", "Asia/Taipei");
				HOSTNAME_LOOKUPS = bool.Parse(serverSettings.getProperty("HostnameLookups", "false"));
				AUTOMATIC_KICK = int.Parse(serverSettings.getProperty("AutomaticKick", "10"));
				AUTO_CREATE_ACCOUNTS = bool.Parse(serverSettings.getProperty("AutoCreateAccounts", "true"));
				MAX_ONLINE_USERS = short.Parse(serverSettings.getProperty("MaximumOnlineUsers", "30"));
				CACHE_MAP_FILES = bool.Parse(serverSettings.getProperty("CacheMapFiles", "false"));
				LOAD_V2_MAP_FILES = bool.Parse(serverSettings.getProperty("LoadV2MapFiles", "false"));
				CHECK_MOVE_INTERVAL = bool.Parse(serverSettings.getProperty("CheckMoveInterval", "false"));
				CHECK_ATTACK_INTERVAL = bool.Parse(serverSettings.getProperty("CheckAttackInterval", "false"));
				CHECK_SPELL_INTERVAL = bool.Parse(serverSettings.getProperty("CheckSpellInterval", "false"));
				INJUSTICE_COUNT = short.Parse(serverSettings.getProperty("InjusticeCount", "10"));
				JUSTICE_COUNT = int.Parse(serverSettings.getProperty("JusticeCount", "4"));
				CHECK_STRICTNESS = int.Parse(serverSettings.getProperty("CheckStrictness", "102"));
				ILLEGAL_SPEEDUP_PUNISHMENT = int.Parse(serverSettings.getProperty("Punishment", "0"));
				AUTOSAVE_INTERVAL = Convert.ToInt32(serverSettings.getProperty("AutosaveInterval", "1200"), 10);
				AUTOSAVE_INTERVAL_INVENTORY = Convert.ToInt32(serverSettings.getProperty("AutosaveIntervalOfInventory", "300"), 10);
				SKILLTIMER_IMPLTYPE = int.Parse(serverSettings.getProperty("SkillTimerImplType", "1"));
				NPCAI_IMPLTYPE = int.Parse(serverSettings.getProperty("NpcAIImplType", "1"));
				TELNET_SERVER = bool.Parse(serverSettings.getProperty("TelnetServer", "false"));
				TELNET_SERVER_PORT = int.Parse(serverSettings.getProperty("TelnetServerPort", "23"));
				PC_RECOGNIZE_RANGE = int.Parse(serverSettings.getProperty("PcRecognizeRange", "20"));
				CHARACTER_CONFIG_IN_SERVER_SIDE = bool.Parse(serverSettings.getProperty("CharacterConfigInServerSide", "true"));
				ALLOW_2PC = bool.Parse(serverSettings.getProperty("Allow2PC", "true"));
				LEVEL_DOWN_RANGE = int.Parse(serverSettings.getProperty("LevelDownRange", "0"));
				SEND_PACKET_BEFORE_TELEPORT = bool.Parse(serverSettings.getProperty("SendPacketBeforeTeleport", "false"));
				DETECT_DB_RESOURCE_LEAKS = bool.Parse(serverSettings.getProperty("EnableDatabaseResourceLeaksDetection", "false"));
				CmdActive = bool.Parse(serverSettings.getProperty("CmdActive", "false"));
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
				throw new Exception("Failed to Load " + SERVER_CONFIG_FILE + " File.");
			}

			// rates.properties
			try
			{
				Properties rateSettings = new Properties();
				Stream @is = new FileStream(RATES_CONFIG_FILE, FileMode.Open, FileAccess.Read);
				rateSettings.load(@is);
				@is.Close();

				RATE_XP = double.Parse(rateSettings.getProperty("RateXp", "1.0"));
				RATE_LA = double.Parse(rateSettings.getProperty("RateLawful", "1.0"));
				RATE_KARMA = double.Parse(rateSettings.getProperty("RateKarma", "1.0"));
				RATE_DROP_ADENA = double.Parse(rateSettings.getProperty("RateDropAdena", "1.0"));
				RATE_DROP_ITEMS = double.Parse(rateSettings.getProperty("RateDropItems", "1.0"));
				ENCHANT_CHANCE_WEAPON = int.Parse(rateSettings.getProperty("EnchantChanceWeapon", "68"));
				ENCHANT_CHANCE_ARMOR = int.Parse(rateSettings.getProperty("EnchantChanceArmor", "52"));
				ATTR_ENCHANT_CHANCE = int.Parse(rateSettings.getProperty("AttrEnchantChance", "10"));
				RATE_WEIGHT_LIMIT = double.Parse(rateSettings.getProperty("RateWeightLimit", "1"));
				RATE_WEIGHT_LIMIT_PET = double.Parse(rateSettings.getProperty("RateWeightLimitforPet", "1"));
				RATE_SHOP_SELLING_PRICE = double.Parse(rateSettings.getProperty("RateShopSellingPrice", "1.0"));
				RATE_SHOP_PURCHASING_PRICE = double.Parse(rateSettings.getProperty("RateShopPurchasingPrice", "1.0"));
				CREATE_CHANCE_DIARY = int.Parse(rateSettings.getProperty("CreateChanceDiary", "33"));
				CREATE_CHANCE_RECOLLECTION = int.Parse(rateSettings.getProperty("CreateChanceRecollection", "90"));
				CREATE_CHANCE_MYSTERIOUS = int.Parse(rateSettings.getProperty("CreateChanceMysterious", "90"));
				CREATE_CHANCE_PROCESSING = int.Parse(rateSettings.getProperty("CreateChanceProcessing", "90"));
				CREATE_CHANCE_PROCESSING_DIAMOND = int.Parse(rateSettings.getProperty("CreateChanceProcessingDiamond", "90"));
				CREATE_CHANCE_DANTES = int.Parse(rateSettings.getProperty("CreateChanceDantes", "50"));
				CREATE_CHANCE_ANCIENT_AMULET = int.Parse(rateSettings.getProperty("CreateChanceAncientAmulet", "90"));
				CREATE_CHANCE_HISTORY_BOOK = int.Parse(rateSettings.getProperty("CreateChanceHistoryBook", "50"));
				MAGIC_STONE_TYPE = int.Parse(rateSettings.getProperty("MagicStoneAttr", "50"));
				MAGIC_STONE_LEVEL = int.Parse(rateSettings.getProperty("MagicStoneLevel", "50"));
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
				throw new Exception("Failed to Load " + RATES_CONFIG_FILE + " File.");
			}

			// altsettings.properties
			try
			{
				Properties altSettings = new Properties();
				Stream @is = new FileStream(ALT_SETTINGS_FILE, FileMode.Open, FileAccess.Read);
				altSettings.load(@is);
				@is.Close();

				GLOBAL_CHAT_LEVEL = short.Parse(altSettings.getProperty("GlobalChatLevel", "30"));
				WHISPER_CHAT_LEVEL = short.Parse(altSettings.getProperty("WhisperChatLevel", "5"));
				AUTO_LOOT = sbyte.Parse(altSettings.getProperty("AutoLoot", "2"));
				LOOTING_RANGE = int.Parse(altSettings.getProperty("LootingRange", "3"));
				ALT_NONPVP = bool.Parse(altSettings.getProperty("NonPvP", "true"));
				ALT_ATKMSG = bool.Parse(altSettings.getProperty("AttackMessageOn", "true"));
				CHANGE_TITLE_BY_ONESELF = bool.Parse(altSettings.getProperty("ChangeTitleByOneself", "false"));
				MAX_CLAN_MEMBER = int.Parse(altSettings.getProperty("MaxClanMember", "0"));
				CLAN_ALLIANCE = bool.Parse(altSettings.getProperty("ClanAlliance", "true"));
				MAX_PT = int.Parse(altSettings.getProperty("MaxPT", "8"));
				MAX_CHAT_PT = int.Parse(altSettings.getProperty("MaxChatPT", "8"));
				SIM_WAR_PENALTY = bool.Parse(altSettings.getProperty("SimWarPenalty", "true"));
				GET_BACK = bool.Parse(altSettings.getProperty("GetBack", "false"));
				ALT_ITEM_DELETION_TYPE = altSettings.getProperty("ItemDeletionType", "auto");
				ALT_ITEM_DELETION_TIME = int.Parse(altSettings.getProperty("ItemDeletionTime", "10"));
				ALT_ITEM_DELETION_RANGE = int.Parse(altSettings.getProperty("ItemDeletionRange", "5"));
				ALT_GMSHOP = bool.Parse(altSettings.getProperty("GMshop", "false"));
				ALT_GMSHOP_MIN_ID = int.Parse(altSettings.getProperty("GMshopMinID", "0xffffffff")); // 設定錯誤時就取消GM商店
				ALT_GMSHOP_MAX_ID = int.Parse(altSettings.getProperty("GMshopMaxID", "0xffffffff")); // 設定錯誤時就取消GM商店
				ALT_HALLOWEENIVENT = bool.Parse(altSettings.getProperty("HalloweenIvent", "true"));
				ALT_JPPRIVILEGED = bool.Parse(altSettings.getProperty("JpPrivileged", "false"));
				ALT_TALKINGSCROLLQUEST = bool.Parse(altSettings.getProperty("TalkingScrollQuest", "false"));
				ALT_WHO_COMMAND = bool.Parse(altSettings.getProperty("WhoCommand", "false"));
				ALT_REVIVAL_POTION = bool.Parse(altSettings.getProperty("RevivalPotion", "false"));
				GDROPITEM_TIME = int.Parse(altSettings.getProperty("GDropItemTime", "10"));
				REST_TIME = int.Parse(altSettings.getProperty("RestartTime", "60"));
				Use_Show_Announcecycle = bool.Parse(altSettings.getProperty("UseShowAnnouncecycle", "false")); // 循環公告 2/3by 丫傑
				Show_Announcecycle_Time = int.Parse(altSettings.getProperty("ShowAnnouncecycleTime", "30")); // 循環時間 2/3 by 丫傑
				ALT_WHO_TYPE = int.Parse(altSettings.getProperty("ALTWHOTYPE","1"));
				GM_TALK = bool.Parse(altSettings.getProperty("GMSpeakName", "false")); //TODO ＧＭ使用公頻(&)顯示方式 2/3
				Attack_Mob_HP_Bar = bool.Parse(altSettings.getProperty("AttackMobHPBar", "false")); // 攻擊顯示怪物血條
				ALL_ITEM_SELL = bool.Parse(altSettings.getProperty("AllItemSell", "false")); // 全道具販賣 by 丫傑 end
				SHOW_NPC_ID = bool.Parse(altSettings.getProperty("SHOWNPCID", "true"));
				RATE_AIN_TIME = int.Parse(altSettings.getProperty("RateAinTime", "30")); //TODO 殷海薩的祝福
				RATE_AIN_OUTTIME = int.Parse(altSettings.getProperty("RateAinOutTime", "30")); //TODO 殷海薩的祝福
				RATE_MAX_CHARGE_PERCENT = int.Parse(altSettings.getProperty("RateMaxChargePercent", "200")); //TODO 殷海薩的祝福
				BONUS_STATS1 = int.Parse(altSettings.getProperty("BONUS_STATS1", "25")); //調整能力值上限 by 丫傑 end
				BONUS_STATS2 = int.Parse(altSettings.getProperty("BONUS_STATS2", "5")); //調整能力值上限 by 丫傑 end
				BONUS_STATS3 = int.Parse(altSettings.getProperty("BONUS_STATS3", "25")); //調整能力值上限 by 丫傑 end
				Drop_Item = bool.Parse(altSettings.getProperty("DropItem", "false")); //TODO　刪除丟棄物品 by bill00148
				DropItemMinLv = sbyte.Parse(altSettings.getProperty("DropItemMinLv", "200")); //TODO　刪除丟棄物品 by bill00148
				MaxHPMP = bool.Parse(altSettings.getProperty("FullHPMP", "false")); //TODO 升級血魔滿
				DeleteFood = bool.Parse(altSettings.getProperty("DeleteFood", "false")); //TODO 廣播扣飽食度
				Gamesleep = int.Parse(altSettings.getProperty("Gamesleep", "30"));
				RATE_XP_PET = double.Parse(altSettings.getProperty("PetRateXp","1.0")); //TODO 寵物經驗倍率
				Pet_Max_LV = int.Parse(altSettings.getProperty("PetMaxLV", "50")); //TODO 寵物最高等級設定
				GM_OVERHEARD = bool.Parse(altSettings.getProperty("GM_OVERHEARD", "false")); // 密語頻道
				GM_OVERHEARD0 = bool.Parse(altSettings.getProperty("GM_OVERHEARD0", "false")); // 一般頻道
				GM_OVERHEARD4 = bool.Parse(altSettings.getProperty("GM_OVERHEARD4", "false")); // 血盟頻道
				GM_OVERHEARD11 = bool.Parse(altSettings.getProperty("GM_OVERHEARD11", "false")); // 組隊頻道
				GM_OVERHEARD13 = bool.Parse(altSettings.getProperty("GM_OVERHEARD13", "false")); // 聯盟頻道
				string strWar;
				strWar = altSettings.getProperty("WarTime", "2h");
				if (strWar.IndexOf("d", StringComparison.Ordinal) >= 0)
				{
					ALT_WAR_TIME_UNIT = DateTime.DATE;
					strWar = strWar.Replace("d", "");
				}
				else if (strWar.IndexOf("h", StringComparison.Ordinal) >= 0)
				{
					ALT_WAR_TIME_UNIT = DateTime.HOUR_OF_DAY;
					strWar = strWar.Replace("h", "");
				}
				else if (strWar.IndexOf("m", StringComparison.Ordinal) >= 0)
				{
					ALT_WAR_TIME_UNIT = DateTime.MINUTE;
					strWar = strWar.Replace("m", "");
				}
				ALT_WAR_TIME = int.Parse(strWar);
				strWar = altSettings.getProperty("WarInterval", "4d");
				if (strWar.IndexOf("d", StringComparison.Ordinal) >= 0)
				{
					ALT_WAR_INTERVAL_UNIT = DateTime.DATE;
					strWar = strWar.Replace("d", "");
				}
				else if (strWar.IndexOf("h", StringComparison.Ordinal) >= 0)
				{
					ALT_WAR_INTERVAL_UNIT = DateTime.HOUR_OF_DAY;
					strWar = strWar.Replace("h", "");
				}
				else if (strWar.IndexOf("m", StringComparison.Ordinal) >= 0)
				{
					ALT_WAR_INTERVAL_UNIT = DateTime.MINUTE;
					strWar = strWar.Replace("m", "");
				}
				ALT_WAR_INTERVAL = int.Parse(strWar);
				SPAWN_HOME_POINT = bool.Parse(altSettings.getProperty("SpawnHomePoint", "true"));
				SPAWN_HOME_POINT_COUNT = int.Parse(altSettings.getProperty("SpawnHomePointCount", "2"));
				SPAWN_HOME_POINT_DELAY = int.Parse(altSettings.getProperty("SpawnHomePointDelay", "100"));
				SPAWN_HOME_POINT_RANGE = int.Parse(altSettings.getProperty("SpawnHomePointRange", "8"));
				INIT_BOSS_SPAWN = bool.Parse(altSettings.getProperty("InitBossSpawn", "true"));
				ELEMENTAL_STONE_AMOUNT = int.Parse(altSettings.getProperty("ElementalStoneAmount", "300"));
				HOUSE_TAX_INTERVAL = int.Parse(altSettings.getProperty("HouseTaxInterval", "10"));
				MAX_DOLL_COUNT = int.Parse(altSettings.getProperty("MaxDollCount", "1"));
				RETURN_TO_NATURE = bool.Parse(altSettings.getProperty("ReturnToNature", "false"));
				MAX_NPC_ITEM = int.Parse(altSettings.getProperty("MaxNpcItem", "8"));
				MAX_PERSONAL_WAREHOUSE_ITEM = int.Parse(altSettings.getProperty("MaxPersonalWarehouseItem", "100"));
				MAX_CLAN_WAREHOUSE_ITEM = int.Parse(altSettings.getProperty("MaxClanWarehouseItem", "200"));
				DELETE_CHARACTER_AFTER_7DAYS = bool.Parse(altSettings.getProperty("DeleteCharacterAfter7Days", "True"));
				NPC_DELETION_TIME = int.Parse(altSettings.getProperty("NpcDeletionTime", "10"));
				DEFAULT_CHARACTER_SLOT = int.Parse(altSettings.getProperty("DefaultCharacterSlot", "6"));
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
				throw new Exception("Failed to Load " + ALT_SETTINGS_FILE + " File.");
			}

			// charsettings.properties
			try
			{
				Properties charSettings = new Properties();
				Stream @is = new FileStream(CHAR_SETTINGS_CONFIG_FILE, FileMode.Open, FileAccess.Read);
				charSettings.load(@is);
				@is.Close();

				PRINCE_MAX_HP = int.Parse(charSettings.getProperty("PrinceMaxHP", "1000"));
				PRINCE_MAX_MP = int.Parse(charSettings.getProperty("PrinceMaxMP", "800"));
				KNIGHT_MAX_HP = int.Parse(charSettings.getProperty("KnightMaxHP", "1400"));
				KNIGHT_MAX_MP = int.Parse(charSettings.getProperty("KnightMaxMP", "600"));
				ELF_MAX_HP = int.Parse(charSettings.getProperty("ElfMaxHP", "1000"));
				ELF_MAX_MP = int.Parse(charSettings.getProperty("ElfMaxMP", "900"));
				WIZARD_MAX_HP = int.Parse(charSettings.getProperty("WizardMaxHP", "800"));
				WIZARD_MAX_MP = int.Parse(charSettings.getProperty("WizardMaxMP", "1200"));
				DARKELF_MAX_HP = int.Parse(charSettings.getProperty("DarkelfMaxHP", "1000"));
				DARKELF_MAX_MP = int.Parse(charSettings.getProperty("DarkelfMaxMP", "900"));
				DRAGONKNIGHT_MAX_HP = int.Parse(charSettings.getProperty("DragonKnightMaxHP", "1400"));
				DRAGONKNIGHT_MAX_MP = int.Parse(charSettings.getProperty("DragonKnightMaxMP", "600"));
				ILLUSIONIST_MAX_HP = int.Parse(charSettings.getProperty("IllusionistMaxHP", "900"));
				ILLUSIONIST_MAX_MP = int.Parse(charSettings.getProperty("IllusionistMaxMP", "1100"));
				LV50_EXP = int.Parse(charSettings.getProperty("Lv50Exp", "1"));
				LV51_EXP = int.Parse(charSettings.getProperty("Lv51Exp", "1"));
				LV52_EXP = int.Parse(charSettings.getProperty("Lv52Exp", "1"));
				LV53_EXP = int.Parse(charSettings.getProperty("Lv53Exp", "1"));
				LV54_EXP = int.Parse(charSettings.getProperty("Lv54Exp", "1"));
				LV55_EXP = int.Parse(charSettings.getProperty("Lv55Exp", "1"));
				LV56_EXP = int.Parse(charSettings.getProperty("Lv56Exp", "1"));
				LV57_EXP = int.Parse(charSettings.getProperty("Lv57Exp", "1"));
				LV58_EXP = int.Parse(charSettings.getProperty("Lv58Exp", "1"));
				LV59_EXP = int.Parse(charSettings.getProperty("Lv59Exp", "1"));
				LV60_EXP = int.Parse(charSettings.getProperty("Lv60Exp", "1"));
				LV61_EXP = int.Parse(charSettings.getProperty("Lv61Exp", "1"));
				LV62_EXP = int.Parse(charSettings.getProperty("Lv62Exp", "1"));
				LV63_EXP = int.Parse(charSettings.getProperty("Lv63Exp", "1"));
				LV64_EXP = int.Parse(charSettings.getProperty("Lv64Exp", "1"));
				LV65_EXP = int.Parse(charSettings.getProperty("Lv65Exp", "2"));
				LV66_EXP = int.Parse(charSettings.getProperty("Lv66Exp", "2"));
				LV67_EXP = int.Parse(charSettings.getProperty("Lv67Exp", "2"));
				LV68_EXP = int.Parse(charSettings.getProperty("Lv68Exp", "2"));
				LV69_EXP = int.Parse(charSettings.getProperty("Lv69Exp", "2"));
				LV70_EXP = int.Parse(charSettings.getProperty("Lv70Exp", "4"));
				LV71_EXP = int.Parse(charSettings.getProperty("Lv71Exp", "4"));
				LV72_EXP = int.Parse(charSettings.getProperty("Lv72Exp", "4"));
				LV73_EXP = int.Parse(charSettings.getProperty("Lv73Exp", "4"));
				LV74_EXP = int.Parse(charSettings.getProperty("Lv74Exp", "4"));
				LV75_EXP = int.Parse(charSettings.getProperty("Lv75Exp", "8"));
				LV76_EXP = int.Parse(charSettings.getProperty("Lv76Exp", "8"));
				LV77_EXP = int.Parse(charSettings.getProperty("Lv77Exp", "8"));
				LV78_EXP = int.Parse(charSettings.getProperty("Lv78Exp", "8"));
				LV79_EXP = int.Parse(charSettings.getProperty("Lv79Exp", "16"));
				LV80_EXP = int.Parse(charSettings.getProperty("Lv80Exp", "32"));
				LV81_EXP = int.Parse(charSettings.getProperty("Lv81Exp", "64"));
				LV82_EXP = int.Parse(charSettings.getProperty("Lv82Exp", "128"));
				LV83_EXP = int.Parse(charSettings.getProperty("Lv83Exp", "256"));
				LV84_EXP = int.Parse(charSettings.getProperty("Lv84Exp", "512"));
				LV85_EXP = int.Parse(charSettings.getProperty("Lv85Exp", "1024"));
				LV86_EXP = int.Parse(charSettings.getProperty("Lv86Exp", "2048"));
				LV87_EXP = int.Parse(charSettings.getProperty("Lv87Exp", "4096"));
				LV88_EXP = int.Parse(charSettings.getProperty("Lv88Exp", "8192"));
				LV89_EXP = int.Parse(charSettings.getProperty("Lv89Exp", "16384"));
				LV90_EXP = int.Parse(charSettings.getProperty("Lv90Exp", "32768"));
				LV91_EXP = int.Parse(charSettings.getProperty("Lv91Exp", "65536"));
				LV92_EXP = int.Parse(charSettings.getProperty("Lv92Exp", "131072"));
				LV93_EXP = int.Parse(charSettings.getProperty("Lv93Exp", "262144"));
				LV94_EXP = int.Parse(charSettings.getProperty("Lv94Exp", "524288"));
				LV95_EXP = int.Parse(charSettings.getProperty("Lv95Exp", "1048576"));
				LV96_EXP = int.Parse(charSettings.getProperty("Lv96Exp", "2097152"));
				LV97_EXP = int.Parse(charSettings.getProperty("Lv97Exp", "4194304"));
				LV98_EXP = int.Parse(charSettings.getProperty("Lv98Exp", "8388608"));
				LV99_EXP = int.Parse(charSettings.getProperty("Lv99Exp", "16777216"));
				LV100_EXP = int.Parse(charSettings.getProperty("Lv100Exp", "16777216"));
				LV101_EXP = int.Parse(charSettings.getProperty("Lv101Exp", "65536"));
				LV102_EXP = int.Parse(charSettings.getProperty("Lv102Exp", "131072"));
				LV103_EXP = int.Parse(charSettings.getProperty("Lv103Exp", "262144"));
				LV104_EXP = int.Parse(charSettings.getProperty("Lv104Exp", "524288"));
				LV105_EXP = int.Parse(charSettings.getProperty("Lv105Exp", "1048576"));
				LV106_EXP = int.Parse(charSettings.getProperty("Lv106Exp", "2097152"));
				LV107_EXP = int.Parse(charSettings.getProperty("Lv107Exp", "4194304"));
				LV108_EXP = int.Parse(charSettings.getProperty("Lv108Exp", "8388608"));
				LV109_EXP = int.Parse(charSettings.getProperty("Lv109Exp", "16777216"));
				LV110_EXP = int.Parse(charSettings.getProperty("Lv110Exp", "16777216"));
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
				throw new Exception("Failed to Load " + CHAR_SETTINGS_CONFIG_FILE + " File.");
			}

			// fights.properties
			Properties fightSettings = new Properties();
			try
			{
				Stream @is = new FileStream(FIGHT_SETTINGS_CONFIG_FILE, FileMode.Open, FileAccess.Read);
				fightSettings.load(@is);
				@is.Close();

				FIGHT_IS_ACTIVE = bool.Parse(fightSettings.getProperty("FightIsActive", "False"));
				NOVICE_PROTECTION_IS_ACTIVE = bool.Parse(fightSettings.getProperty("NoviceProtectionIsActive", "False"));
				NOVICE_MAX_LEVEL = int.Parse(fightSettings.getProperty("NoviceMaxLevel", "20"));
				NOVICE_PROTECTION_LEVEL_RANGE = int.Parse(fightSettings.getProperty("ProtectionLevelRange", "10"));
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
				throw new Exception("無法讀取設定檔: " + FIGHT_SETTINGS_CONFIG_FILE);
			}

			// record.properties
			try
			{
				Properties recordSettings = new Properties();
				Stream @is = new FileStream(RECORD_SETTINGS_CONFIG_FILE, FileMode.Open, FileAccess.Read);
				recordSettings.load(@is);
				@is.Close();

				LOGGING_WEAPON_ENCHANT = sbyte.Parse(recordSettings.getProperty("LoggingWeaponEnchant", "0"));
				LOGGING_ARMOR_ENCHANT = sbyte.Parse(recordSettings.getProperty("LoggingArmorEnchant", "0"));
				LOGGING_CHAT_NORMAL = bool.Parse(recordSettings.getProperty("LoggingChatNormal", "false"));
				LOGGING_CHAT_WHISPER = bool.Parse(recordSettings.getProperty("LoggingChatWhisper", "false"));
				LOGGING_CHAT_SHOUT = bool.Parse(recordSettings.getProperty("LoggingChatShout", "false"));
				LOGGING_CHAT_WORLD = bool.Parse(recordSettings.getProperty("LoggingChatWorld", "false"));
				LOGGING_CHAT_CLAN = bool.Parse(recordSettings.getProperty("LoggingChatClan", "false"));
				LOGGING_CHAT_PARTY = bool.Parse(recordSettings.getProperty("LoggingChatParty", "false"));
				LOGGING_CHAT_COMBINED = bool.Parse(recordSettings.getProperty("LoggingChatCombined", "false"));
				LOGGING_CHAT_CHAT_PARTY = bool.Parse(recordSettings.getProperty("LoggingChatChatParty", "false"));
				writeTradeLog = bool.Parse(recordSettings.getProperty("writeTradeLog", "false"));
				writeRobotsLog = bool.Parse(recordSettings.getProperty("writeRobotsLog", "false"));
				writeDropLog = bool.Parse(recordSettings.getProperty("writeDropLog", "false"));
				MysqlAutoBackup = int.Parse(recordSettings.getProperty("MysqlAutoBackup", "false"));
				CompressGzip = bool.Parse(recordSettings.getProperty("CompressGzip", "false"));

			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
				throw new Exception("Failed to Load: " + RECORD_SETTINGS_CONFIG_FILE);
			}

			validate();
		}

		private static void validate()
		{
			if (!IntRange.includes(Config.ALT_ITEM_DELETION_RANGE, 0, 5))
			{
				throw new System.InvalidOperationException("ItemDeletionRange 的設定值超出範圍。");
			}

			if (!IntRange.includes(Config.ALT_ITEM_DELETION_TIME, 1, 35791))
			{
				throw new System.InvalidOperationException("ItemDeletionTime 的設定值超出範圍。");
			}
		}

		public static bool setParameterValue(string pName, string pValue)
		{
			// server.properties
			if (pName.Equals("GameserverHostname", StringComparison.OrdinalIgnoreCase))
			{
				GAME_SERVER_HOST_NAME = pValue;
			}
			else if (pName.Equals("GameserverPort", StringComparison.OrdinalIgnoreCase))
			{
				GAME_SERVER_PORT = int.Parse(pValue);
			}
			else if (pName.Equals("Driver", StringComparison.OrdinalIgnoreCase))
			{
				DB_DRIVER = pValue;
			}
			else if (pName.Equals("URL", StringComparison.OrdinalIgnoreCase))
			{
				DB_URL = pValue;
			}
			else if (pName.Equals("Login", StringComparison.OrdinalIgnoreCase))
			{
				DB_LOGIN = pValue;
			}
			else if (pName.Equals("Password", StringComparison.OrdinalIgnoreCase))
			{
				DB_PASSWORD = pValue;
			}
			else if (pName.Equals("ClientLanguage", StringComparison.OrdinalIgnoreCase))
			{
				CLIENT_LANGUAGE = int.Parse(pValue);
			}
			else if (pName.Equals("TimeZone", StringComparison.OrdinalIgnoreCase))
			{
				TIME_ZONE = pValue;
			}
			else if (pName.Equals("AutomaticKick", StringComparison.OrdinalIgnoreCase))
			{
				AUTOMATIC_KICK = int.Parse(pValue);
			}
			else if (pName.Equals("AutoCreateAccounts", StringComparison.OrdinalIgnoreCase))
			{
				AUTO_CREATE_ACCOUNTS = bool.Parse(pValue);
			}
			else if (pName.Equals("MaximumOnlineUsers", StringComparison.OrdinalIgnoreCase))
			{
				MAX_ONLINE_USERS = short.Parse(pValue);
			}
			else if (pName.Equals("CharacterConfigInServerSide", StringComparison.OrdinalIgnoreCase))
			{
				CHARACTER_CONFIG_IN_SERVER_SIDE = bool.Parse(pValue);
			}
			else if (pName.Equals("Allow2PC", StringComparison.OrdinalIgnoreCase))
			{
				ALLOW_2PC = bool.Parse(pValue);
			}
			else if (pName.Equals("LevelDownRange", StringComparison.OrdinalIgnoreCase))
			{
				LEVEL_DOWN_RANGE = int.Parse(pValue);
			}
			else if (pName.Equals("SendPacketBeforeTeleport", StringComparison.OrdinalIgnoreCase))
			{
				SEND_PACKET_BEFORE_TELEPORT = bool.Parse(pValue);
			}
			else if (pName.Equals("Punishment", StringComparison.OrdinalIgnoreCase))
			{
				ILLEGAL_SPEEDUP_PUNISHMENT = int.Parse(pValue);
			}
			// rates.properties
			else if (pName.Equals("RateXp", StringComparison.OrdinalIgnoreCase))
			{
				RATE_XP = double.Parse(pValue);
			}
			else if (pName.Equals("RateLawful", StringComparison.OrdinalIgnoreCase))
			{
				RATE_LA = double.Parse(pValue);
			}
			else if (pName.Equals("RateKarma", StringComparison.OrdinalIgnoreCase))
			{
				RATE_KARMA = double.Parse(pValue);
			}
			else if (pName.Equals("RateDropAdena", StringComparison.OrdinalIgnoreCase))
			{
				RATE_DROP_ADENA = double.Parse(pValue);
			}
			else if (pName.Equals("RateDropItems", StringComparison.OrdinalIgnoreCase))
			{
				RATE_DROP_ITEMS = double.Parse(pValue);
			}
			else if (pName.Equals("EnchantChanceWeapon", StringComparison.OrdinalIgnoreCase))
			{
				ENCHANT_CHANCE_WEAPON = int.Parse(pValue);
			}
			else if (pName.Equals("EnchantChanceArmor", StringComparison.OrdinalIgnoreCase))
			{
				ENCHANT_CHANCE_ARMOR = int.Parse(pValue);
			}
			else if (pName.Equals("AttrEnchantChance", StringComparison.OrdinalIgnoreCase))
			{
				ATTR_ENCHANT_CHANCE = int.Parse(pValue);
			}
			else if (pName.Equals("Weightrate", StringComparison.OrdinalIgnoreCase))
			{
				RATE_WEIGHT_LIMIT = sbyte.Parse(pValue);
			}
			// altsettings.properties
			else if (pName.Equals("GlobalChatLevel", StringComparison.OrdinalIgnoreCase))
			{
				GLOBAL_CHAT_LEVEL = short.Parse(pValue);
			}
			else if (pName.Equals("WhisperChatLevel", StringComparison.OrdinalIgnoreCase))
			{
				WHISPER_CHAT_LEVEL = short.Parse(pValue);
			}
			else if (pName.Equals("AutoLoot", StringComparison.OrdinalIgnoreCase))
			{
				AUTO_LOOT = sbyte.Parse(pValue);
			}
			else if (pName.Equals("LOOTING_RANGE", StringComparison.OrdinalIgnoreCase))
			{
				LOOTING_RANGE = int.Parse(pValue);
			}
			else if (pName.Equals("AltNonPvP", StringComparison.OrdinalIgnoreCase))
			{
				ALT_NONPVP = Convert.ToBoolean(pValue);
			}
			else if (pName.Equals("AttackMessageOn", StringComparison.OrdinalIgnoreCase))
			{
				ALT_ATKMSG = Convert.ToBoolean(pValue);
			}
			else if (pName.Equals("ChangeTitleByOneself", StringComparison.OrdinalIgnoreCase))
			{
				CHANGE_TITLE_BY_ONESELF = Convert.ToBoolean(pValue);
			}
			else if (pName.Equals("MaxClanMember", StringComparison.OrdinalIgnoreCase))
			{
				MAX_CLAN_MEMBER = int.Parse(pValue);
			}
			else if (pName.Equals("ClanAlliance", StringComparison.OrdinalIgnoreCase))
			{
				CLAN_ALLIANCE = Convert.ToBoolean(pValue);
			}
			else if (pName.Equals("MaxPT", StringComparison.OrdinalIgnoreCase))
			{
				MAX_PT = int.Parse(pValue);
			}
			else if (pName.Equals("MaxChatPT", StringComparison.OrdinalIgnoreCase))
			{
				MAX_CHAT_PT = int.Parse(pValue);
			}
			else if (pName.Equals("SimWarPenalty", StringComparison.OrdinalIgnoreCase))
			{
				SIM_WAR_PENALTY = Convert.ToBoolean(pValue);
			}
			else if (pName.Equals("GetBack", StringComparison.OrdinalIgnoreCase))
			{
				GET_BACK = Convert.ToBoolean(pValue);
			}
			else if (pName.Equals("AutomaticItemDeletionTime", StringComparison.OrdinalIgnoreCase))
			{
				ALT_ITEM_DELETION_TIME = int.Parse(pValue);
			}
			else if (pName.Equals("AutomaticItemDeletionRange", StringComparison.OrdinalIgnoreCase))
			{
				ALT_ITEM_DELETION_RANGE = sbyte.Parse(pValue);
			}
			else if (pName.Equals("GMshop", StringComparison.OrdinalIgnoreCase))
			{
				ALT_GMSHOP = Convert.ToBoolean(pValue);
			}
			else if (pName.Equals("GMshopMinID", StringComparison.OrdinalIgnoreCase))
			{
				ALT_GMSHOP_MIN_ID = int.Parse(pValue);
			}
			else if (pName.Equals("GMshopMaxID", StringComparison.OrdinalIgnoreCase))
			{
				ALT_GMSHOP_MAX_ID = int.Parse(pValue);
			}
			else if (pName.Equals("HalloweenIvent", StringComparison.OrdinalIgnoreCase))
			{
				ALT_HALLOWEENIVENT = Convert.ToBoolean(pValue);
			}
			else if (pName.Equals("JpPrivileged", StringComparison.OrdinalIgnoreCase))
			{
				ALT_JPPRIVILEGED = Convert.ToBoolean(pValue);
			}
			else if (pName.Equals("TalkingScrollQuest", StringComparison.OrdinalIgnoreCase))
			{
				ALT_TALKINGSCROLLQUEST = Convert.ToBoolean(pValue);
			}
			else if (pName.Equals("HouseTaxInterval", StringComparison.OrdinalIgnoreCase))
			{
				HOUSE_TAX_INTERVAL = Convert.ToInt32(pValue);
			}
			else if (pName.Equals("MaxDollCount", StringComparison.OrdinalIgnoreCase))
			{
				MAX_DOLL_COUNT = Convert.ToInt32(pValue);
			}
			else if (pName.Equals("ReturnToNature", StringComparison.OrdinalIgnoreCase))
			{
				RETURN_TO_NATURE = Convert.ToBoolean(pValue);
			}
			else if (pName.Equals("MaxNpcItem", StringComparison.OrdinalIgnoreCase))
			{
				MAX_NPC_ITEM = Convert.ToInt32(pValue);
			}
			else if (pName.Equals("MaxPersonalWarehouseItem", StringComparison.OrdinalIgnoreCase))
			{
				MAX_PERSONAL_WAREHOUSE_ITEM = Convert.ToInt32(pValue);
			}
			else if (pName.Equals("MaxClanWarehouseItem", StringComparison.OrdinalIgnoreCase))
			{
				MAX_CLAN_WAREHOUSE_ITEM = Convert.ToInt32(pValue);
			}
			else if (pName.Equals("DeleteCharacterAfter7Days", StringComparison.OrdinalIgnoreCase))
			{
				DELETE_CHARACTER_AFTER_7DAYS = Convert.ToBoolean(pValue);
			}
			else if (pName.Equals("NpcDeletionTime", StringComparison.OrdinalIgnoreCase))
			{
				NPC_DELETION_TIME = Convert.ToInt32(pValue);
			}
			else if (pName.Equals("DefaultCharacterSlot", StringComparison.OrdinalIgnoreCase))
			{
				DEFAULT_CHARACTER_SLOT = Convert.ToInt32(pValue);
			}
			else if (pName.Equals("GDropItemTime", StringComparison.OrdinalIgnoreCase))
			{
					GDROPITEM_TIME = int.Parse(pValue);
			}

			// charsettings.properties
			else if (pName.Equals("PrinceMaxHP", StringComparison.OrdinalIgnoreCase))
			{
				PRINCE_MAX_HP = int.Parse(pValue);
			}
			else if (pName.Equals("PrinceMaxMP", StringComparison.OrdinalIgnoreCase))
			{
				PRINCE_MAX_MP = int.Parse(pValue);
			}
			else if (pName.Equals("KnightMaxHP", StringComparison.OrdinalIgnoreCase))
			{
				KNIGHT_MAX_HP = int.Parse(pValue);
			}
			else if (pName.Equals("KnightMaxMP", StringComparison.OrdinalIgnoreCase))
			{
				KNIGHT_MAX_MP = int.Parse(pValue);
			}
			else if (pName.Equals("ElfMaxHP", StringComparison.OrdinalIgnoreCase))
			{
				ELF_MAX_HP = int.Parse(pValue);
			}
			else if (pName.Equals("ElfMaxMP", StringComparison.OrdinalIgnoreCase))
			{
				ELF_MAX_MP = int.Parse(pValue);
			}
			else if (pName.Equals("WizardMaxHP", StringComparison.OrdinalIgnoreCase))
			{
				WIZARD_MAX_HP = int.Parse(pValue);
			}
			else if (pName.Equals("WizardMaxMP", StringComparison.OrdinalIgnoreCase))
			{
				WIZARD_MAX_MP = int.Parse(pValue);
			}
			else if (pName.Equals("DarkelfMaxHP", StringComparison.OrdinalIgnoreCase))
			{
				DARKELF_MAX_HP = int.Parse(pValue);
			}
			else if (pName.Equals("DarkelfMaxMP", StringComparison.OrdinalIgnoreCase))
			{
				DARKELF_MAX_MP = int.Parse(pValue);
			}
			else if (pName.Equals("DragonKnightMaxHP", StringComparison.OrdinalIgnoreCase))
			{
				DRAGONKNIGHT_MAX_HP = int.Parse(pValue);
			}
			else if (pName.Equals("DragonKnightMaxMP", StringComparison.OrdinalIgnoreCase))
			{
				DRAGONKNIGHT_MAX_MP = int.Parse(pValue);
			}
			else if (pName.Equals("IllusionistMaxHP", StringComparison.OrdinalIgnoreCase))
			{
				ILLUSIONIST_MAX_HP = int.Parse(pValue);
			}
			else if (pName.Equals("IllusionistMaxMP", StringComparison.OrdinalIgnoreCase))
			{
				ILLUSIONIST_MAX_MP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv50Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV50_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv51Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV51_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv52Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV52_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv53Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV53_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv54Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV54_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv55Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV55_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv56Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV56_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv57Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV57_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv58Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV58_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv59Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV59_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv60Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV60_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv61Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV61_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv62Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV62_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv63Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV63_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv64Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV64_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv65Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV65_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv66Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV66_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv67Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV67_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv68Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV68_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv69Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV69_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv70Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV70_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv71Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV71_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv72Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV72_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv73Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV73_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv74Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV74_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv75Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV75_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv76Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV76_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv77Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV77_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv78Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV78_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv79Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV79_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv80Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV80_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv81Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV81_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv82Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV82_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv83Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV83_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv84Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV84_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv85Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV85_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv86Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV86_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv87Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV87_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv88Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV88_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv89Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV89_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv90Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV90_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv91Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV91_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv92Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV92_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv93Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV93_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv94Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV94_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv95Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV95_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv96Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV96_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv97Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV97_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv98Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV98_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv99Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV99_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv100Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV100_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv101Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV101_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv102Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV102_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv103Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV103_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv104Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV104_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv105Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV105_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv106Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV106_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv107Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV107_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv108Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV108_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv109Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV109_EXP = int.Parse(pValue);
			}
			else if (pName.Equals("Lv110Exp", StringComparison.OrdinalIgnoreCase))
			{
				LV110_EXP = int.Parse(pValue);
			}
			//record.properties
			else if (pName.Equals("LoggingWeaponEnchant", StringComparison.OrdinalIgnoreCase))
			{
				LOGGING_WEAPON_ENCHANT = sbyte.Parse(pValue);
			}
			else if (pName.Equals("LoggingArmorEnchant", StringComparison.OrdinalIgnoreCase))
			{
				LOGGING_ARMOR_ENCHANT = sbyte.Parse(pValue);
			}
			else
			{
				return false;
			}
			return true;
		}

		private Config()
		{
		}
	}
}