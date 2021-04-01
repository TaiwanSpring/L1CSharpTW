using LineageServer.Language;

namespace LineageServer.Server
{
	/// <summary>
	/// 國際化的英文是Internationalization 因為單字中總共有18個字母，簡稱I18N， 目的是讓應用程式可以應地區不同而顯示不同的訊息。
	/// </summary>
	class L1Message
	{/// <summary>
	 /// static 變數 
	 /// </summary>
		public static string memoryUse { get; private set; }
		public static string onGroundItem { get; private set; }
		public static string secondsDelete { get; private set; }
		public static string deleted { get; private set; }
		public static string ver { get; private set; }
		public static string settingslist { get; private set; }
		public static string exp { get; private set; }
		public static string x { get; private set; }
		public static string level { get; private set; }
		public static string justice { get; private set; }
		public static string karma { get; private set; }
		public static string dropitems { get; private set; }
		public static string dropadena { get; private set; }
		public static string enchantweapon { get; private set; }
		public static string enchantarmor { get; private set; }
		public static string chatlevel { get; private set; }
		public static string nonpvpNo { get; private set; }
		public static string nonpvpYes { get; private set; }
		public static string memory { get; private set; }
		public static string maxplayer { get; private set; }
		public static string player { get; private set; }
		public static string waitingforuser { get; private set; }
		public static string from { get; private set; }
		public static string attempt { get; private set; }
		public static string setporton { get; private set; }
		public static string initialfinished { get; private set; }

		static L1Message()
		{
			LoadLanguage(new ZhTW());
		}

		public static void Reload(string language)
		{

		}

		private static void LoadLanguage(LanguageBase language)
		{
			memoryUse = language.GetString("l1j.server.memoryUse");
			memory = language.GetString("l1j.server.memory");
			onGroundItem = language.GetString("l1j.server.server.model.onGroundItem");
			secondsDelete = language.GetString("l1j.server.server.model.seconds");
			deleted = language.GetString("l1j.server.server.model.deleted");
			ver = language.GetString("l1j.server.server.GameServer.ver");
			settingslist = language.GetString("l1j.server.server.GameServer.settingslist");
			exp = language.GetString("l1j.server.server.GameServer.exp");
			x = language.GetString("l1j.server.server.GameServer.x");
			level = language.GetString("l1j.server.server.GameServer.level");
			justice = language.GetString("l1j.server.server.GameServer.justice");
			karma = language.GetString("l1j.server.server.GameServer.karma");
			dropitems = language.GetString("l1j.server.server.GameServer.dropitems");
			dropadena = language.GetString("l1j.server.server.GameServer.dropadena");
			enchantweapon = language.GetString("l1j.server.server.GameServer.enchantweapon");
			enchantarmor = language.GetString("l1j.server.server.GameServer.enchantarmor");
			chatlevel = language.GetString("l1j.server.server.GameServer.chatlevel");
			nonpvpNo = language.GetString("l1j.server.server.GameServer.nonpvp1");
			nonpvpYes = language.GetString("l1j.server.server.GameServer.nonpvp2");
			maxplayer = language.GetString("l1j.server.server.GameServer.maxplayer");
			player = language.GetString("l1j.server.server.GameServer.player");
			waitingforuser = language.GetString("l1j.server.server.GameServer.waitingforuser");
			from = language.GetString("l1j.server.server.GameServer.from");
			attempt = language.GetString("l1j.server.server.GameServer.attempt");
			setporton = language.GetString("l1j.server.server.GameServer.setporton");
			initialfinished = language.GetString("l1j.server.server.GameServer.initialfinished");
		}
	}

}