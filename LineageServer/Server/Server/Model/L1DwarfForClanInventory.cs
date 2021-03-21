﻿using System;

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
	using InnKeyTable = LineageServer.Server.Server.datatables.InnKeyTable;
	using ItemTable = LineageServer.Server.Server.datatables.ItemTable;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1Item = LineageServer.Server.Server.Templates.L1Item;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;

	[Serializable]
	public class L1DwarfForClanInventory : L1Inventory
	{

		private const long serialVersionUID = 1L;
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1DwarfForClanInventory).FullName);
		private readonly L1Clan _clan;

		public L1DwarfForClanInventory(L1Clan clan)
		{
			_clan = clan;
		}

		// ＤＢのcharacter_itemsの読込
		public override void loadItems()
		{
			lock (this)
			{
				IDataBaseConnection con = null;
				PreparedStatement pstm = null;
				ResultSet rs = null;
				try
				{
					con = L1DatabaseFactory.Instance.Connection;
					pstm = con.prepareStatement("SELECT * FROM clan_warehouse WHERE clan_name = ?");
					pstm.setString(1, _clan.ClanName);
					rs = pstm.executeQuery();
					while (rs.next())
					{
						L1ItemInstance item = new L1ItemInstance();
						int objectId = rs.getInt("id");
						item.Id = objectId;
						int itemId = rs.getInt("item_id");
						L1Item itemTemplate = ItemTable.Instance.getTemplate(itemId);
						if (itemTemplate == null)
						{
							throw new System.NullReferenceException("item_id=" + itemId + " not found");
						}
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
		}

		// ＤＢのclan_warehouseへ登録
		public override void insertItem(L1ItemInstance item)
		{
			lock (this)
			{
				IDataBaseConnection con = null;
				PreparedStatement pstm = null;
				try
				{
					con = L1DatabaseFactory.Instance.Connection;
					pstm = con.prepareStatement("INSERT INTO clan_warehouse SET id = ?, clan_name = ?, item_id = ?, item_name = ?, count = ?, is_equipped=0, enchantlvl = ?, is_id= ?, durability = ?, charge_count = ?, remaining_time = ?, last_used = ?, bless = ?, attr_enchant_kind = ?, attr_enchant_level = ?,firemr = ?,watermr = ?,earthmr = ?,windmr = ?,addsp = ?,addhp = ?,addmp = ?,hpr = ?,mpr = ?,m_def = ?");
					pstm.setInt(1, item.Id);
					pstm.setString(2, _clan.ClanName);
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
		}

		// ＤＢのclan_warehouseを更新
		public override void updateItem(L1ItemInstance item)
		{
			lock (this)
			{
				IDataBaseConnection con = null;
				PreparedStatement pstm = null;
				try
				{
					con = L1DatabaseFactory.Instance.Connection;
					pstm = con.prepareStatement("UPDATE clan_warehouse SET count = ? WHERE id = ?");
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
		}

		// ＤＢのclan_warehouseから削除
		public override void deleteItem(L1ItemInstance item)
		{
			lock (this)
			{
				IDataBaseConnection con = null;
				PreparedStatement pstm = null;
				try
				{
					con = L1DatabaseFactory.Instance.Connection;
					pstm = con.prepareStatement("DELETE FROM clan_warehouse WHERE id = ?");
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
		}

		// DBのクラン倉庫のアイテムを全て削除(血盟解散時のみ使用)
		public virtual void deleteAllItems()
		{
			lock (this)
			{
				IDataBaseConnection con = null;
				PreparedStatement pstm = null;
				try
				{
					con = L1DatabaseFactory.Instance.Connection;
					pstm = con.prepareStatement("DELETE FROM clan_warehouse WHERE clan_name = ?");
					pstm.setString(1, _clan.ClanName);
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
		}

		/// <summary>
		/// 寫入血盟使用紀錄 </summary>
		/// <param name="pc">    L1PcInstance</br> </param>
		/// <param name="item">  L1ItemInstance</br> </param>
		/// <param name="count"> 物品數量</br> </param>
		/// <param name="type">  領出: 1, 存入: 0 </br> </param>
		public virtual void writeHistory(L1PcInstance pc, L1ItemInstance item, int count, int type)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("INSERT INTO clan_warehouse_history SET clan_id=?, char_name=?, type=?, item_name=?, item_count=?, record_time=?");
				pstm.setInt(1, _clan.ClanId);
				pstm.setString(2, pc.Name);
				pstm.setInt(3, type);
				pstm.setString(4, item.Name);
				pstm.setInt(5, count);
				pstm.setTimestamp(6, new Timestamp(DateTimeHelper.CurrentUnixTimeMillis()));
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

	}

}