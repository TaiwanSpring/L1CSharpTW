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
namespace LineageServer.Server.Server.DataSources
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using SQLUtil = LineageServer.Server.Server.Utils.SQLUtil;
	using Maps = LineageServer.Server.Server.Utils.collections.Maps;

	public sealed class DropItemTable
	{
		private class dropItemData
		{
			private readonly DropItemTable outerInstance;

			public dropItemData(DropItemTable outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public double dropRate = 1;

			public double dropAmount = 1;
		}

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(DropItemTable).FullName);

		private static DropItemTable _instance;

		private readonly IDictionary<int, dropItemData> _dropItem = Maps.newMap();

		public static DropItemTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new DropItemTable();
				}
				return _instance;
			}
		}

		private DropItemTable()
		{
			loadMapsFromDatabase();
		}

		private void loadMapsFromDatabase()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM drop_item");

				for (rs = pstm.executeQuery(); rs.next();)
				{
					dropItemData data = new dropItemData(this);
					int itemId = dataSourceRow.getInt("item_id");
					data.dropRate = dataSourceRow.getDouble("drop_rate");
					data.dropAmount = dataSourceRow.getDouble("drop_amount");

					_dropItem[itemId] = data;
				}

				_log.config("drop_item " + _dropItem.Count);
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

		public double getDropRate(int itemId)
		{
			dropItemData data = _dropItem[itemId];
			if (data == null)
			{
				return 1;
			}
			return data.dropRate;
		}

		public double getDropAmount(int itemId)
		{
			dropItemData data = _dropItem[itemId];
			if (data == null)
			{
				return 1;
			}
			return data.dropAmount;
		}

	}

}