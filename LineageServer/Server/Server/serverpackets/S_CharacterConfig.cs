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
	using SQLUtil = LineageServer.Server.Server.Utils.SQLUtil;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_CharacterConfig : ServerBasePacket
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(S_CharacterConfig).FullName);
		private const string S_CHARACTER_CONFIG = "[S] S_CharacterConfig";
		private byte[] _byte = null;

		public S_CharacterConfig(int objectId)
		{
			buildPacket(objectId);
		}

		private void buildPacket(int objectId)
		{
			int length = 0;
			sbyte[] data = null;
			IDataBaseConnection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM character_config WHERE object_id=?");
				pstm.setInt(1, objectId);
				rs = pstm.executeQuery();
				while (rs.next())
				{
					length = dataSourceRow.getInt(2);
					data = dataSourceRow.getBytes(3);
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

			if (length != 0)
			{
				WriteC(Opcodes.S_OPCODE_SKILLICONGFX);
				WriteC(S_PacketBox.CHARACTER_CONFIG);
				WriteD(length);
				WriteByte(data);
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
				return S_CHARACTER_CONFIG;
			}
		}
	}

}