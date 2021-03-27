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

//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.Opcodes.S_OPCODE_PACKETBOX;
	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using ItemTable = LineageServer.Server.DataSources.ItemTable;
	using L1Item = LineageServer.Server.Templates.L1Item;
	using SQLUtil = LineageServer.Utils.SQLUtil;

	//Referenced classes of package l1j.server.server.serverpackets:
	//ServerBasePacket

	/// <summary>
	/// 處理查詢血盟倉庫使用紀錄的封包
	/// </summary>
	public class S_PledgeWarehouseHistory : ServerBasePacket
	{

//JAVA TO C# CONVERTER NOTE: Members cannot have the same name as their enclosing type:
		private const string S_PledgeWarehouseHistory_Conflict = "[S] S_PledgeWarehouseHistory";

		private byte[] _byte = null;


		public S_PledgeWarehouseHistory(int clanId)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				// 刪除超過3天的紀錄
				pstm = con.prepareStatement("DELETE FROM clan_warehouse_history WHERE clan_id=? AND record_time < ?");
				pstm.setInt(1, clanId);
				pstm.setTimestamp(2, new Timestamp(DateTimeHelper.CurrentUnixTimeMillis() - 259200000));
				pstm.execute();
				pstm.close();

				// 查詢紀錄
				pstm = con.prepareStatement("SELECT * FROM clan_warehouse_history WHERE clan_id=? ORDER BY id DESC");
				pstm.setInt(1, clanId);
				rs = pstm.executeQuery();
				rs.last(); // 為了取得總列數，先將指標拉到最後
				int rowcount = rs.Row; // 取得總列數
				rs.beforeFirst(); // 將指標移回最前頭
				/// <summary>
				/// 封包部分 </summary>
				WriteC(S_OPCODE_PACKETBOX);
				WriteC(S_PacketBox.HTML_CLAN_WARHOUSE_RECORD);
				WriteD(rowcount); // 總共筆數
				while (rs.next())
				{
					L1Item item = ItemTable.Instance.getTemplate(ItemTable.Instance.findItemIdByName(dataSourceRow.getString("item_name")));
					WriteS(dataSourceRow.getString("char_name"));
					WriteC(dataSourceRow.getInt("type")); // 領出: 1, 存入: 0
					WriteS(item.UnidentifiedNameId); // 物品名稱
					WriteD(dataSourceRow.getInt("item_count")); // 物品數量
					WriteD((int)((DateTimeHelper.CurrentUnixTimeMillis() - dataSourceRow.getTimestamp("record_time").Time) / 60000)); // 過了幾分鐘
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




		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = memoryStream.toByteArray();
				}
				return _byte;
			}
		}

		public override string Type
		{
			get
			{
				return S_PledgeWarehouseHistory_Conflict;
			}
		}
	}

}