namespace LineageServer.Server.Server.Utils.Internationalization
{

	/// <summary>
	/// @category 日本-日本語<br>
	///           國際化的英文是Internationalization 因為單字中總共有18個字母，簡稱I18N，
	///           目的是讓應用程式可以應地區不同而顯示不同的訊息。
	/// </summary>
	public class messages_ja_JP : ListResourceBundle
	{
		internal static readonly object[][] contents = new object[][]
		{
			new object[] {"l1j.server.memoryUse", "利用メモリ: "},
			new object[] {"l1j.server.memory", "MB"},
			new object[] {"l1j.server.server.model.onGroundItem", "ワールドマップ上のアイテム"},
			new object[] {"l1j.server.server.model.seconds", "10秒後に削除されます"},
			new object[] {"l1j.server.server.model.deleted", "削除されました"},
			new object[] {"l1j.server.server.GameServer.ver", "バージョン: Lineage 3.80C 開発  By L1J For All User"},
			new object[] {"l1j.server.server.GameServer.settingslist", "●●●●〈サーバー設定〉●●●●"},
			new object[] {"l1j.server.server.GameServer.exp", "「経験値」"},
			new object[] {"l1j.server.server.GameServer.x", "【倍】"},
			new object[] {"l1j.server.server.GameServer.level", ""},
			new object[] {"l1j.server.server.GameServer.justice", "「アライメント」"},
			new object[] {"l1j.server.server.GameServer.karma", "「カルマ」"},
			new object[] {"l1j.server.server.GameServer.dropitems", "「ドロップ率」"},
			new object[] {"l1j.server.server.GameServer.dropadena", "「取得アデナ」"},
			new object[] {"l1j.server.server.GameServer.enchantweapon", "「武器エンチャント成功率」"},
			new object[] {"l1j.server.server.GameServer.enchantarmor", "「防具エンチャント成功率」"},
			new object[] {"l1j.server.server.GameServer.chatlevel", "「全体チャット可能Lv」"},
			new object[] {"l1j.server.server.GameServer.nonpvp1", "「Non-PvP設定」: 無効（PvP可能）"},
			new object[] {"l1j.server.server.GameServer.nonpvp2", "「Non-PvP設定」: 有効（PvP不可）"},
			new object[] {"l1j.server.server.GameServer.maxplayer", "接続人数制限： 最大 "},
			new object[] {"l1j.server.server.GameServer.player", " 人 "},
			new object[] {"l1j.server.server.GameServer.waitingforuser", "クライアント接続待機中..."},
			new object[] {"l1j.server.server.GameServer.from", "接続試行中IP "},
			new object[] {"l1j.server.server.GameServer.attempt", ""},
			new object[] {"l1j.server.server.GameServer.setporton", "サーバーセッティング: サーバーソケット生成 "},
			new object[] {"l1j.server.server.GameServer.initialfinished", "ローディング完了"}
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