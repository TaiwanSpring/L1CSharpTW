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
namespace LineageServer.Server.Server.Model.npc.action
{

	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	using Element = org.w3c.dom.Element;

	public class L1NpcActionFactory
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1NpcActionFactory).FullName);

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: private static java.util.Map<String, java.lang.reflect.Constructor<? extends L1NpcXmlAction>> _actions = l1j.server.server.utils.collections.Maps.newMap();
		private static IDictionary<string, System.Reflection.ConstructorInfo<L1NpcXmlAction>> _actions = Maps.newMap();

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private static java.lang.reflect.Constructor<? extends L1NpcXmlAction> loadConstructor(Class c) throws NoSuchMethodException
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
		private static System.Reflection.ConstructorInfo<L1NpcXmlAction> loadConstructor(Type c)
		{
			return c.GetConstructor(new Type[] {typeof(Element)});
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