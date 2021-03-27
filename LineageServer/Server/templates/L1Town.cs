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
	public class L1Town
	{
		public L1Town()
		{
		}

		private int _townid;

		public virtual int get_townid()
		{
			return _townid;
		}

		public virtual void set_townid(int i)
		{
			_townid = i;
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

		private int _leader_id;

		public virtual int get_leader_id()
		{
			return _leader_id;
		}

		public virtual void set_leader_id(int i)
		{
			_leader_id = i;
		}

		private string _leader_name;

		public virtual string get_leader_name()
		{
			return _leader_name;
		}

		public virtual void set_leader_name(string s)
		{
			_leader_name = s;
		}

		private int _tax_rate;

		public virtual int get_tax_rate()
		{
			return _tax_rate;
		}

		public virtual void set_tax_rate(int i)
		{
			_tax_rate = i;
		}

		private int _tax_rate_reserved;

		public virtual int get_tax_rate_reserved()
		{
			return _tax_rate_reserved;
		}

		public virtual void set_tax_rate_reserved(int i)
		{
			_tax_rate_reserved = i;
		}

		private int _sales_money;

		public virtual int get_sales_money()
		{
			return _sales_money;
		}

		public virtual void set_sales_money(int i)
		{
			_sales_money = i;
		}

		private int _sales_money_yesterday;

		public virtual int get_sales_money_yesterday()
		{
			return _sales_money_yesterday;
		}

		public virtual void set_sales_money_yesterday(int i)
		{
			_sales_money_yesterday = i;
		}

		private int _town_tax;

		public virtual int get_town_tax()
		{
			return _town_tax;
		}

		public virtual void set_town_tax(int i)
		{
			_town_tax = i;
		}

		private int _town_fix_tax;

		public virtual int get_town_fix_tax()
		{
			return _town_fix_tax;
		}

		public virtual void set_town_fix_tax(int i)
		{
			_town_fix_tax = i;
		}
	}

}