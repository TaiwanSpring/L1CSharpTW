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
	using L1Character = LineageServer.Server.Model.L1Character;
	using MoveUtil = LineageServer.Utils.MoveUtil;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_MoveCharPacket : ServerBasePacket
	{

		private const string _S__1F_MOVECHARPACKET = "[S] S_MoveCharPacket";

		private byte[] _byte = null;

		public S_MoveCharPacket(L1Character cha)
		{
			int heading = cha.Heading;
			int x = cha.X - MoveUtil.MoveX(heading);
			int y = cha.Y - MoveUtil.MoveY(heading);

			WriteC(Opcodes.S_OPCODE_MOVEOBJECT);
			WriteD(cha.Id);
			WriteH(x);
			WriteH(y);
			WriteC(heading);
			WriteC(129);
			WriteD(0);
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
				return _S__1F_MOVECHARPACKET;
			}
		}
	}
}