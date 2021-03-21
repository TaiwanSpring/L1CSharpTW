using System.Collections.Generic;
using System.Text;

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
namespace LineageServer.Server.Server.datatables
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using IdFactory = LineageServer.Server.Server.IdFactory;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1Armor = LineageServer.Server.Server.Templates.L1Armor;
	using L1EtcItem = LineageServer.Server.Server.Templates.L1EtcItem;
	using L1Item = LineageServer.Server.Server.Templates.L1Item;
	using L1Weapon = LineageServer.Server.Server.Templates.L1Weapon;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	public class ItemTable
	{
		private const long serialVersionUID = 1L;

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(ItemTable).FullName);

		private static readonly IDictionary<string, int> _armorTypes = Maps.newMap();

		private static readonly IDictionary<string, int> _weaponTypes = Maps.newMap();

		private static readonly IDictionary<string, int> _weaponId = Maps.newMap();

		private static readonly IDictionary<string, int> _materialTypes = Maps.newMap();

		private static readonly IDictionary<string, int> _etcItemTypes = Maps.newMap();

		private static readonly IDictionary<string, int> _useTypes = Maps.newMap();

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
			IDictionary<int, L1EtcItem> result = Maps.newMap();

			Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			L1EtcItem item = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("select * from etcitem");

				rs = pstm.executeQuery();
				while (rs.next())
				{
					item = new L1EtcItem();
					item.ItemId = rs.getInt("item_id");
					item.Name = rs.getString("name");
					item.UnidentifiedNameId = rs.getString("unidentified_name_id");
					item.IdentifiedNameId = rs.getString("identified_name_id");
					item.Type = (_etcItemTypes[rs.getString("item_type")]);
					item.UseType = _useTypes[rs.getString("use_type")];
					// item.setType1(0); // 使わない
					item.Type2 = 0;
					item.Material = (_materialTypes[rs.getString("material")]);
					item.Weight = rs.getInt("weight");
					item.GfxId = rs.getInt("invgfx");
					item.GroundGfxId = rs.getInt("grdgfx");
					item.ItemDescId = rs.getInt("itemdesc_id");
					item.MinLevel = rs.getInt("min_lvl");
					item.MaxLevel = rs.getInt("max_lvl");
					item.Bless = rs.getInt("bless");
					item.Tradable = rs.getInt("trade") == 0 ? true : false;
					item.CantDelete = rs.getInt("cant_delete") == 1 ? true : false;
					item.CanSeal = rs.getInt("can_seal") == 1 ? true : false;
					item.DmgSmall = rs.getInt("dmg_small");
					item.DmgLarge = rs.getInt("dmg_large");
					item.set_stackable(rs.getInt("stackable") == 1 ? true : false);
					item.MaxChargeCount = rs.getInt("max_charge_count");
					item.set_locx(rs.getInt("locx"));
					item.set_locy(rs.getInt("locy"));
					item.set_mapid(rs.getShort("mapid"));
					item.set_delayid(rs.getInt("delay_id"));
					item.set_delaytime(rs.getInt("delay_time"));
					item.set_delayEffect(rs.getInt("delay_effect"));
					item.FoodVolume = rs.getInt("food_volume");
					item.ToBeSavedAtOnce = (rs.getInt("save_at_once") == 1) ? true : false;

					result[item.ItemId] = item;
				}
			}
			catch (System.NullReferenceException)
			{
				_log.log(Enum.Level.Server, (new StringBuilder()).Append(item.Name).Append("(" + item.ItemId + ")").Append("の読み込みに失敗しました。").ToString());
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
			return result;
		}

		private IDictionary<int, L1Weapon> allWeapon()
		{
			IDictionary<int, L1Weapon> result = Maps.newMap();

			Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			L1Weapon weapon = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("select * from weapon");

				rs = pstm.executeQuery();
				while (rs.next())
				{
					weapon = new L1Weapon();
					weapon.ItemId = rs.getInt("item_id");
					weapon.Name = rs.getString("name");
					weapon.UnidentifiedNameId = rs.getString("unidentified_name_id");
					weapon.IdentifiedNameId = rs.getString("identified_name_id");
					weapon.Type = (_weaponTypes[rs.getString("type")]);
					weapon.Type1 = (_weaponId[rs.getString("type")]);
					weapon.Type2 = 1;
					weapon.UseType = 1;
					weapon.Material = (_materialTypes[rs.getString("material")]);
					weapon.Weight = rs.getInt("weight");
					weapon.GfxId = rs.getInt("invgfx");
					weapon.GroundGfxId = rs.getInt("grdgfx");
					weapon.ItemDescId = rs.getInt("itemdesc_id");
					weapon.DmgSmall = rs.getInt("dmg_small");
					weapon.DmgLarge = rs.getInt("dmg_large");
					weapon.Range = rs.getInt("range");
					weapon.set_safeenchant(rs.getInt("safenchant"));
					weapon.UseRoyal = rs.getInt("use_royal") == 0 ? false : true;
					weapon.UseKnight = rs.getInt("use_knight") == 0 ? false : true;
					weapon.UseElf = rs.getInt("use_elf") == 0 ? false : true;
					weapon.UseMage = rs.getInt("use_mage") == 0 ? false : true;
					weapon.UseDarkelf = rs.getInt("use_darkelf") == 0 ? false : true;
					weapon.UseDragonknight = rs.getInt("use_dragonknight") == 0 ? false : true;
					weapon.UseIllusionist = rs.getInt("use_illusionist") == 0 ? false : true;
					weapon.HitModifier = rs.getInt("hitmodifier");
					weapon.DmgModifier = rs.getInt("dmgmodifier");
					weapon.set_addstr(rs.getByte("add_str"));
					weapon.set_adddex(rs.getByte("add_dex"));
					weapon.set_addcon(rs.getByte("add_con"));
					weapon.set_addint(rs.getByte("add_int"));
					weapon.set_addwis(rs.getByte("add_wis"));
					weapon.set_addcha(rs.getByte("add_cha"));
					weapon.set_addhp(rs.getInt("add_hp"));
					weapon.set_addmp(rs.getInt("add_mp"));
					weapon.set_addhpr(rs.getInt("add_hpr"));
					weapon.set_addmpr(rs.getInt("add_mpr"));
					weapon.set_addsp(rs.getInt("add_sp"));
					weapon.set_mdef(rs.getInt("m_def"));
					weapon.DoubleDmgChance = rs.getInt("double_dmg_chance");
					weapon.MagicDmgModifier = rs.getInt("magicdmgmodifier");
					weapon.set_canbedmg(rs.getInt("canbedmg"));
					weapon.MinLevel = rs.getInt("min_lvl");
					weapon.MaxLevel = rs.getInt("max_lvl");
					weapon.Bless = rs.getInt("bless");
					weapon.Tradable = rs.getInt("trade") == 0 ? true : false;
					weapon.CantDelete = rs.getInt("cant_delete") == 1 ? true : false;
					weapon.HasteItem = rs.getInt("haste_item") == 0 ? false : true;
					weapon.MaxUseTime = rs.getInt("max_use_time");

					result[weapon.ItemId] = weapon;
				}
			}
			catch (System.NullReferenceException)
			{
				_log.log(Enum.Level.Server, (new StringBuilder()).Append(weapon.Name).Append("(" + weapon.ItemId + ")").Append("の読み込みに失敗しました。").ToString());
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);

			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);

			}
			return result;
		}

		private IDictionary<int, L1Armor> allArmor()
		{
			IDictionary<int, L1Armor> result = Maps.newMap();
			Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			L1Armor armor = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("select * from armor");

				rs = pstm.executeQuery();
				while (rs.next())
				{
					armor = new L1Armor();
					armor.ItemId = rs.getInt("item_id");
					armor.Name = rs.getString("name");
					armor.UnidentifiedNameId = rs.getString("unidentified_name_id");
					armor.IdentifiedNameId = rs.getString("identified_name_id");
					armor.Type = (_armorTypes[rs.getString("type")]);
					// armor.setType1((_armorId
					// .get(rs.getString("armor_type"))).intValue()); // 使わない
					armor.Type2 = 2;
					armor.UseType = (_useTypes[rs.getString("type")]);
					armor.Material = (_materialTypes[rs.getString("material")]);
					armor.Weight = rs.getInt("weight");
					armor.GfxId = rs.getInt("invgfx");
					armor.GroundGfxId = rs.getInt("grdgfx");
					armor.ItemDescId = rs.getInt("itemdesc_id");
					armor.set_ac(rs.getInt("ac"));
					armor.set_safeenchant(rs.getInt("safenchant"));
					armor.UseRoyal = rs.getInt("use_royal") == 0 ? false : true;
					armor.UseKnight = rs.getInt("use_knight") == 0 ? false : true;
					armor.UseElf = rs.getInt("use_elf") == 0 ? false : true;
					armor.UseMage = rs.getInt("use_mage") == 0 ? false : true;
					armor.UseDarkelf = rs.getInt("use_darkelf") == 0 ? false : true;
					armor.UseDragonknight = rs.getInt("use_dragonknight") == 0 ? false : true;
					armor.UseIllusionist = rs.getInt("use_illusionist") == 0 ? false : true;
					armor.set_addstr(rs.getByte("add_str"));
					armor.set_addcon(rs.getByte("add_con"));
					armor.set_adddex(rs.getByte("add_dex"));
					armor.set_addint(rs.getByte("add_int"));
					armor.set_addwis(rs.getByte("add_wis"));
					armor.set_addcha(rs.getByte("add_cha"));
					armor.set_addhp(rs.getInt("add_hp"));
					armor.set_addmp(rs.getInt("add_mp"));
					armor.set_addhpr(rs.getInt("add_hpr"));
					armor.set_addmpr(rs.getInt("add_mpr"));
					armor.set_addsp(rs.getInt("add_sp"));
					armor.MinLevel = rs.getInt("min_lvl");
					armor.MaxLevel = rs.getInt("max_lvl");
					armor.set_mdef(rs.getInt("m_def"));
					armor.DamageReduction = rs.getInt("damage_reduction");
					armor.WeightReduction = rs.getInt("weight_reduction");
					armor.HitModifierByArmor = rs.getInt("hit_modifier");
					armor.DmgModifierByArmor = rs.getInt("dmg_modifier");
					armor.BowHitModifierByArmor = rs.getInt("bow_hit_modifier");
					armor.BowDmgModifierByArmor = rs.getInt("bow_dmg_modifier");
					armor.HasteItem = rs.getInt("haste_item") == 0 ? false : true;
					armor.Bless = rs.getInt("bless");
					armor.Tradable = rs.getInt("trade") == 0 ? true : false;
					armor.CantDelete = rs.getInt("cant_delete") == 1 ? true : false;
					armor.set_defense_earth(rs.getInt("defense_earth"));
					armor.set_defense_water(rs.getInt("defense_water"));
					armor.set_defense_wind(rs.getInt("defense_wind"));
					armor.set_defense_fire(rs.getInt("defense_fire"));
					armor.set_regist_stun(rs.getInt("regist_stun"));
					armor.set_regist_stone(rs.getInt("regist_stone"));
					armor.set_regist_sleep(rs.getInt("regist_sleep"));
					armor.set_regist_freeze(rs.getInt("regist_freeze"));
					armor.set_regist_sustain(rs.getInt("regist_sustain"));
					armor.set_regist_blind(rs.getInt("regist_blind"));
					armor.MaxUseTime = rs.getInt("max_use_time");
					armor.Grade = rs.getInt("grade");

					result[armor.ItemId] = armor;
				}
			}
			catch (System.NullReferenceException)
			{
				_log.log(Enum.Level.Server, (new StringBuilder()).Append(armor.Name).Append("(" + armor.ItemId + ")").Append("の読み込みに失敗しました。").ToString());
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);

			}
			return result;
		}

		private void buildFastLookupTable()
		{
			int highestId = 0;

			IDictionary<int, L1EtcItem>.ValueCollection items = _etcitems.Values;
			foreach (L1EtcItem item in items)
			{
				if (item.ItemId > highestId)
				{
					highestId = item.ItemId;
				}
			}

			IDictionary<int, L1Weapon>.ValueCollection weapons = _weapons.Values;
			foreach (L1Weapon weapon in weapons)
			{
				if (weapon.ItemId > highestId)
				{
					highestId = weapon.ItemId;
				}
			}

			IDictionary<int, L1Armor>.ValueCollection armors = _armors.Values;
			foreach (L1Armor armor in armors)
			{
				if (armor.ItemId > highestId)
				{
					highestId = armor.ItemId;
				}
			}

			_allTemplates = new L1Item[highestId + 1];

			foreach (int? id in _etcitems.Keys)
			{
				L1EtcItem item = _etcitems[id];
				_allTemplates[id.Value] = item;
			}

			foreach (int? id in _weapons.Keys)
			{
				L1Weapon item = _weapons[id];
				_allTemplates[id.Value] = item;
			}

			foreach (int? id in _armors.Keys)
			{
				L1Armor item = _armors[id];
				_allTemplates[id.Value] = item;
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

		public static long Serialversionuid
		{
			get
			{
				return serialVersionUID;
			}
		}
	}

}