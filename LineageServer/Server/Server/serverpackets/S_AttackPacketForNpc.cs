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

	public class S_AttackPacketForNpc : ServerBasePacket
	{
		private const string S_ATTACK_PACKET_FOR_NPC = "[S] S_AttackPacketForNpc";

		private byte[] _byte = null;

		public S_AttackPacketForNpc(L1Character cha, int npcObjectId, int type)
		{
			buildpacket(cha, npcObjectId, type);
		}

		private void buildpacket(L1Character cha, int npcObjectId, int type)
		{
			writeC(Opcodes.S_OPCODE_ATTACKPACKET);
			writeC(type);
			writeD(npcObjectId);
			writeD(cha.Id);
			writeH(0x01); // 3.3C damage
			writeC(cha.Heading);
			writeH(0x0000); // target x
			writeH(0x0000); // target y
			writeC(0x00); // 0x00:none 0x04:Claw 0x08:CounterMirror
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
				return S_ATTACK_PACKET_FOR_NPC;
			}
		}
	}

}