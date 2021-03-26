namespace LineageServer.Server.Server.serverpackets
{
    class S_Ability : ServerBasePacket
    {

        private const string S_ABILITY = "[S] S_Ability";
        /// <param name="type">
        /// 1, 傳送控制戒指
        /// 2, 變形控制戒指
        /// 3, 全白天
        /// 4, 夜視功能
        /// 5, 召喚控制戒指 </param>
        /// <param name="equipped"> 是否裝備 </param>
        public S_Ability(int type, bool equipped)
        {
            buildPacket(type, equipped);
        }

        private void buildPacket(int type, bool equipped)
        {
            WriteC(Opcodes.S_OPCODE_ABILITY);
            WriteC(type);
            if (equipped)
            {
                WriteC(0x01);
            }
            else
            {
                WriteC(0x00);
            }
            WriteC(0x00);
            WriteH(0x0000);
        }

        public override string Type
        {
            get
            {
                return S_ABILITY;
            }
        }
    }

}