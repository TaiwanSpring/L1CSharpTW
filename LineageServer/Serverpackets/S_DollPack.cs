using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
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
            WriteC(Opcodes.S_OPCODE_CHARPACK);
            WriteH(doll.X);
            WriteH(doll.Y);
            WriteD(doll.Id);
            WriteH(doll.GfxId); // SpriteID in List.spr
            WriteC(doll.Status); // Modes in List.spr
            WriteC(doll.Heading);
            WriteC(0); // (Bright) - 0~15
            WriteC(doll.MoveSpeed); // 1段加速狀態
            WriteD(0);
            WriteH(0);
            WriteS(doll.NameId);
            WriteS(doll.Title);
            WriteC((doll.BraveSpeed * 16)); // 狀態、2段加速狀態
            WriteD(0); // ??
            WriteS(null); // ??
            WriteS(doll.Master != null ? doll.Master.Name : "");
            WriteC(0); // ??
            WriteC(0xFF);
            WriteC(0);
            WriteC(doll.Level); // PC = 0, Mon = Lv
            WriteC(0);
            WriteC(0xFF);
            WriteC(0xFF);
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