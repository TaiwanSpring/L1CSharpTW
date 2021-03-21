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
namespace LineageServer.Server.Server.utils
{

	public class SystemUtil
	{
		/// <summary>
		/// システムが利用中のヒープサイズをメガバイト単位で返す。<br>
		/// この値にスタックのサイズは含まれない。
		/// </summary>
		/// <returns> 利用中のヒープサイズ </returns>
		public static long UsedMemoryMB
		{
			get
			{
				return (Runtime.Runtime.totalMemory() - Runtime.Runtime.freeMemory()) / 1024L / 1024L;
			}
		}

		/// <summary>
		/// 得知作業系統是幾位元 Only for Windows
		/// </summary>
		/// <returns> x86 or x64 </returns>
		public static string OsArchitecture
		{
			get
			{
				string x64_System = "C:\\Windows\\SysWOW64", result;
				File dir = new File(x64_System);
				if (dir.exists())
				{
					result = "x64";
				}
				else
				{
					result = "x86";
				}
				return result;
			}
		}

		/// <summary>
		/// 取得目前的作業系統
		/// </summary>
		/// <returns> Linux or Windows </returns>
		public static string gerOs()
		{
			string Os = "", OsName = System.getProperty("os.name");
			if (OsName.ToLower().IndexOf("windows", StringComparison.Ordinal) >= 0)
			{
				Os = "Windows";
			}
			else if (OsName.ToLower().IndexOf("linux", StringComparison.Ordinal) >= 0)
			{
				Os = "Linux";
			}
			return Os;
		}
	}

}