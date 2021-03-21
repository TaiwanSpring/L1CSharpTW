using System;
using System.IO;
using System.Text;

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
namespace LineageServer.Server.Server.storage.mysql
{

	using Config = LineageServer.Server.Config;
	using L1Message = LineageServer.Server.L1Message;
	using LogRecorder = LineageServer.Server.Server.utils.LogRecorder;
	using SystemUtil = LineageServer.Server.Server.utils.SystemUtil;
	using UnZipUtil = LineageServer.Server.Server.utils.UnZipUtil;

	/// <summary>
	/// MySQL dump 備份程序
	/// 
	/// @author L1J-TW-99nets
	/// </summary>
	public class MysqlAutoBackup : TimerTask
	{
		private static MysqlAutoBackup _instance;
		private static readonly string Username = Config.DB_LOGIN;
		private static readonly string Passwords = Config.DB_PASSWORD;
		private static string FilenameEx = "";
		private static string GzipCmd = "";
		private static string Database = null;
		private static File dir = new File(".\\DbBackup\\");
		private static bool GzipUse = Config.CompressGzip;
		private static bool done = false;
		private static string os = SystemUtil.gerOs();
		private static string osArch = SystemUtil.OsArchitecture;

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
			L1Message.Instance;
			Database = DatabaseName();
			if (!dir.Directory)
			{
				dir.mkdir();
				dir.Writable = true;
			}

			// 壓縮是否開啟
			GzipCmd = GzipUse ? " | gzip" : "";
			FilenameEx = GzipUse ? ".sql.gz" : ".sql";

			// 檢查gzip.exe是否安裝 for Windows
			if (GzipUse && string.ReferenceEquals(os, "Windows") && !done)
			{
				if (string.ReferenceEquals(osArch, "x86"))
				{
					checkGzip("C:\\Windows\\System32");
				}
				else if (string.ReferenceEquals(osArch, "x64"))
				{
					checkGzip("C:\\Windows\\SysWOW64");
				}
				done = true;
			}
		}

		public override void run()
		{
			if (string.ReferenceEquals(os, "Windows"))
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
						string msg = null;
						while (!string.ReferenceEquals((msg = bf.ReadLine()), null))
						{
							LogRecorder.writeLog(msg);
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
			else if (string.ReferenceEquals(os, "Linux"))
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
						rt.exec(exeText.ToString());
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
		}

		/// <summary>
		/// 負責檢查Gzip.exe是否安裝
		/// </summary>
		private static void checkGzip(string SystemRoot)
		{
            System.Console.WriteLine("[MySQL]checking gzip.exe is installed or not...");
			File gzip = new File(SystemRoot + "\\gzip.exe");
			if (gzip.exists())
			{
                System.Console.WriteLine("mysql auto backup is running...ok!");
			}
			else
			{
                System.Console.Error.WriteLine("[MySQL]Gzip.exe不存在，系統正在處理中...");
				gzip = new File(".\\docs\\gzip124xN.zip");
				UnZipUtil.unZip(gzip.AbsolutePath, SystemRoot);
			}
		}

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
	}
}