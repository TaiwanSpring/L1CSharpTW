using System;
using System.Collections.Generic;
using System.Linq;

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
	using L1Castle = LineageServer.Server.Server.Templates.L1Castle;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	// Referenced classes of package l1j.server.server:
	// IdFactory

	public class CastleTable
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(CastleTable).FullName);

		private static CastleTable _instance;

		private readonly IDictionary<int, L1Castle> _castles = Maps.newConcurrentMap();

		public static CastleTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new CastleTable();
				}
				return _instance;
			}
		}

		private CastleTable()
		{
			load();
		}

		private DateTime timestampToCalendar(Timestamp ts)
		{
			DateTime cal = new DateTime();
			cal.TimeInMillis = ts.Time;
			return cal;
		}

		private void load()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM castle ORDER BY castle_id ASC");

				rs = pstm.executeQuery();

				while (rs.next())
				{
					L1Castle castle = new L1Castle(rs.getInt(1), rs.getString(2));
					castle.WarTime = timestampToCalendar((Timestamp) rs.getObject(3));
					castle.TaxRate = rs.getInt(4);
					castle.PublicMoney = rs.getInt(5);

					/// <summary>
					/// 設置擁有該城堡的血盟 </summary>
					pstm = con.prepareStatement("SELECT clan_id FROM clan_data WHERE hascastle = ?");
					pstm.setInt(1, castle.Id);
					ResultSet rstemp = pstm.executeQuery();

					while (rstemp.next())
					{
						castle.HeldClan = rstemp.getInt(1);
					}

					_castles[castle.Id] = castle;
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

		public virtual L1Castle[] CastleTableList
		{
			get
			{
				return _castles.Values.ToArray();
			}
		}

		public virtual L1Castle getCastleTable(int id)
		{
			return _castles[id];
		}

		public virtual void updateCastle(L1Castle castle)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("UPDATE castle SET name=?, war_time=?, tax_rate=?, public_money=? WHERE castle_id=?");
				pstm.setString(1, castle.Name);
				SimpleDateFormat sdf = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss");
				string fm = sdf.format(castle.WarTime);
				pstm.setString(2, fm);
				pstm.setInt(3, castle.TaxRate);
				pstm.setInt(4, castle.PublicMoney);
				pstm.setInt(5, castle.Id);
				pstm.execute();

				_castles[castle.Id] = castle;
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