using LineageServer.Models;
using LineageServer.Utils;
using System;
using System.IO;
using System.Text;
namespace LineageServer.Server.storage.mysql
{
    //再做 再做
    /*
    /// <summary>
    /// MySQL dump 備份程序
    /// 
    /// @author L1J-TW-99nets
    /// </summary>
    class MysqlAutoBackup : TimerTask
    {
        private static MysqlAutoBackup _instance;
        private static readonly string Username = Config.DB_LOGIN;
        private static readonly string Passwords = Config.DB_PASSWORD;
        private static string FilenameEx = "";
        private static string GzipCmd = "";
        private static string Database = null;
        private static DirectoryInfo dir = new DirectoryInfo(".\\DbBackup\\");
        private static bool GzipUse = Config.CompressGzip;
        private static bool done = false;

        public static MysqlAutoBackup Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MysqlAutoBackup();
                }
                return _instance;
            }
        }

        public MysqlAutoBackup()
        {
            Database = DatabaseName();

            if (!dir.Exists)
            {
                dir.Create();
            }

            // 壓縮是否開啟
            GzipCmd = GzipUse ? " | gzip" : "";
            FilenameEx = GzipUse ? ".sql.gz" : ".sql";

            // 檢查gzip.exe是否安裝 for Windows

            if (Environment.Is64BitOperatingSystem)
            {
                checkGzip("C:\\Windows\\SysWOW64");
            }
            else
            {
                checkGzip("C:\\Windows\\System32");
            }
        }

        public override void run()
        {

            try
            {
                System.Console.WriteLine("(MYSQL is backing now...)");
                /// <summary>
                /// mysqldump --user=[Username] --password=[password]
                /// [databasename] > [backupfile.sql]
                /// </summary>
                StringBuilder exeText = new StringBuilder("mysqldump --user=");
                exeText.Append(Username + " --password=");
                exeText.Append(Passwords + " ");
                exeText.Append(Database + " --opt --skip-extended-insert --skip-quick");
                exeText.Append(GzipCmd + " > ");
                exeText.Append(dir.CanonicalPath + (new SimpleDateFormat("\\yyyy-MM-dd-kkmm")).format(DateTime.Now) + FilenameEx);
                try
                {
                    Runtime rt = Runtime.Runtime;

                    Process p = rt.exec("cmd /c " + exeText.ToString());

                    StreamReader bf = new StreamReader(p.InputStream);

                    string msg = bf.ReadLine();

                    while (!string.IsNullOrEmpty(msg))
                    {
                        System.Console.WriteLine(msg);
                    }
                }
                finally
                {
                    System.Console.WriteLine("(MYSQL is backed over.)" + "\n" + L1Message.waitingforuser); // 等待玩家連線
                }
            }
            catch (IOException ioe)
            {
                System.Console.WriteLine(ioe.ToString());
                System.Console.Write(ioe.StackTrace);

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
                System.Console.Write(e.StackTrace);
            }
        }

        /// <summary>
        /// 負責檢查Gzip.exe是否安裝
        /// </summary>
        //private static void checkGzip(string SystemRoot)
        //{
        //    System.Console.WriteLine("[MySQL]checking gzip.exe is installed or not...");
        //    File gzip = new File(SystemRoot + "\\gzip.exe");
        //    if (gzip.exists())
        //    {
        //        System.Console.WriteLine("mysql auto backup is running...ok!");
        //    }
        //    else
        //    {
        //        System.Console.Error.WriteLine("[MySQL]Gzip.exe不存在，系統正在處理中...");
        //        gzip = new File(".\\docs\\gzip124xN.zip");
        //        UnZipUtil.unZip(gzip.AbsolutePath, SystemRoot);
        //    }
        //}

        /// <returns> database name </returns>
        private static string DatabaseName()
        {
            StringTokenizer sk = new StringTokenizer(Config.DB_URL, "/");
            sk.nextToken();
            sk.nextToken();
            sk = new StringTokenizer(sk.nextToken(), "?");
            Database = sk.nextToken();
            return Database;
        }
    }*/
}