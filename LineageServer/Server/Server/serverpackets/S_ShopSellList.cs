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

	using Config = LineageServer.Server.Config;
	using Opcodes = LineageServer.Server.Server.Opcodes;
	using ShopTable = LineageServer.Server.Server.DataSources.ShopTable;
	using ItemTable = LineageServer.Server.Server.DataSources.ItemTable;
	using L1Object = LineageServer.Server.Server.Model.L1Object;
	using L1TaxCalculator = LineageServer.Server.Server.Model.L1TaxCalculator;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1Shop = LineageServer.Server.Server.Model.shop.L1Shop;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1Item = LineageServer.Server.Server.Templates.L1Item;
	using L1ShopItem = LineageServer.Server.Server.Templates.L1ShopItem;

	public class S_ShopSellList : ServerBasePacket
	{

		/// <summary>
		/// 商店販賣的物品清單
		/// 店の品物リストを表示する。キャラクターがBUYボタンを押した時に送る。
		/// </summary>
		public S_ShopSellList(int objId, L1PcInstance pc)
		{
			WriteC(Opcodes.S_OPCODE_SHOWSHOPBUYLIST);
			WriteD(objId);

			L1Object npcObj = L1World.Instance.findObject(objId);
			if (!(npcObj is L1NpcInstance))
			{
				WriteH(0);
				return;
			}
			int npcId = ((L1NpcInstance) npcObj).NpcTemplate.get_npcId();

			L1TaxCalculator calc = new L1TaxCalculator(npcId);
			L1Shop shop = ShopTable.Instance.get(npcId);
			IList<L1ShopItem> shopItems = shop.SellingItems;

			WriteH(shopItems.Count);

			// L1ItemInstanceのgetStatusBytesを利用するため
			L1ItemInstance dummy = new L1ItemInstance();

			for (int i = 0; i < shopItems.Count; i++)
			{
				L1ShopItem shopItem = shopItems[i];
				L1Item item = shopItem.Item;
				int price = calc.layTax((int)(shopItem.Price * Config.RATE_SHOP_SELLING_PRICE));
				WriteD(i);
				WriteH(shopItem.Item.GfxId);
				WriteD(price);

				if (shopItem.PackCount > 1)
				{
					WriteS(item.Name + " (" + shopItem.PackCount + ")");
				}
				else
				{
					if (item.ItemId == 40309)
					{ // 食人妖精RaceTicket
						string[] temp = item.Name.Split(" ", true);
						string buf = temp[temp.Length - 1];
						temp = buf.Split("-", true);
						WriteS(buf + " $" + (1212 + int.Parse(temp[temp.Length - 1])));
					}
					else
					{
						WriteS(item.Name);
					}
				}

				L1Item template = ItemTable.Instance.getTemplate(item.ItemId);
				if (template == null)
				{
					WriteC(0);
				}
				else
				{
					dummy.Item = template;
					sbyte[] status = dummy.StatusBytes;
					WriteC(status.Length);
					foreach (sbyte b in status)
					{
						WriteC(b);
					}
				}
			}
			WriteH(0x07); // 0x00:kaimo 0x01:pearl 0x07:adena
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public byte[] getContent() throws java.io.IOException
		public override sbyte[] Content
		{
			get
			{
				return memoryStream.toByteArray();
			}
		}
	}

}