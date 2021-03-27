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
	using L1NpcInstance = LineageServer.Server.Model.Instance.L1NpcInstance;

	public class S_NpcChatPacket : ServerBasePacket
	{
		private const string S_NPC_CHAT_PACKET = "[S] S_NpcChatPacket";

		private byte[] _byte = null;

		public S_NpcChatPacket(L1NpcInstance npc, string chat, int type)
		{
			buildPacket(npc, chat, type);
		}

		private void buildPacket(L1NpcInstance npc, string chat, int type)
		{
			switch (type)
			{
				case 0: // normal chat
					WriteC(Opcodes.S_OPCODE_NPCSHOUT); // Key is 16 , can use
														// desc-?.tbl
					WriteC(type); // Color
					WriteD(npc.Id);
					WriteS(npc.Name + ": " + chat);
					break;

				case 2: // shout
					WriteC(Opcodes.S_OPCODE_NPCSHOUT); // Key is 16 , can use
														// desc-?.tbl
					WriteC(type); // Color
					WriteD(npc.Id);
					WriteS("<" + npc.Name + "> " + chat);
					break;

				case 3: // world chat
					WriteC(Opcodes.S_OPCODE_NPCSHOUT);
					WriteC(type); // XXX 白色になる
					WriteD(npc.Id);
					WriteS("[" + npc.Name + "] " + chat);
					break;

				default:
					break;
			}
		}

		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = Bytes;
				}
				return _byte;
			}
		}

		public override string Type
		{
			get
			{
				return S_NPC_CHAT_PACKET;
			}
		}
	}

}