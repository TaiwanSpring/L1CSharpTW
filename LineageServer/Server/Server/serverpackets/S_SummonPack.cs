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
            WriteC(Opcodes.S_OPCODE_CHARPACK);
            WriteH(pet.X);
            WriteH(pet.Y);
            WriteD(pet.Id);
            WriteH(pet.GfxId); // SpriteID in List.spr
            WriteC(pet.Status); // Modes in List.spr
            WriteC(pet.Heading);
            WriteC(pet.ChaLightSize); // (Bright) - 0~15
            WriteC(pet.MoveSpeed); // スピード - 0:normal, 1:fast, 2:slow
            WriteD(0);
            WriteH(0);
            WriteS(pet.NameId);
            WriteS(pet.Title);
            int status = 0;
            if (pet.Poison != null)
            { // 毒状態
                if (pet.Poison.EffectId == 1)
                {
                    status |= STATUS_POISON;
                }
            }
            WriteC(status);
            WriteD(0);
            WriteS(null);
            if (isCheckMaster && pet.ExsistMaster)
            {
                WriteS(pet.Master.Name);
            }
            else
            {
                WriteS("");
            }
            WriteC(0); // ??
                       // HPのパーセント
            if ((pet.Master != null) && (pet.Master.Id == pc.Id))
            {
                int percent = pet.MaxHp != 0 ? 100 * pet.CurrentHp / pet.MaxHp : 100;
                WriteC(percent);
            }
            else
            {
                WriteC(0xFF);
            }
            WriteC(0);
            WriteC(pet.Level); // PC = 0, Mon = Lv
            WriteC(0);
            WriteC(0xFF);
            WriteC(0xFF);
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