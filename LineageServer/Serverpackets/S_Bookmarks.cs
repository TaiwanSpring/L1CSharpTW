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
namespace LineageServer.Serverpackets
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using Opcodes = LineageServer.Server.Opcodes;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using L1BookMark = LineageServer.Server.Templates.L1BookMark;
	using SQLUtil = LineageServer.Utils.SQLUtil;

	public class S_Bookmarks : ServerBasePacket
	{
		private const string _S__1F_S_Bookmarks = "[S] S_Bookmarks";

		private byte[] _byte = null;

		public S_Bookmarks(string name, int map, int id, int x, int y)
		{
			buildPacket(name, map, id, x, y);
		}

		/// <summary>
		/// 角色重登載入 </summary>
		/// <param name="pc"> </param>
		public S_Bookmarks(L1PcInstance pc)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM character_teleport WHERE char_id=? ORDER BY name ASC");
				pstm.setInt(1, pc.Id);
				rs = pstm.executeQuery();
				rs.last(); // 為了取得總列數，先將指標拉到最後
				int rowcount = rs.Row; // 取得總列數
				rs.beforeFirst(); // 將指標移回最前頭
				WriteC(Opcodes.S_OPCODE_CHARRESET);
				WriteC(0x2a);
				WriteC(0x80);
				WriteC(0x00);
				WriteC(0x02);
				for (int i = 0; i <= 126 ; i++)
				{
					if (i < rowcount)
					{
						WriteC(i);
					}
					else
					{
						WriteC(0x00);
					}
				}
				WriteC(0x3c);
				WriteC(0);
				WriteC(rowcount);
				WriteC(0);

				while (rs.next())
				{
					L1BookMark bookmark = new L1BookMark();
					bookmark.Id = dataSourceRow.getInt("id");
					bookmark.CharId = dataSourceRow.getInt("char_id");
					bookmark.Name = dataSourceRow.getString("name");
					bookmark.LocX = dataSourceRow.getInt("locx");
					bookmark.LocY = dataSourceRow.getInt("locy");
					bookmark.MapId = dataSourceRow.getShort("mapid");
					WriteH(bookmark.LocX);
					WriteH(bookmark.LocY);
					WriteS(bookmark.Name);
					WriteH(bookmark.MapId);
					WriteD(bookmark.Id);
					pc.addBookMark(bookmark);
				}

			}
			catch (SQLException e)
			{
                System.Console.WriteLine(e.Message);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
		}

		private void buildPacket(string name, int map, int id, int x, int y)
		{
			WriteC(Opcodes.S_OPCODE_BOOKMARKS);
			WriteS(name);
			WriteH(map);
			WriteD(id);
			WriteH(x);
			WriteH(y);
		}



		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = Bytes;
				}
    
				return _byte;
			}
		}

		public override string Type
		{
			get
			{
				return _S__1F_S_Bookmarks;
			}
		}
	}
}