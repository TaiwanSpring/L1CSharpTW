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
namespace LineageServer.Server.Templates
{
	// Referenced classes of package l1j.server.server.templates:
	// L1PrivateShopBuyList

	public class L1PrivateShopBuyList
	{
		public L1PrivateShopBuyList()
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


		private int _buyTotalCount; // 買う予定の個数

		public virtual int BuyTotalCount
		{
			set
			{
				_buyTotalCount = value;
			}
			get
			{
				return _buyTotalCount;
			}
		}


		private int _buyPrice;

		public virtual int BuyPrice
		{
			set
			{
				_buyPrice = value;
			}
			get
			{
				return _buyPrice;
			}
		}


		private int _buyCount; // 買った累計

		public virtual int BuyCount
		{
			set
			{
				_buyCount = value;
			}
			get
			{
				return _buyCount;
			}
		}

	}

}