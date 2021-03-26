using System.Collections.Generic;

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

	public class S_InvList : ServerBasePacket
	{

		private const string S_INV_LIST = "[S] S_InvList";

		/// <summary>
		/// インベントリにアイテムを複数個まとめて追加する。
		/// </summary>
		public S_InvList(IList<L1ItemInstance> items)
		{
			WriteC(Opcodes.S_OPCODE_INVLIST);
			WriteC(items.Count); // 物品數量

			foreach (L1ItemInstance item in items)
			{
				WriteD(item.Id);
				WriteH(item.Item.MagicCatalystType > 0 ? item.Item.MagicCatalystType : item.Item.ItemDescId > 0 ? item.Item.ItemDescId : item.Item.GroundGfxId);
				WriteC(item.Item.UseType);
				WriteC(item.ChargeCount);
				WriteH(item.get_gfxid());
				WriteC(item.Bless);
				WriteD(item.Count);
				WriteC(item.ItemStatusX); // 3.80C 物品驗證機制
				WriteS(item.ViewName);
				if (!item.Identified)
				{ // 未鑑定
					WriteC(0);
				}
				else
				{
					byte[] status = item.StatusBytes;
					WriteC(status.Length);
					foreach (byte b in status)
					{
						WriteC(b);
					}
				}
				WriteC(0x17);
				WriteC(0);
				WriteH(0);
				WriteH(0);
				if (item.Item.Type == 10)
				{ // 如果是法書，傳出法術編號
					WriteC(0);
				}
				else
				{
					WriteC(item.EnchantLevel); // 物品武捲等級
				}
				WriteD(item.Id); // 3.80 物品世界流水編號
				WriteD(0);
				WriteD(0);
				WriteD(item.Bless >= 128 ? 3 : item.Item.Tradable ? 7 : 2); // 7:可刪除, 2: 不可刪除, 3: 封印狀態
				WriteC(0);

				/*WriteC(0x17);
				WriteD(0);
				WriteD(0);
				WriteD(0);
				WriteD(0);
				WriteD(0);
				WriteH(0);
				WriteC(0);*/
			}
		}

		public override sbyte[] Content
		{
			get
			{
				return memoryStream.toByteArray();
			}
		}

		public override string Type
		{
			get
			{
				return S_INV_LIST;
			}
		}
	}

}