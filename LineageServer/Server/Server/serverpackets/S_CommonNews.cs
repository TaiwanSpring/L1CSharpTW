using System;
using System.Collections.Generic;
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
namespace LineageServer.Server.Server.serverpackets
{

	using Opcodes = LineageServer.Server.Server.Opcodes;
	using Lists = LineageServer.Server.Server.Utils.collections.Lists;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_CommonNews : ServerBasePacket
	{

		public S_CommonNews()
		{
			_announcements = Lists.newList();
			loadAnnouncements();
			if (_announcements.Count == 0)
			{
				return;
			}
			WriteC(Opcodes.S_OPCODE_COMMONNEWS);
			string message = "";
			for (int i = 0; i < _announcements.Count; i++)
			{
				message = (new StringBuilder()).Append(message).Append(_announcements[i]).Append("\n").ToString();
			}
			WriteS(message);
		}

		public S_CommonNews(string s)
		{
			WriteC(Opcodes.S_OPCODE_COMMONNEWS);
			WriteS(s);
		}

		private void loadAnnouncements()
		{
			_announcements.Clear();
			File file = new File("data/announcements.txt");
			if (file.exists())
			{
				readFromDisk(file);
			}
		}

		private void readFromDisk(File file)
		{
			LineNumberReader lnr = null;
			try
			{
				string line = null;
				lnr = new LineNumberReader(new StreamReader(file));
				do
				{
					if (string.ReferenceEquals((line = lnr.readLine()), null))
					{
						break;
					}
					StringTokenizer st = new StringTokenizer(line, "\n\r");
					if (st.hasMoreTokens())
					{
						string announcement = st.nextToken();
						_announcements.Add(announcement);
					}
					else
					{
						_announcements.Add(" ");
					}
				} while (true);
			}
			catch (Exception)
			{
			}
		}

		public override sbyte[] Content
		{
			get
			{
				return Bytes;
			}
		}

		public override string Type
		{
			get
			{
				return "[S] S_CommonNews";
			}
		}

		private IList<string> _announcements;

	}

}