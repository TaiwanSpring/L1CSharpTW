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
namespace LineageServer.Server.Templates
{

	public class L1House
	{
		public L1House()
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


		private int _keeperId;

		public virtual int KeeperId
		{
			get
			{
				return _keeperId;
			}
			set
			{
				_keeperId = value;
			}
		}


		private bool _isOnSale;

		public virtual bool OnSale
		{
			get
			{
				return _isOnSale;
			}
			set
			{
				_isOnSale = value;
			}
		}


		private bool _isPurchaseBasement;

		public virtual bool PurchaseBasement
		{
			get
			{
				return _isPurchaseBasement;
			}
			set
			{
				_isPurchaseBasement = value;
			}
		}


		private DateTime _taxDeadline;

		public virtual DateTime TaxDeadline
		{
			get
			{
				return _taxDeadline;
			}
			set
			{
				_taxDeadline = value;
			}
		}


	}
}