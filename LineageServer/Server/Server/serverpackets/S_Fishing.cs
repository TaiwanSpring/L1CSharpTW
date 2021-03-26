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

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_Fishing : ServerBasePacket
	{

		private const string S_FISHING = "[S] S_Fishing";

		private byte[] _byte = null;

		public S_Fishing()
		{
			buildPacket();
		}

		public S_Fishing(int objectId, int motionNum, int x, int y)
		{
			buildPacket(objectId, motionNum, x, y);
		}

		private void buildPacket()
		{
			WriteC(Opcodes.S_OPCODE_DOACTIONGFX);
			WriteC(0x37); // ?
			WriteD(0x76002822); // ?
			WriteH(0x8AC3); // ?
		}

		private void buildPacket(int objectId, int motionNum, int x, int y)
		{
			WriteC(Opcodes.S_OPCODE_DOACTIONGFX);
			WriteD(objectId);
			WriteC(motionNum);
			WriteH(x);
			WriteH(y);
			WriteD(0);
			WriteH(0);
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
				return S_FISHING;
			}
		}
	}

}