using System;
using System.Collections.Generic;

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
namespace LineageServer.Server.DataSources
{


	using GameObject = LineageServer.Server.Model.GameObject;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using INpcAction = LineageServer.Server.Model.Npc.Action.INpcAction;
	using L1NpcXmlParser = LineageServer.Server.Model.Npc.Action.L1NpcXmlParser;
	using FileUtil = LineageServer.Utils.FileUtil;
	using PerformanceTimer = LineageServer.Utils.PerformanceTimer;
	using ListFactory = LineageServer.Utils.ListFactory;

	using Document = org.w3c.dom.Document;
	using SAXException = org.xml.sax.SAXException;

	public class NpcActionTable
	{
		//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.GetLogger(typeof(NpcActionTable).FullName);

		private static NpcActionTable _instance;

		private readonly IList<INpcAction> _actions = ListFactory.NewList<INpcAction>();

		private readonly IList<INpcAction> _talkActions = ListFactory.NewList<INpcAction>();

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: private java.util.List<l1j.server.server.model.npc.action.L1NpcAction> loadAction(java.io.File file, String nodeName) throws javax.xml.parsers.ParserConfigurationException, org.xml.sax.SAXException, java.io.IOException
		private IList<INpcAction> loadAction(File file, string nodeName)
		{
			DocumentBuilder builder = DocumentBuilderFactory.newInstance().newDocumentBuilder();
			Document doc = builder.parse(file);

			if (!doc.DocumentElement.NodeName.equalsIgnoreCase(nodeName))
			{
				return ListFactory.newList();
			}
			return L1NpcXmlParser.listActions(doc.DocumentElement);
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: private void loadAction(java.io.File file) throws Exception
		private void loadAction(File file)
		{
			( (List<INpcAction>)_actions ).AddRange(loadAction(file, "NpcActionList"));
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: private void loadTalkAction(java.io.File file) throws Exception
		private void loadTalkAction(File file)
		{
			( (List<INpcAction>)_talkActions ).AddRange(loadAction(file, "NpcTalkActionList"));
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: private void loadDirectoryActions(java.io.File dir) throws Exception
		private void loadDirectoryActions(File dir)
		{
			foreach (string file in dir.list())
			{
				File f = new File(dir, file);
				if (FileUtil.getExtension(f).Equals("xml", StringComparison.OrdinalIgnoreCase))
				{
					loadAction(f);
					loadTalkAction(f);
				}
			}
		}

		//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
		//ORIGINAL LINE: private NpcActionTable() throws Exception
		private NpcActionTable()
		{
			File usersDir = new File("./data/xml/NpcActions/users/");
			if (usersDir.exists())
			{
				loadDirectoryActions(usersDir);
			}
			loadDirectoryActions(new File("./data/xml/NpcActions/"));
		}

		public static void load()
		{
			try
			{
				PerformanceTimer timer = new PerformanceTimer();
				System.Console.Write("【讀取】 【npcaction】【設定】");
				_instance = new NpcActionTable();
				System.Console.WriteLine("【完成】【" + timer.get() + "】【毫秒】。");
			}
			catch (Exception e)
			{
				_log.Error(Enum.Level.Server, "找不到NpcAction讀取的位置。", e);
				Environment.Exit(0);
			}
		}

		public static NpcActionTable Instance
		{
			get
			{
				return _instance;
			}
		}

		public virtual INpcAction get(string actionName, L1PcInstance pc, GameObject obj)
		{
			foreach (INpcAction action in _actions)
			{
				if (action.acceptsRequest(actionName, pc, obj))
				{
					return action;
				}
			}
			return null;
		}

		public virtual INpcAction get(L1PcInstance pc, GameObject obj)
		{
			foreach (INpcAction action in _talkActions)
			{
				if (action.acceptsRequest("", pc, obj))
				{
					return action;
				}
			}
			return null;
		}
	}

}