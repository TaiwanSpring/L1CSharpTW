using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_CharPacks : ServerBasePacket
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

        public override string Type
        {
            get
            {
                return S_CHAR_PACKS;
            }
        }
    }

}