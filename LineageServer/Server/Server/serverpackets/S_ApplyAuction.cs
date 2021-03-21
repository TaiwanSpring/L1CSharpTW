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

	public class S_ApplyAuction : ServerBasePacket
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(S_ApplyAuction).FullName);
		private const string S_APPLYAUCTION = "[S] S_ApplyAuction";
		private byte[] _byte = null;

		public S_ApplyAuction(int objectId, string houseNumber)
		{
			buildPacket(objectId, houseNumber);
		}

		private void buildPacket(int objectId, string houseNumber)
		{
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM board_auction WHERE house_id=?");
				int number = Convert.ToInt32(houseNumber);
				pstm.setInt(1, number);
				rs = pstm.executeQuery();
				while (rs.next())
				{
					int nowPrice = rs.getInt(5);
					int bidderId = rs.getInt(10);
					writeC(Opcodes.S_OPCODE_INPUTAMOUNT);
					writeD(objectId);
					writeD(0); // ?
					if (bidderId == 0)
					{ // 入札者なし
						writeD(nowPrice); // スピンコントロールの初期価格
						writeD(nowPrice); // 価格の下限
					}
					else
					{ // 入札者あり
						writeD(nowPrice + 1); // スピンコントロールの初期価格
						writeD(nowPrice + 1); // 価格の下限
					}
					writeD(2000000000); // 価格の上限
					writeH(0); // ?
					writeS("agapply");
					writeS("agapply " + houseNumber);
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
				return S_APPLYAUCTION;
			}
		}
	}

}