using LineageServer.Interfaces;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.skill;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;

namespace LineageServer.Server.Model.Instance
{
    class L1FieldObjectInstance : L1NpcInstance
    {

        public L1FieldObjectInstance(L1Npc template) : base(template)
        {
        }

        public override void onAction(L1PcInstance pc)
        {
            if (NpcTemplate.get_npcId() == 81171)
            { // おばけ屋敷のゴールの炎
                if (L1HauntedHouse.Instance.HauntedHouseStatus == L1HauntedHouse.STATUS_PLAYING)
                {
                    int winnersCount = L1HauntedHouse.Instance.WinnersCount;
                    int goalCount = L1HauntedHouse.Instance.GoalCount;
                    if (winnersCount == goalCount + 1)
                    {
                        L1ItemInstance item = ItemTable.Instance.createItem(49280); // 勇者のパンプキン袋(銅)
                        int count = 1;
                        if (item != null)
                        {
                            if (pc.Inventory.checkAddItem(item, count) == L1Inventory.OK)
                            {
                                item.Count = count;
                                pc.Inventory.storeItem(item);
                                pc.sendPackets(new S_ServerMessage(403, item.LogName)); // %0を手に入れました。
                            }
                        }
                        L1HauntedHouse.Instance.endHauntedHouse();
                    }
                    else if (winnersCount > goalCount + 1)
                    {
                        L1HauntedHouse.Instance.GoalCount = goalCount + 1;
                        L1HauntedHouse.Instance.removeMember(pc);
                        L1ItemInstance item = null;
                        if (winnersCount == 3)
                        {
                            if (goalCount == 1)
                            {
                                item = ItemTable.Instance.createItem(49278); // 勇者のパンプキン袋(金)
                            }
                            else if (goalCount == 2)
                            {
                                item = ItemTable.Instance.createItem(49279); // 勇者のパンプキン袋(銀)
                            }
                        }
                        else if (winnersCount == 2)
                        {
                            item = ItemTable.Instance.createItem(49279); // 勇者のパンプキン袋(銀)
                        }
                        int count = 1;
                        if (item != null)
                        {
                            if (pc.Inventory.checkAddItem(item, count) == L1Inventory.OK)
                            {
                                item.Count = count;
                                pc.Inventory.storeItem(item);
                                pc.sendPackets(new S_ServerMessage(403, item.LogName)); // %0を手に入れました。
                            }
                        }
                        L1SkillUse l1skilluse = new L1SkillUse();
                        l1skilluse.handleCommands(pc, L1SkillId.CANCELLATION, pc.Id, pc.X, pc.Y, null, 0, L1SkillUse.TYPE_LOGIN);
                        L1Teleport.teleport(pc, 32624, 32813, (short)4, 5, true);
                    }
                }
            }
        }

        public override void deleteMe()
        {
            _destroyed = true;
            if (Inventory != null)
            {
                Inventory.clearItems();
            }
            Container.Instance.Resolve<IGameWorld>().removeVisibleObject(this);
            Container.Instance.Resolve<IGameWorld>().removeObject(this);
            foreach (L1PcInstance pc in Container.Instance.Resolve<IGameWorld>().getRecognizePlayer(this))
            {
                pc.removeKnownObject(this);
                pc.sendPackets(new S_RemoveObject(this));
            }
            removeAllKnownObjects();
        }
    }

}