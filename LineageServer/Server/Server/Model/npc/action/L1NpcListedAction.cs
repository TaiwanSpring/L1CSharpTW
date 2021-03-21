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

	using Element = org.w3c.dom.Element;
	using L1Object = LineageServer.Server.Server.Model.L1Object;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1NpcHtml = LineageServer.Server.Server.Model.npc.L1NpcHtml;

	public class L1NpcListedAction : L1NpcXmlAction
	{
		private IList<INpcAction> _actions;

		public L1NpcListedAction(Element element) : base(element)
		{
			_actions = L1NpcXmlParser.listActions(element);
		}

		public override L1NpcHtml execute(string actionName, L1PcInstance pc, L1Object obj, sbyte[] args)
		{
			L1NpcHtml result = null;
			foreach (INpcAction action in _actions)
			{
				if (!action.acceptsRequest(actionName, pc, obj))
				{
					continue;
				}
				L1NpcHtml r = action.execute(actionName, pc, obj, args);
				if (r != null)
				{
					result = r;
				}
			}
			return result;
		}

		public override L1NpcHtml executeWithAmount(string actionName, L1PcInstance pc, L1Object obj, int amount)
		{
			L1NpcHtml result = null;
			foreach (INpcAction action in _actions)
			{
				if (!action.acceptsRequest(actionName, pc, obj))
				{
					continue;
				}
				L1NpcHtml r = action.executeWithAmount(actionName, pc, obj, amount);
				if (r != null)
				{
					result = r;
				}
			}
			return result;
		}
	}

}