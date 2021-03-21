﻿using System.Collections.Generic;

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
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1PrivateShopBuyList = LineageServer.Server.Server.Templates.L1PrivateShopBuyList;
	using L1PrivateShopSellList = LineageServer.Server.Server.Templates.L1PrivateShopSellList;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_PrivateShop : ServerBasePacket
	{

		public S_PrivateShop(L1PcInstance pc, int objectId, int type)
		{
			L1PcInstance shopPc = (L1PcInstance) L1World.Instance.findObject(objectId);

			if (shopPc == null)
			{
				return;
			}

			writeC(Opcodes.S_OPCODE_PRIVATESHOPLIST);
			writeC(type);
			writeD(objectId);

			if (type == 0)
			{
				IList<L1PrivateShopSellList> list = shopPc.SellList;
				int size = list.Count;
				pc.PartnersPrivateShopItemCount = size;
				writeH(size);
				for (int i = 0; i < size; i++)
				{
					L1PrivateShopSellList pssl = list[i];
					int itemObjectId = pssl.ItemObjectId;
					int count = pssl.SellTotalCount - pssl.SellCount;
					int price = pssl.SellPrice;
					L1ItemInstance item = shopPc.Inventory.getItem(itemObjectId);
					if (item != null)
					{
						writeC(i);
						writeC(item.Bless);
						writeH(item.Item.GfxId);
						writeD(count);
						writeD(price);
						writeS(item.getNumberedViewName(count));
						writeC(0);
					}
				}
			}
			else if (type == 1)
			{
				IList<L1PrivateShopBuyList> list = shopPc.BuyList;
				int size = list.Count;
				writeH(size);
				for (int i = 0; i < size; i++)
				{
					L1PrivateShopBuyList psbl = list[i];
					int itemObjectId = psbl.ItemObjectId;
					int count = psbl.BuyTotalCount;
					int price = psbl.BuyPrice;
					L1ItemInstance item = shopPc.Inventory.getItem(itemObjectId);
					foreach (L1ItemInstance pcItem in pc.Inventory.Items)
					{
						if ((item.ItemId == pcItem.ItemId) && (item.EnchantLevel == pcItem.EnchantLevel))
						{
							writeC(i);
							writeD(pcItem.Id);
							writeD(count);
							writeD(price);
						}
					}
				}
			}
		}

		public override sbyte[] Content
		{
			get
			{
				return Bytes;
			}
		}
	}

}