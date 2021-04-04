using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using System;
namespace LineageServer.Server.Model.item.Action
{
    class FurnitureItem
    {
        private static ILogger _log = Logger.GetLogger(nameof(FurnitureItem));

        public static void useFurnitureItem(L1PcInstance pc, int itemId, int itemObjectId)
        {

            L1FurnitureItem furniture_item = FurnitureItemTable.Instance.getTemplate((itemId));

            bool isAppear = true;

            if (furniture_item == null)
            {
                pc.sendPackets(new S_ServerMessage(79)); // \f1沒有任何事情發生。
                return;
            }

            if (!L1HouseLocation.isInHouse(pc.X, pc.Y, pc.MapId))
            {
                pc.sendPackets(new S_ServerMessage(563)); // \f1ここでは使えません。
                return;
            }
            L1FurnitureInstance find = null;
            foreach (GameObject l1object in Container.Instance.Resolve<IGameWorld>().Object)
            {
                if (l1object is L1FurnitureInstance furniture)
                {
                    if (furniture.ItemObjId == itemObjectId)
                    { // 既に引き出している家具
                        isAppear = false;
                        find = furniture;
                        break;
                    }
                }
            }

            if (isAppear)
            {
                if ((pc.Heading != 0) && (pc.Heading != 2))
                {
                    return;
                }
                int npcId = furniture_item.FurnitureNpcId;
                try
                {
                    L1Npc l1npc = Container.Instance.Resolve<INpcController>().getTemplate(npcId);
                    if (l1npc != null)
                    {
                        try
                        {
                            if (L1NpcInstance.Factory(l1npc) is L1FurnitureInstance furniture)
                            {
                                furniture.Id = Container.Instance.Resolve<IIdFactory>().nextId();
                                furniture.MapId = pc.MapId;
                                if (pc.Heading == 0)
                                {
                                    furniture.X = pc.X;
                                    furniture.Y = pc.Y - 1;
                                }
                                else if (pc.Heading == 2)
                                {
                                    furniture.X = pc.X + 1;
                                    furniture.Y = pc.Y;
                                }
                                furniture.HomeX = furniture.X;
                                furniture.HomeY = furniture.Y;
                                furniture.Heading = 0;
                                furniture.ItemObjId = itemObjectId;

                                Container.Instance.Resolve<IGameWorld>().storeObject(furniture);
                                Container.Instance.Resolve<IGameWorld>().addVisibleObject(furniture);
                                FurnitureSpawnTable.Instance.insertFurniture(furniture);
                            }
                        }
                        catch (Exception e)
                        {
                            _log.Error(e);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            else
            {
                if (find != null)
                {
                    find.deleteMe();
                    FurnitureSpawnTable.Instance.deleteFurniture(find);
                }
            }
        }

        // 傢俱移除魔杖
        public static void useFurnitureRemovalWand(L1PcInstance pc, int targetId, L1ItemInstance item)
        {
            S_AttackPacket s_attackPacket = new S_AttackPacket(pc, 0, ActionCodes.ACTION_Wand);
            pc.sendPackets(s_attackPacket);
            pc.broadcastPacket(s_attackPacket);
            int chargeCount = item.ChargeCount;
            if (chargeCount <= 0)
            {
                return;
            }

            GameObject target = Container.Instance.Resolve<IGameWorld>().findObject(targetId);
            if ((target != null) && (target is L1FurnitureInstance))
            {
                L1FurnitureInstance furniture = (L1FurnitureInstance)target;
                furniture.deleteMe();
                FurnitureSpawnTable.Instance.deleteFurniture(furniture);
                item.ChargeCount = item.ChargeCount - 1;
                pc.Inventory.updateItem(item, L1PcInventory.COL_CHARGE_COUNT);
            }
        }

    }

}