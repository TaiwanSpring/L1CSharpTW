using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.Utils.collections;
using System.Collections.Generic;
namespace LineageServer.Server.Server.Model.shop
{
	internal class L1ShopSellOrder
	{
		private readonly L1AssessedItem _item;

		private readonly int _count;

		public L1ShopSellOrder(L1AssessedItem item, int count)
		{
			_item = item;
			_count = count;
		}

		public virtual L1AssessedItem Item
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

	class L1ShopSellOrderList
	{
		private readonly L1Shop _shop;

		private readonly L1PcInstance _pc;

		private readonly IList<L1ShopSellOrder> _list = Lists.newList<L1ShopSellOrder>();

		internal L1ShopSellOrderList(L1Shop shop, L1PcInstance pc)
		{
			_shop = shop;
			_pc = pc;
		}

		public virtual void add(int itemObjectId, int count)
		{
			L1AssessedItem assessedItem = _shop.assessItem(_pc.Inventory.getItem(itemObjectId));
			if (assessedItem == null)
			{
				/*
				 * 買取リストに無いアイテムが指定された。 不正パケの可能性。
				 */
				throw new System.ArgumentException();
			}

			_list.Add(new L1ShopSellOrder(assessedItem, count));
		}

		internal virtual L1PcInstance Pc
		{
			get
			{
				return _pc;
			}
		}

		internal virtual IList<L1ShopSellOrder> List
		{
			get
			{
				return _list;
			}
		}
	}

}