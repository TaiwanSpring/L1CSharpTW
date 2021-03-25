using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

/*
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA
 * 02111-1307, USA.
 *
 * http://www.gnu.org/copyleft/gpl.html
 * 
 */

/// <summary>
/// 循環公告 </summary>
/// <returns> bymca </returns>

namespace LineageServer.Server.Server
{

	using Config = LineageServer.Server.Config;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;

	public class Announcecycle
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(Announcecycle).FullName);

		private static Announcecycle _instance;

		private IList<string> _Announcecycle = new List<string>(); //TODO 加入循環公告元件型字串陣列組

		private int _Announcecyclesize = 0;

		private Announcecycle()
		{
			loadAnnouncecycle();
		}

		public static Announcecycle Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new Announcecycle();
				}
    
				return _instance;
			}
		}

		public virtual void loadAnnouncecycle()
		{ // 讀取循環公告
			_Announcecycle.Clear();
			File file = new File("data/Announcecycle.txt");
			if (file.exists())
			{
				readFromDiskmulti(file);
				doAnnouncecycle(); // 若有載入檔案即開始運作循環公告執行緒
			}
			else
			{
				_log.config("data/Announcecycle.txt 檔案不存在");
			}
		}

		private void readFromDiskmulti(File file)
		{ // 循環公告多型法讀取應用
			LineNumberReader lnr = null;
			try
			{
				int i = 0;
				string line = null;
				lnr = new LineNumberReader(new StreamReader(file)); // 實作行數讀取器讀取file檔案內的字串資料
				while (!string.ReferenceEquals((line = lnr.readLine()), null))
				{ // 執行LOOP直到最後一行讀取為止
					StringTokenizer st = new StringTokenizer(line, "\n\r"); // 實作字串標記處理
					if (st.hasMoreTokens())
					{
						string showAnnouncecycle = st.nextToken(); // 讀取某行後就換下一行
						_Announcecycle.Add(showAnnouncecycle); // 將每行的字串載入_Announcecycle元件處理

						i++;
					}
				}

				_log.config("Announcecycle: Loaded " + i + " Announcecycle.");
			}
			catch (IOException e1)
			{
				_log.log(Enum.Level.Server, "Error reading Announcecycle", e1);
			}
			finally
			{
				try
				{
					lnr.close();
				}
				catch (IOException e)
				{
                    System.Console.WriteLine(e.ToString());
                    System.Console.Write(e.StackTrace);
				}
			}
		}

		public virtual void doAnnouncecycle()
		{
			AnnouncTask rs = new AnnouncTask(this); // 建立執行緒
			RunnableExecuter.Instance.scheduleAtFixedRate(rs, 180000, 60000 * Config.Show_Announcecycle_Time);
			// 10分鐘公告一次(60秒*1分*1000毫秒)
		}

		/// <summary>
		/// The task launching the function doAnnouncCycle() </summary>
		internal class AnnouncTask : IRunnableStart
		{
			private readonly Announcecycle outerInstance;

			public AnnouncTask(Announcecycle outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void run()
			{
				try
				{
					outerInstance.ShowAnnounceToAll(outerInstance._Announcecycle[outerInstance._Announcecyclesize]); // 輪迴式公告發布
					outerInstance._Announcecyclesize++;
					if (outerInstance._Announcecyclesize >= outerInstance._Announcecycle.Count)
					{
						outerInstance._Announcecyclesize = 0;
					}
				}
				catch (Exception e)
				{
					_log.log(Level.WARNING, "", e);
				}
			}
		}

		private void ShowAnnounceToAll(string msg)
		{
			ICollection<L1PcInstance> allpc = L1World.Instance.AllPlayers;
			foreach (L1PcInstance pc in allpc)
			{
				pc.sendPackets(new S_SystemMessage(msg));
			}
		}
	}

}