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
namespace LineageServer.Server.Server.utils.Internationalization
{

	/// <summary>
	/// @category 英美-英語<br>
	///           國際化的英文是Internationalization 因為單字中總共有18個字母，簡稱I18N，
	///           目的是讓應用程式可以應地區不同而顯示不同的訊息。
	/// </summary>

	public class messages_en_US : ListResourceBundle
	{
		internal static readonly object[][] contents = new object[][]
		{
			new object[] {"l1j.server.memoryUse", "Used: "},
			new object[] {"l1j.server.memory", "MB of memory"},
			new object[] {"l1j.server.server.model.onGroundItem", "items on the ground"},
			new object[] {"l1j.server.server.model.seconds", "will be delete after 10 seconds"},
			new object[] {"l1j.server.server.model.deleted", "was deleted"},
			new object[] {"l1j.server.server.GameServer.ver", "Version: Lineage 3.80C  Dev. By L1J-TW For All User"},
			new object[] {"l1j.server.server.GameServer.settingslist", "●●●●〈Server Config List〉●●●●"},
			new object[] {"l1j.server.server.GameServer.exp", "「exp」"},
			new object[] {"l1j.server.server.GameServer.x", "【times】"},
			new object[] {"l1j.server.server.GameServer.level", "【LV】"},
			new object[] {"l1j.server.server.GameServer.justice", "「justice」"},
			new object[] {"l1j.server.server.GameServer.karma", "「karma」"},
			new object[] {"l1j.server.server.GameServer.dropitems", "「dropitems」"},
			new object[] {"l1j.server.server.GameServer.dropadena", "「dropadena」"},
			new object[] {"l1j.server.server.GameServer.enchantweapon", "「enchantweapon」"},
			new object[] {"l1j.server.server.GameServer.enchantarmor", "「enchantarmor」"},
			new object[] {"l1j.server.server.GameServer.chatlevel", "「chatLevel」"},
			new object[] {"l1j.server.server.GameServer.nonpvp1", "「Non-PvP」: Not Work (PvP)"},
			new object[] {"l1j.server.server.GameServer.nonpvp2", "「Non-PvP」: Work (Non-PvP)"},
			new object[] {"l1j.server.server.GameServer.maxplayer", "Max connection limit "},
			new object[] {"l1j.server.server.GameServer.player", " players"},
			new object[] {"l1j.server.server.GameServer.waitingforuser", "Waiting for user's connection..."},
			new object[] {"l1j.server.server.GameServer.from", "from "},
			new object[] {"l1j.server.server.GameServer.attempt", " attempt to connect."},
			new object[] {"l1j.server.server.GameServer.setporton", "Server is successfully set on port "},
			new object[] {"l1j.server.server.GameServer.initialfinished", "Initialize finished.."}
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