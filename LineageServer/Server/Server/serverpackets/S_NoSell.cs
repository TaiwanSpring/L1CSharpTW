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
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;

	public class S_NoSell : ServerBasePacket
	{
		private const string _S__25_NoSell = "[S] _S__25_NoSell";

		private byte[] _byte = null;

		public S_NoSell(L1NpcInstance npc)
		{
			buildPacket(npc);
		}

		private void buildPacket(L1NpcInstance npc)
		{
			writeC(Opcodes.S_OPCODE_SHOWHTML);
			writeD(npc.Id);
			writeS("nosell");
			writeC(0x00);
			writeH(0x00);
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
				return _S__25_NoSell;
			}
		}
	}

}