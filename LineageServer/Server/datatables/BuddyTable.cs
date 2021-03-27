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
namespace LineageServer.Server.DataSources
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using L1Buddy = LineageServer.Server.Model.L1Buddy;
	using SQLUtil = LineageServer.Utils.SQLUtil;
	using MapFactory = LineageServer.Utils.MapFactory;

	// import l1j.server.server.model.Instance.L1PcInstance;

	// Referenced classes of package l1j.server.server:
	// IdFactory

	public class BuddyTable
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(BuddyTable).FullName);

		private static BuddyTable _instance;

		private readonly IDictionary<int, L1Buddy> _buddys = MapFactory.newMap();

		public static BuddyTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new BuddyTable();
				}
				return _instance;
			}
		}

		private BuddyTable()
		{

			IDataBaseConnection con = null;
			PreparedStatement charIdPS = null;
			ResultSet charIdRS = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				charIdPS = con.prepareStatement("SELECT distinct(char_id) as char_id FROM character_buddys");

				charIdRS = charIdPS.executeQuery();
				while (charIdRS.next())
				{
					PreparedStatement buddysPS = null;
					ResultSet buddysRS = null;

					try
					{
						buddysPS = con.prepareStatement("SELECT buddy_id, buddy_name FROM character_buddys WHERE char_id = ?");
						int charId = charIddataSourceRow.getInt("char_id");
						buddysPS.setInt(1, charId);
						L1Buddy buddy = new L1Buddy(charId);

						buddysRS = buddysPS.executeQuery();
						while (buddysRS.next())
						{
							buddy.add(buddysdataSourceRow.getInt("buddy_id"), buddysdataSourceRow.getString("buddy_name"));
						}

						_buddys[buddy.CharId] = buddy;
					}
					catch (Exception e)
					{
						_log.Error(e);
					}
					finally
					{
						SQLUtil.close(buddysRS);
						SQLUtil.close(buddysPS);
					}
				}
				_log.config("loaded " + _buddys.Count + " character's buddylists");
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(charIdRS);
				SQLUtil.close(charIdPS);
				SQLUtil.close(con);
			}
		}

		public virtual L1Buddy getBuddyTable(int charId)
		{
			L1Buddy buddy = _buddys[charId];
			if (buddy == null)
			{
				buddy = new L1Buddy(charId);
				_buddys[charId] = buddy;
			}
			return buddy;
		}

		public virtual void addBuddy(int charId, int objId, string name)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{

				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("INSERT INTO character_buddys SET char_id=?, buddy_id=?, buddy_name=?");
				pstm.setInt(1, charId);
				pstm.setInt(2, objId);
				pstm.setString(3, name);
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

		public virtual void removeBuddy(int charId, string buddyName)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			L1Buddy buddy = getBuddyTable(charId);
			if (!buddy.containsName(buddyName))
			{
				return;
			}

			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("DELETE FROM character_buddys WHERE char_id=? AND buddy_name=?");
				pstm.setInt(1, charId);
				pstm.setString(2, buddyName);
				pstm.execute();

				buddy.remove(buddyName);
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