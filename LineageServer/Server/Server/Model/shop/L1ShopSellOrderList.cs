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

	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;

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

	public class L1ShopSellOrderList
	{
		private readonly L1Shop _shop;

		private readonly L1PcInstance _pc;

		private readonly IList<L1ShopSellOrder> _list = Lists.newList();

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