namespace LineageServer.Server.Server.Utils.Internationalization
{

	/// <summary>
	/// @category 台灣-正體中文<br>
	///           國際化的英文是Internationalization 因為單字中總共有18個字母，簡稱I18N，
	///           目的是讓應用程式可以應地區不同而顯示不同的訊息。
	/// </summary>
	public class messages_zh_TW : ListResourceBundle
	{
		internal static readonly object[][] contents = new object[][]
		{
			new object[] {"l1j.server.memoryUse", "使用了: "},
			new object[] {"l1j.server.memory", "MB 的記憶體"},
			new object[] {"l1j.server.server.model.onGroundItem", "地上的物品"},
			new object[] {"l1j.server.server.model.seconds", "10秒後將被清除"},
			new object[] {"l1j.server.server.model.deleted", "已經被清除了"},
			new object[] {"l1j.server.server.GameServer.ver", "版本: Lineage 3.80C  開發 By L1J-TW For All User"},
			new object[] {"l1j.server.server.GameServer.settingslist", "●●●●〈伺服器設置清單〉●●●●"},
			new object[] {"l1j.server.server.GameServer.exp", "「經驗值」"},
			new object[] {"l1j.server.server.GameServer.x", "【倍】"},
			new object[] {"l1j.server.server.GameServer.level", "【級】"},
			new object[] {"l1j.server.server.GameServer.justice", "「正義值」"},
			new object[] {"l1j.server.server.GameServer.karma", "「友好度」"},
			new object[] {"l1j.server.server.GameServer.dropitems", "「物品掉落」"},
			new object[] {"l1j.server.server.GameServer.dropadena", "「金幣掉落」"},
			new object[] {"l1j.server.server.GameServer.enchantweapon", "「衝武」"},
			new object[] {"l1j.server.server.GameServer.enchantarmor", "「衝防」"},
			new object[] {"l1j.server.server.GameServer.chatlevel", "「廣播頻道可用等級」"},
			new object[] {"l1j.server.server.GameServer.nonpvp1", "「Non-PvP設定」: 【無效 (PvP可能)】"},
			new object[] {"l1j.server.server.GameServer.nonpvp2", "「Non-PvP設定」: 【有效 (PvP不可)】"},
			new object[] {"l1j.server.server.GameServer.maxplayer", "連線人數上限為 "},
			new object[] {"l1j.server.server.GameServer.player", " 人 "},
			new object[] {"l1j.server.server.GameServer.waitingforuser", "等待客戶端連接中..."},
			new object[] {"l1j.server.server.GameServer.from", "從 "},
			new object[] {"l1j.server.server.GameServer.attempt", " 試圖連線"},
			new object[] {"l1j.server.server.GameServer.setporton", "伺服器成功建立在 port "},
			new object[] {"l1j.server.server.GameServer.initialfinished", "初始化完畢"}
		};

		protected internal override object[][] Contents
		{
			get
			{
				return contents;
			}
		}

	}

}