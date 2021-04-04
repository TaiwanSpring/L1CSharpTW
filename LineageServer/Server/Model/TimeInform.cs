using System;
namespace LineageServer.Server.Model
{
    class TimeInform
    {
        /// <summary>
        /// Calendar本身是abstract,使用getInstance()來取得物件
        /// </summary>
        internal static DateTime rightNow = DateTime.Now;

        /// <returns> getYear 年 </returns>
        /// <param name="type"> 0:原始(可加減) 1:西元 2:民國 </param>
        /// <param name="i"> = +|- years      </param>
        public static string getYear(int type, int i)
        {
            string year = null;
            if (type == 0)
            {
                year = (rightNow.Year + i).ToString();
            }
            else if (type == 1) // 西元
            {
                year = "西元 " + rightNow.Year.ToString();
            }
            else if (type == 2)
            {
                // 民國
                year = "民國 " + (rightNow.Year - 1911).ToString();
            }
            else
            {
                year = null;
            }
            return year;
        }

        /// <returns> getMonth 月 </returns>
        public static string Month
        {
            get
            {
                // Calendar.MONTH - index從0開始
                return (rightNow.Month + 1).ToString();
            }
        }

        /// <returns> getDay 日 </returns>
        public static string Day
        {
            get
            {

                return rightNow.Day.ToString();
            }
        }

        /// <returns> getHour 時 </returns>
        public static string Hour
        {
            get
            {

                return rightNow.Hour.ToString();
            }
        }

        /// <returns> getMinute 分 </returns>
        public static string Minute
        {
            get
            {

                return rightNow.Minute.ToString();
            }
        }

        /// <returns> getSecond 秒 </returns>
        public static string Second
        {
            get
            {

                return rightNow.Minute.ToString();
            }
        }

        /// 
        /// <param name="type">
        ///            時間格式<BR>
        ///            type = 1 : X年X月X日星期X<BR>
        ///            type = 2 : X時X分X秒<BR>
        ///            type = 3 : X年X月X日-X時X分X秒<BR> </param>
        /// <param name="type_year">
        ///            0:西元 1:民國
        /// @return </param>
        public static string getNowTime(int type, int type_year)
        {
            string NowTime = null;
            switch (type)
            {
                case 1:
                    NowTime = $"{getYear(type_year, 0)}年 {Month}月{Day}日 {DayOfWeek}";
                    break;
                case 2:
                    NowTime = $"{Hour}時{Minute}分{Second}秒";
                    break;
                case 3:
                    NowTime = $"{getYear(type_year, 0)}年{Month}月{Day}日{Hour}時{Minute}分{Second}秒";
                    goto default;
                default:

                    break;
            }
            return NowTime;
        }

        /// <returns> getDayOfWeek 星期 </returns>
        public static string DayOfWeek
        {
            get
            {
                string DayOfWeek = null;
                switch (rightNow.DayOfWeek)
                {
                    case System.DayOfWeek.Sunday: // index 1~7 星期日~星期六
                        DayOfWeek = "星期日";
                        break;
                    case System.DayOfWeek.Monday:
                        DayOfWeek = "星期一";
                        break;
                    case System.DayOfWeek.Tuesday:
                        DayOfWeek = "星期二";
                        break;
                    case System.DayOfWeek.Wednesday:
                        DayOfWeek = "星期三";
                        break;
                    case System.DayOfWeek.Thursday:
                        DayOfWeek = "星期四";
                        break;
                    case System.DayOfWeek.Friday:
                        DayOfWeek = "星期五";
                        break;
                    case System.DayOfWeek.Saturday:
                        DayOfWeek = "星期六";
                        break;

                }
                return DayOfWeek;
            }
        }

        /// <returns> yyyy-MM-dd HH:mm:ss </returns>
        public virtual string NowTime_Standard
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }

}