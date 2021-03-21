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
namespace LineageServer.Server.Server.Templates
{
	// Referenced classes of package l1j.server.server.templates:
	// L1PrivateShopSellList

	public class L1PrivateShopSellList
	{
		public L1PrivateShopSellList()
		{
		}

		private int _itemObjectId;

		public virtual int ItemObjectId
		{
			set
			{
				_itemObjectId = value;
			}
			get
			{
				return _itemObjectId;
			}
		}


		private int _sellTotalCount; // 売る予定の個数

		public virtual int SellTotalCount
		{
			set
			{
				_sellTotalCount = value;
			}
			get
			{
				return _sellTotalCount;
			}
		}


		private int _sellPrice;

		public virtual int SellPrice
		{
			set
			{
				_sellPrice = value;
			}
			get
			{
				return _sellPrice;
			}
		}


		private int _sellCount; // 売った累計

		public virtual int SellCount
		{
			set
			{
				_sellCount = value;
			}
			get
			{
				return _sellCount;
			}
		}

	}

}