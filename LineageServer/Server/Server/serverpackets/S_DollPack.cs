using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.serverpackets
{
    class S_DollPack : ServerBasePacket
    {

        private const string S_DOLLPACK = "[S] S_DollPack";
        public S_DollPack(L1DollInstance doll)
        {
            /*
			 * int addbyte = 0; int addbyte1 = 1; int addbyte2 = 13; int setting =
			 * 4;
			 */
            writeC(Opcodes.S_OPCODE_CHARPACK);
            writeH(doll.X);
            writeH(doll.Y);
            writeD(doll.Id);
            writeH(doll.GfxId); // SpriteID in List.spr
            writeC(doll.Status); // Modes in List.spr
            writeC(doll.Heading);
            writeC(0); // (Bright) - 0~15
            writeC(doll.MoveSpeed); // 1段加速狀態
            writeD(0);
            writeH(0);
            writeS(doll.NameId);
            writeS(doll.Title);
            writeC((doll.BraveSpeed * 16)); // 狀態、2段加速狀態
            writeD(0); // ??
            writeS(null); // ??
            writeS(doll.Master != null ? doll.Master.Name : "");
            writeC(0); // ??
            writeC(0xFF);
            writeC(0);
            writeC(doll.Level); // PC = 0, Mon = Lv
            writeC(0);
            writeC(0xFF);
            writeC(0xFF);
        }

        public override string Type
        {
            get
            {
                return S_DOLLPACK;
            }
        }

    }

}