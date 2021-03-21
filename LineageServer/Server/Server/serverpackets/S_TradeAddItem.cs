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

	public class S_TradeAddItem : ServerBasePacket
	{
		private const string S_TRADE_ADD_ITEM = "[S] S_TradeAddItem";

		public S_TradeAddItem(L1ItemInstance item, int count, int type)
		{
			writeC(Opcodes.S_OPCODE_TRADEADDITEM);
			writeC(type); // 0:トレードウィンドウ上段 1:トレードウィンドウ下段
			writeH(item.Item.GfxId);
			writeS(item.getNumberedViewName(count));
			// 0:祝福 1:通常 2:呪い 3:未鑑定
			// 128:祝福&封印 129:&封印 130:呪い&封印 131:未鑑定&封印
			if (!item.Identified)
			{
				writeC(3);
				writeC(0);
				writeC(7);
				writeC(0);
			}
			else
			{
				writeC(item.Bless);
				sbyte[] status = item.StatusBytes;
				  writeC(status.Length);
				  foreach (sbyte b in status)
				  {
					  writeC(b);
				  }
				  writeH(0);
			}
		}

		public override sbyte[] Content
		{
			get
			{
				return Bytes;
			}
		}

		public override string Type
		{
			get
			{
				return S_TRADE_ADD_ITEM;
			}
		}
	}

}