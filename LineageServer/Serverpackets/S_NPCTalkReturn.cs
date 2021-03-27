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
	using L1NpcHtml = LineageServer.Server.Model.Npc.L1NpcHtml;

	public class S_NPCTalkReturn : ServerBasePacket
	{
		private const string _S__25_TalkReturn = "[S] _S__25_TalkReturn";
		private byte[] _byte = null;

		public S_NPCTalkReturn(L1NpcTalkData npc, int objid, int action, string[] data)
		{

			string htmlid = "";

			if (action == 1)
			{
				htmlid = npc.NormalAction;
			}
			else if (action == 2)
			{
				htmlid = npc.CaoticAction;
			}
			else
			{
				throw new System.ArgumentException();
			}

			buildPacket(objid, htmlid, data);
		}

		public S_NPCTalkReturn(L1NpcTalkData npc, int objid, int action) : this(npc, objid, action, null)
		{
		}

		public S_NPCTalkReturn(int objid, string htmlid, string[] data)
		{
			buildPacket(objid, htmlid, data);
		}

		public S_NPCTalkReturn(int objid, string htmlid)
		{
			buildPacket(objid, htmlid, null);
		}

		public S_NPCTalkReturn(int objid, L1NpcHtml html)
		{
			buildPacket(objid, html.Name, html.Args);
		}

		private void buildPacket(int objid, string htmlid, string[] data)
		{

			WriteC(Opcodes.S_OPCODE_SHOWHTML);
			WriteD(objid);
			WriteS(htmlid);
			if (data != null && 1 <= data.Length)
			{
				WriteH(0x01); // 不明バイト 分かる人居たら修正願います
				WriteH(data.Length); // 引数の数
				foreach (string datum in data)
				{
					WriteS(datum);
				}
			}
			else
			{
				WriteH(0x00);
				WriteH(0x00);
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
				return _S__25_TalkReturn;
			}
		}
	}

}