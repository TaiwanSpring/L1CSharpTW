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
namespace LineageServer.Server.Server.Model.Npc.Action
{

	using L1Object = LineageServer.Server.Server.Model.L1Object;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1NpcHtml = LineageServer.Server.Server.Model.Npc.L1NpcHtml;
	using IterableElementList = LineageServer.Server.Server.utils.IterableElementList;
	using Lists = LineageServer.Server.Server.utils.collections.Lists;

	using Element = org.w3c.dom.Element;
	using NodeList = org.w3c.dom.NodeList;

	public class L1NpcShowHtmlAction : L1NpcXmlAction
	{
		private readonly string _htmlId;

		private readonly string[] _args;

		public L1NpcShowHtmlAction(Element element) : base(element)
		{

			_htmlId = element.getAttribute("HtmlId");
			NodeList list = element.ChildNodes;
			IList<string> dataList = Lists.newList();
			foreach (Element elem in new IterableElementList(list))
			{
				if (elem.NodeName.equalsIgnoreCase("Data"))
				{
					dataList.Add(elem.getAttribute("Value"));
				}
			}
			_args = ((List<string>)dataList).ToArray();
		}

		public override L1NpcHtml execute(string actionName, L1PcInstance pc, L1Object obj, sbyte[] args)
		{
			return new L1NpcHtml(_htmlId, _args);
		}

	}

}