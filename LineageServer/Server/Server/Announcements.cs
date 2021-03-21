using System.Collections.Generic;
using System.IO;

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
namespace LineageServer.Server.Server
{

	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;
	using StreamUtil = LineageServer.Server.Server.utils.StreamUtil;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;

	public class Announcements
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(Announcements).FullName);

		private static Announcements _instance;

		private readonly IList<string> _announcements = Lists.newList();

		private Announcements()
		{
			loadAnnouncements();
		}

		public static Announcements Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new Announcements();
				}
    
				return _instance;
			}
		}

		private void loadAnnouncements()
		{
			_announcements.Clear();
			File file = new File("data/announcements.txt");
			if (file.exists())
			{
				readFromDisk(file);
			}
			else
			{
				_log.config("data/announcements.txt doesn't exist");
			}
		}

		public virtual void showAnnouncements(L1PcInstance showTo)
		{
			foreach (string msg in _announcements)
			{
				showTo.sendPackets(new S_SystemMessage(msg));
			}
		}

		private void readFromDisk(File file)
		{
			LineNumberReader lnr = null;
			try
			{
				int i = 0;
				string line = null;
				lnr = new LineNumberReader(new StreamReader(file));
				while (!string.ReferenceEquals((line = lnr.readLine()), null))
				{
					StringTokenizer st = new StringTokenizer(line, "\n\r");
					if (st.hasMoreTokens())
					{
						string announcement = st.nextToken();
						_announcements.Add(announcement);

						i++;
					}
				}

				_log.config("讀取了  " + i + " 件公告");
			}
			catch (FileNotFoundException)
			{
				// 如果檔案不存在
			}
			catch (IOException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				StreamUtil.close(lnr);
			}
		}

		public virtual void announceToAll(string msg)
		{
			L1World.Instance.broadcastServerMessage(msg);
		}
	}
}