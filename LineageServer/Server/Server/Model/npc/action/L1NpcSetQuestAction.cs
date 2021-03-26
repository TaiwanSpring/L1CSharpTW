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
	using Element = org.w3c.dom.Element;

	using L1Object = LineageServer.Server.Server.Model.L1Object;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1NpcHtml = LineageServer.Server.Server.Model.Npc.L1NpcHtml;

	public class L1NpcSetQuestAction : L1NpcXmlAction
	{
		private readonly int _id;
		private readonly int _step;

		public L1NpcSetQuestAction(Element element) : base(element)
		{

			_id = L1NpcXmlParser.parseQuestId(element.getAttribute("Id"));
			_step = L1NpcXmlParser.parseQuestStep(element.getAttribute("Step"));

			if (_id == -1 || _step == -1)
			{
				throw new System.ArgumentException();
			}
		}

		public override L1NpcHtml execute(string actionName, L1PcInstance pc, L1Object obj, sbyte[] args)
		{
			pc.Quest.set_step(_id, _step);
			return null;
		}

	}

}