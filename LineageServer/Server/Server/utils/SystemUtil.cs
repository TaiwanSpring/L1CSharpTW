using System;

namespace LineageServer.Server.Server.Utils
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
                return System.Diagnostics.Process.GetCurrentProcess().VirtualMemorySize64 / 1024L / 1024L;
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
                if (Environment.Is64BitOperatingSystem)
                {
                    return "x64";
                }
                else
                {
                    return "x86";
                }
            }
        }

        /// <summary>
        /// 取得目前的作業系統
        /// </summary>
        /// <returns> Linux or Windows </returns>
        public static string Platform
        {
            get
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    return "Windows";
                }
                else if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    return "Linux";
                }
                else
                {
                    return "Unknow";
                }
            }
        }
    }

}