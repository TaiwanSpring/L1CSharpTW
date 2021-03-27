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
	using Opcodes = LineageServer.Server.Opcodes;

	public class S_CharCreateStatus : ServerBasePacket
	{
		private const string S_CHAR_CREATE_STATUS = "[S] S_CharCreateStatus";

		public const int REASON_OK = 0x02;

		public const int REASON_ALREADY_EXSISTS = 0x06;

		public const int REASON_INVALID_NAME = 0x09;

		public const int REASON_WRONG_AMOUNT = 0x15;

		public S_CharCreateStatus(int reason)
		{
			WriteC(Opcodes.S_OPCODE_NEWCHARWRONG);
			WriteC(reason);
			WriteD(0x00000000);
			WriteH(0x0000);
		}

		public override sbyte[] Content
		{
			get
			{
				return Bytes;
			}
		}

		public override string Type
		{
			get
			{
				return S_CHAR_CREATE_STATUS;
			}
		}
	}

}