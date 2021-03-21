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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.Opcodes.S_OPCODE_PLEDGE_RECOMMENDATION;


	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using CharacterTable = LineageServer.Server.Server.datatables.CharacterTable;
	using ClanRecommendTable = LineageServer.Server.Server.datatables.ClanRecommendTable;
	using L1Clan = LineageServer.Server.Server.Model.L1Clan;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	/// <summary>
	/// 推薦血盟 
	/// </summary>
	public class S_PledgeRecommendation : ServerBasePacket
	{

//JAVA TO C# CONVERTER NOTE: Members cannot have the same name as their enclosing type:
		private const string S_PledgeRecommendation_Conflict = "[S] S_PledgeRecommendation";

		private byte[] _byte = null;


		/// <summary>
		/// 打開推薦血盟 邀請目錄 </summary>
		/// <param name="type"> </param>
		/// <param name="clan_id"> </param>
		public S_PledgeRecommendation(int type, int clan_id)
		{
			buildPacket(type, clan_id, null, 0, null);
		}

		/// <summary>
		/// 打開推薦血盟 血盟目錄 / 申請目錄 </summary>
		/// <param name="type"> </param>
		/// <param name="char_name"> </param>
		public S_PledgeRecommendation(int type, string char_name)
		{
			buildPacket(type, 0, null, 0, char_name);
		}

		/// <summary>
		/// 推薦血盟  申請/處理申請 </summary>
		/// <param name="type"> </param>
		/// <param name="id"> 申請:血盟 id 處理申請: 流水號 </param>
		/// <param name="acceptType"> 0:申請  1:接受  2:拒絕  3:刪除 </param>
		public S_PledgeRecommendation(int type, int record_id, int acceptType)
		{
			buildPacket(type, record_id, null, acceptType, null);
		}

		/// <summary>
		/// 登錄結果
		/// </summary>
		public S_PledgeRecommendation(bool postStatus, int clan_id)
		{
			buildPacket(postStatus, clan_id);
		}

		/// <summary>
		/// 血盟推薦登錄狀態 </summary>
		/// <param name="postStatus"> 登錄成功:True, 取消登陸:False </param>
		private void buildPacket(bool postStatus, int clan_id)
		{
			writeC(S_OPCODE_PLEDGE_RECOMMENDATION);
			writeC(postStatus ? 0 : 1);
			if (!ClanRecommendTable.Instance.isRecorded(clan_id))
			{
				writeC(0x82);
			}
			else
			{
				writeC(0);
			}
			writeD(0);
			writeC(0);
		}

		private void buildPacket(int type, int record_id, string typeMessage, int acceptType, string char_name)
		{
			writeC(S_OPCODE_PLEDGE_RECOMMENDATION);
			writeC(type);

			Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;

			switch (type)
			{
			case 2: // 查詢
				try
				{
					con = L1DatabaseFactory.Instance.Connection;
					pstm = con.prepareStatement("SELECT * FROM clan_recommend_record ORDER BY RAND() LIMIT 10");
					rs = pstm.executeQuery();

					int rows = 0;

					while (rs.next())
					{
						if (ClanRecommendTable.Instance.isApplyForTheClan(rs.getInt("clan_id"), char_name))
						{
							continue;
						}
						else
						{
							rows++;
						}
					}

					rs.beforeFirst();

					writeC(0x00);
					writeC(rows);

					while (rs.next())
					{
						if (ClanRecommendTable.Instance.isApplyForTheClan(rs.getInt("clan_id"), char_name))
						{
							continue;
						}
						writeD(rs.getInt("clan_id")); // 血盟id
						writeS(rs.getString("clan_name")); // 血盟名稱
						writeS(rs.getString("crown_name")); // 王族名稱
						writeD(0); // 一周最大上線人數
						writeC(rs.getInt("clan_type")); // 血盟登錄類型
						L1Clan clan = L1World.Instance.getClan(rs.getString("clan_name"));
						writeC(clan.HouseId > 0 ? 1 : 0); // 是否有盟屋
						writeC(0); // 戰爭狀態
						writeC(0); // 尚未使用
						writeS(typeMessage); // 血盟類型說明
						writeD(clan.EmblemId); // 盟徽編號
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
				break;
			case 3: // 申請目錄
				try
				{
					con = L1DatabaseFactory.Instance.Connection;
					pstm = con.prepareStatement("SELECT * FROM clan_recommend_apply WHERE char_name=?");
					pstm.setString(1, char_name);
					rs = pstm.executeQuery();

					rs.last();
					int rows = rs.Row;
					rs.beforeFirst();
					writeC(0x00);
					writeC(rows);

					while (rs.next())
					{
						PreparedStatement pstm2 = con.prepareStatement("SELECT * FROM clan_recommend_record WHERE clan_id=?");
						pstm2.setInt(1, rs.getInt("clan_id"));
						ResultSet rs2 = pstm2.executeQuery();

						if (rs2.first())
						{
							writeD(rs.getInt("id")); // id
							writeC(0);
							writeD(rs2.getInt("clan_id")); // 血盟id
							writeS(rs2.getString("clan_name")); // 血盟名稱
							L1Clan clan = L1World.Instance.getClan(rs.getString("clan_name"));
							writeS(clan.LeaderName); // 王族名稱
							writeD(0); // 一周最大上線人數
							writeC(rs2.getInt("clan_type")); // 血盟登錄類型
							writeC(clan.HouseId > 0 ? 1 : 0); // 是否有盟屋
							writeC(0); // 戰爭狀態
							writeC(0); // 尚未使用
							writeS(rs2.getString("type_message")); // 血盟類型說明
							writeD(clan.EmblemId); // 盟徽編號
						}
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
				break;
			case 4: // 邀請名單
				if (!ClanRecommendTable.Instance.isRecorded(record_id))
				{
					writeC(0x82);
				}
				else
				{
					try
					{
						con = L1DatabaseFactory.Instance.Connection;
						pstm = con.prepareStatement("SELECT * FROM clan_recommend_record WHERE clan_id=?");
						pstm.setInt(1, record_id);
						rs = pstm.executeQuery();

						writeC(0x00);

						if (rs.first())
						{
							writeC(rs.getInt("clan_type")); // 血盟類型
							writeS(rs.getString("type_message"));
						}

						PreparedStatement pstm2 = con.prepareStatement("SELECT * FROM clan_recommend_apply WHERE clan_id=?");
						pstm2.setInt(1, record_id);
						ResultSet rs2 = pstm2.executeQuery();
						rs2.last();
						int rows = rs2.Row;
						rs2.beforeFirst();

						writeC(rows);

						while (rs2.next())
						{
							writeD(rs2.getInt("id"));
							L1PcInstance pc = L1World.Instance.getPlayer(rs2.getString("char_name"));
							if (pc == null)
							{
								pc = CharacterTable.Instance.restoreCharacter(rs2.getString("char_name"));
							}
							writeC(0);
							writeC(pc.OnlineStatus); // 上線狀態
							writeS(pc.Name); // 角色明稱
							writeC(pc.Type); // 職業
							writeH(pc.Lawful); // 角色 正義值
							writeH(pc.Level); // 角色 等級
						}
					}
					catch (SQLException e)
					{
                            System.Console.WriteLine(e.Message);
					}
					catch (Exception e)
					{
                            System.Console.WriteLine(e.ToString());
                            System.Console.Write(e.StackTrace);
					}
					finally
					{
						SQLUtil.close(rs);
						SQLUtil.close(pstm);
						SQLUtil.close(con);
					}
				}
				break;
			case 5: // 申請加入
			case 6: // 刪除申請
				writeC(0x00);
				writeD(record_id);
				writeC(acceptType);
				break;
			}
			writeD(0);
			writeC(0);
		}


		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = _bao.toByteArray();
				}
				return _byte;
			}
		}

		public override string Type
		{
			get
			{
				return S_PledgeRecommendation_Conflict;
			}
		}
	}

}