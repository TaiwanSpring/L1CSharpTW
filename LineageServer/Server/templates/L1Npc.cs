using LineageServer.Extensions;
using LineageServer.Server.Model;
using System;

namespace LineageServer.Server.Templates
{
	class L1Npc : GameObject
	{
		public L1Npc()
		{
		}

		private int _npcid;

		public virtual int get_npcId()
		{
			return _npcid;
		}

		public virtual void set_npcId(int i)
		{
			_npcid = i;
		}

		private string _name;

		public virtual string get_name()
		{
			return _name;
		}

		public virtual void set_name(string s)
		{
			_name = s;
		}

		private string _impl;

		public virtual string Impl
		{
			get
			{
				return _impl;
			}
			set
			{
				_impl = value;
			}
		}


		private int _level;

		public virtual int get_level()
		{
			return _level;
		}

		public virtual void set_level(int i)
		{
			_level = i;
		}

		private int _hp;

		public virtual int get_hp()
		{
			return _hp;
		}

		public virtual void set_hp(int i)
		{
			_hp = i;
		}

		private int _mp;

		public virtual int get_mp()
		{
			return _mp;
		}

		public virtual void set_mp(int i)
		{
			_mp = i;
		}

		private int _ac;

		public virtual int get_ac()
		{
			return _ac;
		}

		public virtual void set_ac(int i)
		{
			_ac = i;
		}

		private byte _str;

		public virtual byte get_str()
		{
			return _str;
		}

		public virtual void set_str(byte i)
		{
			_str = i;
		}

		private byte _con;

		public virtual byte get_con()
		{
			return _con;
		}

		public virtual void set_con(byte i)
		{
			_con = i;
		}

		private byte _dex;

		public virtual byte get_dex()
		{
			return _dex;
		}

		public virtual void set_dex(byte i)
		{
			_dex = i;
		}

		private byte _wis;

		public virtual byte get_wis()
		{
			return _wis;
		}

		public virtual void set_wis(byte i)
		{
			_wis = i;
		}

		private byte _int;

		public virtual byte get_int()
		{
			return _int;
		}

		public virtual void set_int(byte i)
		{
			_int = i;
		}

		private int _mr;

		public virtual int get_mr()
		{
			return _mr;
		}

		public virtual void set_mr(int i)
		{
			_mr = i;
		}

		private int _exp;

		public virtual int get_exp()
		{
			return _exp;
		}

		public virtual void set_exp(int i)
		{
			_exp = i;
		}

		private int _lawful;

		public virtual int get_lawful()
		{
			return _lawful;
		}

		public virtual void set_lawful(int i)
		{
			_lawful = i;
		}

		private string _size;

		public virtual string get_size()
		{
			return _size;
		}

		public virtual void set_size(string s)
		{
			_size = s;
		}

		private int _weakAttr;

		public virtual int get_weakAttr()
		{
			return _weakAttr;
		}

		public virtual void set_weakAttr(int i)
		{
			_weakAttr = i;
		}

		private int _ranged;

		public virtual int get_ranged()
		{
			return _ranged;
		}

		public virtual void set_ranged(int i)
		{
			_ranged = i;
		}

		private bool _agrososc;

		public virtual bool is_agrososc()
		{
			return _agrososc;
		}

		public virtual void set_agrososc(bool flag)
		{
			_agrososc = flag;
		}

		private bool _agrocoi;

		public virtual bool is_agrocoi()
		{
			return _agrocoi;
		}

		public virtual void set_agrocoi(bool flag)
		{
			_agrocoi = flag;
		}

		private bool _tameable;

		public virtual bool Tamable
		{
			get
			{
				return _tameable;
			}
			set
			{
				_tameable = value;
			}
		}


		private int _passispeed;

		public virtual int get_passispeed()
		{
			return _passispeed;
		}

		public virtual void set_passispeed(int i)
		{
			_passispeed = i;
		}

		private int _atkspeed;

		public virtual int get_atkspeed()
		{
			return _atkspeed;
		}

		public virtual void set_atkspeed(int i)
		{
			_atkspeed = i;
		}

		private bool _agro;

		public virtual bool is_agro()
		{
			return _agro;
		}

		public virtual void set_agro(bool flag)
		{
			_agro = flag;
		}

		private int _gfxid;

		public virtual int get_gfxid()
		{
			return _gfxid;
		}

		public virtual void set_gfxid(int i)
		{
			_gfxid = i;
		}

		private string _nameid;

		public virtual string get_nameid()
		{
			return _nameid;
		}

		public virtual void set_nameid(string s)
		{
			_nameid = s;
		}

		private int _undead;

		public virtual int get_undead()
		{
			return _undead;
		}

		public virtual void set_undead(int i)
		{
			_undead = i;
		}

		private int _poisonatk;

		public virtual int get_poisonatk()
		{
			return _poisonatk;
		}

		public virtual void set_poisonatk(int i)
		{
			_poisonatk = i;
		}

		private int _paralysisatk;

		public virtual int get_paralysisatk()
		{
			return _paralysisatk;
		}

		public virtual void set_paralysisatk(int i)
		{
			_paralysisatk = i;
		}

		private int _family;

		public virtual int get_family()
		{
			return _family;
		}

		public virtual void set_family(int i)
		{
			_family = i;
		}

		private int _agrofamily;

		public virtual int get_agrofamily()
		{
			return _agrofamily;
		}

		public virtual void set_agrofamily(int i)
		{
			_agrofamily = i;
		}

		private int _agrogfxid1;

		public virtual int is_agrogfxid1()
		{
			return _agrogfxid1;
		}

		public virtual void set_agrogfxid1(int i)
		{
			_agrogfxid1 = i;
		}

		private int _agrogfxid2;

		public virtual int is_agrogfxid2()
		{
			return _agrogfxid2;
		}

		public virtual void set_agrogfxid2(int i)
		{
			_agrogfxid2 = i;
		}

		private bool _picupitem;

		public virtual bool is_picupitem()
		{
			return _picupitem;
		}

		public virtual void set_picupitem(bool flag)
		{
			_picupitem = flag;
		}

		private int _digestitem;

		public virtual int get_digestitem()
		{
			return _digestitem;
		}

		public virtual void set_digestitem(int i)
		{
			_digestitem = i;
		}

		private bool _bravespeed;

		public virtual bool is_bravespeed()
		{
			return _bravespeed;
		}

		public virtual void set_bravespeed(bool flag)
		{
			_bravespeed = flag;
		}

		private int _hprinterval;

		public virtual int get_hprinterval()
		{
			return _hprinterval;
		}

		public virtual void set_hprinterval(int i)
		{
			_hprinterval = i;
		}

		private int _hpr;

		public virtual int get_hpr()
		{
			return _hpr;
		}

		public virtual void set_hpr(int i)
		{
			_hpr = i;
		}

		private int _mprinterval;

		public virtual int get_mprinterval()
		{
			return _mprinterval;
		}

		public virtual void set_mprinterval(int i)
		{
			_mprinterval = i;
		}

		private int _mpr;

		public virtual int get_mpr()
		{
			return _mpr;
		}

		public virtual void set_mpr(int i)
		{
			_mpr = i;
		}

		private bool _teleport;

		public virtual bool is_teleport()
		{
			return _teleport;
		}

		public virtual void set_teleport(bool flag)
		{
			_teleport = flag;
		}

		private int _randomlevel;

		public virtual int get_randomlevel()
		{
			return _randomlevel;
		}

		public virtual void set_randomlevel(int i)
		{
			_randomlevel = i;
		}

		private int _randomhp;

		public virtual int get_randomhp()
		{
			return _randomhp;
		}

		public virtual void set_randomhp(int i)
		{
			_randomhp = i;
		}

		private int _randommp;

		public virtual int get_randommp()
		{
			return _randommp;
		}

		public virtual void set_randommp(int i)
		{
			_randommp = i;
		}

		private int _randomac;

		public virtual int get_randomac()
		{
			return _randomac;
		}

		public virtual void set_randomac(int i)
		{
			_randomac = i;
		}

		private int _randomexp;

		public virtual int get_randomexp()
		{
			return _randomexp;
		}

		public virtual void set_randomexp(int i)
		{
			_randomexp = i;
		}

		private int _randomlawful;

		public virtual int get_randomlawful()
		{
			return _randomlawful;
		}

		public virtual void set_randomlawful(int i)
		{
			_randomlawful = i;
		}

		private int _damagereduction;

		public virtual int get_damagereduction()
		{
			return _damagereduction;
		}

		public virtual void set_damagereduction(int i)
		{
			_damagereduction = i;
		}

		private bool _hard;

		public virtual bool is_hard()
		{
			return _hard;
		}

		public virtual void set_hard(bool flag)
		{
			_hard = flag;
		}

		private bool _doppel;

		public virtual bool is_doppel()
		{
			return _doppel;
		}

		public virtual void set_doppel(bool flag)
		{
			_doppel = flag;
		}

		private bool _tu;

		public virtual void set_IsTU(bool i)
		{
			_tu = i;
		}

		public virtual bool get_IsTU()
		{
			return _tu;
		}

		private bool _erase;

		public virtual void set_IsErase(bool i)
		{
			_erase = i;
		}

		public virtual bool get_IsErase()
		{
			return _erase;
		}

		private int bowActId = 0;

		public virtual int BowActId
		{
			get
			{
				return bowActId;
			}
			set
			{
				bowActId = value;
			}
		}


		private int _karma;

		public virtual int Karma
		{
			get
			{
				return _karma;
			}
			set
			{
				_karma = value;
			}
		}


		private int _transformId;

		public virtual int TransformId
		{
			get
			{
				return _transformId;
			}
			set
			{
				_transformId = value;
			}
		}


		private int _transformGfxId;

		public virtual int TransformGfxId
		{
			get
			{
				return _transformGfxId;
			}
			set
			{
				_transformGfxId = value;
			}
		}


		private int _altAtkSpeed;

		public virtual int AltAtkSpeed
		{
			get
			{
				return _altAtkSpeed;
			}
			set
			{
				_altAtkSpeed = value;
			}
		}


		private int _atkMagicSpeed;

		public virtual int AtkMagicSpeed
		{
			get
			{
				return _atkMagicSpeed;
			}
			set
			{
				_atkMagicSpeed = value;
			}
		}


		private int _subMagicSpeed;

		public virtual int SubMagicSpeed
		{
			get
			{
				return _subMagicSpeed;
			}
			set
			{
				_subMagicSpeed = value;
			}
		}


		private int _lightSize;

		public virtual int LightSize
		{
			get
			{
				return _lightSize;
			}
			set
			{
				_lightSize = value;
			}
		}


		private bool _amountFixed;

		/// <summary>
		/// mapidsテーブルで設定されたモンスター量倍率の影響を受けるかどうかを返す。
		/// </summary>
		/// <returns> 影響を受けないように設定されている場合はtrueを返す。 </returns>
		public virtual bool AmountFixed
		{
			get
			{
				return _amountFixed;
			}
			set
			{
				_amountFixed = value;
			}
		}


		private bool _changeHead;

		public virtual bool ChangeHead
		{
			get
			{
				return _changeHead;
			}
			set
			{
				_changeHead = value;
			}
		}


		private bool _isCantResurrect;

		public virtual bool CantResurrect
		{
			get
			{
				return _isCantResurrect;
			}
			set
			{
				_isCantResurrect = value;
			}
		}

		public L1Npc clone()
		{
			return this.DeepCopyByExpressionTree();
		}

	}

}