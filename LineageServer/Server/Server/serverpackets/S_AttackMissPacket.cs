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
			writeC(Opcodes.S_OPCODE_ATTACKPACKET);
			writeC(1);
			writeD(attacker.Id);
			writeD(targetId);
			writeH(0);
			writeC(attacker.Heading);
			writeD(0);
			writeC(0);
		}

		public S_AttackMissPacket(L1Character attacker, int targetId, int actId)
		{
			writeC(Opcodes.S_OPCODE_ATTACKPACKET);
			writeC(actId);
			writeD(attacker.Id);
			writeD(targetId);
			writeH(0);
			writeC(attacker.Heading);
			writeD(0);
			writeC(0);
		}

		public S_AttackMissPacket(int attackId, int targetId)
		{
			writeC(Opcodes.S_OPCODE_ATTACKPACKET);
			writeC(1);
			writeD(attackId);
			writeD(targetId);
			writeH(0);
			writeC(0);
			writeD(0);
		}

		public S_AttackMissPacket(int attackId, int targetId, int actId)
		{
			writeC(Opcodes.S_OPCODE_ATTACKPACKET);
			writeC(actId);
			writeD(attackId);
			writeD(targetId);
			writeH(0);
			writeC(0);
			writeD(0);
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