using LineageServer.Server.Server.utils;
using System;
namespace LineageServer.Server.Server.Model.Gametime
{
	public enum L1GameTimeTypeEnum
	{
		Month,
		DayOfMonth,
		HourOfDay,
		Minute,
	}

	public enum OnTimeTypeEnum
	{
		Night
	}
	public class L1GameTime
	{
		private static readonly DateTime baseTime = new DateTime(2003, 07, 03, 0, 0, 0);
		private readonly DateTime time = DateTime.Now;

		public virtual int GetValue(L1GameTimeTypeEnum field)
		{
			switch (field)
			{
				case L1GameTimeTypeEnum.Month:
					return time.Month;
				case L1GameTimeTypeEnum.DayOfMonth:
					return time.Day;
				case L1GameTimeTypeEnum.HourOfDay:
					return time.Hour;
				case L1GameTimeTypeEnum.Minute:
				default:
					return time.Minute;
			}
		}

		public bool IsOnTime(OnTimeTypeEnum onTimeType)
		{
			switch (onTimeType)
			{
				// 6:00-17:59(昼)で無ければtrue
				case OnTimeTypeEnum.Night:
					return !IntRange.includes(this.time.Hour, 6, 17);
				default:
					return false;
			}
		}

		public virtual int Seconds
		{
			get
			{
				return ( this.time.Hour * 3600 ) + ( this.time.Minute * 60 ) + this.time.Second;
			}
		}

		public virtual DateTime Calendar
		{
			get
			{
				return time;
			}
		}

		public virtual bool Night
		{
			get
			{
				return IsOnTime(OnTimeTypeEnum.Night);
			}
		}

		public override string ToString()
		{
			return time.ToString("yyyyMMdd HH:mm:ss");
		}
	}

}