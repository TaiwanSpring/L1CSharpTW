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
namespace LineageServer.Server.Model.Npc.Action
{
	using Element = org.w3c.dom.Element;

	using L1Location = LineageServer.Server.Model.L1Location;
	using GameObject = LineageServer.Server.Model.GameObject;
	using L1Teleport = LineageServer.Server.Model.L1Teleport;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using L1ItemId = LineageServer.Server.Model.identity.L1ItemId;
	using L1NpcHtml = LineageServer.Server.Model.Npc.L1NpcHtml;
	using S_ServerMessage = LineageServer.Serverpackets.S_ServerMessage;

	public class L1NpcTeleportAction : L1NpcXmlAction
	{
		private readonly L1Location _loc;
		private readonly int _heading;
		private readonly int _price;
		private readonly bool _effect;

		public L1NpcTeleportAction(Element element) : base(element)
		{

			int x = L1NpcXmlParser.getIntAttribute(element, "X", -1);
			int y = L1NpcXmlParser.getIntAttribute(element, "Y", -1);
			int mapId = L1NpcXmlParser.getIntAttribute(element, "Map", -1);
			_loc = new L1Location(x, y, mapId);

			_heading = L1NpcXmlParser.getIntAttribute(element, "Heading", 5);

			_price = L1NpcXmlParser.getIntAttribute(element, "Price", 0);
			_effect = L1NpcXmlParser.getBoolAttribute(element, "Effect", true);
		}

		public override L1NpcHtml execute(string actionName, L1PcInstance pc, GameObject obj, sbyte[] args)
		{
			if (!pc.Inventory.checkItem(L1ItemId.ADENA, _price))
			{
				pc.sendPackets(new S_ServerMessage(337, "$4")); // アデナが不足しています。
				return L1NpcHtml.HTML_CLOSE;
			}
			pc.Inventory.consumeItem(L1ItemId.ADENA, _price);
			L1Teleport.teleport(pc, _loc.X, _loc.Y, (short) _loc.MapId, _heading, _effect);
			return null;
		}

	}

}