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
namespace LineageServer.Server.Server.Model.gametime
{

	using IntRange = LineageServer.Server.Server.utils.IntRange;

	public class L1GameTime
	{
		// 2003年7月3日 12:00(UTC)が1月1日00:00
		private const long BASE_TIME_IN_MILLIS_REAL = 1057233600000L;

		private readonly int _time;

		private readonly DateTime _calendar;

		private DateTime makeCalendar(int time)
		{
			DateTime cal = DateTime.getInstance(TimeZone.getTimeZone("UTC"));
			cal.TimeInMillis = 0;
			cal.AddSeconds(time);
			return cal;
		}

		private L1GameTime(int time)
		{
			_time = time;
			_calendar = makeCalendar(time);
		}

		public static L1GameTime valueOf(long timeMillis)
		{
			long t1 = timeMillis - BASE_TIME_IN_MILLIS_REAL;
			if (t1 < 0)
			{
				throw new System.ArgumentException();
			}
			int t2 = (int)((t1 * 6) / 1000L);
			int t3 = t2 % 3; // 時間が3の倍数になるように調整
			return new L1GameTime(t2 - t3);
		}
		public L1GameTime() : this((int) DateTimeHelper.CurrentUnixTimeMillis()) //TODO 自動重啟
		{
		}
		public static L1GameTime fromSystemCurrentTime()
		{
			return L1GameTime.valueOf(DateTimeHelper.CurrentUnixTimeMillis());
		}

		public static L1GameTime valueOfGameTime(Time time)
		{
			long t = time.Time + TimeZone.Default.RawOffset;
			return new L1GameTime((int)(t / 1000L));
		}

		public virtual Time toTime()
		{
			int t = _time % (24 * 3600); // 日付情報分を切り捨て
			return new Time(t * 1000L - TimeZone.Default.RawOffset);
		}

		public virtual int get(int field)
		{
			return _calendar.get(field);
		}

		public virtual int Seconds
		{
			get
			{
				return _time;
			}
		}

		public virtual DateTime Calendar
		{
			get
			{
				return (DateTime) _calendar.clone();
			}
		}

		public virtual bool Night
		{
			get
			{
				int hour = _calendar.Hour;
				return !IntRange.includes(hour, 6, 17); // 6:00-17:59(昼)で無ければtrue
			}
		}

		public override string ToString()
		{
			SimpleDateFormat f = new SimpleDateFormat("yyyy.MM.dd G 'at' HH:mm:ss z");
			f.TimeZone = _calendar.TimeZone;
			return f.format(_calendar) + "(" + Seconds + ")";
		}
	}

}