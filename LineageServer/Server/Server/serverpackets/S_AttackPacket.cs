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

	public class S_AttackPacket : ServerBasePacket
	{
		private const string S_ATTACK_PACKET = "[S] S_AttackPacket";

		private byte[] _byte = null;

		public S_AttackPacket(L1Character atk, int objid, int[] data)
		{
			buildpacket(atk, objid, data);
		}

		public S_AttackPacket(L1Character atk, int objid, int actid)
		{
			int[] data = new int[] {actid, 0, 0};
			buildpacket(atk, objid, data);
		}

		private void buildpacket(L1Character atk, int objid, int[] data)
		{ // data = {actid, dmg, effect}
			WriteC(Opcodes.S_OPCODE_ATTACKPACKET);
			WriteC(data[0]); // actid
			WriteD(atk.Id);
			WriteD(objid);
			WriteH(data[1]); // dmg
			WriteC(atk.Heading);
			WriteD(0x00000000);
			WriteC(data[2]); // effect 0:none 2:爪痕 4:雙擊 8:鏡返射
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
				return S_ATTACK_PACKET;
			}
		}
	}

}