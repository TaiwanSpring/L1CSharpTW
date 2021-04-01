using System.Collections.Generic;

namespace LineageServer.Language
{

	/// <summary>
	/// @category 中國-簡體中文<br>
	///           國際化的英文是Internationalization 因為單字中總共有18個字母，簡稱I18N，
	///           目的是讓應用程式可以應地區不同而顯示不同的訊息。
	/// </summary>
	class ZhCN : LanguageBase
    {
        private readonly IDictionary<string, string> stringMapping = new Dictionary<string, string>()
        {
            {"l1j.server.memoryUse", "使用了: "},
            {"l1j.server.memory", "MB 的记忆体"},
            {"l1j.server.server.model.onGroundItem", "地上的物品"},
            {"l1j.server.server.model.seconds", "10秒后将被清除"},
            {"l1j.server.server.model.deleted", "已经被清除了"},
            {"l1j.server.server.GameServer.ver", "版本: Lineage 3.80C  开发 By L1J-TW For All User"},
            {"l1j.server.server.GameServer.settingslist", "●●●●〈伺服器设置清单〉●●●●"},
            {"l1j.server.server.GameServer.exp", "「经验值」"},
            {"l1j.server.server.GameServer.x", "【倍】"},
            {"l1j.server.server.GameServer.level", "【级】"},
            {"l1j.server.server.GameServer.justice", "「正义值」"},
            {"l1j.server.server.GameServer.karma", "「友好度」"},
            {"l1j.server.server.GameServer.dropitems", "「物品掉落」"},
            {"l1j.server.server.GameServer.dropadena", "「金币掉落」"},
            {"l1j.server.server.GameServer.enchantweapon", "「冲武」"},
            {"l1j.server.server.GameServer.enchantarmor", "「冲防」"},
            {"l1j.server.server.GameServer.chatlevel", "「广播频道可用等级」"},
            {"l1j.server.server.GameServer.nonpvp1", "「Non-PvP设定」: 【无效 (PvP可能)】"},
            {"l1j.server.server.GameServer.nonpvp2", "「Non-PvP设定」: 【有效 (PvP不可)】"},
            {"l1j.server.server.GameServer.maxplayer", "连线人数上限为 "},
            {"l1j.server.server.GameServer.player", " 人 "},
            {"l1j.server.server.GameServer.waitingforuser", "等待客户端连接中..."},
            {"l1j.server.server.GameServer.from", "从 "},
            {"l1j.server.server.GameServer.attempt", " 试图连线"},
            {"l1j.server.server.GameServer.setporton", "伺服器成功建立在 port "},
            {"l1j.server.server.GameServer.initialfinished", "初始化完毕"}
        };


        public override string LanguageName => "zh-CN";
        public override string GetString(string key)
        {
            if (this.stringMapping.ContainsKey(key))
            {
                return this.stringMapping[key];
            }
            else
            {
                return base.GetString(key);
            }
        }

    }

}