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
namespace LineageServer.Server.Server.Model
{

	using Config = LineageServer.Server.Config;

	/// <summary>
	/// <strong>Time Information</strong>
	/// 
	/// @author ibm
	/// 
	/// </summary>
	public class TimeInform
	{
		/// <summary>
		/// Calendar本身是abstract,使用getInstance()來取得物件
		/// </summary>
		internal static TimeZone timezone = TimeZone.getTimeZone(Config.TIME_ZONE);
		internal static DateTime rightNow = DateTime.getInstance(timezone);

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
				NowTime = TimeInform.getYear(type_year, 0) + "年 " + TimeInform.Month + "月" + TimeInform.Day + "日 " + TimeInform.DayOfWeek;
				break;
			case 2:
				NowTime = TimeInform.Hour + "時" + TimeInform.Minute + "分" + TimeInform.Second + "秒";
				break;
			case 3:
				NowTime = TimeInform.getYear(type_year, 0) + "年" + TimeInform.Month + "月" + TimeInform.Day + "日" + TimeInform.Hour + "時" + TimeInform.Minute + "分" + TimeInform.Second + "秒";
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
		}

		/// <returns> yyyy-MM-dd HH:mm:ss </returns>
		public virtual string NowTime_Standard
		{
			get
			{
    
				string NowTime = (new SimpleDateFormat("yyyy-MM-dd HH:mm:ss")).format(DateTime.Now);
    
				return NowTime;
			}
		}
	}

}