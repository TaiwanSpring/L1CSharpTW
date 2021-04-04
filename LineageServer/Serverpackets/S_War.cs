using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_War : ServerBasePacket
    {

        private const string S_WAR = "[S] S_War";

        private byte[] _byte = null;

        public S_War(int type, string clan_name1, string clan_name2)
        {
            buildPacket(type, clan_name1, clan_name2);
        }

        private void buildPacket(int type, string clan_name1, string clan_name2)
        {
            // 1 : _血盟が_血盟に宣戦布告しました。
            // 2 : _血盟が_血盟に降伏しました。
            // 3 : _血盟と_血盟との戦争が終結しました。
            // 4 : _血盟 贏了對_血盟 的戰爭。
            // 6 : _血盟と_血盟が同盟を結びました。
            // 7 : _血盟と_血盟との同盟関係が解除されました。
            // 8 : あなたの血盟が現在_血盟と交戦中です。

            WriteC(Opcodes.S_OPCODE_WAR);
            WriteC(type);
            WriteS(clan_name1);
            WriteS(clan_name2);
        }

        public override string Type
        {
            get
            {
                return S_WAR;
            }
        }
    }

}