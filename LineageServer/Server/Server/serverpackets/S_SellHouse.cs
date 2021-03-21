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

	public class S_SellHouse : ServerBasePacket
	{

		private const string S_SELLHOUSE = "[S] S_SellHouse";

		private byte[] _byte = null;

		public S_SellHouse(int objectId, string houseNumber)
		{
			buildPacket(objectId, houseNumber);
		}

		private void buildPacket(int objectId, string houseNumber)
		{
			writeC(Opcodes.S_OPCODE_INPUTAMOUNT);
			writeD(objectId);
			writeD(0); // ?
			writeD(100000); // スピンコントロールの初期価格
			writeD(100000); // 価格の下限
			writeD(2000000000); // 価格の上限
			writeH(0); // ?
			writeS("agsell");
			writeS("agsell " + houseNumber);
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
				return S_SELLHOUSE;
			}
		}
	}

}