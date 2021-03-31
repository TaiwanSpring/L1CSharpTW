using LineageServer.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Server.Model.Npc.Action
{
	class L1NpcShowHtmlAction : L1NpcXmlAction
	{
		private readonly string _htmlId;

		private readonly string[] _args;
		public L1NpcShowHtmlAction(XmlElement xmlElement) : base(xmlElement)
		{
			_htmlId = xmlElement.GetString("HtmlId");
			if (string.IsNullOrEmpty(_htmlId))
			{
				throw new NullReferenceException(nameof(_htmlId));
			}
			List<string> dataList = new List<string>();

			for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
			{
				if (xmlElement.ChildNodes[i].NodeType == XmlNodeType.Element
					&& xmlElement.ChildNodes[i] is XmlElement element)
				{
					if (element.Name == "Data")
					{
						string value = element.GetString("Value");
						if (string.IsNullOrEmpty(value))
						{
							throw new NullReferenceException(nameof(value));
						}
						dataList.Add(value);
					}
				}
			}
			_args = dataList.ToArray();
		}

		public override L1NpcHtml Execute(string actionName, L1PcInstance pc, GameObject obj, byte[] args)
		{
			return new L1NpcHtml(_htmlId, _args);
		}

	}

}