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
namespace LineageServer.Server.Templates
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using IdFactory = LineageServer.Server.IdFactory;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using S_Bookmarks = LineageServer.Serverpackets.S_Bookmarks;
	using S_ServerMessage = LineageServer.Serverpackets.S_ServerMessage;
	using SQLUtil = LineageServer.Utils.SQLUtil;

	public class L1BookMark
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(L1BookMark).FullName);

		private int _charId;

		private int _id;

		private string _name;

		private int _locX;

		private int _locY;

		private short _mapId;

		public L1BookMark()
		{
		}

		public static void deleteBookmark(L1PcInstance player, string s)
		{
			L1BookMark book = player.getBookMark(s);
			if (book != null)
			{
				IDataBaseConnection con = null;
				PreparedStatement pstm = null;
				try
				{

					con = L1DatabaseFactory.Instance.Connection;
					pstm = con.prepareStatement("DELETE FROM character_teleport WHERE id=?");
					pstm.setInt(1, book.Id);
					pstm.execute();
					player.removeBookMark(book);
				}
				catch (SQLException e)
				{
					_log.log(Enum.Level.Server, "ブックマークの削除でエラーが発生しました。", e);
				}
				finally
				{
					SQLUtil.close(pstm);
					SQLUtil.close(con);
				}
			}
		}

		public static void addBookmark(L1PcInstance pc, string s)
		{
			// クライアント側でチェックされるため不要
	//		if (s.length() > 12) {
	//			pc.sendPackets(new S_ServerMessage(204));
	//			return;
	//		}

			if (!pc.Map.Markable)
			{
				pc.sendPackets(new S_ServerMessage(214)); // \f1ここを記憶することができません。
				return;
			}

			int size = pc.BookMarkSize;
			if (size > 49)
			{
				return;
			}

			if (pc.getBookMark(s) == null)
			{
				L1BookMark bookmark = new L1BookMark();
				bookmark.Id = Container.Instance.Resolve<IIdFactory>().nextId();
				bookmark.CharId = pc.Id;
				bookmark.Name = s;
				bookmark.LocX = pc.X;
				bookmark.LocY = pc.Y;
				bookmark.MapId = pc.MapId;

				IDataBaseConnection con = null;
				PreparedStatement pstm = null;

				try
				{
					con = L1DatabaseFactory.Instance.Connection;
					pstm = con.prepareStatement("INSERT INTO character_teleport SET id = ?, char_id = ?, name = ?, locx = ?, locy = ?, mapid = ?");
					pstm.setInt(1, bookmark.Id);
					pstm.setInt(2, bookmark.CharId);
					pstm.setString(3, bookmark.Name);
					pstm.setInt(4, bookmark.LocX);
					pstm.setInt(5, bookmark.LocY);
					pstm.setInt(6, bookmark.MapId);
					pstm.execute();
				}
				catch (SQLException e)
				{
					_log.log(Enum.Level.Server, "ブックマークの追加でエラーが発生しました。", e);
				}
				finally
				{
					SQLUtil.close(pstm);
					SQLUtil.close(con);
				}

				pc.addBookMark(bookmark);
				pc.sendPackets(new S_Bookmarks(s, bookmark.MapId, bookmark.Id, bookmark.LocX, bookmark.LocY));
			}
			else
			{
				pc.sendPackets(new S_ServerMessage(327)); // 同じ名前がすでに存在しています。
			}
		}

		public virtual int Id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}


		public virtual int CharId
		{
			get
			{
				return _charId;
			}
			set
			{
				_charId = value;
			}
		}


		public virtual string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}


		public virtual int LocX
		{
			get
			{
				return _locX;
			}
			set
			{
				_locX = value;
			}
		}


		public virtual int LocY
		{
			get
			{
				return _locY;
			}
			set
			{
				_locY = value;
			}
		}


		public virtual short MapId
		{
			get
			{
				return _mapId;
			}
			set
			{
				_mapId = value;
			}
		}

	}
}