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
	using L1Character = LineageServer.Server.Server.Model.L1Character;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_AttackMissPacket : ServerBasePacket
	{

		private const string _S__OB_ATTACKMISSPACKET = "[S] S_AttackMissPacket";

		private byte[] _byte = null;

		public S_AttackMissPacket(L1Character attacker, int targetId)
		{
			WriteC(Opcodes.S_OPCODE_ATTACKPACKET);
			WriteC(1);
			WriteD(attacker.Id);
			WriteD(targetId);
			WriteH(0);
			WriteC(attacker.Heading);
			WriteD(0);
			WriteC(0);
		}

		public S_AttackMissPacket(L1Character attacker, int targetId, int actId)
		{
			WriteC(Opcodes.S_OPCODE_ATTACKPACKET);
			WriteC(actId);
			WriteD(attacker.Id);
			WriteD(targetId);
			WriteH(0);
			WriteC(attacker.Heading);
			WriteD(0);
			WriteC(0);
		}

		public S_AttackMissPacket(int attackId, int targetId)
		{
			WriteC(Opcodes.S_OPCODE_ATTACKPACKET);
			WriteC(1);
			WriteD(attackId);
			WriteD(targetId);
			WriteH(0);
			WriteC(0);
			WriteD(0);
		}

		public S_AttackMissPacket(int attackId, int targetId, int actId)
		{
			WriteC(Opcodes.S_OPCODE_ATTACKPACKET);
			WriteC(actId);
			WriteD(attackId);
			WriteD(targetId);
			WriteH(0);
			WriteC(0);
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
				return _S__OB_ATTACKMISSPACKET;
			}
		}
	}

}