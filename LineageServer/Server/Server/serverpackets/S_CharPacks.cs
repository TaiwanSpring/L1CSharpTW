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
namespace LineageServer.Server.Server.serverpackets
{
	using Opcodes = LineageServer.Server.Server.Opcodes;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_CharPacks : ServerBasePacket
	{
		private const string S_CHAR_PACKS = "[S] S_CharPacks";

		public S_CharPacks(string name, string clanName, int type, int sex, int lawful, int hp, int mp, int ac, int lv, int str, int dex, int con, int wis, int cha, int intel, int accessLevel, int birthday)
		{
			WriteC(Opcodes.S_OPCODE_CHARLIST);
			WriteS(name); // 角色名稱
			WriteS(clanName); // 血盟
			WriteC(type); // 職業種類
			WriteC(sex); // 性別
			WriteH(lawful); // 相性
			WriteH(hp); // 體力
			WriteH(mp); // 魔力
			WriteC(ac); // 防禦力
			WriteC(lv); // 等級
			WriteC(str); // 力量
			WriteC(dex); // 敏捷
			WriteC(con); // 體質
			WriteC(wis); // 精力
			WriteC(cha); // 魅力
			WriteC(intel); // 智力
			WriteC(0); // 是否為管理員
			WriteD(birthday); // 創造日
			WriteC((lv ^ str ^ dex ^ con ^ wis ^ cha ^ intel) & 0xff); // XOR 驗證

		}

		public override sbyte[] Content
		{
			get
			{
				return Bytes;
			}
		}

		public override string Type
		{
			get
			{
				return S_CHAR_PACKS;
			}
		}
	}

}