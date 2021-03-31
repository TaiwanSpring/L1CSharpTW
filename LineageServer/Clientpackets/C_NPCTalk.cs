
using LineageServer.Server;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.Npc;
using LineageServer.Server.Model.Npc.Action;
using LineageServer.Serverpackets;

namespace LineageServer.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來NPC講話的封包
	/// </summary>
	class C_NPCTalk : ClientBasePacket
	{

		private const string C_NPC_TALK = "[C] C_NPCTalk";
		public C_NPCTalk(byte[] abyte0, ClientThread client) : base(abyte0)
		{
			L1PcInstance pc = client.ActiveChar;
			if (pc == null)
			{
				return;
			}

			int objid = ReadD();
			GameObject obj = L1World.Instance.findObject(objid);

			if (obj != null)
			{
				INpcAction action = NpcActionTable.Instance.get(pc, obj);
				if (action != null)
				{
					L1NpcHtml html = action.Execute("", pc, obj, new byte[0]);
					if (html != null)
					{
						pc.sendPackets(new S_NPCTalkReturn(obj.Id, html));
					}
					return;
				}
				obj.onTalkAction(pc);
			}
		}

		public override string Type
		{
			get
			{
				return C_NPC_TALK;
			}
		}
	}

}