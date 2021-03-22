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
	using L1RaceTicket = LineageServer.Server.Server.Templates.L1RaceTicket;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;

	// Referenced classes of package l1j.server.server:
	// IdFactory

	public class RaceTicketTable
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(PetTable).FullName);

		private static RaceTicketTable _instance;

		private readonly Dictionary<int, L1RaceTicket> _tickets = new Dictionary<int, L1RaceTicket>();

		private int _maxRoundNumber;

		public static RaceTicketTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new RaceTicketTable();
				}
				return _instance;
			}
		}

		private RaceTicketTable()
		{
			load();
		}

		private void load()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM race_ticket");
				int temp = 0;
				rs = pstm.executeQuery();
				while (rs.next())
				{
					L1RaceTicket ticket = new L1RaceTicket();
					int itemobjid = rs.getInt(1);
					ticket.set_itemobjid(itemobjid);
					ticket.set_round(rs.getInt(2));
					ticket.set_allotment_percentage(rs.getInt(3));
					ticket.set_victory(rs.getInt(4));
					ticket.set_runner_num(rs.getInt(5));

					if (ticket.get_round() > temp)
					{
						temp = ticket.get_round();
					}
					_tickets[itemobjid] = ticket;
				}
				_maxRoundNumber = temp;
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

		public virtual void storeNewTiket(L1RaceTicket ticket)
		{
			// PCのインベントリーが増える場合に実行
			// XXX 呼ばれる前と処理の重複
			if (ticket.get_itemobjid() != 0)
			{
				_tickets[ticket.get_itemobjid()] = ticket;
			}

			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("INSERT INTO race_ticket SET item_obj_id=?,round=?," + "allotment_percentage=?,victory=?,runner_num=?");
				pstm.setInt(1, ticket.get_itemobjid());
				pstm.setInt(2, ticket.get_round());
				pstm.setDouble(3, ticket.get_allotment_percentage());
				pstm.setInt(4, ticket.get_victory());
				pstm.setInt(5, ticket.get_runner_num());
				pstm.execute();
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);

			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);

			}
		}

		public virtual void deleteTicket(int itemobjid)
		{
			// PCのインベントリーが減少する再に使用
			if (_tickets.ContainsKey(itemobjid))
			{
				_tickets.Remove(itemobjid);
			}
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("delete from race_ticket WHERE item_obj_id=?");
				pstm.setInt(1, itemobjid);
				pstm.execute();
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		public virtual void oldTicketDelete(int round)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("delete from race_ticket WHERE item_obj_id=0 and round!=?");
				pstm.setInt(1, round);
				pstm.execute();
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		public virtual void updateTicket(int round, int num, double allotment_percentage)
		{
			foreach (L1RaceTicket ticket in RaceTicketTableList)
			{
				if (ticket.get_round() == round && ticket.get_runner_num() == num)
				{
					ticket.set_victory(1);
					ticket.set_allotment_percentage(allotment_percentage);
				}
			}
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("UPDATE " + "race_ticket SET victory=? ,allotment_percentage=? WHERE round=? and runner_num=?");

				pstm.setInt(1, 1);
				pstm.setDouble(2, allotment_percentage);
				pstm.setInt(3, round);
				pstm.setInt(4, num);
				pstm.execute();
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		public virtual L1RaceTicket getTemplate(int itemobjid)
		{
			if (_tickets.ContainsKey(itemobjid))
			{
				return _tickets[itemobjid];
			}
			return null;
		}

		public virtual L1RaceTicket[] RaceTicketTableList
		{
			get
			{
				return _tickets.Values.ToArray();
			}
		}

		public virtual int RoundNumOfMax
		{
			get
			{
				return _maxRoundNumber;
			}
		}
	}

}