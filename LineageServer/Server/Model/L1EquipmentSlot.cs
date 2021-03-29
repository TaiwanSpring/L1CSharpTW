using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Server.Model
{
    class L1EquipmentSlot
    {
        private L1PcInstance _owner;

        /// <summary>
        /// 効果中のセットアイテム
        /// </summary>
        private IList<L1ArmorSet> _currentArmorSet;

        private L1ItemInstance _weapon;

        private IList<L1ItemInstance> _armors;

        public L1EquipmentSlot(L1PcInstance owner)
        {
            _owner = owner;
            _armors = ListFactory.NewList<L1ItemInstance>();
            _currentArmorSet = ListFactory.NewList<L1ArmorSet>();
        }

        private L1ItemInstance Weapon
        {
            set
            {
                _owner.Weapon = value;
                _owner.CurrentWeapon = value.Item.Type1;
                value.startEquipmentTimer(_owner);
                _weapon = value;
            }
            get
            {
                return _weapon;
            }
        }


        private L1ItemInstance Armor
        {
            set
            {
                L1Item item = value.Item;
                int itemId = value.Item.ItemId;
                // 飾品不加防判斷
                if (value.Item.Type2 == 2 && value.Item.Type >= 8 && value.Item.Type <= 12)
                {
                    _owner.addAc(item.get_ac() - value.AcByMagic);
                }
                else
                {
                    _owner.addAc(item.get_ac() - value.EnchantLevel - value.AcByMagic);
                }
                _owner.addDamageReductionByArmor(item.DamageReduction);
                _owner.addWeightReduction(item.WeightReduction);
                _owner.addHitModifierByArmor(item.HitModifierByArmor);
                _owner.addDmgModifierByArmor(item.DmgModifierByArmor);
                _owner.addBowHitModifierByArmor(item.BowHitModifierByArmor);
                _owner.addBowDmgModifierByArmor(item.BowDmgModifierByArmor);
                _owner.addRegistStun(item.get_regist_stun());
                _owner.addRegistStone(item.get_regist_stone());
                _owner.addRegistSleep(item.get_regist_sleep());
                _owner.add_regist_freeze(item.get_regist_freeze());
                _owner.addRegistSustain(item.get_regist_sustain());
                _owner.addRegistBlind(item.get_regist_blind());
                // 飾品強化 Scroll of Enchant Accessory
                _owner.addEarth(item.get_defense_earth() + value.EarthMr);
                _owner.addWind(item.get_defense_wind() + value.WindMr);
                _owner.addWater(item.get_defense_water() + value.WaterMr);
                _owner.addFire(item.get_defense_fire() + value.FireMr);

                _armors.Add(value);

                foreach (L1ArmorSet armorSet in L1ArmorSet.AllSet)
                {
                    if (armorSet.isPartOfSet(itemId) && armorSet.isValid(_owner))
                    {
                        if ((value.Item.Type2 == 2) && (value.Item.Type == 9))
                        { // ring
                            if (!armorSet.isEquippedRingOfArmorSet(_owner))
                            {
                                armorSet.giveEffect(_owner);
                                _currentArmorSet.Add(armorSet);
                            }
                        }
                        else
                        {
                            armorSet.giveEffect(_owner);
                            _currentArmorSet.Add(armorSet);
                        }
                    }
                }

                if ((itemId == 20077) || (itemId == 20062) || (itemId == 120077))
                {
                    if (!_owner.hasSkillEffect(L1SkillId.INVISIBILITY))
                    {
                        _owner.killSkillEffectTimer(L1SkillId.BLIND_HIDING);
                        _owner.setSkillEffect(L1SkillId.INVISIBILITY, 0);
                        _owner.sendPackets(new S_Invis(_owner.Id, 1));
                        _owner.broadcastPacketForFindInvis(new S_RemoveObject(_owner), false);
                        _owner.broadcastPacket(new S_RemoveObject(_owner));
                    }
                }
                if (itemId == 20281)
                { // 變形控制戒指
                    _owner.sendPackets(new S_Ability(2, true));
                }
                else if (itemId == 20288)
                { // 傳送控制戒指
                    _owner.sendPackets(new S_Ability(1, true));
                }
                else if (itemId == 20284)
                { // 召喚控制戒指
                    _owner.sendPackets(new S_Ability(5, true));
                }
                if (itemId == 20383)
                { // 騎馬用ヘルム
                    if (value.ChargeCount != 0)
                    {
                        value.ChargeCount = value.ChargeCount - 1;
                        _owner.Inventory.updateItem(value, L1PcInventory.COL_CHARGE_COUNT);
                    }
                }
                value.startEquipmentTimer(_owner);
            }
        }

        public virtual IList<L1ItemInstance> Armors
        {
            get
            {
                return _armors;
            }
        }

        private void removeWeapon(L1ItemInstance weapon)
        {
            _owner.Weapon = null;
            _owner.CurrentWeapon = 0;
            weapon.stopEquipmentTimer(_owner);
            _weapon = null;
            if (_owner.hasSkillEffect(L1SkillId.COUNTER_BARRIER))
            {
                _owner.removeSkillEffect(L1SkillId.COUNTER_BARRIER);
            }
        }

        private void removeArmor(L1ItemInstance armor)
        {
            L1Item item = armor.Item;
            int itemId = armor.Item.ItemId;
            // 飾品不加防判斷
            if (armor.Item.Type2 == 2 && armor.Item.Type >= 8 && armor.Item.Type <= 12)
            {
                _owner.addAc(-(item.get_ac() - armor.AcByMagic));
            }
            else
            {
                _owner.addAc(-(item.get_ac() - armor.EnchantLevel - armor.AcByMagic));
            }
            _owner.addDamageReductionByArmor(-item.DamageReduction);
            _owner.addWeightReduction(-item.WeightReduction);
            _owner.addHitModifierByArmor(-item.HitModifierByArmor);
            _owner.addDmgModifierByArmor(-item.DmgModifierByArmor);
            _owner.addBowHitModifierByArmor(-item.BowHitModifierByArmor);
            _owner.addBowDmgModifierByArmor(-item.BowDmgModifierByArmor);
            _owner.addRegistStun(-item.get_regist_stun());
            _owner.addRegistStone(-item.get_regist_stone());
            _owner.addRegistSleep(-item.get_regist_sleep());
            _owner.add_regist_freeze(-item.get_regist_freeze());
            _owner.addRegistSustain(-item.get_regist_sustain());
            _owner.addRegistBlind(-item.get_regist_blind());
            // 飾品強化 Scroll of Enchant Accessory
            _owner.addEarth(-item.get_defense_earth() - armor.EarthMr);
            _owner.addWind(-item.get_defense_wind() - armor.WindMr);
            _owner.addWater(-item.get_defense_water() - armor.WaterMr);
            _owner.addFire(-item.get_defense_fire() - armor.FireMr);

            foreach (L1ArmorSet armorSet in L1ArmorSet.AllSet)
            {
                if (armorSet.isPartOfSet(itemId) && _currentArmorSet.Contains(armorSet) && !armorSet.isValid(_owner))
                {
                    armorSet.cancelEffect(_owner);
                    _currentArmorSet.Remove(armorSet);
                }
            }

            if ((itemId == 20077) || (itemId == 20062) || (itemId == 120077))
            {
                _owner.delInvis(); // インビジビリティ状態解除
            }
            if (itemId == 20281)
            { // 變形控制戒指
                _owner.sendPackets(new S_Ability(2, false));
            }
            else if (itemId == 20288)
            { // 傳送控制戒指
                _owner.sendPackets(new S_Ability(1, false));
            }
            else if (itemId == 20284)
            { // 召喚控制戒指
                _owner.sendPackets(new S_Ability(5, false));
            }
            armor.stopEquipmentTimer(_owner);

            _armors.Remove(armor);
        }

        public virtual void set(L1ItemInstance equipment)
        {
            L1Item item = equipment.Item;
            if (item.Type2 == 0)
            {
                return;
            }

            if (item.get_addhp() != 0)
            {
                _owner.addMaxHp(item.get_addhp());
            }
            if (item.get_addmp() != 0)
            {
                _owner.addMaxMp(item.get_addmp());
            }
            if (equipment.getaddHp() != 0)
            {
                _owner.addMaxHp(equipment.getaddHp());
            }
            if (equipment.getaddMp() != 0)
            {
                _owner.addMaxMp(equipment.getaddMp());
            }
            _owner.addStr(item.get_addstr());
            _owner.addCon(item.get_addcon());
            _owner.addDex(item.get_adddex());
            _owner.addInt(item.get_addint());
            _owner.addWis(item.get_addwis());
            if (item.get_addwis() != 0)
            {
                _owner.resetBaseMr();
            }
            _owner.addCha(item.get_addcha());

            int addMr = 0;
            addMr += equipment.Mr;
            if ((item.ItemId == 20236) && _owner.Elf)
            {
                addMr += 5;
            }
            if (addMr != 0)
            {
                _owner.addMr(addMr);
                _owner.sendPackets(new S_SPMR(_owner));
            }
            if (item.get_addsp() != 0 || equipment.getaddSp() != 0)
            {
                _owner.addSp(item.get_addsp() + equipment.getaddSp());
                _owner.sendPackets(new S_SPMR(_owner));
            }
            if (item.HasteItem)
            {
                _owner.addHasteItemEquipped(1);
                _owner.removeHasteSkillEffect();
                if (_owner.MoveSpeed != 1)
                {
                    _owner.MoveSpeed = 1;
                    _owner.sendPackets(new S_SkillHaste(_owner.Id, 1, -1));
                    _owner.broadcastPacket(new S_SkillHaste(_owner.Id, 1, 0));
                }
            }
            if (item.ItemId == 20383)
            { // 騎馬用ヘルム
                if (_owner.hasSkillEffect(L1SkillId.STATUS_BRAVE))
                {
                    _owner.killSkillEffectTimer(L1SkillId.STATUS_BRAVE);
                    _owner.sendPackets(new S_SkillBrave(_owner.Id, 0, 0));
                    _owner.broadcastPacket(new S_SkillBrave(_owner.Id, 0, 0));
                    _owner.BraveSpeed = 0;
                }
            }
            _owner.EquipSlot.MagicHelm = equipment;

            if (item.Type2 == 1)
            {
                Weapon = equipment;
            }
            else if (item.Type2 == 2)
            {
                Armor = equipment;
                _owner.sendPackets(new S_SPMR(_owner));
            }
        }

        public virtual void remove(L1ItemInstance equipment)
        {
            L1Item item = equipment.Item;
            if (item.Type2 == 0)
            {
                return;
            }

            if (item.get_addhp() != 0)
            {
                _owner.addMaxHp(-item.get_addhp());
            }
            if (item.get_addmp() != 0)
            {
                _owner.addMaxMp(-item.get_addmp());
            }
            if (equipment.getaddHp() != 0)
            {
                _owner.addMaxHp(-equipment.getaddHp());
            }
            if (equipment.getaddMp() != 0)
            {
                _owner.addMaxMp(-equipment.getaddMp());
            }
            _owner.addStr((sbyte)-item.get_addstr());
            _owner.addCon((sbyte)-item.get_addcon());
            _owner.addDex((sbyte)-item.get_adddex());
            _owner.addInt((sbyte)-item.get_addint());
            _owner.addWis((sbyte)-item.get_addwis());
            if (item.get_addwis() != 0)
            {
                _owner.resetBaseMr();
            }
            _owner.addCha((sbyte)-item.get_addcha());

            int addMr = 0;
            addMr -= equipment.Mr;
            if ((item.ItemId == 20236) && _owner.Elf)
            {
                addMr -= 5;
            }
            if (addMr != 0)
            {
                _owner.addMr(addMr);
                _owner.sendPackets(new S_SPMR(_owner));
            }
            if (item.get_addsp() != 0 || equipment.getaddSp() != 0)
            {
                _owner.addSp(-(item.get_addsp() + equipment.getaddSp()));
                _owner.sendPackets(new S_SPMR(_owner));
            }
            if (item.HasteItem)
            {
                _owner.addHasteItemEquipped(-1);
                if (_owner.HasteItemEquipped == 0)
                {
                    _owner.MoveSpeed = 0;
                    _owner.sendPackets(new S_SkillHaste(_owner.Id, 0, 0));
                    _owner.broadcastPacket(new S_SkillHaste(_owner.Id, 0, 0));
                }
            }
            _owner.EquipSlot.removeMagicHelm(_owner.Id, equipment);

            if (item.Type2 == 1)
            {
                removeWeapon(equipment);
            }
            else if (item.Type2 == 2)
            {
                removeArmor(equipment);
            }
        }

        public virtual L1ItemInstance MagicHelm
        {
            set
            {
                switch (value.ItemId)
                {
                    case 20013:
                        _owner.SkillMastery = L1SkillId.PHYSICAL_ENCHANT_DEX;
                        _owner.SkillMastery = L1SkillId.HASTE;
                        _owner.sendPackets(new S_AddSkill(0, 0, 0, 2, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                        break;
                    case 20014:
                        _owner.SkillMastery = L1SkillId.HEAL;
                        _owner.SkillMastery = L1SkillId.EXTRA_HEAL;
                        _owner.sendPackets(new S_AddSkill(1, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                        break;
                    case 20015:
                        _owner.SkillMastery = L1SkillId.ENCHANT_WEAPON;
                        _owner.SkillMastery = L1SkillId.DETECTION;
                        _owner.SkillMastery = L1SkillId.PHYSICAL_ENCHANT_STR;
                        _owner.sendPackets(new S_AddSkill(0, 24, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                        break;
                    case 20008:
                        _owner.SkillMastery = L1SkillId.HASTE;
                        _owner.sendPackets(new S_AddSkill(0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                        break;
                    case 20023:
                        _owner.SkillMastery = L1SkillId.HASTE;
                        _owner.SkillMastery = L1SkillId.GREATER_HASTE;
                        _owner.sendPackets(new S_AddSkill(0, 0, 0, 0, 0, 4, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                        break;
                }
            }
        }

        public virtual void removeMagicHelm(int objectId, L1ItemInstance item)
        {
            switch (item.ItemId)
            {
                case 20013: // 敏捷魔法頭盔
                    if (!SkillsTable.Instance.spellCheck(objectId, L1SkillId.PHYSICAL_ENCHANT_DEX))
                    {
                        _owner.removeSkillMastery(L1SkillId.PHYSICAL_ENCHANT_DEX);
                        _owner.sendPackets(new S_DelSkill(0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                    }
                    if (!SkillsTable.Instance.spellCheck(objectId, L1SkillId.HASTE))
                    {
                        _owner.removeSkillMastery(L1SkillId.HASTE);
                        _owner.sendPackets(new S_DelSkill(0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                    }
                    break;
                case 20014: // 治癒魔法頭盔
                    if (!SkillsTable.Instance.spellCheck(objectId, L1SkillId.HEAL))
                    {
                        _owner.removeSkillMastery(L1SkillId.HEAL);
                        _owner.sendPackets(new S_DelSkill(1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                    }
                    if (!SkillsTable.Instance.spellCheck(objectId, L1SkillId.EXTRA_HEAL))
                    {
                        _owner.removeSkillMastery(L1SkillId.EXTRA_HEAL);
                        _owner.sendPackets(new S_DelSkill(0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                    }
                    break;
                case 20015: // 力量魔法頭盔
                    if (!SkillsTable.Instance.spellCheck(objectId, L1SkillId.ENCHANT_WEAPON))
                    {
                        _owner.removeSkillMastery(L1SkillId.ENCHANT_WEAPON);
                        _owner.sendPackets(new S_DelSkill(0, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                    }
                    if (!SkillsTable.Instance.spellCheck(objectId, L1SkillId.DETECTION))
                    {
                        _owner.removeSkillMastery(L1SkillId.DETECTION);
                        _owner.sendPackets(new S_DelSkill(0, 16, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                    }
                    if (!SkillsTable.Instance.spellCheck(objectId, L1SkillId.PHYSICAL_ENCHANT_STR))
                    {
                        _owner.removeSkillMastery(L1SkillId.PHYSICAL_ENCHANT_STR);
                        _owner.sendPackets(new S_DelSkill(0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                    }
                    break;
                case 20008: // 小型風之頭盔
                    if (!SkillsTable.Instance.spellCheck(objectId, L1SkillId.HASTE))
                    {
                        _owner.removeSkillMastery(L1SkillId.HASTE);
                        _owner.sendPackets(new S_DelSkill(0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                    }
                    break;
                case 20023: // 風之頭盔
                    if (!SkillsTable.Instance.spellCheck(objectId, L1SkillId.HASTE))
                    {
                        _owner.removeSkillMastery(L1SkillId.HASTE);
                        _owner.sendPackets(new S_DelSkill(0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                    }
                    if (!SkillsTable.Instance.spellCheck(objectId, L1SkillId.GREATER_HASTE))
                    {
                        _owner.removeSkillMastery(L1SkillId.GREATER_HASTE);
                        _owner.sendPackets(new S_DelSkill(0, 0, 0, 0, 0, 0, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                    }
                    break;
            }
        }

    }

}