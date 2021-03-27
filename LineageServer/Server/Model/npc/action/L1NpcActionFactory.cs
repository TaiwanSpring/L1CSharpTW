using System;
using System.Collections.Generic;
namespace LineageServer.Server.Model.Npc.Action
{
	public class L1NpcActionFactory
	{
		private static IDictionary<string, System.Reflection.ConstructorInfo<L1NpcXmlAction>> _actions = Maps.newMap();

		private static System.Reflection.ConstructorInfo<L1NpcXmlAction> loadConstructor(Type c)
		{
			return c.GetConstructor(new Type[] { typeof(Element) });
		}

		static L1NpcActionFactory()
		{
			try
			{
				_actions["Action"] = loadConstructor(typeof(L1NpcListedAction));
				_actions["MakeItem"] = loadConstructor(typeof(L1NpcMakeItemAction));
				_actions["ShowHtml"] = loadConstructor(typeof(L1NpcShowHtmlAction));
				_actions["SetQuest"] = loadConstructor(typeof(L1NpcSetQuestAction));
				_actions["Teleport"] = loadConstructor(typeof(L1NpcTeleportAction));
			}
			catch (NoSuchMethodException e)
			{
				_log.log(Enum.Level.Server, "NpcActionのクラスロードに失敗", e);
			}
		}

		public static INpcAction newAction(Element element)
		{
			try
			{
				//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
				//ORIGINAL LINE: java.lang.reflect.Constructor<? extends L1NpcXmlAction> con = _actions.get(element.getNodeName());
				System.Reflection.ConstructorInfo<L1NpcXmlAction> con = _actions[element.NodeName];
				return con.Invoke(element);
			}
			catch (System.NullReferenceException)
			{
				_log.warning(element.NodeName + " 未定義のNPCアクションです");
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, "NpcActionのクラスロードに失敗", e);
			}
			return null;
		}
	}

}