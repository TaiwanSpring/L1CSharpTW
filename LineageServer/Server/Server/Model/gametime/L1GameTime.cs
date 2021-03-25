using LineageServer.Server.Server.utils;
using System;
namespace LineageServer.Server.Server.Model.Gametime
{
	public enum L1GameTimeTypeEnum
	{
		MONTH,
		DAY_OF_MONTH,
		HOUR_OF_DAY,
		MINUTE,
	}
	public class L1GameTime
	{
		private readonly int _time;

		private readonly DateTime _calendar = DateTime.Now;

		public L1GameTime()//TODO 自動重啟
		{

		}

		//public static L1GameTime valueOf(long timeMillis)
		//{
		//	long t1 = timeMillis - BASE_TIME_IN_MILLIS_REAL;
		//	if (t1 < 0)
		//	{
		//		throw new System.ArgumentException();
		//	}
		//	int t2 = (int)( ( t1 * 6 ) / 1000L );
		//	int t3 = t2 % 3; // 時間が3の倍数になるように調整
		//	return new L1GameTime(t2 - t3);
		//}
		public static L1GameTime fromSystemCurrentTime()
		{
			return new L1GameTime();
		}

		public virtual int get(L1GameTimeTypeEnum field)
		{
			switch (field)
			{
				case L1GameTimeTypeEnum.MONTH:
					return _calendar.Month;
				case L1GameTimeTypeEnum.DAY_OF_MONTH:
					return _calendar.Day;
				case L1GameTimeTypeEnum.HOUR_OF_DAY:
					return _calendar.Hour;
				case L1GameTimeTypeEnum.MINUTE:
				default:
					return _calendar.Minute;
			}
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
				return _calendar;
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
			return _calendar.ToString("yyyyMMdd HH:mm:ss");
		}
	}

}