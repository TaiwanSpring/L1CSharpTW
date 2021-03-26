using System;
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

	using StreamUtil = LineageServer.Server.Server.Utils.StreamUtil;
	using Lists = LineageServer.Server.Server.Utils.collections.Lists;

	public class BadNamesList
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(BadNamesList).FullName);

		private static BadNamesList _instance;

		private IList<string> _nameList = Lists.newList();

		public static BadNamesList Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new BadNamesList();
				}
				return _instance;
			}
		}

		private BadNamesList()
		{
			LineNumberReader lnr = null;

			try
			{
				File mobDataFile = new File("data/badnames.txt");
				lnr = new LineNumberReader(new StreamReader(mobDataFile));

				string line = null;
				while (!string.ReferenceEquals((line = lnr.readLine()), null))
				{
					if ((line.Trim().Length == 0) || line.StartsWith("#", StringComparison.Ordinal))
					{
						continue;
					}
					StringTokenizer st = new StringTokenizer(line, ";");

					while (st.hasMoreTokens())
					{
						_nameList.Add(st.nextToken());
					}
				}

				_log.config("loaded " + _nameList.Count + " bad names");
			}
			catch (FileNotFoundException)
			{
				_log.warning("badnames.txt is missing in data folder");
			}
			catch (Exception e)
			{
				_log.warning("error while loading bad names list : " + e);
			}
			finally
			{
				StreamUtil.close(lnr);
			}
		}

		public virtual bool isBadName(string name)
		{
			foreach (string badName in _nameList)
			{
				if (name.ToLower().Contains(badName.ToLower()))
				{
					return true;
				}
			}
			return false;
		}

		public virtual string[] AllBadNames
		{
			get
			{
				return ((List<string>)_nameList).ToArray();
			}
		}
	}

}