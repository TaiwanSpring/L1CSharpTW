using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model.Game;
using LineageServer.Server.Server.Model.identity;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Templates;
using LineageServer.Server.Server.utils;
using LineageServer.Server.Server.utils.collections;
using System;
using System.Collections.Generic;
namespace LineageServer.Server.Server.Model.shop
{
	class L1Shop
	{
		private readonly int _npcId;

		private readonly IList<L1ShopItem> _sellingItems;

		private readonly IList<L1ShopItem> _purchasingItems;

		public L1Shop(int npcId, IList<L1ShopItem> sellingItems, IList<L1ShopItem> purchasingItems)
		{
			if (( sellingItems == null ) || ( purchasingItems == null ))
			{
				throw new NullReferenceException();
			}

			_npcId = npcId;
			_sellingItems = sellingItems;
			_purchasingItems = purchasingItems;
		}

		public virtual int NpcId
		{
			get
			{
				return _npcId;
			}
		}

		public virtual IList<L1ShopItem> SellingItems
		{
			get
			{
				return _sellingItems;
			}
		}

		/// <summary>
		/// この商店で、指定されたアイテムが買取可能な状態であるかを返す。
		/// </summary>
		/// <param name="item"> </param>
		/// <returns> アイテムが買取可能であればtrue </returns>
		private bool isPurchaseableItem(L1ItemInstance item)
		{
			if (item == null)
			{
				return false;
			}
			if (item.Equipped)
			{ // 装備中であれば不可
				return false;
			}
			if (item.EnchantLevel != 0)
			{ // 強化(or弱化)されていれば不可
				return false;
			}
			if (item.Bless >= 128)
			{ // 封印された装備
				return false;
			}

			return true;
		}

		private L1ShopItem getPurchasingItem(int itemId)
		{
			foreach (L1ShopItem shopItem in _purchasingItems)
			{
				if (shopItem.ItemId == itemId)
				{
					return shopItem;
				}
			}
			return null;
		}

		public virtual L1AssessedItem assessItem(L1ItemInstance item)
		{
			L1ShopItem shopItem = getPurchasingItem(item.ItemId);
			if (shopItem == null)
			{
				return null;
			}
			return new L1AssessedItem(item.Id, getAssessedPrice(shopItem));
		}

		private int getAssessedPrice(L1ShopItem item)
		{
			return (int)( item.Price * Config.RATE_SHOP_PURCHASING_PRICE / item.PackCount );
		}

		/// <summary>
		/// インベントリ内の買取可能アイテムを査定する。
		/// </summary>
		/// <param name="inv">
		///            査定対象のインベントリ </param>
		/// <returns> 査定された買取可能アイテムのリスト </returns>
		public virtual IList<L1AssessedItem> assessItems(L1PcInventory inv)
		{
			IList<L1AssessedItem> result = Lists.newList<L1AssessedItem>();
			foreach (L1ShopItem item in _purchasingItems)
			{
				foreach (L1ItemInstance targetItem in inv.findItemsId(item.ItemId))
				{
					if (!isPurchaseableItem(targetItem))
					{
						continue;
					}

					result.Add(new L1AssessedItem(targetItem.Id, getAssessedPrice(item)));
				}
			}
			return result;
		}

		/// <summary>
		/// プレイヤーへアイテムを販売できることを保証する。
		/// </summary>
		/// <returns> 何らかの理由でアイテムを販売できない場合、false </returns>
		private bool ensureSell(L1PcInstance pc, L1ShopBuyOrderList orderList)
		{
			int price = orderList.TotalPriceTaxIncluded;
			// オーバーフローチェック
			if (!IntRange.includes(price, 0, 2000000000))
			{
				// 総販売価格は%dアデナを超過できません。
				pc.sendPackets(new S_ServerMessage(904, "2000000000"));
				return false;
			}
			// 購入できるかチェック
			if (!pc.Inventory.checkItem(L1ItemId.ADENA, price))
			{
				System.Console.WriteLine(price);
				// \f1アデナが不足しています。
				pc.sendPackets(new S_ServerMessage(189));
				return false;
			}
			// 重量チェック
			int currentWeight = pc.Inventory.Weight * 1000;
			if (currentWeight + orderList.TotalWeight > pc.MaxWeight * 1000)
			{
				// アイテムが重すぎて、これ以上持てません。
				pc.sendPackets(new S_ServerMessage(82));
				return false;
			}
			// 個数チェック
			int totalCount = pc.Inventory.Size;
			foreach (L1ShopBuyOrder order in orderList.List)
			{
				L1Item temp = order.Item.Item;
				if (temp.Stackable)
				{
					if (!pc.Inventory.checkItem(temp.ItemId))
					{
						totalCount += 1;
					}
				}
				else
				{
					totalCount += 1;
				}
			}
			if (totalCount > 180)
			{
				// \f1一人のキャラクターが持って歩けるアイテムは最大180個までです。
				pc.sendPackets(new S_ServerMessage(263));
				return false;
			}
			return true;
		}

		/// <summary>
		/// 地域税納税処理 アデン城・ディアド要塞を除く城はアデン城へ国税として10%納税する
		/// </summary>
		/// <param name="orderList"> </param>
		private void payCastleTax(L1ShopBuyOrderList orderList)
		{
			L1TaxCalculator calc = orderList.TaxCalculator;

			int price = orderList.TotalPrice;

			int castleId = L1CastleLocation.getCastleIdByNpcid(_npcId);
			int castleTax = calc.calcCastleTaxPrice(price);
			int nationalTax = calc.calcNationalTaxPrice(price);
			// アデン城・ディアド城の場合は国税なし
			if (( castleId == L1CastleLocation.ADEN_CASTLE_ID ) || ( castleId == L1CastleLocation.DIAD_CASTLE_ID ))
			{
				castleTax += nationalTax;
				nationalTax = 0;
			}

			if (( castleId != 0 ) && ( castleTax > 0 ))
			{
				L1Castle castle = CastleTable.Instance.getCastleTable(castleId);

				lock (castle)
				{
					int money = castle.PublicMoney;
					if (2000000000 > money)
					{
						money = money + castleTax;
						castle.PublicMoney = money;
						CastleTable.Instance.updateCastle(castle);
					}
				}

				if (nationalTax > 0)
				{
					L1Castle aden = CastleTable.Instance.getCastleTable(L1CastleLocation.ADEN_CASTLE_ID);
					lock (aden)
					{
						int money = aden.PublicMoney;
						if (2000000000 > money)
						{
							money = money + nationalTax;
							aden.PublicMoney = money;
							CastleTable.Instance.updateCastle(aden);
						}
					}
				}
			}
		}

		/// <summary>
		/// ディアド税納税処理 戦争税の10%がディアド要塞の公金となる。
		/// </summary>
		/// <param name="orderList"> </param>
		private void payDiadTax(L1ShopBuyOrderList orderList)
		{
			L1TaxCalculator calc = orderList.TaxCalculator;

			int price = orderList.TotalPrice;

			// ディアド税
			int diadTax = calc.calcDiadTaxPrice(price);
			if (diadTax <= 0)
			{
				return;
			}

			L1Castle castle = CastleTable.Instance.getCastleTable(L1CastleLocation.DIAD_CASTLE_ID);
			lock (castle)
			{
				int money = castle.PublicMoney;
				if (2000000000 > money)
				{
					money = money + diadTax;
					castle.PublicMoney = money;
					CastleTable.Instance.updateCastle(castle);
				}
			}
		}

		/// <summary>
		/// 町税納税処理
		/// </summary>
		/// <param name="orderList"> </param>
		private void payTownTax(L1ShopBuyOrderList orderList)
		{
			int price = orderList.TotalPrice;

			// 町の売上
			if (!L1World.Instance.ProcessingContributionTotal)
			{
				int town_id = L1TownLocation.getTownIdByNpcid(_npcId);
				if (( town_id >= 1 ) && ( town_id <= 10 ))
				{
					TownTable.Instance.addSalesMoney(town_id, price);
				}
			}
		}

		// XXX 納税処理はこのクラスの責務では無い気がするが、とりあえず
		private void payTax(L1ShopBuyOrderList orderList)
		{
			payCastleTax(orderList);
			payTownTax(orderList);
			payDiadTax(orderList);
		}

		/// <summary>
		/// 販売取引
		/// </summary>
		private void sellItems(L1PcInventory inv, L1ShopBuyOrderList orderList)
		{
			if (!inv.consumeItem(L1ItemId.ADENA, orderList.TotalPriceTaxIncluded))
			{
				throw new System.InvalidOperationException("購入に必要なアデナを消費できませんでした。");
			}
			foreach (L1ShopBuyOrder order in orderList.List)
			{
				int itemId = order.Item.ItemId;
				int amount = order.Count;
				L1ItemInstance item = ItemTable.Instance.createItem(itemId);
				if (item.ItemId == 40309)
				{ // Race Tickets
					item.Item = order.Item.Item;
					L1BugBearRace.Instance.AllBet = L1BugBearRace.Instance.AllBet + ( amount * order.Item.Price );
					string[] runNum = item.Item.IdentifiedNameId.Split("-", true);
					int trueNum = 0;
					for (int i = 0; i < 5; i++)
					{
						if (L1BugBearRace.Instance.getRunner(i).NpcId - 91350 == ( int.Parse(runNum[runNum.Length - 1]) - 1 ))
						{
							trueNum = i;
							break;
						}
					}
					L1BugBearRace.Instance.setBetCount(trueNum, L1BugBearRace.Instance.getBetCount(trueNum) + amount);
				}
				item.Count = amount;
				item.Identified = true;
				inv.storeItem(item);
				if (( _npcId == 70068 ) || ( _npcId == 70020 ))
				{
					item.Identified = false;
					int chance = RandomHelper.Next(100) + 1;
					if (chance <= 15)
					{
						item.EnchantLevel = -2;
					}
					else if (( chance >= 16 ) && ( chance <= 30 ))
					{
						item.EnchantLevel = -1;
					}
					else if (( chance >= 31 ) && ( chance <= 70 ))
					{
						item.EnchantLevel = 0;
					}
					else if (( chance >= 71 ) && ( chance <= 87 ))
					{
						item.EnchantLevel = RandomHelper.Next(2) + 1;
					}
					else if (( chance >= 88 ) && ( chance <= 97 ))
					{
						item.EnchantLevel = RandomHelper.Next(3) + 3;
					}
					else if (( chance >= 98 ) && ( chance <= 99 ))
					{
						item.EnchantLevel = 6;
					}
					else if (chance == 100)
					{
						item.EnchantLevel = 7;
					}
				}
			}
		}

		/// <summary>
		/// プレイヤーに、L1ShopBuyOrderListに記載されたアイテムを販売する。
		/// </summary>
		/// <param name="pc">
		///            販売するプレイヤー </param>
		/// <param name="orderList">
		///            販売すべきアイテムが記載されたL1ShopBuyOrderList </param>
		public virtual void sellItems(L1PcInstance pc, L1ShopBuyOrderList orderList)
		{
			if (!ensureSell(pc, orderList))
			{
				return;
			}

			sellItems(pc.Inventory, orderList);
			payTax(orderList);
		}

		/// <summary>
		/// L1ShopSellOrderListに記載されたアイテムを買い取る。
		/// </summary>
		/// <param name="orderList">
		///            買い取るべきアイテムと価格が記載されたL1ShopSellOrderList </param>
		public virtual void buyItems(L1ShopSellOrderList orderList)
		{
			L1PcInventory inv = orderList.Pc.Inventory;
			int totalPrice = 0;
			foreach (L1ShopSellOrder order in orderList.List)
			{
				int count = inv.removeItem(order.Item.TargetId, order.Count);
				totalPrice += order.Item.AssessedPrice * count;
			}

			totalPrice = IntRange.ensure(totalPrice, 0, 2000000000);
			if (0 < totalPrice)
			{
				inv.storeItem(L1ItemId.ADENA, totalPrice);
			}
		}

		public virtual L1ShopBuyOrderList newBuyOrderList()
		{
			return new L1ShopBuyOrderList(this);
		}

		public virtual L1ShopSellOrderList newSellOrderList(L1PcInstance pc)
		{
			return new L1ShopSellOrderList(this, pc);
		}
	}

}