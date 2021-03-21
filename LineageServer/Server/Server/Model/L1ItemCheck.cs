using System;

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
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1ItemId = LineageServer.Server.Server.Model.identity.L1ItemId;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;

	/// <summary>
	/// 負責物品狀態檢查是否作弊
	/// </summary>
	public class L1ItemCheck
	{
		private int itemId;
		private bool isStackable = false;

		public virtual bool ItemCheck(L1ItemInstance item, L1PcInstance pc)
		{
			itemId = item.Item.ItemId;
			int itemCount = item.Count;
			bool isCheat = false;

			if ((findWeapon() || findArmor()) && itemCount != 1)
			{
				isCheat = true;
			}
			else if (findEtcItem())
			{
				// 不可堆疊的道具卻堆疊，就視為作弊
				if (!isStackable && itemCount != 1)
				{
					isCheat = true;
					// 金幣大於20億以及金幣負值則為作弊
				}
				else if (itemId == L1ItemId.ADENA && (itemCount > 2000000000 || itemCount < 0))
				{
					isCheat = true;
					// 可堆疊道具(金幣除外)堆疊超過十萬個以及堆疊負值設定為作弊
				}
				else if (isStackable && itemId != L1ItemId.ADENA && (itemCount > 100000 || itemCount < 0))
				{
					isCheat = true;
				}
			}
			if (isCheat)
			{
				// 作弊直接刪除物品
				pc.Inventory.removeItem(item, itemCount);
			}
			return isCheat;
		}

		private bool findWeapon()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			bool inWeapon = false;

			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM weapon WHERE item_id = ?");
				pstm.setInt(1, itemId);
				rs = pstm.executeQuery();
				if (rs != null)
				{
					if (rs.next())
					{
						inWeapon = true;
					}
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				SQLUtil.close(rs, pstm, con);
			}
			return inWeapon;
		}

		private bool findArmor()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			bool inArmor = false;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM armor WHERE item_id = ?");
				pstm.setInt(1, itemId);
				rs = pstm.executeQuery();
				if (rs != null)
				{
					if (rs.next())
					{
						inArmor = true;
					}
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				SQLUtil.close(rs, pstm, con);
			}
			return inArmor;
		}

		private bool findEtcItem()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			bool inEtcitem = false;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM etcitem WHERE item_id = ?");
				pstm.setInt(1, itemId);
				rs = pstm.executeQuery();
				if (rs != null)
				{
					if (rs.next())
					{
						inEtcitem = true;
						isStackable = rs.getInt("stackable") == 1 ? true : false;
					}
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				SQLUtil.close(rs, pstm, con);
			}
			return inEtcitem;
		}
	}
}