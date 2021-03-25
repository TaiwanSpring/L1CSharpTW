using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.serverpackets
{
	class S_PetPack : ServerBasePacket
	{

		private const string S_PET_PACK = "[S] S_PetPack";

		private const int STATUS_POISON = 1;

		private byte[] _byte = null;

		public S_PetPack(L1PetInstance pet, L1PcInstance pc)
		{
			buildPacket(pet, pc);
		}

		private void buildPacket(L1PetInstance pet, L1PcInstance pc)
		{
			writeC(Opcodes.S_OPCODE_CHARPACK);
			writeH(pet.X);
			writeH(pet.Y);
			writeD(pet.Id);
			writeH(pet.GfxId); // SpriteID in List.spr
			writeC(pet.Status); // Modes in List.spr
			writeC(pet.Heading);
			writeC(pet.ChaLightSize); // (Bright) - 0~15
			writeC(pet.MoveSpeed); // スピード - 0:normal, 1:fast,
			// 2:slow
			writeExp(pet.Exp);
			writeH(pet.TempLawful);
			writeS(pet.Name);
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
			writeD(0); // ??
			writeS(null); // ??
			writeS(pet.Master != null ? pet.Master.Name : "");
			writeC(0); // ??
			// HPのパーセント
			if ((pet.Master != null) && (pet.Master.Id == pc.Id))
			{
				writeC(100 * pet.CurrentHp / pet.MaxHp);
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
				return S_PET_PACK;
			}
		}

	}

}