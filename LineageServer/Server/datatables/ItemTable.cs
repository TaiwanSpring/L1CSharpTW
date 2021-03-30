using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
    class ItemTable
    {
        private readonly static IDataSource etcItemDataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.Etcitem);
        private readonly static IDataSource weaponDataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.Weapon);
        private readonly static IDataSource armorDataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.Armor);
        private static readonly IDictionary<string, int> _armorTypes = MapFactory.NewMap<string, int>();

        private static readonly IDictionary<string, int> _weaponTypes = MapFactory.NewMap<string, int>();

        private static readonly IDictionary<string, int> _weaponId = MapFactory.NewMap<string, int>();

        private static readonly IDictionary<string, int> _materialTypes = MapFactory.NewMap<string, int>();

        private static readonly IDictionary<string, int> _etcItemTypes = MapFactory.NewMap<string, int>();

        private static readonly IDictionary<string, int> _useTypes = MapFactory.NewMap<string, int>();

        private static ItemTable _instance;

        private L1Item[] _allTemplates;

        private readonly IDictionary<int, L1EtcItem> _etcitems;

        private readonly IDictionary<int, L1Armor> _armors;

        private readonly IDictionary<int, L1Weapon> _weapons;

        static ItemTable()
        {

            _etcItemTypes["arrow"] = 0;
            _etcItemTypes["wand"] = 1;
            _etcItemTypes["light"] = 2;
            _etcItemTypes["gem"] = 3;
            _etcItemTypes["totem"] = 4;
            _etcItemTypes["firecracker"] = 5;
            _etcItemTypes["potion"] = 6;
            _etcItemTypes["food"] = 7;
            _etcItemTypes["scroll"] = 8;
            _etcItemTypes["questitem"] = 9;
            _etcItemTypes["spellbook"] = 10;
            _etcItemTypes["petitem"] = 11;
            _etcItemTypes["other"] = 12;
            _etcItemTypes["material"] = 13;
            _etcItemTypes["event"] = 14;
            _etcItemTypes["sting"] = 15;
            _etcItemTypes["treasure_box"] = 16;
            _etcItemTypes["magic_doll"] = 17;
            _etcItemTypes["furniture"] = 18; // 家具

            _useTypes["none"] = -1; // 使用不可能
            _useTypes["weapon"] = 1;
            _useTypes["armor"] = 2;
            // _useTypes.put("wand1", 3);
            // _useTypes.put("wand", 4);
            _useTypes["spell_long"] = 5; // 地面 / オブジェクト選択(遠距離)
            _useTypes["ntele"] = 6;
            _useTypes["identify"] = 7;
            _useTypes["res"] = 8;
            _useTypes["letter"] = 12;
            _useTypes["letter_w"] = 13;
            _useTypes["choice"] = 14;
            _useTypes["instrument"] = 15;
            _useTypes["sosc"] = 16;
            _useTypes["spell_short"] = 17; // 地面 / オブジェクト選択(近距離)
            _useTypes["T"] = 18;
            _useTypes["cloak"] = 19;
            _useTypes["glove"] = 20;
            _useTypes["boots"] = 21;
            _useTypes["helm"] = 22;
            _useTypes["amulet"] = 24;
            _useTypes["shield"] = 25;
            _useTypes["guarder"] = 25;
            _useTypes["dai"] = 26;
            _useTypes["zel"] = 27;
            _useTypes["blank"] = 28;
            _useTypes["btele"] = 29;
            _useTypes["spell_buff"] = 30; // オブジェクト選択(遠距離)
                                          // Ctrlを押さないとパケットが飛ばない？
            _useTypes["ccard"] = 31;
            _useTypes["ccard_w"] = 32;
            _useTypes["vcard"] = 33;
            _useTypes["vcard_w"] = 34;
            _useTypes["wcard"] = 35;
            _useTypes["wcard_w"] = 36;
            _useTypes["belt"] = 37;
            // _useTypes.put("spell_long2", 39); // 地面 / オブジェクト選択(遠距離)
            // 5と同じ？
            _useTypes["earring"] = 40;
            _useTypes["fishing_rod"] = 42;
            _useTypes["rune"] = 43; // add 符石/遺物 : UseType= 43(右) 44(左-目前開放) 45(上)
            _useTypes["rune2"] = 44;
            _useTypes["rune3"] = 45;
            _useTypes["del"] = 46;
            _useTypes["normal"] = 51; // 一般類(藥水)
            _useTypes["ring"] = 57; // 戒指類

            _armorTypes["none"] = 0;
            _armorTypes["helm"] = 1;
            _armorTypes["armor"] = 2;
            _armorTypes["T"] = 3;
            _armorTypes["cloak"] = 4;
            _armorTypes["glove"] = 5;
            _armorTypes["boots"] = 6;
            _armorTypes["shield"] = 7;
            _armorTypes["amulet"] = 8;
            _armorTypes["ring"] = 9;
            _armorTypes["belt"] = 10;
            _armorTypes["ring2"] = 11;
            _armorTypes["earring"] = 12;
            _armorTypes["guarder"] = 13;
            _armorTypes["rune"] = 14;
            _armorTypes["rune2"] = 15;
            _armorTypes["rune3"] = 16;

            _weaponTypes["sword"] = 1;
            _weaponTypes["dagger"] = 2;
            _weaponTypes["tohandsword"] = 3;
            _weaponTypes["bow"] = 4;
            _weaponTypes["spear"] = 5;
            _weaponTypes["blunt"] = 6;
            _weaponTypes["staff"] = 7;
            _weaponTypes["throwingknife"] = 8;
            _weaponTypes["arrow"] = 9;
            _weaponTypes["gauntlet"] = 10;
            _weaponTypes["claw"] = 11;
            _weaponTypes["edoryu"] = 12;
            _weaponTypes["singlebow"] = 13;
            _weaponTypes["singlespear"] = 14;
            _weaponTypes["tohandblunt"] = 15;
            _weaponTypes["tohandstaff"] = 16;
            _weaponTypes["kiringku"] = 17;
            _weaponTypes["chainsword"] = 18;
            _weaponTypes["tohandkiringku"] = 19;

            _weaponId["sword"] = 4;
            _weaponId["dagger"] = 46;
            _weaponId["tohandsword"] = 50;
            _weaponId["bow"] = 20;
            _weaponId["blunt"] = 11;
            _weaponId["spear"] = 24;
            _weaponId["staff"] = 40;
            _weaponId["throwingknife"] = 2922;
            _weaponId["arrow"] = 66;
            _weaponId["gauntlet"] = 62;
            _weaponId["claw"] = 58;
            _weaponId["edoryu"] = 54;
            _weaponId["singlebow"] = 20;
            _weaponId["singlespear"] = 24;
            _weaponId["tohandblunt"] = 11;
            _weaponId["tohandstaff"] = 40;
            _weaponId["kiringku"] = 58;
            _weaponId["chainsword"] = 24;
            _weaponId["tohandkiringku"] = 58;

            _materialTypes["none"] = 0;
            _materialTypes["liquid"] = 1;
            _materialTypes["web"] = 2;
            _materialTypes["vegetation"] = 3;
            _materialTypes["animalmatter"] = 4;
            _materialTypes["paper"] = 5;
            _materialTypes["cloth"] = 6;
            _materialTypes["leather"] = 7;
            _materialTypes["wood"] = 8;
            _materialTypes["bone"] = 9;
            _materialTypes["dragonscale"] = 10;
            _materialTypes["iron"] = 11;
            _materialTypes["steel"] = 12;
            _materialTypes["copper"] = 13;
            _materialTypes["silver"] = 14;
            _materialTypes["gold"] = 15;
            _materialTypes["platinum"] = 16;
            _materialTypes["mithril"] = 17;
            _materialTypes["blackmithril"] = 18;
            _materialTypes["glass"] = 19;
            _materialTypes["gemstone"] = 20;
            _materialTypes["mineral"] = 21;
            _materialTypes["oriharukon"] = 22;
        }

        public static ItemTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ItemTable();
                }
                return _instance;
            }
        }

        private ItemTable()
        {
            _etcitems = allEtcItem();
            _weapons = allWeapon();
            _armors = allArmor();
            buildFastLookupTable();
        }

        private IDictionary<int, L1EtcItem> allEtcItem()
        {
            IDictionary<int, L1EtcItem> result = MapFactory.NewMap<int, L1EtcItem>();
            IList<IDataSourceRow> dataSourceRows = etcItemDataSource.Select().Query();

            L1EtcItem item;
            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                item = new L1EtcItem();
                item.ItemId = dataSourceRow.getInt(Etcitem.Column_item_id);
                item.Name = dataSourceRow.getString(Etcitem.Column_name);
                item.UnidentifiedNameId = dataSourceRow.getString(Etcitem.Column_unidentified_name_id);
                item.IdentifiedNameId = dataSourceRow.getString(Etcitem.Column_identified_name_id);
                item.Type = _etcItemTypes[dataSourceRow.getString(Etcitem.Column_item_type)];
                item.UseType = _useTypes[dataSourceRow.getString(Etcitem.Column_use_type)];
                // item.setType1(0); // 使わない
                item.Type2 = 0;
                item.Material = _materialTypes[dataSourceRow.getString(Etcitem.Column_material)];
                item.Weight = dataSourceRow.getInt(Etcitem.Column_weight);
                item.GfxId = dataSourceRow.getInt(Etcitem.Column_invgfx);
                item.GroundGfxId = dataSourceRow.getInt(Etcitem.Column_grdgfx);
                item.ItemDescId = dataSourceRow.getInt(Etcitem.Column_itemdesc_id);
                item.MinLevel = dataSourceRow.getInt(Etcitem.Column_min_lvl);
                item.MaxLevel = dataSourceRow.getInt(Etcitem.Column_max_lvl);
                item.Bless = dataSourceRow.getInt(Etcitem.Column_bless);
                item.Tradable = dataSourceRow.getInt(Etcitem.Column_trade) == 0 ? true : false;
                item.CantDelete = dataSourceRow.getInt(Etcitem.Column_cant_delete) == 1 ? true : false;
                item.CanSeal = dataSourceRow.getInt(Etcitem.Column_can_seal) == 1 ? true : false;
                item.DmgSmall = dataSourceRow.getInt(Etcitem.Column_dmg_small);
                item.DmgLarge = dataSourceRow.getInt(Etcitem.Column_dmg_large);
                item.set_stackable(dataSourceRow.getInt(Etcitem.Column_stackable) == 1 ? true : false);
                item.MaxChargeCount = dataSourceRow.getInt(Etcitem.Column_max_charge_count);
                item.set_locx(dataSourceRow.getInt(Etcitem.Column_locx));
                item.set_locy(dataSourceRow.getInt(Etcitem.Column_locy));
                item.set_mapid(dataSourceRow.getShort(Etcitem.Column_mapid));
                item.set_delayid(dataSourceRow.getInt(Etcitem.Column_delay_id));
                item.set_delaytime(dataSourceRow.getInt(Etcitem.Column_delay_time));
                item.set_delayEffect(dataSourceRow.getInt(Etcitem.Column_delay_effect));
                item.FoodVolume = dataSourceRow.getInt(Etcitem.Column_food_volume);
                item.ToBeSavedAtOnce = (dataSourceRow.getInt(Etcitem.Column_save_at_once) == 1) ? true : false;

                result[item.ItemId] = item;
            }
            return result;
        }

        private IDictionary<int, L1Weapon> allWeapon()
        {
            IDictionary<int, L1Weapon> result = MapFactory.NewMap<int, L1Weapon>();

            IList<IDataSourceRow> dataSourceRows = weaponDataSource.Select().Query();

            L1Weapon weapon;

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                weapon = new L1Weapon();
                weapon.ItemId = dataSourceRow.getInt(Weapon.Column_item_id);
                weapon.Name = dataSourceRow.getString(Weapon.Column_name);
                weapon.UnidentifiedNameId = dataSourceRow.getString(Weapon.Column_unidentified_name_id);
                weapon.IdentifiedNameId = dataSourceRow.getString(Weapon.Column_identified_name_id);
                weapon.Type = _weaponTypes[dataSourceRow.getString(Weapon.Column_type)];
                weapon.Type1 = _weaponId[dataSourceRow.getString(Weapon.Column_type)];
                weapon.Type2 = 1;
                weapon.UseType = 1;
                weapon.Material = _materialTypes[dataSourceRow.getString(Weapon.Column_material)];
                weapon.Weight = dataSourceRow.getInt(Weapon.Column_weight);
                weapon.GfxId = dataSourceRow.getInt(Weapon.Column_invgfx);
                weapon.GroundGfxId = dataSourceRow.getInt(Weapon.Column_grdgfx);
                weapon.ItemDescId = dataSourceRow.getInt(Weapon.Column_itemdesc_id);
                weapon.DmgSmall = dataSourceRow.getInt(Weapon.Column_dmg_small);
                weapon.DmgLarge = dataSourceRow.getInt(Weapon.Column_dmg_large);
                weapon.Range = dataSourceRow.getInt(Weapon.Column_range);
                weapon.set_safeenchant(dataSourceRow.getInt(Weapon.Column_safenchant));
                weapon.UseRoyal = dataSourceRow.getInt(Weapon.Column_use_royal) == 0 ? false : true;
                weapon.UseKnight = dataSourceRow.getInt(Weapon.Column_use_knight) == 0 ? false : true;
                weapon.UseElf = dataSourceRow.getInt(Weapon.Column_use_elf) == 0 ? false : true;
                weapon.UseMage = dataSourceRow.getInt(Weapon.Column_use_mage) == 0 ? false : true;
                weapon.UseDarkelf = dataSourceRow.getInt(Weapon.Column_use_darkelf) == 0 ? false : true;
                weapon.UseDragonknight = dataSourceRow.getInt(Weapon.Column_use_dragonknight) == 0 ? false : true;
                weapon.UseIllusionist = dataSourceRow.getInt(Weapon.Column_use_illusionist) == 0 ? false : true;
                weapon.HitModifier = dataSourceRow.getInt(Weapon.Column_hitmodifier);
                weapon.DmgModifier = dataSourceRow.getInt(Weapon.Column_dmgmodifier);
                weapon.set_addstr(dataSourceRow.getByte(Weapon.Column_add_str));
                weapon.set_adddex(dataSourceRow.getByte(Weapon.Column_add_dex));
                weapon.set_addcon(dataSourceRow.getByte(Weapon.Column_add_con));
                weapon.set_addint(dataSourceRow.getByte(Weapon.Column_add_int));
                weapon.set_addwis(dataSourceRow.getByte(Weapon.Column_add_wis));
                weapon.set_addcha(dataSourceRow.getByte(Weapon.Column_add_cha));
                weapon.set_addhp(dataSourceRow.getInt(Weapon.Column_add_hp));
                weapon.set_addmp(dataSourceRow.getInt(Weapon.Column_add_mp));
                weapon.set_addhpr(dataSourceRow.getInt(Weapon.Column_add_hpr));
                weapon.set_addmpr(dataSourceRow.getInt(Weapon.Column_add_mpr));
                weapon.set_addsp(dataSourceRow.getInt(Weapon.Column_add_sp));
                weapon.set_mdef(dataSourceRow.getInt(Weapon.Column_m_def));
                weapon.DoubleDmgChance = dataSourceRow.getInt(Weapon.Column_double_dmg_chance);
                weapon.MagicDmgModifier = dataSourceRow.getInt(Weapon.Column_magicdmgmodifier);
                weapon.set_canbedmg(dataSourceRow.getInt(Weapon.Column_canbedmg));
                weapon.MinLevel = dataSourceRow.getInt(Weapon.Column_min_lvl);
                weapon.MaxLevel = dataSourceRow.getInt(Weapon.Column_max_lvl);
                weapon.Bless = dataSourceRow.getInt(Weapon.Column_bless);
                weapon.Tradable = dataSourceRow.getInt(Weapon.Column_trade) == 0 ? true : false;
                weapon.CantDelete = dataSourceRow.getInt(Weapon.Column_cant_delete) == 1 ? true : false;
                weapon.HasteItem = dataSourceRow.getInt(Weapon.Column_haste_item) == 0 ? false : true;
                weapon.MaxUseTime = dataSourceRow.getInt(Weapon.Column_max_use_time);

                result[weapon.ItemId] = weapon;
            }
            return result;
        }

        private IDictionary<int, L1Armor> allArmor()
        {
            IDictionary<int, L1Armor> result = MapFactory.NewMap<int, L1Armor>();
            L1Armor armor;
            IList<IDataSourceRow> dataSourceRows = armorDataSource.Select().Query();

            for (int i = 0; i < dataSourceRows.Count; i++)
            {
                IDataSourceRow dataSourceRow = dataSourceRows[i];
                armor = new L1Armor();
                armor = new L1Armor();
                armor.ItemId = dataSourceRow.getInt(Armor.Column_item_id);
                armor.Name = dataSourceRow.getString(Armor.Column_name);
                armor.UnidentifiedNameId = dataSourceRow.getString(Armor.Column_unidentified_name_id);
                armor.IdentifiedNameId = dataSourceRow.getString(Armor.Column_identified_name_id);
                armor.Type = _armorTypes[dataSourceRow.getString(Armor.Column_type)];
                armor.Type2 = 2;
                armor.UseType = _useTypes[dataSourceRow.getString(Armor.Column_type)];
                armor.Material = _materialTypes[dataSourceRow.getString(Armor.Column_material)];
                armor.Weight = dataSourceRow.getInt(Armor.Column_weight);
                armor.GfxId = dataSourceRow.getInt(Armor.Column_invgfx);
                armor.GroundGfxId = dataSourceRow.getInt(Armor.Column_grdgfx);
                armor.ItemDescId = dataSourceRow.getInt(Armor.Column_itemdesc_id);
                armor.set_ac(dataSourceRow.getInt(Armor.Column_ac));
                armor.set_safeenchant(dataSourceRow.getInt(Armor.Column_safenchant));
                armor.UseRoyal = dataSourceRow.getInt(Armor.Column_use_royal) == 0 ? false : true;
                armor.UseKnight = dataSourceRow.getInt(Armor.Column_use_knight) == 0 ? false : true;
                armor.UseElf = dataSourceRow.getInt(Armor.Column_use_elf) == 0 ? false : true;
                armor.UseMage = dataSourceRow.getInt(Armor.Column_use_mage) == 0 ? false : true;
                armor.UseDarkelf = dataSourceRow.getInt(Armor.Column_use_darkelf) == 0 ? false : true;
                armor.UseDragonknight = dataSourceRow.getInt(Armor.Column_use_dragonknight) == 0 ? false : true;
                armor.UseIllusionist = dataSourceRow.getInt(Armor.Column_use_illusionist) == 0 ? false : true;
                armor.set_addstr(dataSourceRow.getByte(Armor.Column_add_str));
                armor.set_adddex(dataSourceRow.getByte(Armor.Column_add_dex));
                armor.set_addcon(dataSourceRow.getByte(Armor.Column_add_con));
                armor.set_addint(dataSourceRow.getByte(Armor.Column_add_int));
                armor.set_addwis(dataSourceRow.getByte(Armor.Column_add_wis));
                armor.set_addcha(dataSourceRow.getByte(Armor.Column_add_cha));
                armor.set_addhp(dataSourceRow.getInt(Armor.Column_add_hp));
                armor.set_addmp(dataSourceRow.getInt(Armor.Column_add_mp));
                armor.set_addhpr(dataSourceRow.getInt(Armor.Column_add_hpr));
                armor.set_addmpr(dataSourceRow.getInt(Armor.Column_add_mpr));
                armor.set_addsp(dataSourceRow.getInt(Armor.Column_add_sp));
                armor.MinLevel = dataSourceRow.getInt(Armor.Column_min_lvl);
                armor.MaxLevel = dataSourceRow.getInt(Armor.Column_max_lvl);
                armor.set_mdef(dataSourceRow.getInt(Armor.Column_m_def));
                armor.DamageReduction = dataSourceRow.getInt(Armor.Column_damage_reduction);
                armor.WeightReduction = dataSourceRow.getInt(Armor.Column_weight_reduction);
                armor.HitModifierByArmor = dataSourceRow.getInt(Armor.Column_hit_modifier);
                armor.DmgModifierByArmor = dataSourceRow.getInt(Armor.Column_dmg_modifier);
                armor.BowHitModifierByArmor = dataSourceRow.getInt(Armor.Column_bow_hit_modifier);
                armor.BowDmgModifierByArmor = dataSourceRow.getInt(Armor.Column_bow_dmg_modifier);
                armor.HasteItem = dataSourceRow.getInt(Armor.Column_haste_item) == 0 ? false : true;
                armor.Bless = dataSourceRow.getInt(Armor.Column_bless);
                armor.Tradable = dataSourceRow.getInt(Armor.Column_trade) == 0 ? true : false;
                armor.CantDelete = dataSourceRow.getInt(Armor.Column_cant_delete) == 1 ? true : false;
                armor.set_defense_earth(dataSourceRow.getInt(Armor.Column_defense_earth));
                armor.set_defense_water(dataSourceRow.getInt(Armor.Column_defense_water));
                armor.set_defense_wind(dataSourceRow.getInt(Armor.Column_defense_wind));
                armor.set_defense_fire(dataSourceRow.getInt(Armor.Column_defense_fire));
                armor.set_regist_stun(dataSourceRow.getInt(Armor.Column_regist_stun));
                armor.set_regist_stone(dataSourceRow.getInt(Armor.Column_regist_stone));
                armor.set_regist_sleep(dataSourceRow.getInt(Armor.Column_regist_sleep));
                armor.set_regist_freeze(dataSourceRow.getInt(Armor.Column_regist_freeze));
                armor.set_regist_sustain(dataSourceRow.getInt(Armor.Column_regist_sustain));
                armor.set_regist_blind(dataSourceRow.getInt(Armor.Column_regist_blind));
                armor.MaxUseTime = dataSourceRow.getInt(Armor.Column_max_use_time);
                armor.Grade = dataSourceRow.getInt(Armor.Column_grade);
            }
            return result;
        }

        private void buildFastLookupTable()
        {
            int highestId = 0;

            foreach (L1EtcItem item in _etcitems.Values)
            {
                if (item.ItemId > highestId)
                {
                    highestId = item.ItemId;
                }
            }

            foreach (L1Weapon weapon in _weapons.Values)
            {
                if (weapon.ItemId > highestId)
                {
                    highestId = weapon.ItemId;
                }
            }

            foreach (L1Armor armor in _armors.Values)
            {
                if (armor.ItemId > highestId)
                {
                    highestId = armor.ItemId;
                }
            }

            _allTemplates = new L1Item[highestId + 1];

            foreach (int id in _etcitems.Keys)
            {
                L1EtcItem item = _etcitems[id];
                _allTemplates[id] = item;
            }

            foreach (int id in _weapons.Keys)
            {
                L1Weapon item = _weapons[id];
                _allTemplates[id] = item;
            }

            foreach (int id in _armors.Keys)
            {
                L1Armor item = _armors[id];
                _allTemplates[id] = item;
            }
        }

        public virtual L1Item getTemplate(int id)
        {
            return _allTemplates[id];
        }

        public virtual L1ItemInstance createItem(int itemId)
        {
            L1Item temp = getTemplate(itemId);
            if (temp == null)
            {
                return null;
            }
            L1ItemInstance item = new L1ItemInstance();
            item.Id = IdFactory.Instance.nextId();
            item.Item = temp;
            L1World.Instance.storeObject(item);
            return item;
        }

        public virtual int findItemIdByName(string name)
        {
            int itemid = 0;
            foreach (L1Item item in _allTemplates)
            {
                if ((item != null) && item.Name.Equals(name))
                {
                    itemid = item.ItemId;
                    break;
                }
            }
            return itemid;
        }

        public virtual int findItemIdByNameWithoutSpace(string name)
        {
            int itemid = 0;
            foreach (L1Item item in _allTemplates)
            {
                if ((item != null) && item.Name.Replace(" ", "").Equals(name))
                {
                    itemid = item.ItemId;
                    break;
                }
            }
            return itemid;
        }
    }

}