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
	public class L1Pet
	{
		public L1Pet()
		{
		}

		private int _itemobjid;

		public virtual int get_itemobjid()
		{
			return _itemobjid;
		}

		public virtual void set_itemobjid(int i)
		{
			_itemobjid = i;
		}

		private int _objid;

		public virtual int get_objid()
		{
			return _objid;
		}

		public virtual void set_objid(int i)
		{
			_objid = i;
		}

		private int _npcid;

		public virtual int get_npcid()
		{
			return _npcid;
		}

		public virtual void set_npcid(int i)
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

		// 飽食度
		private int _food;

		public virtual int get_food()
		{
			return _food;
		}

		public virtual void set_food(int i)
		{
			_food = i;
		}
	}
}