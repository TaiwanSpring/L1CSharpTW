namespace LineageServer.Server.Server.utils.Internationalization
{

	/// <summary>
	/// @category 中國-簡體中文<br>
	///           國際化的英文是Internationalization 因為單字中總共有18個字母，簡稱I18N，
	///           目的是讓應用程式可以應地區不同而顯示不同的訊息。
	/// </summary>
	public class messages_zh_CN : ListResourceBundle
	{
		internal static readonly object[][] contents = new object[][]
		{
			new object[] {"l1j.server.memoryUse", "使用了: "},
			new object[] {"l1j.server.memory", "MB 的记忆体"},
			new object[] {"l1j.server.server.model.onGroundItem", "地上的物品"},
			new object[] {"l1j.server.server.model.seconds", "10秒后将被清除"},
			new object[] {"l1j.server.server.model.deleted", "已经被清除了"},
			new object[] {"l1j.server.server.GameServer.ver", "版本: Lineage 3.80C  开发 By L1J-TW For All User"},
			new object[] {"l1j.server.server.GameServer.settingslist", "●●●●〈伺服器设置清单〉●●●●"},
			new object[] {"l1j.server.server.GameServer.exp", "「经验值」"},
			new object[] {"l1j.server.server.GameServer.x", "【倍】"},
			new object[] {"l1j.server.server.GameServer.level", "【级】"},
			new object[] {"l1j.server.server.GameServer.justice", "「正义值」"},
			new object[] {"l1j.server.server.GameServer.karma", "「友好度」"},
			new object[] {"l1j.server.server.GameServer.dropitems", "「物品掉落」"},
			new object[] {"l1j.server.server.GameServer.dropadena", "「金币掉落」"},
			new object[] {"l1j.server.server.GameServer.enchantweapon", "「冲武」"},
			new object[] {"l1j.server.server.GameServer.enchantarmor", "「冲防」"},
			new object[] {"l1j.server.server.GameServer.chatlevel", "「广播频道可用等级」"},
			new object[] {"l1j.server.server.GameServer.nonpvp1", "「Non-PvP设定」: 【无效 (PvP可能)】"},
			new object[] {"l1j.server.server.GameServer.nonpvp2", "「Non-PvP设定」: 【有效 (PvP不可)】"},
			new object[] {"l1j.server.server.GameServer.maxplayer", "连线人数上限为 "},
			new object[] {"l1j.server.server.GameServer.player", " 人 "},
			new object[] {"l1j.server.server.GameServer.waitingforuser", "等待客户端连接中..."},
			new object[] {"l1j.server.server.GameServer.from", "从 "},
			new object[] {"l1j.server.server.GameServer.attempt", " 试图连线"},
			new object[] {"l1j.server.server.GameServer.setporton", "伺服器成功建立在 port "},
			new object[] {"l1j.server.server.GameServer.initialfinished", "初始化完毕"}
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