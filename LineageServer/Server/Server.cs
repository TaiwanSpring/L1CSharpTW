/*
using LineageServer.Interfaces;
using System;
using System.IO;
namespace LineageServer.Server
{
    /// <summary>
    /// l1j 伺服器啟動
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 紀錄用 </summary>
        private static ILogger _log = Logger.GetLogger(nameof(Program));

        /// <summary>
        /// 紀錄檔的路徑 </summary>
        private const string LOG_PROP = "./config/log.properties";

        /// <summary>
        /// サーバメイン.
        /// </summary>
        /// <param name="args">
        ///            命令列參數 </param>
        /// <exception cref="Exception"> </exception>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: public static void main(final String[] args) throws Exception
        public static void Main(in string[] args)
        {
            //File logFolder = new File("log");
            //logFolder.mkdir();

            //try
            //{
            //    Stream @is = new BufferedInputStream(new FileStream(LOG_PROP, FileMode.Open, FileAccess.Read));
            //    LogManager.LogManager.readConfiguration(@is);
            //    @is.Close();
            //}
            //catch (IOException e)
            //{
            //    _log.log(Enum.Level.Server, "Failed to Load " + LOG_PROP + " File.", e);
            //    Environment.Exit(0);
            //}
            //try
            //{
            //    Config.load();
            //}
            //catch (Exception e)
            //{
            //    _log.log(Enum.Level.Server, e.Message, e);
            //    Environment.Exit(0);
            //}

            // L1DatabaseFactory初期設定
            int retryCount = 0;
            do
            {
                if (L1DatabaseFactory.Instance.Initialize(Config.DB_DRIVER, Config.DB_URL, Config.DB_LOGIN, Config.DB_PASSWORD))
                {
                    break;
                }
                retryCount++;
            } while (retryCount < 10);

            GameServer.Instance.initialize();

            if (Config.TELNET_SERVER)
            {
                TelnetServer.Instance.start();
            }
        }
    }

}*/