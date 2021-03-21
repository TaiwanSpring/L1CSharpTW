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
namespace LineageServer.Server.Server.serverpackets
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using Opcodes = LineageServer.Server.Server.Opcodes;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1BookMark = LineageServer.Server.Server.Templates.L1BookMark;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;

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
				writeC(Opcodes.S_OPCODE_CHARRESET);
				writeC(0x2a);
				writeC(0x80);
				writeC(0x00);
				writeC(0x02);
				for (int i = 0; i <= 126 ; i++)
				{
					if (i < rowcount)
					{
						writeC(i);
					}
					else
					{
						writeC(0x00);
					}
				}
				writeC(0x3c);
				writeC(0);
				writeC(rowcount);
				writeC(0);

				while (rs.next())
				{
					L1BookMark bookmark = new L1BookMark();
					bookmark.Id = rs.getInt("id");
					bookmark.CharId = rs.getInt("char_id");
					bookmark.Name = rs.getString("name");
					bookmark.LocX = rs.getInt("locx");
					bookmark.LocY = rs.getInt("locy");
					bookmark.MapId = rs.getShort("mapid");
					writeH(bookmark.LocX);
					writeH(bookmark.LocY);
					writeS(bookmark.Name);
					writeH(bookmark.MapId);
					writeD(bookmark.Id);
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
			writeC(Opcodes.S_OPCODE_BOOKMARKS);
			writeS(name);
			writeH(map);
			writeD(id);
			writeH(x);
			writeH(y);
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