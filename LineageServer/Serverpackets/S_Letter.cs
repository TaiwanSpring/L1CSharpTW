using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_Letter : ServerBasePacket
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

        public override string Type
        {
            get
            {
                return S_LETTER;
            }
        }
    }

}