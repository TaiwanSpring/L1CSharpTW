using LineageServer.Enum;
using LineageServer.Utils;
using System;
namespace LineageServer.Server.Model.Gametime
{
    public class L1GameTime
    {
        private static readonly DateTime baseTime = new DateTime(2003, 07, 03, 0, 0, 0);
        private readonly DateTime time = DateTime.Now;

        public virtual int GetValue(DateTimeFieldTypeEnum dateTimeFieldType)
        {
            switch (dateTimeFieldType)
            {
                case DateTimeFieldTypeEnum.Month:
                    return time.Month;
                case DateTimeFieldTypeEnum.Day:
                    return time.Day;
                case DateTimeFieldTypeEnum.Hour:
                    return time.Hour;
                case DateTimeFieldTypeEnum.Minute:
                default:
                    return time.Minute;
            }
        }

        public bool IsOnTime(TimeRangeTypeEnum timeRangeType)
        {
            switch (timeRangeType)
            {
                // 6:00-17:59(昼)で無ければtrue
                case TimeRangeTypeEnum.Night:
                    return !IntRange.includes(this.time.Hour, 6, 17);
                default:
                    return false;
            }
        }

        public virtual int Seconds
        {
            get
            {
                return (this.time.Hour * 3600) + (this.time.Minute * 60) + this.time.Second;
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
                return IsOnTime(TimeRangeTypeEnum.Night);
            }
        }

        public override string ToString()
        {
            return time.ToString("yyyyMMdd HH:mm:ss");
        }
    }

}