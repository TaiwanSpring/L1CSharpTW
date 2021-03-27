﻿/// <summary>
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
	using L1ItemInstance = LineageServer.Server.Model.Instance.L1ItemInstance;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket, S_SendInvOnLogin

	public class S_ItemName : ServerBasePacket
	{

		private const string S_ITEM_NAME = "[S] S_ItemName";

		/// <summary>
		/// アイテムの名前を変更する。装備や強化状態が変わったときに送る。
		/// </summary>
		public S_ItemName(L1ItemInstance item)
		{
			if (item == null)
			{
				return;
			}
			// jumpを見る限り、このOpcodeはアイテム名を更新させる目的だけに使用される模様（装備後やOE後専用？）
			// 後に何かデータを続けて送っても全て無視されてしまう
			WriteC(Opcodes.S_OPCODE_ITEMNAME);
			WriteD(item.Id);
			WriteS(item.ViewName);
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
				return S_ITEM_NAME;
			}
		}
	}

}