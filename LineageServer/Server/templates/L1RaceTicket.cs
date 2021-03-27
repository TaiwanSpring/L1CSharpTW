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
	public class L1RaceTicket
	{

		private int _itemobjid;
		private int _round;
		private double _allotment_percentage;
		private int victory;
		private int _runner_num;
		public virtual int get_itemobjid()
		{
			return _itemobjid;
		}

		public virtual void set_itemobjid(int i)
		{
			_itemobjid = i;
		}

		public virtual void set_allotment_percentage(double allotment_percentage)
		{
			this._allotment_percentage = allotment_percentage;
		}

		public virtual double get_allotment_percentage()
		{
			return _allotment_percentage;
		}

		public virtual void set_round(int _round)
		{
			this._round = _round;
		}

		public virtual int get_round()
		{
			return _round;
		}

		public virtual void set_victory(int victory)
		{
			this.victory = victory;
		}

		public virtual int get_victory()
		{
			return victory;
		}

		public virtual void set_runner_num(int _runner_num)
		{
			this._runner_num = _runner_num;
		}

		public virtual int get_runner_num()
		{
			return _runner_num;
		}

	}

}