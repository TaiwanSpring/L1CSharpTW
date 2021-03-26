using System.Collections.Generic;

namespace LineageServer.william
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using SQLUtil = LineageServer.Server.Server.Utils.SQLUtil;
	public class ItemPrice
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(ItemPrice).FullName);

		private static ItemPrice _instance;

		private readonly Dictionary<int, L1WilliamItemPrice> _itemIdIndex = new Dictionary<int, L1WilliamItemPrice>();

		public static ItemPrice Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ItemPrice();
				}
				return _instance;
			}
		}

		private ItemPrice()
		{
			loadItemPrice();
		}

		private void loadItemPrice()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM william_item_price");
				rs = pstm.executeQuery();
				fillItemPrice(rs);
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, "error while creating william_item_price table", e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private void fillItemPrice(java.sql.ResultSet rs) throws java.sql.SQLException
		private void fillItemPrice(ResultSet rs)
		{
			while (rs.next())
			{
				int itemId = dataSourceRow.getInt("item_id");
				int price = dataSourceRow.getInt("price");

				L1WilliamItemPrice item_price = new L1WilliamItemPrice(itemId, price);
				_itemIdIndex[itemId] = item_price;
			}
		}

		public virtual L1WilliamItemPrice getTemplate(int itemId)
		{
			return _itemIdIndex[itemId];
		}
	}

}