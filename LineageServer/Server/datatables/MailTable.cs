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
	using IdFactory = LineageServer.Server.IdFactory;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using L1Mail = LineageServer.Server.Templates.L1Mail;
	using SQLUtil = LineageServer.Utils.SQLUtil;
	using ListFactory = LineageServer.Utils.ListFactory;

	// Referenced classes of package l1j.server.server:
	// IdFactory

	public class MailTable
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(MailTable).FullName);

		private static MailTable _instance;

		private static IList<L1Mail> _allMail = ListFactory.newList();

		public static MailTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new MailTable();
				}
				return _instance;
			}
		}

		private MailTable()
		{
			loadMail();
		}

		private void loadMail()
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM mail");
				rs = pstm.executeQuery();
				while (rs.next())
				{
					L1Mail mail = new L1Mail();
					mail.Id = dataSourceRow.getInt("id");
					mail.Type = dataSourceRow.getInt("type");
					mail.SenderName = dataSourceRow.getString("sender");
					mail.ReceiverName = dataSourceRow.getString("receiver");
					mail.Date = dataSourceRow.getTimestamp("date");
					mail.ReadStatus = dataSourceRow.getInt("read_status");
					mail.Subject = dataSourceRow.getBytes("subject");
					mail.Content = dataSourceRow.getBytes("content");
					mail.InBoxId = dataSourceRow.getInt("inbox_id");

					_allMail.Add(mail);
				}
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, "error while creating mail table", e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		public virtual int ReadStatus
		{
			set
			{
				IDataBaseConnection con = null;
				PreparedStatement pstm = null;
				ResultSet rs = null;
				try
				{
					con = L1DatabaseFactory.Instance.Connection;
					rs = con.createStatement().executeQuery("SELECT * FROM mail WHERE id=" + value);
					if ((rs != null) && rs.next())
					{
						pstm = con.prepareStatement("UPDATE mail SET read_status=? WHERE id=" + value);
						pstm.setInt(1, 1);
						pstm.execute();
    
						changeMailStatus(value);
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

		public virtual void setMailType(int mailId, int type)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				rs = con.createStatement().executeQuery("SELECT * FROM mail WHERE id=" + mailId);
				if ((rs != null) && rs.next())
				{
					pstm = con.prepareStatement("UPDATE mail SET type=? WHERE id=" + mailId);
					pstm.setInt(1, type);
					pstm.execute();

					changeMailType(mailId, type);
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

		public virtual void deleteMail(int mailId)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("DELETE FROM mail WHERE id=?");
				pstm.setInt(1, mailId);
				pstm.execute();

				delMail(mailId);
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

		public virtual int writeMail(int type, string receiver, L1PcInstance writer, sbyte[] text, int inboxId)
		{
			Timestamp date = new Timestamp(DateTimeHelper.CurrentUnixTimeMillis());
			int readStatus = 0;
			int id = 0;

			// subjectとcontentの区切り(0x00 0x00)位置を見つける
			int spacePosition1 = 0;
			int spacePosition2 = 0;
			for (int i = 0; i < text.Length; i += 2)
			{
				if ((text[i] == 0) && (text[i + 1] == 0))
				{
					if (spacePosition1 == 0)
					{
						spacePosition1 = i;
					}
					else if ((spacePosition1 != 0) && (spacePosition2 == 0))
					{
						spacePosition2 = i;
						break;
					}
				}
			}

			// mailテーブルに書き込む
			int subjectLength = spacePosition1 + 2;
			int contentLength = spacePosition2 - spacePosition1;
			if (contentLength <= 0)
			{
				contentLength = 1;
			}
			sbyte[] subject = new sbyte[subjectLength];
			sbyte[] content = new sbyte[contentLength];
			Array.Copy(text, 0, subject, 0, subjectLength);
			Array.Copy(text, subjectLength, content, 0, contentLength);

			IDataBaseConnection con = null;
			PreparedStatement pstm2 = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm2 = con.prepareStatement("INSERT INTO mail SET " + "id=?, type=?, sender=?, receiver=?," + " date=?, read_status=?, subject=?, content=?, inbox_id=?");
				id = IdFactory.Instance.nextId();
				pstm2.setInt(1, id);
				pstm2.setInt(2, type);
				pstm2.setString(3, writer.Name);
				pstm2.setString(4, receiver);
				pstm2.setTimestamp(5, date);
				pstm2.setInt(6, readStatus);
				pstm2.setBytes(7, subject);
				pstm2.setBytes(8, content);
				pstm2.setInt(9, inboxId);
				pstm2.execute();

				L1Mail mail = new L1Mail();
				mail.Id = id;
				mail.Type = type;
				mail.SenderName = writer.Name;
				mail.ReceiverName = receiver;
				mail.Date = date;
				mail.Subject = subject;
				mail.Content = content;
				mail.ReadStatus = readStatus;
				mail.InBoxId = inboxId;

				_allMail.Add(mail);
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(pstm2);
				SQLUtil.close(con);
			}
			return id;
		}

		public static IList<L1Mail> AllMail
		{
			get
			{
				return _allMail;
			}
		}

		public static L1Mail getMail(int mailId)
		{
			foreach (L1Mail mail in _allMail)
			{
				if (mail.Id == mailId)
				{
					return mail;
				}
			}
			return null;
		}

		private void changeMailStatus(int mailId)
		{
			foreach (L1Mail mail in _allMail)
			{
				if (mail.Id == mailId)
				{
					L1Mail newMail = mail;
					newMail.ReadStatus = 1;

					_allMail.Remove(mail);
					_allMail.Add(newMail);
					break;
				}
			}
		}

		private void changeMailType(int mailId, int type)
		{
			foreach (L1Mail mail in _allMail)
			{
				if (mail.Id == mailId)
				{
					L1Mail newMail = mail;
					newMail.Type = type;

					_allMail.Remove(mail);
					_allMail.Add(newMail);
					break;
				}
			}
		}

		private void delMail(int mailId)
		{
			foreach (L1Mail mail in _allMail)
			{
				if (mail.Id == mailId)
				{
					_allMail.Remove(mail);
					break;
				}
			}
		}

	}

}