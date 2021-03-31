using LineageServer.Models;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace LineageServer.Server.Model.Npc.Action
{
	class L1NpcActionFactory
	{
		private static IDictionary<string, Func<XmlElement, INpcAction>> _actions = MapFactory.NewMap<string, Func<XmlElement, INpcAction>>();

		static L1NpcActionFactory()
		{
			_actions["Action"] = x => new L1NpcListedAction(x);

			_actions["MakeItem"] = x => new L1NpcMakeItemAction(x);

			_actions["ShowHtml"] = x => new L1NpcShowHtmlAction(x);

			_actions["SetQuest"] = x => new L1NpcSetQuestAction(x);

			_actions["Teleport"] = x => new L1NpcTeleportAction(x);
		}

		public static INpcAction newAction(XmlElement xmlElement)
		{
			if (_actions.ContainsKey(xmlElement.Name))
			{
				return _actions[xmlElement.Name].Invoke(xmlElement);
			}
			else
			{
				Debug.Fail("");
				Logger.GenericLogger.Warning(xmlElement.Name + " 未定義のNPCアクションです");
				return null;
			}
		}
	}
}