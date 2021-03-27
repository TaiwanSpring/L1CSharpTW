/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Model
{

	using PetTable = LineageServer.Server.DataSources.PetTable;
	using PetTypeTable = LineageServer.Server.DataSources.PetTypeTable;
	using L1PetInstance = LineageServer.Server.Model.Instance.L1PetInstance;
	using S_NpcChatPacket = LineageServer.Serverpackets.S_NpcChatPacket;
	using L1Pet = LineageServer.Server.Templates.L1Pet;
	using L1PetType = LineageServer.Server.Templates.L1PetType;

	public class L1PetFood : TimerTask
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