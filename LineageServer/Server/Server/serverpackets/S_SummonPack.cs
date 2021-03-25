using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.serverpackets
{
    class S_SummonPack : ServerBasePacket
    {

        private const string S_SUMMONPACK = "[S] S_SummonPack";

        private const int STATUS_POISON = 1;

        private byte[] _byte = null;

        public S_SummonPack(L1SummonInstance pet, L1PcInstance pc)
        {
            buildPacket(pet, pc, true);
        }

        public S_SummonPack(L1SummonInstance pet, L1PcInstance pc, bool isCheckMaster)
        {
            buildPacket(pet, pc, isCheckMaster);
        }

        private void buildPacket(L1SummonInstance pet, L1PcInstance pc, bool isCheckMaster)
        {
            writeC(Opcodes.S_OPCODE_CHARPACK);
            writeH(pet.X);
            writeH(pet.Y);
            writeD(pet.Id);
            writeH(pet.GfxId); // SpriteID in List.spr
            writeC(pet.Status); // Modes in List.spr
            writeC(pet.Heading);
            writeC(pet.ChaLightSize); // (Bright) - 0~15
            writeC(pet.MoveSpeed); // スピード - 0:normal, 1:fast, 2:slow
            writeD(0);
            writeH(0);
            writeS(pet.NameId);
            writeS(pet.Title);
            int status = 0;
            if (pet.Poison != null)
            { // 毒状態
                if (pet.Poison.EffectId == 1)
                {
                    status |= STATUS_POISON;
                }
            }
            writeC(status);
            writeD(0);
            writeS(null);
            if (isCheckMaster && pet.ExsistMaster)
            {
                writeS(pet.Master.Name);
            }
            else
            {
                writeS("");
            }
            writeC(0); // ??
                       // HPのパーセント
            if ((pet.Master != null) && (pet.Master.Id == pc.Id))
            {
                int percent = pet.MaxHp != 0 ? 100 * pet.CurrentHp / pet.MaxHp : 100;
                writeC(percent);
            }
            else
            {
                writeC(0xFF);
            }
            writeC(0);
            writeC(pet.Level); // PC = 0, Mon = Lv
            writeC(0);
            writeC(0xFF);
            writeC(0xFF);
        }

        public override string Type
        {
            get
            {
                return S_SUMMONPACK;
            }
        }

    }

}