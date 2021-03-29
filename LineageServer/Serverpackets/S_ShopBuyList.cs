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
namespace LineageServer.Serverpackets
{

	using Config = LineageServer.Server.Config;
	using Opcodes = LineageServer.Server.Opcodes;
	using ShopTable = LineageServer.Server.DataTables.ShopTable;
	using L1CastleLocation = LineageServer.Server.Model.L1CastleLocation;
	using GameObject = LineageServer.Server.Model.GameObject;
	using L1World = LineageServer.Server.Model.L1World;
	using L1ItemInstance = LineageServer.Server.Model.Instance.L1ItemInstance;
	using L1NpcInstance = LineageServer.Server.Model.Instance.L1NpcInstance;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using L1AssessedItem = LineageServer.Server.Model.shop.L1AssessedItem;
	using L1Shop = LineageServer.Server.Model.shop.L1Shop;
	using L1WilliamItemPrice = LineageServer.william.L1WilliamItemPrice;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket, S_SystemMessage

	public class S_ShopBuyList : ServerBasePacket
	{

		private const string S_SHOP_BUY_LIST = "[S] S_ShopBuyList";

		public S_ShopBuyList(int objid, L1PcInstance pc)
		{
			GameObject @object = L1World.Instance.findObject(objid);
			if (!(@object is L1NpcInstance))
			{
				return;
			}
			L1NpcInstance npc = (L1NpcInstance) @object;
			int npcId = npc.NpcTemplate.get_npcId();
			// 全道具販賣 
			if (Config.ALL_ITEM_SELL)
			{
				int tax_rate = L1CastleLocation.getCastleTaxRateByNpcId(npcId);

				List<L1ItemInstance> sellItems = new List<L1ItemInstance>();
				for (IEnumerator<L1ItemInstance> iterator = pc.Inventory.Items.GetEnumerator(); iterator.MoveNext();)
				{
					object iObject = iterator.Current;
					L1ItemInstance itm = (L1ItemInstance)iObject;
					if (itm != null && !itm.Equipped && itm.ItemId != 40308 && L1WilliamItemPrice.getItemId(itm.Item.ItemId) != 0)
					{
						sellItems.Add(itm);
					}
				}

				int sell = sellItems.Count;
				if (sell > 0)
				{
					WriteC(Opcodes.S_OPCODE_SHOWSHOPSELLLIST);
					WriteD(objid);
					WriteH(sell);
					foreach (object itemObj in sellItems)
					{
						L1ItemInstance item = (L1ItemInstance) itemObj;
						int getPrice = L1WilliamItemPrice.getItemId(item.Item.ItemId);
						int price = 0;
						if (getPrice > 0)
						{
							price = getPrice;
						}
						else
						{
							price = 0;
						}
						if (tax_rate != 0)
						{
							double tax = (100 + tax_rate) / 100.0;
							price = (int)(price * tax);
						}
						WriteD(item.Id);
						WriteD(price / 2);
					}
				}
				else
				{
					pc.sendPackets(new S_NoSell(npc));
				}
			}
			else
			{
				L1Shop shop = ShopTable.Instance.get(npcId);
				if (shop == null)
				{
					pc.sendPackets(new S_NoSell(npc));
					return;
				}

				IList<L1AssessedItem> assessedItems = shop.assessItems(pc.Inventory);
				if (assessedItems.Count == 0)
				{
					pc.sendPackets(new S_NoSell(npc));
					return;
				}

				WriteC(Opcodes.S_OPCODE_SHOWSHOPSELLLIST);
				WriteD(objid);
				WriteH(assessedItems.Count);

				foreach (L1AssessedItem item in assessedItems)
				{
					WriteD(item.TargetId);
					WriteD(item.AssessedPrice);
				}
			}
			// 全道具販賣  end
			WriteH(0x0007); // 7 = 金幣為單位 顯示總金額
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
				return S_SHOP_BUY_LIST;
			}
		}
	}
}