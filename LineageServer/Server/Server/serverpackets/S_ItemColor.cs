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

	public class S_ItemColor : ServerBasePacket
	{

		private const string S_ITEM_COLOR = "[S] S_ItemColor";

		/// <summary>
		/// アイテムの色を変更する。祝福・呪い、封印状態が変化した時などに送る
		/// </summary>
		public S_ItemColor(L1ItemInstance item)
		{
			if (item == null)
			{
				return;
			}
			buildPacket(item);
		}

		private void buildPacket(L1ItemInstance item)
		{
			writeC(Opcodes.S_OPCODE_ITEMCOLOR);
			writeD(item.Id);
			// 0:祝福 1:通常 2:呪い 3:未鑑定
			// 128:祝福&封印 129:&封印 130:呪い&封印 131:未鑑定&封印
			writeC(item.Bless);
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
				return S_ITEM_COLOR;
			}
		}

	}

}