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
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_Letter : ServerBasePacket
	{

		private const string S_LETTER = "[S] S_Letter";

		private byte[] _byte = null;

		public S_Letter(L1ItemInstance item)
		{
			buildPacket(item);
		}

		private void buildPacket(L1ItemInstance item)
		{
			/*
			 * IDataBaseConnection con = null; PreparedStatement pstm = null; ResultSet rs =
			 * null; try { con = L1DatabaseFactory.getInstance().getConnection();
			 * pstm = con
			 * .prepareStatement("SELECT * FROM letter WHERE item_object_id=?");
			 * pstm.setInt(1, item.getId()); rs = pstm.executeQuery(); while
			 * (rs.next()) { WriteC(Opcodes.S_OPCODE_LETTER); WriteD(item.getId());
			 * if (item.get_gfxid() == 465) { // 開く前 WriteH(466); // 開いた後 } else if
			 * (item.get_gfxid() == 606) { WriteH(605); } else if (item.get_gfxid()
			 * == 616) { WriteH(615); } else { WriteH(item.get_gfxid()); }
			 * WriteH(dataSourceRow.getInt(2)); WriteS(dataSourceRow.getString(3));
			 * WriteS(dataSourceRow.getString(4)); WriteByte(dataSourceRow.getBytes(7));
			 * WriteByte(dataSourceRow.getBytes(8)); WriteC(dataSourceRow.getInt(6)); // テンプレ
			 * WriteS(dataSourceRow.getString(5)); // 日付 } } catch (SQLException e) {
			 * _log.log(Enum.Level.Server, e.getLocalizedMessage(), e); } finally {
			 * SQLUtil.close(rs); SQLUtil.close(pstm); SQLUtil.close(con); }
			 */
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
				return S_LETTER;
			}
		}
	}

}