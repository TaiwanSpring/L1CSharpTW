using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;

namespace LineageServer.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來選擇目標的封包
	/// </summary>
	class C_SelectTarget : ClientBasePacket
	{

		private const string C_SELECT_TARGET = "[C] C_SelectTarget";

		public C_SelectTarget(byte[] abyte0, ClientThread clientthread) : base(abyte0)
		{

			int petId = ReadD();
			ReadC();
			int targetId = ReadD();

			L1PetInstance pet = (L1PetInstance) Container.Instance.Resolve<IGameWorld>().findObject(petId);
			L1Character target = (L1Character) Container.Instance.Resolve<IGameWorld>().findObject(targetId);

			if ((pet != null) && (target != null))
			{
				// 目標為玩家
				if (target is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) target;
					// 目標在安區、攻擊者在安區、NOPVP
					if ((pc.ZoneType == 1) || (pet.ZoneType == 1) || (pc.checkNonPvP(pc, pet)))
					{
						// 寵物主人
						if (pet.Master is L1PcInstance)
						{
							L1PcInstance petMaster = (L1PcInstance) pet.Master;
							petMaster.sendPackets(new S_ServerMessage(328)); // 請選擇正確的對象。
						}
						return;
					}
				}
				// 目標為寵物
				else if (target is L1PetInstance)
				{
					L1PetInstance targetPet = (L1PetInstance) target;
					// 目標在安區、攻擊者在安區
					if ((targetPet.ZoneType == 1) || (pet.ZoneType == 1))
					{
						// 寵物主人
						if (pet.Master is L1PcInstance)
						{
							L1PcInstance petMaster = (L1PcInstance) pet.Master;
							petMaster.sendPackets(new S_ServerMessage(328)); // 請選擇正確的對象。
						}
						return;
					}
				}
				// 目標為召喚怪
				else if (target is L1SummonInstance)
				{
					L1SummonInstance targetSummon = (L1SummonInstance) target;
					// 目標在安區、攻擊者在安區
					if ((targetSummon.ZoneType == 1) || (pet.ZoneType == 1))
					{
						// 寵物主人
						if (pet.Master is L1PcInstance)
						{
							L1PcInstance petMaster = (L1PcInstance) pet.Master;
							petMaster.sendPackets(new S_ServerMessage(328)); // 請選擇正確的對象。
						}
						return;
					}
				}
				// 目標為怪物
				else if (target is L1MonsterInstance)
				{
					L1MonsterInstance mob = (L1MonsterInstance) target;
					// 特定狀態下才可攻擊
					if (pet.Master.isAttackMiss(pet.Master, mob.NpcId))
					{
						if (pet.Master is L1PcInstance)
						{
							L1PcInstance petMaster = (L1PcInstance) pet.Master;
							petMaster.sendPackets(new S_ServerMessage(328)); // 請選擇正確的對象。
						}
						return;
					}
				}
				pet.MasterTarget = target;
			}
		}

		public override string Type
		{
			get
			{
				return C_SELECT_TARGET;
			}
		}
	}

}