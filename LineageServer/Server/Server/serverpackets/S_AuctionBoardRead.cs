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
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_AuctionBoardRead : ServerBasePacket
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(S_AuctionBoardRead).FullName);
		private const string S_AUCTIONBOARDREAD = "[S] S_AuctionBoardRead";
		private byte[] _byte = null;

		public S_AuctionBoardRead(int objectId, string house_number)
		{
			buildPacket(objectId, house_number);
		}

		private void buildPacket(int objectId, string house_number)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				int number = Convert.ToInt32(house_number);
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM board_auction WHERE house_id=?");
				pstm.setInt(1, number);
				rs = pstm.executeQuery();
				while (rs.next())
				{
					writeC(Opcodes.S_OPCODE_SHOWHTML);
					writeD(objectId);
					writeS("agsel");
					writeS(house_number); // アジトの番号
					writeH(9); // 以下の文字列の個数
					writeS(rs.getString(2)); // アジトの名前
					writeS(rs.getString(6)); // アジトの位置
					writeS(rs.getString(3).ToString()); // アジトの広さ
					writeS(rs.getString(7)); // 以前の所有者
					writeS(rs.getString(9)); // 現在の入札者
					writeS(rs.getInt(5).ToString()); // 現在の入札価格
					DateTime cal = timestampToCalendar((Timestamp) rs.getObject(4));
					int month = cal.Month + 1;
					int day = cal.Day;
					int hour = cal.Hour;
					writeS(month.ToString()); // 締切月
					writeS(day.ToString()); // 締切日
					writeS(hour.ToString()); // 締切時
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

		private DateTime timestampToCalendar(Timestamp ts)
		{
			DateTime cal = new DateTime();
			cal.TimeInMillis = ts.Time;
			return cal;
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
				return S_AUCTIONBOARDREAD;
			}
		}
	}

}