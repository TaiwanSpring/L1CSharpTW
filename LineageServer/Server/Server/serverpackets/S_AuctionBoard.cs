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
namespace LineageServer.Server.Server.serverpackets
{

	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using Opcodes = LineageServer.Server.Server.Opcodes;
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_AuctionBoard : ServerBasePacket
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(S_AuctionBoard).FullName);

		private const string S_AUCTIONBOARD = "[S] S_AuctionBoard";

		private byte[] _byte = null;

		public S_AuctionBoard(L1NpcInstance board)
		{
			buildPacket(board);
		}

		private void buildPacket(L1NpcInstance board)
		{
			IList<int> houseList = Lists.newList();
			int houseId = 0;
			int count = 0;
			int[] id = null;
			string[] name = null;
			int[] area = null;
			int[] month = null;
			int[] day = null;
			int[] price = null;
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;

			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM board_auction");
				rs = pstm.executeQuery();
				while (rs.next())
				{
					houseId = rs.getInt(1);
					if ((board.X == 33421) && (board.Y == 32823))
					{ // 競売掲示板(ギラン)
						if ((houseId >= 262145) && (houseId <= 262189))
						{
							houseList.Add(houseId);
							count++;
						}
					}
					else if ((board.X == 33585) && (board.Y == 33235))
					{ // 競売掲示板(ハイネ)
						if ((houseId >= 327681) && (houseId <= 327691))
						{
							houseList.Add(houseId);
							count++;
						}
					}
					else if ((board.X == 33959) && (board.Y == 33253))
					{ // 競売掲示板(アデン)
						if ((houseId >= 458753) && (houseId <= 458819))
						{
							houseList.Add(houseId);
							count++;
						}
					}
					else if ((board.X == 32611) && (board.Y == 32775))
					{ // 競売掲示板(グルーディン)
						if ((houseId >= 524289) && (houseId <= 524294))
						{
							houseList.Add(houseId);
							count++;
						}
					}
				}
				id = new int[count];
				name = new string[count];
				area = new int[count];
				month = new int[count];
				day = new int[count];
				price = new int[count];

				for (int i = 0; i < count; ++i)
				{
					pstm = con.prepareStatement("SELECT * FROM board_auction WHERE house_id=?");
					houseId = houseList[i];
					pstm.setInt(1, houseId);
					rs = pstm.executeQuery();
					while (rs.next())
					{
						id[i] = rs.getInt(1);
						name[i] = rs.getString(2);
						area[i] = rs.getInt(3);
						DateTime cal = timestampToCalendar((Timestamp) rs.getObject(4));
						month[i] = cal.Month + 1;
						day[i] = cal.Day;
						price[i] = rs.getInt(5);
					}
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

			writeC(Opcodes.S_OPCODE_HOUSELIST);
			writeD(board.Id);
			writeH(count); // レコード数
			for (int i = 0; i < count; ++i)
			{
				writeD(id[i]); // アジトの番号
				writeS(name[i]); // アジトの名前
				writeH(area[i]); // アジトの広さ
				writeC(month[i]); // 締切月
				writeC(day[i]); // 締切日
				writeD(price[i]); // 現在の入札価格
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
				return S_AUCTIONBOARD;
			}
		}
	}

}