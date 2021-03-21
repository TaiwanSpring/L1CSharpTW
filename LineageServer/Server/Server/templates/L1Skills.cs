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
	public class L1Skills
	{

		public const int ATTR_NONE = 0;

		public const int ATTR_EARTH = 1;

		public const int ATTR_FIRE = 2;

		public const int ATTR_WATER = 4;

		public const int ATTR_WIND = 8;

		public const int ATTR_RAY = 16;

		public const int TYPE_PROBABILITY = 1;

		public const int TYPE_CHANGE = 2;

		public const int TYPE_CURSE = 4;

		public const int TYPE_DEATH = 8;

		public const int TYPE_HEAL = 16;

		public const int TYPE_RESTORE = 32;

		public const int TYPE_ATTACK = 64;

		public const int TYPE_OTHER = 128;

		public const int TARGET_TO_ME = 0;

		public const int TARGET_TO_PC = 1;

		public const int TARGET_TO_NPC = 2;

		public const int TARGET_TO_CLAN = 4;

		public const int TARGET_TO_PARTY = 8;

		public const int TARGET_TO_PET = 16;

		public const int TARGET_TO_PLACE = 32;

		private int _skillId;

		public virtual int SkillId
		{
			get
			{
				return _skillId;
			}
			set
			{
				_skillId = value;
			}
		}


		private string _name;

		public virtual string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}


		private int _skillLevel;

		public virtual int SkillLevel
		{
			get
			{
				return _skillLevel;
			}
			set
			{
				_skillLevel = value;
			}
		}


		private int _skillNumber;

		public virtual int SkillNumber
		{
			get
			{
				return _skillNumber;
			}
			set
			{
				_skillNumber = value;
			}
		}


		private int _mpConsume;

		public virtual int MpConsume
		{
			get
			{
				return _mpConsume;
			}
			set
			{
				_mpConsume = value;
			}
		}


		private int _hpConsume;

		public virtual int HpConsume
		{
			get
			{
				return _hpConsume;
			}
			set
			{
				_hpConsume = value;
			}
		}


		private int _itmeConsumeId;

		public virtual int ItemConsumeId
		{
			get
			{
				return _itmeConsumeId;
			}
			set
			{
				_itmeConsumeId = value;
			}
		}


		private int _itmeConsumeCount;

		public virtual int ItemConsumeCount
		{
			get
			{
				return _itmeConsumeCount;
			}
			set
			{
				_itmeConsumeCount = value;
			}
		}


		private int _reuseDelay; // 単位：ミリ秒

		public virtual int ReuseDelay
		{
			get
			{
				return _reuseDelay;
			}
			set
			{
				_reuseDelay = value;
			}
		}


		private int _buffDuration; // 単位：秒

		public virtual int BuffDuration
		{
			get
			{
				return _buffDuration;
			}
			set
			{
				_buffDuration = value;
			}
		}


		private string _target;

		public virtual string Target
		{
			get
			{
				return _target;
			}
			set
			{
				_target = value;
			}
		}


		private int _targetTo; // 対象 0:自分 1:PC 2:NPC 4:血盟 8:パーティ 16:ペット 32:場所

		public virtual int TargetTo
		{
			get
			{
				return _targetTo;
			}
			set
			{
				_targetTo = value;
			}
		}


		private int _damageValue;

		public virtual int DamageValue
		{
			get
			{
				return _damageValue;
			}
			set
			{
				_damageValue = value;
			}
		}


		private int _damageDice;

		public virtual int DamageDice
		{
			get
			{
				return _damageDice;
			}
			set
			{
				_damageDice = value;
			}
		}


		private int _damageDiceCount;

		public virtual int DamageDiceCount
		{
			get
			{
				return _damageDiceCount;
			}
			set
			{
				_damageDiceCount = value;
			}
		}


		private int _probabilityValue;

		public virtual int ProbabilityValue
		{
			get
			{
				return _probabilityValue;
			}
			set
			{
				_probabilityValue = value;
			}
		}


		private int _probabilityDice;

		public virtual int ProbabilityDice
		{
			get
			{
				return _probabilityDice;
			}
			set
			{
				_probabilityDice = value;
			}
		}


		private int _attr;

		/// <summary>
		/// スキルの属性を返す。<br>
		/// 0.無属性魔法,1.地魔法,2.火魔法,4.水魔法,8.風魔法,16.光魔法
		/// </summary>
		public virtual int Attr
		{
			get
			{
				return _attr;
			}
			set
			{
				_attr = value;
			}
		}


		private int _type; // タイプ

		/// <summary>
		/// スキルの作用の種類を返す。<br>
		/// 1.確率系,2.エンチャント,4.呪い,8.死,16.治療,32.復活,64.攻撃,128.その他特殊
		/// </summary>
		public virtual int Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}


		private int _lawful;

		public virtual int Lawful
		{
			get
			{
				return _lawful;
			}
			set
			{
				_lawful = value;
			}
		}


		private int _ranged;

		public virtual int Ranged
		{
			get
			{
				return _ranged;
			}
			set
			{
				_ranged = value;
			}
		}


		private int _area;

		public virtual int Area
		{
			get
			{
				return _area;
			}
			set
			{
				_area = value;
			}
		}


		internal bool _isThrough;

		public virtual bool Through
		{
			get
			{
				return _isThrough;
			}
			set
			{
				_isThrough = value;
			}
		}


		private int _id;

		public virtual int Id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}


		private string _nameId;

		public virtual string NameId
		{
			get
			{
				return _nameId;
			}
			set
			{
				_nameId = value;
			}
		}


		private int _actionId;

		public virtual int ActionId
		{
			get
			{
				return _actionId;
			}
			set
			{
				_actionId = value;
			}
		}


		private int _castGfx;

		public virtual int CastGfx
		{
			get
			{
				return _castGfx;
			}
			set
			{
				_castGfx = value;
			}
		}


		private int _castGfx2;

		public virtual int CastGfx2
		{
			get
			{
				return _castGfx2;
			}
			set
			{
				_castGfx2 = value;
			}
		}


		private int _sysmsgIdHappen;

		public virtual int SysmsgIdHappen
		{
			get
			{
				return _sysmsgIdHappen;
			}
			set
			{
				_sysmsgIdHappen = value;
			}
		}


		private int _sysmsgIdStop;

		public virtual int SysmsgIdStop
		{
			get
			{
				return _sysmsgIdStop;
			}
			set
			{
				_sysmsgIdStop = value;
			}
		}


		private int _sysmsgIdFail;

		public virtual int SysmsgIdFail
		{
			get
			{
				return _sysmsgIdFail;
			}
			set
			{
				_sysmsgIdFail = value;
			}
		}


	}

}