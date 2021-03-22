using System;
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
namespace LineageServer.Server.Server.Model
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using InnKeyTable = LineageServer.Server.Server.DataSources.InnKeyTable;
	using ItemTable = LineageServer.Server.Server.DataSources.ItemTable;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1Item = LineageServer.Server.Server.Templates.L1Item;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;

	[Serializable]
	public class L1DwarfInventory : L1Inventory
	{
		/// 
		private const long serialVersionUID = 1L;

		public L1DwarfInventory(L1PcInstance owner)
		{
			_owner = owner;
		}

		// ＤＢのcharacter_itemsの読込
		public override void loadItems()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM character_warehouse WHERE account_name = ?");
				pstm.setString(1, _owner.AccountName);

				rs = pstm.executeQuery();

				while (rs.next())
				{
					L1ItemInstance item = new L1ItemInstance();
					int objectId = rs.getInt("id");
					item.Id = objectId;
					L1Item itemTemplate = ItemTable.Instance.getTemplate(rs.getInt("item_id"));
					item.Item = itemTemplate;
					item.Count = rs.getInt("count");
					item.Equipped = false;
					item.EnchantLevel = rs.getInt("enchantlvl");
					item.Identified = rs.getInt("is_id") != 0 ? true : false;
					item.set_durability(rs.getInt("durability"));
					item.ChargeCount = rs.getInt("charge_count");
					item.RemainingTime = rs.getInt("remaining_time");
					item.LastUsed = rs.getTimestamp("last_used");
					item.Bless = rs.getInt("bless");
					item.AttrEnchantKind = rs.getInt("attr_enchant_kind");
					item.AttrEnchantLevel = rs.getInt("attr_enchant_level");
					item.FireMr = rs.getInt("firemr");
					item.WaterMr = rs.getInt("watermr");
					item.EarthMr = rs.getInt("earthmr");
					item.WindMr = rs.getInt("windmr");
					item.setaddSp(rs.getInt("addsp"));
					item.setaddHp(rs.getInt("addhp"));
					item.setaddMp(rs.getInt("addmp"));
					item.Hpr = rs.getInt("hpr");
					item.Mpr = rs.getInt("mpr");
					item.M_Def = rs.getInt("m_def");
					// 登入鑰匙紀錄
					if (item.Item.ItemId == 40312)
					{
						InnKeyTable.checkey(item);
					}
					_items.Add(item);
					L1World.Instance.storeObject(item);
				}

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
		}

		// ＤＢのcharacter_warehouseへ登録
		public override void insertItem(L1ItemInstance item)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("INSERT INTO character_warehouse SET id = ?, account_name = ?, item_id = ?, item_name = ?, count = ?, is_equipped=0, enchantlvl = ?, is_id = ?, durability = ?, charge_count = ?, remaining_time = ?, last_used = ?, bless = ?, attr_enchant_kind = ?, attr_enchant_level = ?, firemr = ?,watermr = ?,earthmr = ?,windmr = ?,addsp = ?,addhp = ?,addmp = ?,hpr = ?,mpr = ?,m_def = ?");
				pstm.setInt(1, item.Id);
				pstm.setString(2, _owner.AccountName);
				pstm.setInt(3, item.ItemId);
				pstm.setString(4, item.Name);
				pstm.setInt(5, item.Count);
				pstm.setInt(6, item.EnchantLevel);
				pstm.setInt(7, item.Identified ? 1 : 0);
				pstm.setInt(8, item.get_durability());
				pstm.setInt(9, item.ChargeCount);
				pstm.setInt(10, item.RemainingTime);
				pstm.setTimestamp(11, item.LastUsed);
				pstm.setInt(12, item.Bless);
				pstm.setInt(13, item.AttrEnchantKind);
				pstm.setInt(14, item.AttrEnchantLevel);
				pstm.setInt(15, item.FireMr);
				pstm.setInt(16, item.WaterMr);
				pstm.setInt(17, item.EarthMr);
				pstm.setInt(18, item.WindMr);
				pstm.setInt(19, item.getaddSp());
				pstm.setInt(20, item.getaddHp());
				pstm.setInt(21, item.getaddMp());
				pstm.setInt(22, item.Hpr);
				pstm.setInt(23, item.Mpr);
				pstm.setInt(24, item.M_Def);
				pstm.execute();
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}

		}

		// ＤＢのcharacter_warehouseを更新
		public override void updateItem(L1ItemInstance item)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("UPDATE character_warehouse SET count = ? WHERE id = ?");
				pstm.setInt(1, item.Count);
				pstm.setInt(2, item.Id);
				pstm.execute();
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		// ＤＢのcharacter_warehouseから削除
		public override void deleteItem(L1ItemInstance item)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("DELETE FROM character_warehouse WHERE id = ?");
				pstm.setInt(1, item.Id);
				pstm.execute();
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}

			_items.RemoveAt(_items.IndexOf(item));
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static void present(String account, int itemid, int enchant, int count) throws Exception
		public static void present(string account, int itemid, int enchant, int count)
		{

			L1Item temp = ItemTable.Instance.getTemplate(itemid);
			if (temp == null)
			{
				return;
			}

			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;

				if (string.Compare(account, "*", StringComparison.OrdinalIgnoreCase) == 0)
				{
					pstm = con.prepareStatement("SELECT * FROM accounts");
				}
				else
				{
					pstm = con.prepareStatement("SELECT * FROM accounts WHERE login=?");
					pstm.setString(1, account);
				}
				rs = pstm.executeQuery();

				IList<string> accountList = Lists.newList();
				while (rs.next())
				{
					accountList.Add(rs.getString("login"));
				}

				present(accountList, itemid, enchant, count);

			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
				throw e;
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}

		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public static void present(int minlvl, int maxlvl, int itemid, int enchant, int count) throws Exception
		public static void present(int minlvl, int maxlvl, int itemid, int enchant, int count)
		{

			L1Item temp = ItemTable.Instance.getTemplate(itemid);
			if (temp == null)
			{
				return;
			}

			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;

				pstm = con.prepareStatement("SELECT distinct(account_name) as account_name FROM characters WHERE level between ? and ?");
				pstm.setInt(1, minlvl);
				pstm.setInt(2, maxlvl);
				rs = pstm.executeQuery();

				IList<string> accountList = Lists.newList();
				while (rs.next())
				{
					accountList.Add(rs.getString("account_name"));
				}

				present(accountList, itemid, enchant, count);

			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
				throw e;
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}

		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private static void present(java.util.List<String> accountList, int itemid, int enchant, int count) throws Exception
		private static void present(IList<string> accountList, int itemid, int enchant, int count)
		{
			L1Item itemtemp = ItemTable.Instance.getTemplate(itemid);

			if (ItemTable.Instance.getTemplate(itemid) == null)
			{
				throw new Exception("道具編號不存在。");
			}
			else
			{
				IDataBaseConnection con = null;
				PreparedStatement ps = null;

				try
				{
					con = L1DatabaseFactory.Instance.Connection;
					con.AutoCommit = false;

					foreach (string account in accountList)
					{
						if (itemtemp.Stackable)
						{
							L1ItemInstance item = ItemTable.Instance.createItem(itemid);
							item.EnchantLevel = enchant;
							item.Count = count;

							ps = con.prepareStatement("INSERT INTO character_warehouse SET " + "id = ?," + "account_name = ?," + "item_id = ?," + "item_name = ?," + "count = ?," + "is_equipped=0," + "enchantlvl = ?," + "is_id = ?," + "durability = ?," + "charge_count = ?," + "remaining_time = ?," + "last_used = ?," + "bless = ?," + "attr_enchant_kind = ?," + "attr_enchant_level = ?," + "firemr = ?," + "watermr = ?," + "earthmr = ?," + "windmr = ?," + "addsp = ?," + "addhp = ?," + "addmp = ?," + "hpr = ?," + "mpr = ?," + "m_def = ?");

							ps.setInt(1, item.Id);
							ps.setString(2, account);
							ps.setInt(3, item.ItemId);
							ps.setString(4, item.Name);
							ps.setInt(5, item.Count);
							ps.setInt(6, item.EnchantLevel);
							ps.setInt(7, item.Identified ? 1 : 0);
							ps.setInt(8, item.get_durability());
							ps.setInt(9, item.ChargeCount);
							ps.setInt(10, item.RemainingTime);
							ps.setTimestamp(11, item.LastUsed);
							ps.setInt(12, item.Bless);
							ps.setInt(13, item.AttrEnchantKind);
							ps.setInt(14, item.AttrEnchantLevel);
							ps.setInt(15, item.FireMr);
							ps.setInt(16, item.WaterMr);
							ps.setInt(17, item.EarthMr);
							ps.setInt(18, item.WindMr);
							ps.setInt(19, item.getaddSp());
							ps.setInt(20, item.getaddHp());
							ps.setInt(21, item.getaddMp());
							ps.setInt(22, item.Hpr);
							ps.setInt(23, item.Mpr);
							ps.setInt(24, item.M_Def);
							ps.execute();
						}
						else
						{
							L1ItemInstance item = null;
							int createCount;

							for (createCount = 0; createCount < count; createCount++)
							{
								item = ItemTable.Instance.createItem(itemid);
								item.EnchantLevel = enchant;

								ps = con.prepareStatement("INSERT INTO character_warehouse SET " + "id = ?," + "account_name = ?," + "item_id = ?," + "item_name = ?," + "count = ?," + "is_equipped=0," + "enchantlvl = ?," + "is_id = ?," + "durability = ?," + "charge_count = ?," + "remaining_time = ?," + "last_used = ?," + "bless = ?," + "attr_enchant_kind = ?," + "attr_enchant_level = ?," + "firemr = ?," + "watermr = ?," + "earthmr = ?," + "windmr = ?," + "addsp = ?," + "addhp = ?," + "addmp = ?," + "hpr = ?," + "mpr = ?," + "m_def = ?");

								ps.setInt(1, item.Id);
								ps.setString(2, account);
								ps.setInt(3, item.ItemId);
								ps.setString(4, item.Name);
								ps.setInt(5, item.Count);
								ps.setInt(6, item.EnchantLevel);
								ps.setInt(7, item.Identified ? 1 : 0);
								ps.setInt(8, item.get_durability());
								ps.setInt(9, item.ChargeCount);
								ps.setInt(10, item.RemainingTime);
								ps.setTimestamp(11, item.LastUsed);
								ps.setInt(12, item.Bless);
								ps.setInt(13, item.AttrEnchantKind);
								ps.setInt(14, item.AttrEnchantLevel);
								ps.setInt(15, item.FireMr);
								ps.setInt(16, item.WaterMr);
								ps.setInt(17, item.EarthMr);
								ps.setInt(18, item.WindMr);
								ps.setInt(19, item.getaddSp());
								ps.setInt(20, item.getaddHp());
								ps.setInt(21, item.getaddMp());
								ps.setInt(22, item.Hpr);
								ps.setInt(23, item.Mpr);
								ps.setInt(24, item.M_Def);
								ps.execute();
							}
						}
					}

					con.commit();
					con.AutoCommit = true;
				}
				catch (SQLException e)
				{
					try
					{
						con.rollback();
					}
					catch (SQLException)
					{
					}

					_log.log(Enum.Level.Server, e.Message, e);
					throw new Exception(".present 處理時發生了例外的錯誤。");
				}
				finally
				{
					SQLUtil.close(ps);
					SQLUtil.close(con);
				}
			}
		}

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1DwarfInventory).FullName);

		private readonly L1PcInstance _owner;
	}

}