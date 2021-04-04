using LineageServer.Interfaces;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;

namespace LineageServer.Server.Model.item.Action
{
    class MagicDoll
    {

        public static void useMagicDoll(L1PcInstance pc, int itemId, int itemObjectId)
        {
            L1MagicDoll magic_doll = MagicDollTable.Instance.getTemplate((itemId));
            if (magic_doll != null)
            {
                bool isAppear = true;
                L1DollInstance doll = null;

                foreach (L1DollInstance curdoll in pc.DollList.Values)
                {
                    doll = curdoll;
                    if (doll.ItemObjId == itemObjectId)
                    {
                        isAppear = false;
                        break;
                    }
                }

                if (isAppear)
                {
                    if (!pc.Inventory.checkItem(41246, 50))
                    {
                        pc.sendPackets(new S_ServerMessage(337, "$5240")); // 魔法結晶體不足
                        return;
                    }
                    if (pc.DollList.Count >= Config.MAX_DOLL_COUNT)
                    {
                        pc.sendPackets(new S_ServerMessage(79)); // 沒有任何事情發生
                        return;
                    }
                    int npcId = magic_doll.DollId;

                    L1Npc template = Container.Instance.Resolve<INpcController>().getTemplate(npcId);
                    doll = new L1DollInstance(template, pc, itemId, itemObjectId);
                    pc.sendPackets(new S_SkillSound(doll.Id, 5935));
                    pc.broadcastPacket(new S_SkillSound(doll.Id, 5935));
                    pc.sendPackets(new S_SkillIconGFX(56, 1800));
                    pc.sendPackets(new S_OwnCharStatus(pc));
                    pc.Inventory.consumeItem(41246, 50);
                }
                else
                {
                    pc.sendPackets(new S_SkillSound(doll.Id, 5936));
                    pc.broadcastPacket(new S_SkillSound(doll.Id, 5936));
                    doll.deleteDoll();
                    pc.sendPackets(new S_SkillIconGFX(56, 0));
                    pc.sendPackets(new S_OwnCharStatus(pc));
                }
            }
        }

    }

}