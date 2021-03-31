using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.Npc.Action;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace LineageServer.Server.DataTables
{
	class NpcActionTable
	{
		private static NpcActionTable _instance;

		private readonly IList<INpcAction> _actions = ListFactory.NewList<INpcAction>();

		private readonly IList<INpcAction> _talkActions = ListFactory.NewList<INpcAction>();
		private IList<INpcAction> loadActionList(XmlElement element)
		{
			return L1NpcXmlParser.listActions(element);
		}
		private void loadAction(XmlElement element)
		{
			if (element.Name == "NpcActionList")
			{
				foreach (var item in loadActionList(element))
				{
					_actions.Add(item);
				}
			}
		}
		private void loadTalkAction(XmlElement element)
		{
			if (element.Name == "NpcTalkActionList")
			{
				foreach (var item in loadActionList(element))
				{
					_actions.Add(item);
				}
			}
		}
		private void loadDirectoryActions(string path)
		{
			foreach (string file in Directory.GetFiles(path, "*.xml"))
			{
				FileInfo fileInfo = new FileInfo(file);
				Stream stream = fileInfo.OpenRead();
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(fileInfo.FullName);
				loadAction(xmlDocument.DocumentElement);
				loadTalkAction(xmlDocument.DocumentElement);
			}
		}
		private NpcActionTable()
		{
			string path = "./data/xml/NpcActions/users/";
			if (Directory.Exists(path))
			{
				loadDirectoryActions(path);
			}

			path = "./data/xml/NpcActions/";
			if (Directory.Exists(path))
			{
				loadDirectoryActions(path);
			}
		}

		public static void load()
		{
			Stopwatch timer = Stopwatch.StartNew();
			System.Console.Write("【讀取】 【npcaction】【設定】");
			_instance = new NpcActionTable();
			timer.Stop();
			System.Console.WriteLine($"【完成】【{timer.ElapsedMilliseconds}】【毫秒】。");
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
				if (action.AcceptsRequest(actionName, pc, obj))
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
				if (action.AcceptsRequest("", pc, obj))
				{
					return action;
				}
			}
			return null;
		}
	}

}