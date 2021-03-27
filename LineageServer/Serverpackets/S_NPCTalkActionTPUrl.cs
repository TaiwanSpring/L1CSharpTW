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
namespace LineageServer.Serverpackets
{
	using Opcodes = LineageServer.Server.Opcodes;
	using L1NpcTalkData = LineageServer.Server.Model.L1NpcTalkData;

	public class S_NPCTalkActionTPUrl : ServerBasePacket
	{
		private const string _S__25_TalkReturnAction = "[S] S_NPCTalkActionTPUrl";
		private byte[] _byte = null;

		public S_NPCTalkActionTPUrl(L1NpcTalkData cha, object[] prices, int objid)
		{
			buildPacket(cha, prices, objid);
		}

		private void buildPacket(L1NpcTalkData npc, object[] prices, int objid)
		{
			string htmlid = "";
			htmlid = npc.TeleportURL;
			WriteC(Opcodes.S_OPCODE_SHOWHTML);
			WriteD(objid);
			WriteS(htmlid);
			WriteH(0x01); // 不明
			WriteH(prices.Length); // 引数の数

			foreach (object price in prices)
			{
				WriteS((((int?) price).Value).ToString());
			}
		}

		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = memoryStream.toByteArray();
				}
				return _byte;
			}
		}

		public override string Type
		{
			get
			{
				return _S__25_TalkReturnAction;
			}
		}
	}

}