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

	public class L1Castle
	{
		public L1Castle(int id, string name)
		{
			_id = id;
			_name = name;
		}

		private int _id;

		public virtual int Id
		{
			get
			{
				return _id;
			}
		}

		private string _name;

		public virtual string Name
		{
			get
			{
				return _name;
			}
		}

		private DateTime _warTime;

		public virtual DateTime WarTime
		{
			get
			{
				return _warTime;
			}
			set
			{
				_warTime = value;
			}
		}


		private int _taxRate;

		public virtual int TaxRate
		{
			get
			{
				return _taxRate;
			}
			set
			{
				_taxRate = value;
			}
		}


		private int _publicMoney;

		public virtual int PublicMoney
		{
			get
			{
				if (_publicMoney < 0)
				{
					return 0;
				}
				else
				{
					return _publicMoney;
				}
			}
			set
			{
				_publicMoney = value;
			}
		}


		private int heldByClan;

		public virtual int HeldClanId
		{
			get
			{
				return heldByClan;
			}
		}

		public virtual int HeldClan
		{
			set
			{
				heldByClan = value;
			}
		}

	}

}