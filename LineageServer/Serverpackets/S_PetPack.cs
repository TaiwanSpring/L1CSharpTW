using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
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
			WriteC(Opcodes.S_OPCODE_CHARPACK);
			WriteH(pet.X);
			WriteH(pet.Y);
			WriteD(pet.Id);
			WriteH(pet.GfxId); // SpriteID in List.spr
			WriteC(pet.Status); // Modes in List.spr
			WriteC(pet.Heading);
			WriteC(pet.ChaLightSize); // (Bright) - 0~15
			WriteC(pet.MoveSpeed); // スピード - 0:normal, 1:fast,
			// 2:slow
			WriteExp(pet.Exp);
			WriteH(pet.TempLawful);
			WriteS(pet.Name);
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
			WriteD(0); // ??
			WriteS(null); // ??
			WriteS(pet.Master != null ? pet.Master.Name : "");
			WriteC(0); // ??
			// HPのパーセント
			if ((pet.Master != null) && (pet.Master.Id == pc.Id))
			{
				WriteC(100 * pet.CurrentHp / pet.MaxHp);
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
				return S_PET_PACK;
			}
		}

	}

}