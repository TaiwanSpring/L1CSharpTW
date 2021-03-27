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
			WriteC(Opcodes.S_OPCODE_ATTACKPACKET);
			WriteC(type);
			WriteD(npcObjectId);
			WriteD(cha.Id);
			WriteH(0x01); // 3.3C damage
			WriteC(cha.Heading);
			WriteH(0x0000); // target x
			WriteH(0x0000); // target y
			WriteC(0x00); // 0x00:none 0x04:Claw 0x08:CounterMirror
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