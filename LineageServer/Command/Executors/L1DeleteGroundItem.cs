using LineageServer.Interfaces;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using System.Collections.Generic;
namespace LineageServer.Command.Executors
{
    /// <summary>
    /// GM指令：刪除地上道具
    /// </summary>
    class L1DeleteGroundItem : ILineageCommand
    {
        public void Execute(L1PcInstance pc, string cmdName, string arg)
        {
            foreach (GameObject l1object in Container.Instance.Resolve<IGameWorld>().Object)
            {
                if (l1object is L1ItemInstance)
                {
                    L1ItemInstance l1iteminstance = (L1ItemInstance)l1object;
                    if ((l1iteminstance.X == 0) && (l1iteminstance.Y == 0))
                    { // 地面上のアイテムではなく、誰かの所有物
                        continue;
                    }

                    IList<L1PcInstance> players = Container.Instance.Resolve<IGameWorld>().getVisiblePlayer(l1iteminstance, 0);
                    if (0 == players.Count)
                    {
                        L1Inventory groundInventory = Container.Instance.Resolve<IGameWorld>().getInventory(l1iteminstance.X, l1iteminstance.Y, l1iteminstance.MapId);
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
                        Container.Instance.Resolve<IGameWorld>().removeVisibleObject(l1iteminstance);
                        Container.Instance.Resolve<IGameWorld>().removeObject(l1iteminstance);
                    }
                }
            }
            Container.Instance.Resolve<IGameWorld>().broadcastServerMessage("地上的垃圾被GM清除了。");
        }
    }

}