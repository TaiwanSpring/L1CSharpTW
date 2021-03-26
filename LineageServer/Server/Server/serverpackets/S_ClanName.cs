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
//	import static l1j.server.server.Opcodes.S_OPCODE_CLANNAME;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;

	public class S_ClanName : ServerBasePacket
	{
//JAVA TO C# CONVERTER NOTE: Members cannot have the same name as their enclosing type:
		private const string S_ClanName_Conflict = "[S] S_ClanName";

		private byte[] _byte = null;

		public S_ClanName(L1PcInstance pc, bool OnOff)
		{
			WriteC(S_OPCODE_CLANNAME);
			WriteD(pc.Id);
			WriteS(OnOff ? pc.Clanname: "");
			WriteD(0);
			WriteC(0);
			WriteC(OnOff ? 0x0a : 0x0b);
			if (!OnOff)
			{
				WriteD(pc.Clanid);
			}
			else
			{
				WriteD(0);
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
				return S_ClanName_Conflict;
			}
		}
	}

}