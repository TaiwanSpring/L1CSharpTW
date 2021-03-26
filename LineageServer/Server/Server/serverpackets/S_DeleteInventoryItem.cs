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

	public class S_DeleteInventoryItem : ServerBasePacket
	{

		private const string S_DELETE_INVENTORY_ITEM = "[S] S_DeleteInventoryItem";

		/// <summary>
		/// インベントリからアイテムを削除する。
		/// </summary>
		/// <param name="item">
		///            - 削除するアイテム </param>
		public S_DeleteInventoryItem(L1ItemInstance item)
		{
			if (item != null)
			{
				WriteC(Opcodes.S_OPCODE_DELETEINVENTORYITEM);
				WriteD(item.Id);
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
				return S_DELETE_INVENTORY_ITEM;
			}
		}
	}

}