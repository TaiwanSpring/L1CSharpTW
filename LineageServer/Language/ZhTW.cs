using System.Collections.Generic;

namespace LineageServer.Language
{
    class ZhTW : LanguageBase
    {
        private readonly IDictionary<string, string> stringMapping = new Dictionary<string, string>()
        {
            {"l1j.server.memoryUse", "使用了: "},
            {"l1j.server.memory", "MB 的記憶體"},
            {"l1j.server.server.model.onGroundItem", "地上的物品"},
            {"l1j.server.server.model.seconds", "10秒後將被清除"},
            {"l1j.server.server.model.deleted", "已經被清除了"},
            {"l1j.server.server.GameServer.ver", "版本: Lineage 3.80C  開發 By L1J-TW For All User"},
            {"l1j.server.server.GameServer.settingslist", "●●●●〈伺服器設置清單〉●●●●"},
            {"l1j.server.server.GameServer.exp", "「經驗值」"},
            {"l1j.server.server.GameServer.x", "【倍】"},
            {"l1j.server.server.GameServer.level", "【級】"},
            {"l1j.server.server.GameServer.justice", "「正義值」"},
            {"l1j.server.server.GameServer.karma", "「友好度」"},
            {"l1j.server.server.GameServer.dropitems", "「物品掉落」"},
            {"l1j.server.server.GameServer.dropadena", "「金幣掉落」"},
            {"l1j.server.server.GameServer.enchantweapon", "「衝武」"},
            {"l1j.server.server.GameServer.enchantarmor", "「衝防」"},
            {"l1j.server.server.GameServer.chatlevel", "「廣播頻道可用等級」"},
            {"l1j.server.server.GameServer.nonpvp1", "「Non-PvP設定」: 【無效 (PvP可能)】"},
            {"l1j.server.server.GameServer.nonpvp2", "「Non-PvP設定」: 【有效 (PvP不可)】"},
            {"l1j.server.server.GameServer.maxplayer", "連線人數上限為 "},
            {"l1j.server.server.GameServer.player", " 人 "},
            {"l1j.server.server.GameServer.waitingforuser", "等待客戶端連接中..."},
            {"l1j.server.server.GameServer.from", "從 "},
            {"l1j.server.server.GameServer.attempt", " 試圖連線"},
            {"l1j.server.server.GameServer.setporton", "伺服器成功建立在 port "},
            {"l1j.server.server.GameServer.initialfinished", "初始化完畢"}
        };

        public override string LanguageName => "zh-TW";

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
