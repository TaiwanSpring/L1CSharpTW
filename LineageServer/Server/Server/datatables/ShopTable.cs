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
namespace LineageServer.Server.Server.datatables
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1Shop = LineageServer.Server.Server.Model.shop.L1Shop;
	using L1ShopItem = LineageServer.Server.Server.Templates.L1ShopItem;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	public class ShopTable
	{

		private const long serialVersionUID = 1L;

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(ShopTable).FullName);

		private static ShopTable _instance;

		private readonly IDictionary<int, L1Shop> _allShops = Maps.newMap();

		public static ShopTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ShopTable();
				}
				return _instance;
			}
		}

		private ShopTable()
		{
			loadShops();
		}

		private IList<int> enumNpcIds()
		{
			IList<int> ids = Lists.newList();

			Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT DISTINCT npc_id FROM shop");
				rs = pstm.executeQuery();
				while (rs.next())
				{
					ids.Add(rs.getInt("npc_id"));
				}
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(rs, pstm, con);
			}
			return ids;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private l1j.server.server.model.shop.L1Shop loadShop(int npcId, java.sql.ResultSet rs) throws java.sql.SQLException
		private L1Shop loadShop(int npcId, ResultSet rs)
		{
			IList<L1ShopItem> sellingList = Lists.newList();
			IList<L1ShopItem> purchasingList = Lists.newList();
			while (rs.next())
			{
				int itemId = rs.getInt("item_id");
				int sellingPrice = rs.getInt("selling_price");
				int purchasingPrice = rs.getInt("purchasing_price");
				int packCount = rs.getInt("pack_count");
				packCount = packCount == 0 ? 1 : packCount;
				if (0 <= sellingPrice)
				{
					L1ShopItem item = new L1ShopItem(itemId, sellingPrice, packCount);
					sellingList.Add(item);
				}
				if (0 <= purchasingPrice)
				{
					L1ShopItem item = new L1ShopItem(itemId, purchasingPrice, packCount);
					purchasingList.Add(item);
				}
			}
			return new L1Shop(npcId, sellingList, purchasingList);
		}

		private void loadShops()
		{
			Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM shop WHERE npc_id=? ORDER BY order_id");
				foreach (int npcId in enumNpcIds())
				{
					pstm.setInt(1, npcId);
					rs = pstm.executeQuery();
					L1Shop shop = loadShop(npcId, rs);
					_allShops[npcId] = shop;
					rs.close();
				}
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(rs, pstm, con);
			}
		}

		public virtual L1Shop get(int npcId)
		{
			return _allShops[npcId];
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