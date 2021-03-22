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
namespace LineageServer.Server.Server
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using TownTable = LineageServer.Server.Server.DataSources.TownTable;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1GameTime = LineageServer.Server.Server.Model.gametime.L1GameTime;
	using L1GameTimeAdapter = LineageServer.Server.Server.Model.gametime.L1GameTimeAdapter;
	using L1GameTimeClock = LineageServer.Server.Server.Model.gametime.L1GameTimeClock;
	using S_PacketBox = LineageServer.Server.Server.serverpackets.S_PacketBox;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;

	public class HomeTownTimeController
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(HomeTownTimeController).FullName);

		private static HomeTownTimeController _instance;

		public static HomeTownTimeController Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new HomeTownTimeController();
				}
				return _instance;
			}
		}

		private HomeTownTimeController()
		{
			startListener();
		}

		private static L1TownFixedProcListener _listener;

		private void startListener()
		{
			if (_listener == null)
			{
				_listener = new L1TownFixedProcListener(this);
				L1GameTimeClock.Instance.addListener(_listener);
			}
		}

		private class L1TownFixedProcListener : L1GameTimeAdapter
		{
			private readonly HomeTownTimeController outerInstance;

			public L1TownFixedProcListener(HomeTownTimeController outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void onDayChanged(L1GameTime time)
			{
				outerInstance.fixedProc(time);
			}
		}

		private void fixedProc(L1GameTime time)
		{
			DateTime cal = time.Calendar;
			int day = cal.Day; //Calendar.DAY_OF_WEEK 取得週幾之值

			if (day == 25)
			{
				monthlyProc();
			}
			else
			{
				dailyProc();
			}
		}

		public virtual void dailyProc()
		{
			_log.info("城鎮系統：開始處理每日事項");
			TownTable.Instance.updateTaxRate();
			TownTable.Instance.updateSalesMoneyYesterday();
			TownTable.Instance.load();
		}

		public virtual void monthlyProc()
		{
			_log.info("城鎮系統：開始處理每月事項");
			L1World.Instance.ProcessingContributionTotal = true;
			ICollection<L1PcInstance> players = L1World.Instance.AllPlayers;
			foreach (L1PcInstance pc in players)
			{
				try
				{
					// 儲存所有線上玩家的資訊
					pc.Save();
				}
				catch (Exception e)
				{
					_log.log(Enum.Level.Server, e.Message, e);
				}
			}

			for (int townId = 1; townId <= 10; townId++)
			{
				string leaderName = totalContribution(townId);
				if (!string.ReferenceEquals(leaderName, null))
				{
					S_PacketBox packet = new S_PacketBox(S_PacketBox.MSG_TOWN_LEADER, leaderName);
					foreach (L1PcInstance pc in players)
					{
						if (pc.HomeTownId == townId)
						{
							pc.Contribution = 0;
							pc.sendPackets(packet);
						}
					}
				}
			}
			TownTable.Instance.load();

			foreach (L1PcInstance pc in players)
			{
				if (pc.HomeTownId == -1)
				{
					pc.HomeTownId = 0;
				}
				pc.Contribution = 0;
				try
				{
					// 儲存所有線上玩家的資訊
					pc.Save();
				}
				catch (Exception e)
				{
					_log.log(Enum.Level.Server, e.Message, e);
				}
			}
			clearHomeTownID();
			L1World.Instance.ProcessingContributionTotal = false;
		}

		private static string totalContribution(int townId)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm1 = null;
			ResultSet rs1 = null;
			PreparedStatement pstm2 = null;
			ResultSet rs2 = null;
			PreparedStatement pstm3 = null;
			ResultSet rs3 = null;
			PreparedStatement pstm4 = null;
			PreparedStatement pstm5 = null;

			int leaderId = 0;
			string leaderName = null;

			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm1 = con.prepareStatement("SELECT objid, char_name FROM characters WHERE HomeTownID = ? ORDER BY Contribution DESC");

				pstm1.setInt(1, townId);
				rs1 = pstm1.executeQuery();

				if (rs1.next())
				{
					leaderId = rs1.getInt("objid");
					leaderName = rs1.getString("char_name");
				}

				double totalContribution = 0;
				pstm2 = con.prepareStatement("SELECT SUM(Contribution) AS TotalContribution FROM characters WHERE HomeTownID = ?");
				pstm2.setInt(1, townId);
				rs2 = pstm2.executeQuery();
				if (rs2.next())
				{
					totalContribution = rs2.getInt("TotalContribution");
				}

				double townFixTax = 0;
				pstm3 = con.prepareStatement("SELECT town_fix_tax FROM town WHERE town_id = ?");
				pstm3.setInt(1, townId);
				rs3 = pstm3.executeQuery();
				if (rs3.next())
				{
					townFixTax = rs3.getInt("town_fix_tax");
				}

				double contributionUnit = 0;
				if (totalContribution != 0)
				{
					contributionUnit = Math.Floor(townFixTax / totalContribution * 100) / 100;
				}
				pstm4 = con.prepareStatement("UPDATE characters SET Contribution = 0, Pay = Contribution * ? WHERE HomeTownID = ?");
				pstm4.setDouble(1, contributionUnit);
				pstm4.setInt(2, townId);
				pstm4.execute();

				pstm5 = con.prepareStatement("UPDATE town SET leader_id = ?, leader_name = ?, tax_rate = 0, tax_rate_reserved = 0, sales_money = 0, sales_money_yesterday = sales_money, town_tax = 0, town_fix_tax = 0 WHERE town_id = ?");
				pstm5.setInt(1, leaderId);
				pstm5.setString(2, leaderName);
				pstm5.setInt(3, townId);
				pstm5.execute();
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(rs1);
				SQLUtil.close(rs2);
				SQLUtil.close(rs3);
				SQLUtil.close(pstm1);
				SQLUtil.close(pstm2);
				SQLUtil.close(pstm3);
				SQLUtil.close(pstm4);
				SQLUtil.close(pstm5);
				SQLUtil.close(con);
			}

			return leaderName;
		}

		private static void clearHomeTownID()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;

			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("UPDATE characters SET HomeTownID = 0 WHERE HomeTownID = -1");
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

		/// <summary>
		/// 取得報酬
		/// </summary>
		/// <returns> int 報酬 </returns>
		public static int getPay(int objid)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm1 = null;
			PreparedStatement pstm2 = null;
			ResultSet rs1 = null;
			int pay = 0;

			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm1 = con.prepareStatement("SELECT Pay FROM characters WHERE objid = ? FOR UPDATE");

				pstm1.setInt(1, objid);
				rs1 = pstm1.executeQuery();

				if (rs1.next())
				{
					pay = rs1.getInt("Pay");
				}

				pstm2 = con.prepareStatement("UPDATE characters SET Pay = 0 WHERE objid = ?");
				pstm2.setInt(1, objid);
				pstm2.execute();
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(rs1);
				SQLUtil.close(pstm1);
				SQLUtil.close(pstm2);
				SQLUtil.close(con);
			}

			return pay;
		}
	}

}