using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;

namespace LineageServer.Server.Model
{
    class L1Awake
    {
        private L1Awake()
        {
        }

        public static void start(L1PcInstance pc, int skillId)
        {
            if (skillId == pc.AwakeSkillId)
            { // 再次咏唱時解除覺醒狀態
                stop(pc);
            }
            else if (pc.AwakeSkillId != 0)
            { // 無法與其他覺醒狀態並存
                return;
            }
            else
            {
                if (skillId == L1SkillId.AWAKEN_ANTHARAS)
                { // 覺醒：安塔瑞斯
                    pc.addMaxHp(127);
                    pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                    if (pc.InParty)
                    { // 組隊中
                        pc.Party.updateMiniHP(pc);
                    }
                    pc.addAc(-12);
                    pc.sendPackets(new S_OwnCharStatus2(pc, 0));
                }
                else if (skillId == L1SkillId.AWAKEN_FAFURION)
                { // 覺醒：法力昂
                    pc.addMr(30);
                    pc.sendPackets(new S_SPMR(pc));
                    pc.addWind(30);
                    pc.addWater(30);
                    pc.addFire(30);
                    pc.addEarth(30);
                    pc.sendPackets(new S_OwnCharAttrDef(pc));
                }
                else if (skillId == L1SkillId.AWAKEN_VALAKAS)
                { // 覺醒：巴拉卡斯
                    pc.addStr(5);
                    pc.addCon(5);
                    pc.addDex(5);
                    pc.addCha(5);
                    pc.addInt(5);
                    pc.addWis(5);
                    pc.sendPackets(new S_OwnCharStatus2(pc, 0));
                }
                pc.AwakeSkillId = skillId;
                doPoly(pc);
                pc.startMpReductionByAwake();
            }
        }

        public static void stop(L1PcInstance pc)
        {
            int skillId = pc.AwakeSkillId;
            if (skillId == L1SkillId.AWAKEN_ANTHARAS)
            { // 覺醒：安塔瑞斯
                pc.addMaxHp(-127);
                pc.sendPackets(new S_HPUpdate(pc.CurrentHp, pc.MaxHp));
                if (pc.InParty)
                { // パーティー中
                    pc.Party.updateMiniHP(pc);
                }
                pc.addAc(12);
                pc.sendPackets(new S_OwnCharAttrDef(pc));
            }
            else if (skillId == L1SkillId.AWAKEN_FAFURION)
            { // 覺醒：法力昂
                pc.addMr(-30);
                pc.addWind(-30);
                pc.addWater(-30);
                pc.addFire(-30);
                pc.addEarth(-30);
                pc.sendPackets(new S_SPMR(pc));
                pc.sendPackets(new S_OwnCharAttrDef(pc));
            }
            else if (skillId == L1SkillId.AWAKEN_VALAKAS)
            { // 覺醒：巴拉卡斯
                pc.addStr(-5);
                pc.addCon(-5);
                pc.addDex(-5);
                pc.addCha(-5);
                pc.addInt(-5);
                pc.addWis(-5);
                pc.sendPackets(new S_OwnCharStatus2(pc, 0));
            }
            pc.AwakeSkillId = 0;
            undoPoly(pc);
            pc.stopMpReductionByAwake();
        }

        // 變身
        public static void doPoly(L1PcInstance pc)
        {
            int polyId = 6894;
            if (pc.hasSkillEffect(L1SkillId.SHAPE_CHANGE))
            {
                pc.killSkillEffectTimer(L1SkillId.SHAPE_CHANGE);
            }
            L1ItemInstance weapon = pc.Weapon;
            bool weaponTakeoff = (weapon != null && !L1PolyMorph.isEquipableWeapon(polyId, weapon.Item.Type));
            if (weaponTakeoff)
            { // 解除武器時
                pc.CurrentWeapon = 0;
            }
            pc.TempCharGfx = polyId;
            pc.sendPackets(new S_ChangeShape(pc.Id, polyId, pc.CurrentWeapon));
            if (pc.GmInvis)
            { // GM隱身
            }
            else if (pc.Invisble)
            { // 一般隱身
                pc.broadcastPacketForFindInvis(new S_ChangeShape(pc.Id, polyId, pc.CurrentWeapon), true);
            }
            else
            {
                pc.broadcastPacket(new S_ChangeShape(pc.Id, polyId, pc.CurrentWeapon));
            }
            (pc.Inventory as L1PcInventory).takeoffEquip(polyId); // 是否將裝備的武器強制解除。
        }

        // 解除變身
        public static void undoPoly(L1PcInstance pc)
        {
            int classId = pc.ClassId;
            pc.TempCharGfx = classId;
            if (!pc.Dead)
            {
                pc.sendPackets(new S_ChangeShape(pc.Id, classId, pc.CurrentWeapon));
                pc.broadcastPacket(new S_ChangeShape(pc.Id, classId, pc.CurrentWeapon));
            }
        }

    }

}