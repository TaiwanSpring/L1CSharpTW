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
	public class L1PetItem
	{
		public L1PetItem()
		{
		}

		private int _itemId;

		public virtual int ItemId
		{
			get
			{
				return _itemId;
			}
			set
			{
				_itemId = value;
			}
		}


		private int _hitModifier;

		public virtual int HitModifier
		{
			get
			{
				return _hitModifier;
			}
			set
			{
				_hitModifier = value;
			}
		}


		private int _damageModifier;

		public virtual int DamageModifier
		{
			get
			{
				return _damageModifier;
			}
			set
			{
				_damageModifier = value;
			}
		}


		private int _AddAc;

		public virtual int AddAc
		{
			get
			{
				return _AddAc;
			}
			set
			{
				_AddAc = value;
			}
		}


		private int _addStr;

		public virtual int AddStr
		{
			get
			{
				return _addStr;
			}
			set
			{
				_addStr = value;
			}
		}


		private int _addCon;

		public virtual int AddCon
		{
			get
			{
				return _addCon;
			}
			set
			{
				_addCon = value;
			}
		}


		private int _addDex;

		public virtual int AddDex
		{
			get
			{
				return _addDex;
			}
			set
			{
				_addDex = value;
			}
		}


		private int _addInt;

		public virtual int AddInt
		{
			get
			{
				return _addInt;
			}
			set
			{
				_addInt = value;
			}
		}


		private int _addWis;

		public virtual int AddWis
		{
			get
			{
				return _addWis;
			}
			set
			{
				_addWis = value;
			}
		}


		private int _addHp;

		public virtual int AddHp
		{
			get
			{
				return _addHp;
			}
			set
			{
				_addHp = value;
			}
		}


		private int _addMp;

		public virtual int AddMp
		{
			get
			{
				return _addMp;
			}
			set
			{
				_addMp = value;
			}
		}


		private int _addSp;

		public virtual int AddSp
		{
			get
			{
				return _addSp;
			}
			set
			{
				_addSp = value;
			}
		}


		private int _addMr;

		public virtual int AddMr
		{
			get
			{
				return _addMr;
			}
			set
			{
				_addMr = value;
			}
		}


		// 使用類型 - 牙齒? 1  防具? 0
		private int _useType;

		public virtual int UseType
		{
			get
			{
				return _useType;
			}
			set
			{
				_useType = value;
			}
		}


	}

}