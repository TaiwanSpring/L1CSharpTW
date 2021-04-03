using LineageServer.Server.Model;
using LineageServer.Server.Model.identity;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using System;
using System.Collections.Generic;
using LineageServer.Server;

namespace LineageServer.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來商店的封包
	/// </summary>
	class C_Shop : ClientBasePacket
	{

		private const string C_SHOP = "[C] C_Shop";

		/// <summary>
		/// 『來源:客戶端』<位址:38>{長度:36}(時間:-465193548)
		///       0000: 26 00 01 00 d4 b3 75 00 05 00 00 00 01 00 00 00 &.....u.........
		///       0010: 00 00 35 35 ff 00 74 72 61 64 65 7a 6f 6e 65 31 ..55..tradezone1
		///       0020: 00 08 50 57 ..PW
		/// </summary>
		public C_Shop(byte[] abyte0, ClientThread clientthread) : base(abyte0)
		{
			L1PcInstance pc = clientthread.ActiveChar;
			if ((pc == null) || pc.Ghost)
			{
				return;
			}

			int mapId = pc.MapId;
			if ((mapId != 800))
			{
				pc.sendPackets(new S_ServerMessage(876)); // この場所では個人商店を開けません。
				return;
			}

			IList<L1PrivateShopSellList> sellList = pc.SellList;
			IList<L1PrivateShopBuyList> buyList = pc.BuyList;
			L1ItemInstance checkItem;
			bool tradable = true;

			int type = ReadC();
			if (type == 0)
			{ // 開始
				int sellTotalCount = ReadH();
				int sellObjectId;
				int sellPrice;
				int sellCount;
				for (int i = 0; i < sellTotalCount; i++)
				{
					sellObjectId = ReadD();
					sellPrice = ReadD();
					sellCount = ReadD();
					// 檢查交易項目
					checkItem = pc.Inventory.getItem(sellObjectId);
					if (!checkItem.Item.Tradable)
					{
						tradable = false;
						pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message166, checkItem.Item.Name, "這是不可能處理。"));
					}
					foreach (L1NpcInstance petNpc in pc.PetList.Values)
					{
						if (petNpc is L1PetInstance)
						{
							L1PetInstance pet = (L1PetInstance) petNpc;
							if (checkItem.Id == pet.ItemObjId)
							{
								tradable = false;
								pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message166, checkItem.Item.Name, "這是不可能處理。"));
								break;
							}
						}
					}
					L1PrivateShopSellList pssl = new L1PrivateShopSellList();
					pssl.ItemObjectId = sellObjectId;
					pssl.SellPrice = sellPrice;
					pssl.SellTotalCount = sellCount;
					sellList.Add(pssl);
				}
				int buyTotalCount = ReadH();
				int buyObjectId;
				int buyPrice;
				int buyCount;
				for (int i = 0; i < buyTotalCount; i++)
				{
					buyObjectId = ReadD();
					buyPrice = ReadD();
					buyCount = ReadD();
					// 檢查交易項目
					checkItem = pc.Inventory.getItem(buyObjectId);
					if (!checkItem.Item.Tradable)
					{
						tradable = false;
						pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message166, checkItem.Item.Name, "這是不可能處理。"));
					}
					if (checkItem.Bless >= 128)
					{ // 封印的裝備
						pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message210, checkItem.Item.Name));
						return;
					}
					// 防止異常堆疊交易
					if ((checkItem.Count > 1) && (!checkItem.Item.Stackable))
					{
						pc.sendPackets(new S_SystemMessage("此物品非堆疊，但異常堆疊無法交易。"));
						return;
					}

					// 使用中的寵物項鍊 - 無法販賣
					foreach (L1NpcInstance petNpc in pc.PetList.Values)
					{
						if (petNpc is L1PetInstance)
						{
							L1PetInstance pet = (L1PetInstance) petNpc;
							if (checkItem.Id == pet.ItemObjId)
							{
								tradable = false;
								pc.sendPackets(new S_ServerMessage(1187)); // 寵物項鍊正在使用中。
								break;
							}
						}
					}
					// 使用中的魔法娃娃 - 無法販賣
					foreach (L1DollInstance doll in pc.DollList.Values)
					{
						if (doll.ItemObjId == checkItem.Id)
						{
							tradable = false;
							pc.sendPackets(new S_ServerMessage(1181));
							break;
						}
					}
					L1PrivateShopBuyList psbl = new L1PrivateShopBuyList();
					psbl.ItemObjectId = buyObjectId;
					psbl.BuyPrice = buyPrice;
					psbl.BuyTotalCount = buyCount;
					buyList.Add(psbl);
				}
				if (!tradable)
				{ // 如果項目不包括在交易結束零售商
					sellList.Clear();
					buyList.Clear();
					pc.PrivateShop = false;
					pc.sendPackets(new S_DoActionGFX(pc.Id, ActionCodes.ACTION_Idle));
					pc.broadcastPacket(new S_DoActionGFX(pc.Id, ActionCodes.ACTION_Idle));
					return;
				}
				byte[] chat = ReadByte();
				pc.ShopChat = chat;
				pc.PrivateShop = true;
				pc.sendPackets(new S_DoActionShop(pc.Id,ActionCodes.ACTION_Shop, chat));
				pc.broadcastPacket(new S_DoActionShop(pc.Id,ActionCodes.ACTION_Shop, chat));
				//// 3.80C 個人商店變身  
				int SelectedPolyNum = 0;
				try
				{
					SelectedPolyNum = int.Parse(GobalParameters.Encoding.GetString(chat).Split("tradezone", StringSplitOptions.RemoveEmptyEntries)[1].Substring(0, 1));
				}
				catch (Exception e)
				{
                    System.Console.WriteLine(e.ToString());
                    System.Console.Write(e.StackTrace);
				}
				L1PolyMorph.doPolyPraivateShop(pc, SelectedPolyNum);
			}
			else if (type == 1)
			{ // 終了
				sellList.Clear();
				buyList.Clear();
				pc.PrivateShop = false;
				pc.sendPackets(new S_DoActionGFX(pc.Id,ActionCodes.ACTION_Idle));
				pc.broadcastPacket(new S_DoActionGFX(pc.Id,ActionCodes.ACTION_Idle));
				L1PolyMorph.undoPolyPrivateShop(pc); // 取消變身
			}
		}

		public override string Type
		{
			get
			{
				return C_SHOP;
			}
		}

	}

}