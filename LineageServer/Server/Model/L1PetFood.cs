using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;

namespace LineageServer.Server.Model
{
    class L1PetFood : TimerTask
	{
		/// <summary>
		/// 寵物飽食度計時器 </summary>
		public L1PetFood(L1PetInstance pet, int itemObj)
		{
			_pet = pet;
			_l1pet = PetTable.Instance.getTemplate(itemObj);
		}

		public override void run()
		{
			if (_pet != null && !_pet.Dead)
			{
				_food = _pet.get_food() - 2;
				if (_food <= 0)
				{
					_pet.set_food(0);
					_pet.CurrentPetStatus = 3;

					// 非常餓時提醒主人
					L1PetType type = PetTypeTable.Instance.get(_pet.NpcTemplate.get_npcId());
					int id = type.DefyMessageId;
					if (id != 0)
					{
						_pet.broadcastPacket(new S_NpcChatPacket(_pet, "$" + id, 0));
					}
				}
				else
				{
					_pet.set_food(_food);
				}
				if (_l1pet != null)
				{
					// 紀錄寵物飽食度
					_l1pet.set_food(_pet.get_food());
					PetTable.Instance.storePetFood(_l1pet);
				}
			}
			else
			{
				cancel();
			}
		}

		private readonly L1PetInstance _pet;
		private int _food = 0;
		private L1Pet _l1pet;
	}

}