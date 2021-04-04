using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_InitialAbilityGrowth : ServerBasePacket
    {
        public S_InitialAbilityGrowth(L1PcInstance pc)
        {

            int Str = pc.OriginalStr; // 力量
            int Dex = pc.OriginalDex; // 敏捷
            int Con = pc.OriginalCon; // 體質
            int Wis = pc.OriginalWis; // 精神
            int Cha = pc.OriginalCha; // 魅力
            int Int = pc.OriginalInt; // 智力
            int[] growth = new int[6];

            // 王族
            if (pc.Crown)
            {
                int[] Initial = new int[] { 13, 10, 10, 11, 13, 10 };
                growth[0] = Str - Initial[0];
                growth[1] = Dex - Initial[1];
                growth[2] = Con - Initial[2];
                growth[3] = Wis - Initial[3];
                growth[4] = Cha - Initial[4];
                growth[5] = Int - Initial[5];
            }
            // 法師
            if (pc.Wizard)
            {
                int[] Initial = new int[] { 8, 7, 12, 12, 8, 12 };
                growth[0] = Str - Initial[0];
                growth[1] = Dex - Initial[1];
                growth[2] = Con - Initial[2];
                growth[3] = Wis - Initial[3];
                growth[4] = Cha - Initial[4];
                growth[5] = Int - Initial[5];
            }
            // 騎士
            if (pc.Knight)
            {
                int[] Initial = new int[] { 16, 12, 14, 9, 12, 8 };
                growth[0] = Str - Initial[0];
                growth[1] = Dex - Initial[1];
                growth[2] = Con - Initial[2];
                growth[3] = Wis - Initial[3];
                growth[4] = Cha - Initial[4];
                growth[5] = Int - Initial[5];
            }
            // 妖精
            if (pc.Elf)
            {
                int[] Initial = new int[] { 11, 12, 12, 12, 9, 12 };
                growth[0] = Str - Initial[0];
                growth[1] = Dex - Initial[1];
                growth[2] = Con - Initial[2];
                growth[3] = Wis - Initial[3];
                growth[4] = Cha - Initial[4];
                growth[5] = Int - Initial[5];
            }
            // 黑妖
            if (pc.Darkelf)
            {
                int[] Initial = new int[] { 12, 15, 8, 10, 9, 11 };
                growth[0] = Str - Initial[0];
                growth[1] = Dex - Initial[1];
                growth[2] = Con - Initial[2];
                growth[3] = Wis - Initial[3];
                growth[4] = Cha - Initial[4];
                growth[5] = Int - Initial[5];
            }
            // 龍騎士
            if (pc.DragonKnight)
            {
                int[] Initial = new int[] { 13, 11, 14, 12, 8, 11 };
                growth[0] = Str - Initial[0];
                growth[1] = Dex - Initial[1];
                growth[2] = Con - Initial[2];
                growth[3] = Wis - Initial[3];
                growth[4] = Cha - Initial[4];
                growth[5] = Int - Initial[5];
            }
            // 幻術師
            if (pc.Illusionist)
            {
                int[] Initial = new int[] { 11, 10, 12, 12, 8, 12 };
                growth[0] = Str - Initial[0];
                growth[1] = Dex - Initial[1];
                growth[2] = Con - Initial[2];
                growth[3] = Wis - Initial[3];
                growth[4] = Cha - Initial[4];
                growth[5] = Int - Initial[5];
            }

            buildPacket(pc, growth[0], growth[1], growth[2], growth[3], growth[4], growth[5]);
        }

        /// 
        /// <param name="pc">
        ///            腳色 </param>
        /// <param name="Str">
        ///            力量 </param>
        /// <param name="Dex">
        ///            敏捷 </param>
        /// <param name="Con">
        ///            體質 </param>
        /// <param name="Wis">
        ///            精神 </param>
        /// <param name="Cha">
        ///            魅力 </param>
        /// <param name="Int">
        ///            智力 </param>
        private void buildPacket(L1PcInstance pc, int Str, int Dex, int Con, int Wis, int Cha, int Int)
        {
            int Write1 = (Int * 16) + Str;
            int Write2 = (Dex * 16) + Wis;
            int Write3 = (Cha * 16) + Con;
            WriteC(Opcodes.S_OPCODE_CHARRESET);
            WriteC(0x04);
            WriteC(Write1); // 智力&力量
            WriteC(Write2); // 敏捷&精神
            WriteC(Write3); // 魅力&體質
            WriteC(0x00);
        }
    }

}