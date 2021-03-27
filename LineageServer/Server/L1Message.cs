using System;

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
	using LineageServer.Utils.Internationalization;

	/// <summary>
	/// 國際化的英文是Internationalization 因為單字中總共有18個字母，簡稱I18N， 目的是讓應用程式可以應地區不同而顯示不同的訊息。
	/// </summary>
	public class L1Message
	{

		private static L1Message _instance;
		internal ResourceBundle resource;

		private L1Message()
		{
			try
			{
				resource = ResourceBundle.getBundle(typeof(messages).FullName);
				initLocaleMessage();
			}
			catch (MissingResourceException mre)
			{
                System.Console.WriteLine(mre.ToString());
                System.Console.Write(mre.StackTrace);
			}
		}

		public static L1Message Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new L1Message();
				}
				return _instance;
			}
		}

		/// <summary>
		/// 簡短化變數名詞 </summary>
		public virtual void initLocaleMessage()
		{
			memoryUse = resource.getString("l1j.server.memoryUse");
			memory = resource.getString("l1j.server.memory");
			onGroundItem = resource.getString("l1j.server.server.model.onGroundItem");
			secondsDelete = resource.getString("l1j.server.server.model.seconds");
			deleted = resource.getString("l1j.server.server.model.deleted");
			ver = resource.getString("l1j.server.server.GameServer.ver");
			settingslist = resource.getString("l1j.server.server.GameServer.settingslist");
			exp = resource.getString("l1j.server.server.GameServer.exp");
			x = resource.getString("l1j.server.server.GameServer.x");
			level = resource.getString("l1j.server.server.GameServer.level");
			justice = resource.getString("l1j.server.server.GameServer.justice");
			karma = resource.getString("l1j.server.server.GameServer.karma");
			dropitems = resource.getString("l1j.server.server.GameServer.dropitems");
			dropadena = resource.getString("l1j.server.server.GameServer.dropadena");
			enchantweapon = resource.getString("l1j.server.server.GameServer.enchantweapon");
			enchantarmor = resource.getString("l1j.server.server.GameServer.enchantarmor");
			chatlevel = resource.getString("l1j.server.server.GameServer.chatlevel");
			nonpvpNo = resource.getString("l1j.server.server.GameServer.nonpvp1");
			nonpvpYes = resource.getString("l1j.server.server.GameServer.nonpvp2");
			maxplayer = resource.getString("l1j.server.server.GameServer.maxplayer");
			player = resource.getString("l1j.server.server.GameServer.player");
			waitingforuser = resource.getString("l1j.server.server.GameServer.waitingforuser");
			from = resource.getString("l1j.server.server.GameServer.from");
			attempt = resource.getString("l1j.server.server.GameServer.attempt");
			setporton = resource.getString("l1j.server.server.GameServer.setporton");
			initialfinished = resource.getString("l1j.server.server.GameServer.initialfinished");
		}

		/// <summary>
		/// static 變數 </summary>
		public static string memoryUse;
		public static string onGroundItem;
		public static string secondsDelete;
		public static string deleted;
		public static string ver;
		public static string settingslist;
		public static string exp;
		public static string x;
		public static string level;
		public static string justice;
		public static string karma;
		public static string dropitems;
		public static string dropadena;
		public static string enchantweapon;
		public static string enchantarmor;
		public static string chatlevel;
		public static string nonpvpNo;
		public static string nonpvpYes;
		public static string memory;
		public static string maxplayer;
		public static string player;
		public static string waitingforuser;
		public static string from;
		public static string attempt;
		public static string setporton;
		public static string initialfinished;
	}

}