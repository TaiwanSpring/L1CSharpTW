using System.Collections.Generic;

namespace LineageServer.Language
{

    /// <summary>
    /// @category 英美-英語<br>
    ///           國際化的英文是Internationalization 因為單字中總共有18個字母，簡稱I18N，
    ///           目的是讓應用程式可以應地區不同而顯示不同的訊息。
    /// </summary>

    class EnUS : LanguageBase
    {
        private readonly IDictionary<string, string> stringMapping = new Dictionary<string, string>()
        {
           {"l1j.server.memoryUse", "Used: "},
           {"l1j.server.memory", "MB of memory"},
           {"l1j.server.server.model.onGroundItem", "items on the ground"},
           {"l1j.server.server.model.seconds", "will be delete after 10 seconds"},
           {"l1j.server.server.model.deleted", "was deleted"},
           {"l1j.server.server.GameServer.ver", "Version: Lineage 3.80C  Dev. By L1J-TW For All User"},
           {"l1j.server.server.GameServer.settingslist", "●●●●〈Server Config List〉●●●●"},
           {"l1j.server.server.GameServer.exp", "「exp」"},
           {"l1j.server.server.GameServer.x", "【times】"},
           {"l1j.server.server.GameServer.level", "【LV】"},
           {"l1j.server.server.GameServer.justice", "「justice」"},
           {"l1j.server.server.GameServer.karma", "「karma」"},
           {"l1j.server.server.GameServer.dropitems", "「dropitems」"},
           {"l1j.server.server.GameServer.dropadena", "「dropadena」"},
           {"l1j.server.server.GameServer.enchantweapon", "「enchantweapon」"},
           {"l1j.server.server.GameServer.enchantarmor", "「enchantarmor」"},
           {"l1j.server.server.GameServer.chatlevel", "「chatLevel」"},
           {"l1j.server.server.GameServer.nonpvp1", "「Non-PvP」: Not Work (PvP)"},
           {"l1j.server.server.GameServer.nonpvp2", "「Non-PvP」: Work (Non-PvP)"},
           {"l1j.server.server.GameServer.maxplayer", "Max connection limit "},
           {"l1j.server.server.GameServer.player", " players"},
           {"l1j.server.server.GameServer.waitingforuser", "Waiting for user's connection..."},
           {"l1j.server.server.GameServer.from", "from "},
           {"l1j.server.server.GameServer.attempt", " attempt to connect."},
           {"l1j.server.server.GameServer.setporton", "Server is successfully set on port "},
           {"l1j.server.server.GameServer.initialfinished", "Initialize finished.."}
        };

        public override string LanguageName => "en-US";

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