using System.Collections.Generic;

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
namespace LineageServer.Server.Server.command.executor
{

	using FurnitureSpawnTable = LineageServer.Server.Server.datatables.FurnitureSpawnTable;
	using LetterTable = LineageServer.Server.Server.datatables.LetterTable;
	using PetTable = LineageServer.Server.Server.datatables.PetTable;
	using L1Inventory = LineageServer.Server.Server.Model.L1Inventory;
	using L1Object = LineageServer.Server.Server.Model.L1Object;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1FurnitureInstance = LineageServer.Server.Server.Model.Instance.L1FurnitureInstance;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;

	/// <summary>
	/// GM指令：刪除地上道具
	/// </summary>
	public class L1DeleteGroundItem : L1CommandExecutor
	{
		private L1DeleteGroundItem()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1DeleteGroundItem();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			foreach (L1Object l1object in L1World.Instance.Object)
			{
				if (l1object is L1ItemInstance)
				{
					L1ItemInstance l1iteminstance = (L1ItemInstance) l1object;
					if ((l1iteminstance.X == 0) && (l1iteminstance.Y == 0))
					{ // 地面上のアイテムではなく、誰かの所有物
						continue;
					}

					IList<L1PcInstance> players = L1World.Instance.getVisiblePlayer(l1iteminstance, 0);
					if (0 == players.Count)
					{
						L1Inventory groundInventory = L1World.Instance.getInventory(l1iteminstance.X, l1iteminstance.Y, l1iteminstance.MapId);
						int itemId = l1iteminstance.Item.ItemId;
						if ((itemId == 40314) || (itemId == 40316))
						{ // ペットのアミュレット
							PetTable.Instance.deletePet(l1iteminstance.Id);
						}
						else if ((itemId >= 49016) && (itemId <= 49025))
						{ // 便箋
							LetterTable lettertable = new LetterTable();
							lettertable.deleteLetter(l1iteminstance.Id);
						}
						else if ((itemId >= 41383) && (itemId <= 41400))
						{ // 家具
							if (l1object is L1FurnitureInstance)
							{
								L1FurnitureInstance furniture = (L1FurnitureInstance) l1object;
								if (furniture.ItemObjId == l1iteminstance.Id)
								{ // 既に引き出している家具
									FurnitureSpawnTable.Instance.deleteFurniture(furniture);
								}
							}
						}
						groundInventory.deleteItem(l1iteminstance);
						L1World.Instance.removeVisibleObject(l1iteminstance);
						L1World.Instance.removeObject(l1iteminstance);
					}
				}
			}
			L1World.Instance.broadcastServerMessage("地上的垃圾被GM清除了。");
		}
	}

}