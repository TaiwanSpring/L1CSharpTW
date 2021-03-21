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
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_DropItem : ServerBasePacket
	{

		private const string _S__OB_DropItem = "[S] S_DropItem";

		private byte[] _byte = null;

		public S_DropItem(L1ItemInstance item)
		{
			buildPacket(item);
		}

		private void buildPacket(L1ItemInstance item)
		{
			// int addbyte = 0;
			// int addbyte1 = 1;
			// int addbyte2 = 13;
			// int setting = 4;

			string itemName = item.Item.UnidentifiedNameId;
			// 已鑑定
			int isId = item.Identified ? 1 : 0;
			if (isId == 1)
			{
				itemName = item.Item.IdentifiedNameId;
			}
			writeC(Opcodes.S_OPCODE_DROPITEM);
			writeH(item.X);
			writeH(item.Y);
			writeD(item.Id);
			writeH(item.Item.GroundGfxId);
			writeC(0);
			writeC(0);
			if (item.NowLighting)
			{
				writeC(item.Item.LightRange);
			}
			else
			{
				writeC(0);
			}
			writeC(0);
			writeD(item.Count);
			writeC(0);
			writeC(0);
			if (item.Count > 1)
			{
				if (item.Item.ItemId == 40312 && item.KeyId != 0)
				{ // 旅館鑰匙
					writeS(itemName + item.InnKeyName + " (" + item.Count + ")");
				}
				else
				{
					writeS(itemName + " (" + item.Count + ")");
				}
			}
			else
			{
				int itemId = item.Item.ItemId;
				if ((itemId == 20383) && (isId == 1))
				{ // 軍馬頭盔
					writeS(itemName + " [" + item.ChargeCount + "]");
				}
				else if (item.ChargeCount != 0 && (isId == 1))
				{ // 可使用的次數
					writeS(itemName + " (" + item.ChargeCount + ")");
				}
				else if ((item.Item.LightRange != 0) && item.NowLighting)
				{ // 燈具
					writeS(itemName + " ($10)");
				}
				else if (item.Item.ItemId == 40312 && item.KeyId != 0)
				{ // 旅館鑰匙
					writeS(itemName + item.InnKeyName);
				}
				else
				{
					writeS(itemName);
				}
			}
			writeC(0);
			writeD(0);
			writeD(0);
			writeC(255);
			writeC(0);
			writeC(0);
			writeC(0);
			writeH(65535);
			// writeD(0x401799a);
			writeD(0);
			writeC(8);
			writeC(0);
		}

		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = _bao.toByteArray();
				}
				return _byte;
			}
		}

		public override string Type
		{
			get
			{
				return _S__OB_DropItem;
			}
		}

	}

}