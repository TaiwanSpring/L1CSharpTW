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
namespace LineageServer.Server.Server.storage.mysql
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using InnKeyTable = LineageServer.Server.Server.datatables.InnKeyTable;
	using ItemTable = LineageServer.Server.Server.datatables.ItemTable;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using CharactersItemStorage = LineageServer.Server.Server.storage.CharactersItemStorage;
	using L1Item = LineageServer.Server.Server.Templates.L1Item;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;

	public class MySqlCharactersItemStorage : CharactersItemStorage
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static readonly Logger _log = Logger.getLogger(typeof(MySqlCharactersItemStorage).FullName);

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public java.util.List<l1j.server.server.model.Instance.L1ItemInstance> loadItems(int objId) throws Exception
		public override IList<L1ItemInstance> loadItems(int objId)
		{
			IList<L1ItemInstance> items = Lists.newList();

			Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM character_items WHERE char_id = ?");
				pstm.setInt(1, objId);

				L1ItemInstance item;
				rs = pstm.executeQuery();
				while (rs.next())
				{
					int itemId = rs.getInt("item_id");
					L1Item itemTemplate = ItemTable.Instance.getTemplate(itemId);
					if (itemTemplate == null)
					{
						_log.warning(string.Format("item id:{0:D} not found", itemId));
						continue;
					}
					item = new L1ItemInstance();
					item.Id = rs.getInt("id");
					item.Item = itemTemplate;
					item.Count = rs.getInt("count");
					item.Equipped = rs.getInt("is_equipped") != 0 ? true : false;
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
					item.LastStatus.updateAll();
					// 登入鑰匙紀錄
					if (item.Item.ItemId == 40312)
					{
						InnKeyTable.checkey(item);
					}
					items.Add(item);
				}
			}
			catch (SQLException e)
			{
				throw e;
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
			return items;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void storeItem(int objId, l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void storeItem(int objId, L1ItemInstance item)
		{
			Connection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("INSERT INTO character_items SET id = ?, item_id = ?, char_id = ?, item_name = ?, count = ?, is_equipped = 0, enchantlvl = ?, is_id = ?, durability = ?, charge_count = ?, remaining_time = ?, last_used = ?, bless = ?, attr_enchant_kind = ?, attr_enchant_level = ?,firemr = ?,watermr = ?,earthmr = ?,windmr = ?,addsp = ?,addhp = ?,addmp = ?,hpr = ?,mpr = ?, m_def = ?");
				pstm.setInt(1, item.Id);
				pstm.setInt(2, item.Item.ItemId);
				pstm.setInt(3, objId);
				pstm.setString(4, item.Item.Name);
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
				throw e;
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
			item.LastStatus.updateAll();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void deleteItem(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void deleteItem(L1ItemInstance item)
		{
			Connection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("DELETE FROM character_items WHERE id = ?");
				pstm.setInt(1, item.Id);
				pstm.execute();
			}
			catch (SQLException e)
			{
				throw e;
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateItemId(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateItemId(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET item_id = ? WHERE id = ?", item.ItemId);
			item.LastStatus.updateItemId();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateItemCount(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateItemCount(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET count = ? WHERE id = ?", item.Count);
			item.LastStatus.updateCount();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateItemDurability(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateItemDurability(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET durability = ? WHERE id = ?", item.get_durability());
			item.LastStatus.updateDuraility();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateItemChargeCount(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateItemChargeCount(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET charge_count = ? WHERE id = ?", item.ChargeCount);
			item.LastStatus.updateChargeCount();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateItemRemainingTime(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateItemRemainingTime(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET remaining_time = ? WHERE id = ?", item.RemainingTime);
			item.LastStatus.updateRemainingTime();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateItemEnchantLevel(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateItemEnchantLevel(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET enchantlvl = ? WHERE id = ?", item.EnchantLevel);
			item.LastStatus.updateEnchantLevel();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateItemEquipped(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateItemEquipped(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET is_equipped = ? WHERE id = ?", (item.Equipped ? 1 : 0));
			item.LastStatus.updateEquipped();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateItemIdentified(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateItemIdentified(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET is_id = ? WHERE id = ?", (item.Identified ? 1 : 0));
			item.LastStatus.updateIdentified();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateItemDelayEffect(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateItemDelayEffect(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET last_used = ? WHERE id = ?", item.LastUsed);
			item.LastStatus.updateLastUsed();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateItemBless(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateItemBless(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET bless = ? WHERE id = ?", item.Bless);
			item.LastStatus.updateBless();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateItemAttrEnchantKind(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateItemAttrEnchantKind(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET attr_enchant_kind = ? WHERE id = ?", item.AttrEnchantKind);
			item.LastStatus.updateAttrEnchantKind();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateItemAttrEnchantLevel(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateItemAttrEnchantLevel(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET attr_enchant_level = ? WHERE id = ?", item.AttrEnchantLevel);
			item.LastStatus.updateAttrEnchantLevel();
		}

		/// <summary>
		/// 飾品強化卷軸 </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateFireMr(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateFireMr(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET firemr = ? WHERE id = ?", item.FireMr);
			item.LastStatus.updateFireMr();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateWaterMr(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateWaterMr(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET watermr = ? WHERE id = ?", item.WaterMr);
			item.LastStatus.updateWaterMr();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateEarthMr(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateEarthMr(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET earthmr = ? WHERE id = ?", item.EarthMr);
			item.LastStatus.updateEarthMr();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateWindMr(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateWindMr(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET windmr = ? WHERE id = ?", item.WindMr);
			item.LastStatus.updateWindMr();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateaddSp(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateaddSp(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET addsp = ? WHERE id = ?", item.getaddSp());
			item.LastStatus.updateSp();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateaddHp(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateaddHp(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET addhp = ? WHERE id = ?", item.getaddHp());
			item.LastStatus.updateaddHp();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateaddMp(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateaddMp(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET addmp = ? WHERE id = ?", item.getaddMp());
			item.LastStatus.updateaddMp();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateHpr(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateHpr(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET hpr = ? WHERE id = ?", item.Hpr);
			item.LastStatus.updateHpr();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateMpr(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateMpr(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET mpr = ? WHERE id = ?", item.Mpr);
			item.LastStatus.updateMpr();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void updateM_Def(l1j.server.server.model.Instance.L1ItemInstance item) throws Exception
		public override void updateM_Def(L1ItemInstance item)
		{
			executeUpdate(item.Id, "UPDATE character_items SET m_def = ? WHERE id = ?", item.M_Def);
			item.LastStatus.updateM_Def();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public int getItemCount(int objId) throws Exception
		public override int getItemCount(int objId)
		{
			int count = 0;
			Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM character_items WHERE char_id = ?");
				pstm.setInt(1, objId);
				rs = pstm.executeQuery();
				while (rs.next())
				{
					count++;
				}
			}
			catch (SQLException e)
			{
				throw e;
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
			return count;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private void executeUpdate(int objId, String sql, int updateNum) throws java.sql.SQLException
		private void executeUpdate(int objId, string sql, int updateNum)
		{
			Connection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement(sql.ToString());
				pstm.setInt(1, updateNum);
				pstm.setInt(2, objId);
				pstm.execute();
			}
			catch (SQLException e)
			{
				throw e;
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private void executeUpdate(int objId, String sql, java.sql.Timestamp ts) throws java.sql.SQLException
		private void executeUpdate(int objId, string sql, Timestamp ts)
		{
			Connection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement(sql.ToString());
				pstm.setTimestamp(1, ts);
				pstm.setInt(2, objId);
				pstm.execute();
			}
			catch (SQLException e)
			{
				throw e;
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}
	}

}