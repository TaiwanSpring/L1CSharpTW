using System;

//********************************************************************
//  GetNowTime.java
//  by mca
// 2008/4/1
//********************************************************************
namespace LineageServer.Server
{
    //************************************************************************
    // 取得現在時間
    //************************************************************************
    public class GetNowTime
    {
        public static int GetNowYear()
        {
            return DateTime.Now.Year;
        }

        public static int GetNowMonth()
        {
            return DateTime.Now.Month;
        }

        public static int GetNowDay()
        {
            return DateTime.Now.Day;
        }
        public static string GetNowWeek()
        {
            string dayOfWeek = string.Empty;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday: // index 1~7 星期日~星期六
                    dayOfWeek = "星期日";
                    break;
                case DayOfWeek.Monday:
                    dayOfWeek = "星期一";
                    break;
                case DayOfWeek.Tuesday:
                    dayOfWeek = "星期二";
                    break;
                case DayOfWeek.Wednesday:
                    dayOfWeek = "星期三";
                    break;
                case DayOfWeek.Thursday:
                    dayOfWeek = "星期四";
                    break;
                case DayOfWeek.Friday:
                    dayOfWeek = "星期五";
                    break;
                case DayOfWeek.Saturday:
                    dayOfWeek = "星期六";
                    break;

            }
            return dayOfWeek;
        }
        public static int GetNowHour()
        {
            return DateTime.Now.Hour; // 傳回取得此時之值
        }

        public static int GetNowMinute()
        {
            return DateTime.Now.Minute; // 傳回取得此時之值
        }

        public static int GetNowSecond()
        {
            return DateTime.Now.Second; // 傳回取得此時之值
        }
    }
    //<====程式結束
}