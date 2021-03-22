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

	using Config = LineageServer.Server.Config;
	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using ClientThread = LineageServer.Server.Server.ClientThread;
	using CharacterTable = LineageServer.Server.Server.DataSources.CharacterTable;
	using S_CharAmount = LineageServer.Server.Server.serverpackets.S_CharAmount;
	using S_CharPacks = LineageServer.Server.Server.serverpackets.S_CharPacks;
	using S_CharSynAck = LineageServer.Server.Server.serverpackets.S_CharSynAck;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;

	/// <summary>
	/// 角色列表 
	/// 3.70C中，由於不再有登入公告，正式更名。
	/// 處理自動登入後的角色列表封包
	/// </summary>
	public class L1CharList
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1CharList).FullName);

		public L1CharList(ClientThread client)
		{
			deleteCharacter(client); // 到達刪除期限，刪除角色
			int amountOfChars = client.Account.countCharacters();
			client.SendPacket(new S_CharAmount(amountOfChars, client));
			client.SendPacket(new S_CharSynAck(S_CharSynAck.SYN));
			if (amountOfChars > 0)
			{
				sendCharPacks(client);
			}
			client.SendPacket(new S_CharSynAck(S_CharSynAck.ACK));
		}

		private void deleteCharacter(ClientThread client)
		{
			IDataBaseConnection conn = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{

				conn = L1DatabaseFactory.Instance.Connection;
				pstm = conn.prepareStatement("SELECT * FROM characters WHERE account_name=? ORDER BY objid");
				pstm.setString(1, client.AccountName);
				rs = pstm.executeQuery();

				while (rs.next())
				{
					string name = rs.getString("char_name");
					string clanname = rs.getString("Clanname");

					Timestamp deleteTime = rs.getTimestamp("DeleteTime");
					if (deleteTime != null)
					{
						DateTime cal = new DateTime();
						long checkDeleteTime = ((cal.Ticks - deleteTime.Time) / 1000) / 3600;
						if (checkDeleteTime >= 0)
						{
							L1Clan clan = L1World.Instance.getClan(clanname);
							if (clan != null)
							{
								clan.delMemberName(name);
							}
							CharacterTable.Instance.deleteCharacter(client.AccountName, name);
						}
					}
				}
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(conn);
			}
		}

		private void sendCharPacks(ClientThread client)
		{
			IDataBaseConnection conn = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{

				conn = L1DatabaseFactory.Instance.Connection;
				pstm = conn.prepareStatement("SELECT * FROM characters WHERE account_name=? ORDER BY objid");
				pstm.setString(1, client.AccountName);
				rs = pstm.executeQuery();

				while (rs.next())
				{
					string name = rs.getString("char_name");
					string clanname = rs.getString("Clanname");
					int type = rs.getInt("Type");
					sbyte sex = rs.getByte("Sex");
					int lawful = rs.getInt("Lawful");

					int currenthp = rs.getInt("CurHp");
					if (currenthp < 1)
					{
						currenthp = 1;
					}
					else if (currenthp > 32767)
					{
						currenthp = 32767;
					}

					int currentmp = rs.getInt("CurMp");
					if (currentmp < 1)
					{
						currentmp = 1;
					}
					else if (currentmp > 32767)
					{
						currentmp = 32767;
					}

					int lvl;
					if (Config.CHARACTER_CONFIG_IN_SERVER_SIDE)
					{
						lvl = rs.getInt("level");
						if (lvl < 1)
						{
							lvl = 1;
						}
						else if (lvl > 127)
						{
							lvl = 127;
						}
					}
					else
					{
						lvl = 1;
					}

					int ac = rs.getInt("Ac");
					int str = rs.getByte("Str");
					int dex = rs.getByte("Dex");
					int con = rs.getByte("Con");
					int wis = rs.getByte("Wis");
					int cha = rs.getByte("Cha");
					int intel = rs.getByte("Intel");
					int accessLevel = rs.getShort("AccessLevel");
					Timestamp _birthday = (Timestamp) rs.getTimestamp("birthday");
					SimpleDateFormat SimpleDate = new SimpleDateFormat("yyyyMMdd");
					int birthday = int.Parse(SimpleDate.format(_birthday.Time));

					S_CharPacks cpk = new S_CharPacks(name, clanname, type, sex, lawful, currenthp, currentmp, ac, lvl, str, dex, con, wis, cha, intel, accessLevel, birthday);

					client.SendPacket(cpk);
				}
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(conn);
			}
		}
	}
}