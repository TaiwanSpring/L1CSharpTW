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

	public class S_LoginResult : ServerBasePacket
	{
		public const string S_LOGIN_RESULT = "[S] S_LoginResult";

		public const int REASON_LOGIN_OK = 0x00;

		public const int REASON_ACCOUNT_IN_USE = 0x16;

		public const int REASON_ACCOUNT_ALREADY_EXISTS = 0x07;

		public const int REASON_ACCESS_FAILED = 0x08;

		public const int REASON_USER_OR_PASS_WRONG = 0x08;

		public const int REASON_PASS_WRONG = 0x08;

		public const int REASON_OUT_OF_GASH = 0x1c;

		// public static int REASON_SYSTEM_ERROR = 0x01;

		private byte[] _byte = null;

		public S_LoginResult(int reason)
		{
			buildPacket(reason);
		}

		private void buildPacket(int reason)
		{
			WriteC(Opcodes.S_OPCODE_LOGINRESULT);
			WriteC(reason);
			WriteD(0x00000000);
			WriteD(0x00000000);
			WriteD(0x00000000);
			WriteD(0x00000000);
			WriteH(0x8c);
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
				return S_LOGIN_RESULT;
			}
		}
	}

}