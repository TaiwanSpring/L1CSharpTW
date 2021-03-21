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
namespace LineageServer.Server.Server.Model.shop
{

	using Config = LineageServer.Server.Config;
	using L1TaxCalculator = LineageServer.Server.Server.Model.L1TaxCalculator;
	using L1ShopItem = LineageServer.Server.Server.Templates.L1ShopItem;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;

	internal class L1ShopBuyOrder
	{
		private readonly L1ShopItem _item;

		private readonly int _count;

		public L1ShopBuyOrder(L1ShopItem item, int count)
		{
			_item = item;
			_count = count;
		}

		public virtual L1ShopItem Item
		{
			get
			{
				return _item;
			}
		}

		public virtual int Count
		{
			get
			{
				return _count;
			}
		}
	}

	public class L1ShopBuyOrderList
	{
		private readonly L1Shop _shop;

		private readonly IList<L1ShopBuyOrder> _list = Lists.newList();

		private readonly L1TaxCalculator _taxCalc;

		private int _totalWeight = 0;

		private int _totalPrice = 0;

		private int _totalPriceTaxIncluded = 0;

		internal L1ShopBuyOrderList(L1Shop shop)
		{
			_shop = shop;
			_taxCalc = new L1TaxCalculator(shop.NpcId);
		}

		public virtual void add(int orderNumber, int count)
		{
			if (_shop.SellingItems.Count < orderNumber)
			{
				return;
			}
			L1ShopItem shopItem = _shop.SellingItems[orderNumber];

			int price = (int)(shopItem.Price * Config.RATE_SHOP_SELLING_PRICE);
			// オーバーフローチェック
			for (int j = 0; j < count; j++)
			{
				if (price * j < 0)
				{
					return;
				}
			}
			if (_totalPrice < 0)
			{
				return;
			}
			_totalPrice += price * count;
			_totalPriceTaxIncluded += _taxCalc.layTax(price) * count;
			_totalWeight += shopItem.Item.Weight * count * shopItem.PackCount;

			if (shopItem.Item.Stackable)
			{
				_list.Add(new L1ShopBuyOrder(shopItem, count * shopItem.PackCount));
				return;
			}

			for (int i = 0; i < (count * shopItem.PackCount); i++)
			{
				_list.Add(new L1ShopBuyOrder(shopItem, 1));
			}
		}

		internal virtual IList<L1ShopBuyOrder> List
		{
			get
			{
				return _list;
			}
		}

		public virtual int TotalWeight
		{
			get
			{
				return _totalWeight;
			}
		}

		public virtual int TotalPrice
		{
			get
			{
				return _totalPrice;
			}
		}

		public virtual int TotalPriceTaxIncluded
		{
			get
			{
				return _totalPriceTaxIncluded;
			}
		}

		internal virtual L1TaxCalculator TaxCalculator
		{
			get
			{
				return _taxCalc;
			}
		}
	}

}