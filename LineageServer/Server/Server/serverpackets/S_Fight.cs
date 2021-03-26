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

	using Opcodes = LineageServer.Server.Server.Opcodes;

	public class S_Fight : ServerBasePacket
	{
		private const string S_FIGHT = "[S] S_Fight";

		private byte[] _byte = null;

		// 開啟
		public const int FLAG_ON = 1;

		// 關閉
		public const int FLAG_OFF = 0;

		// 正義第一階段(10000 ~ 19999)
		public const int TYPE_JUSTICE_LEVEL1 = 0;

		// 正義第二階段(20000 ~ 29999)
		public const int TYPE_JUSTICE_LEVEL2 = 1;

		// 正義第三階段(30000 ~ 32767)
		public const int TYPE_JUSTICE_LEVEL3 = 2;

		// 邪惡第一階段(-10000 ~ -19999)
		public const int TYPE_EVIL_LEVEL1 = 3;

		// 邪惡第二階段(-20000 ~ -29999)
		public const int TYPE_EVIL_LEVEL2 = 4;

		// 邪惡第三階段(-30000 ~ -32768)
		public const int TYPE_EVIL_LEVEL3 = 5;

		// 遭遇的守護(新手保護)
		public const int TYPE_ENCOUNTER = 6;

		public S_Fight(int type, int flag)
		{
			WriteC(Opcodes.S_OPCODE_PACKETBOX);
			WriteC(0x72);
			WriteD(type);
			WriteD((flag == FLAG_OFF) ? FLAG_OFF : FLAG_ON);
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
				return S_FIGHT;
			}
		}
	}

}