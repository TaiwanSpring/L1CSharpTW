using System;

//********************************************************************
//  GetNowTime.java
//  by mca
// 2008/4/1
//********************************************************************
namespace LineageServer.Server.Server
{
	//************************************************************************
	// 取得現在時間
	//************************************************************************
	public class GetNowTime
	{
	  public static int GetNowYear()
	  {
		DateTime rightNow = new DateTime(); //取得預設月曆物件
		int nowYear;
		nowYear = rightNow.Year; //取得現年之值
		return nowYear; // 傳回取得現年之值
	  }

		public static int GetNowMonth()
		{
		DateTime rightNow = new DateTime(); //取得預設月曆物件
		int nowMonth;
		nowMonth = rightNow.Month; //取得現月之值
		return nowMonth; // 傳回取得現月之值
		}

		public static int GetNowDay()
		{
		DateTime rightNow = new DateTime(); //取得預設月曆物件
		int nowDay;
		nowDay = rightNow.Day; //取得今日之值
		return nowDay; // 傳回取得今日之值
		}
		public static string GetNowWeek()
		{
			DateTime rightNow = new DateTime(); // 取得預設月曆物件
			string DayOfWeek = null;
			int nowWeek;
			nowWeek = rightNow.DayOfWeek; // 取得今日星期之值
			switch (nowWeek)
			{
			case 1: // index 1~7 星期日~星期六
				DayOfWeek = "星期日";
				break;
			case 2:
				DayOfWeek = "星期一";
				break;
			case 3:
				DayOfWeek = "星期二";
				break;
			case 4:
				DayOfWeek = "星期三";
				break;
			case 5:
				DayOfWeek = "星期四";
				break;
			case 6:
				DayOfWeek = "星期五";
				break;
			case 7:
				DayOfWeek = "星期六";
				break;

			}
			return DayOfWeek;
		}
		public static int GetNowHour()
		{
		DateTime rightNow = new DateTime(); //取得預設月曆物件
		int nowHour;
		nowHour = rightNow.Hour; //取得此時之值
		return nowHour; // 傳回取得此時之值
		}

		public static int GetNowMinute()
		{
		DateTime rightNow = new DateTime(); //取得預設月曆物件
		int nowMinute;
		nowMinute = rightNow.Minute; //取得此分之值
		return nowMinute; // 傳回取得此分之值
		}

		public static int GetNowSecond()
		{
		DateTime rightNow = new DateTime(); //取得預設月曆物件
		int nowSecond;
		nowSecond = rightNow.Second; //取得此秒之值
		return nowSecond; // 傳回取得此秒之值
		}
	}
	//<====程式結束
}