using System;

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

	public class L1AuctionBoard
	{
		public L1AuctionBoard()
		{
		}

		private int _houseId;

		public virtual int HouseId
		{
			get
			{
				return _houseId;
			}
			set
			{
				_houseId = value;
			}
		}


		private string _houseName;

		public virtual string HouseName
		{
			get
			{
				return _houseName;
			}
			set
			{
				_houseName = value;
			}
		}


		private int _houseArea;

		public virtual int HouseArea
		{
			get
			{
				return _houseArea;
			}
			set
			{
				_houseArea = value;
			}
		}


		private DateTime _deadline;

		public virtual DateTime Deadline
		{
			get
			{
				return _deadline;
			}
			set
			{
				_deadline = value;
			}
		}


		private int _price;

		public virtual int Price
		{
			get
			{
				return _price;
			}
			set
			{
				_price = value;
			}
		}


		private string _location;

		public virtual string Location
		{
			get
			{
				return _location;
			}
			set
			{
				_location = value;
			}
		}


		private string _oldOwner;

		public virtual string OldOwner
		{
			get
			{
				return _oldOwner;
			}
			set
			{
				_oldOwner = value;
			}
		}


		private int _oldOwnerId;

		public virtual int OldOwnerId
		{
			get
			{
				return _oldOwnerId;
			}
			set
			{
				_oldOwnerId = value;
			}
		}


		private string _bidder;

		public virtual string Bidder
		{
			get
			{
				return _bidder;
			}
			set
			{
				_bidder = value;
			}
		}


		private int _bidderId;

		public virtual int BidderId
		{
			get
			{
				return _bidderId;
			}
			set
			{
				_bidderId = value;
			}
		}


	}
}