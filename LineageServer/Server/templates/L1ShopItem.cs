using LineageServer.Server.DataTables;
using LineageServer.Server.Model.Game;

namespace LineageServer.Server.Templates
{
	public class L1ShopItem
	{
		private const long serialVersionUID = 1L;

		private readonly int _itemId;

		private L1Item _item;

		private readonly int _price;

		private readonly int _packCount;

		public L1ShopItem(int itemId, int price, int packCount)
		{
			_itemId = itemId;
			_item = ItemTable.Instance.getTemplate(itemId);
			_price = price;
			_packCount = packCount;
		}

		public virtual int ItemId
		{
			get
			{
				return _itemId;
			}
		}

		public virtual L1Item Item
		{
			get
			{
				return _item;
			}
		}

		public virtual int Price
		{
			get
			{
				return _price;
			}
		}

		public virtual int PackCount
		{
			get
			{
				return _packCount;
			}
		}

		// 食人妖精賽跑用
		public virtual int Name
		{
			set
			{
				int trueNum = L1BugBearRace.Instance.getRunner(value).NpcId - 91350 + 1;
				_item = (L1Item)_item.clone();
				string temp = "" + _item.IdentifiedNameId + " " + L1BugBearRace.Instance.Round + "-" + trueNum;
				_item.Name = temp;
				_item.UnidentifiedNameId = temp;
				_item.IdentifiedNameId = temp;
			}
		}

		public static long Serialversionuid
		{
			get
			{
				return serialVersionUID;
			}
		}
	}

}