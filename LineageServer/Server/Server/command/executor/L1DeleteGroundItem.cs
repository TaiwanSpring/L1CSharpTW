using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using System.Collections.Generic;
namespace LineageServer.Server.Server.Command.Executor
{
    /// <summary>
    /// GM指令：刪除地上道具
    /// </summary>
    class L1DeleteGroundItem : IL1CommandExecutor
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            foreach (L1Object l1object in L1World.Instance.Object)
            {
                if (l1object is L1ItemInstance)
                {
                    L1ItemInstance l1iteminstance = (L1ItemInstance)l1object;
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
                                L1FurnitureInstance furniture = (L1FurnitureInstance)l1object;
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