using LineageServer.Server;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;

namespace LineageServer.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來的密語封包
	/// </summary>
	class C_ChatWhisper : ClientBasePacket
	{

		private const string C_CHAT_WHISPER = "[C] C_ChatWhisper";

		public C_ChatWhisper(byte[] abyte0, ClientThread client) : base(abyte0)
		{

			L1PcInstance whisperFrom = client.ActiveChar;
			if (whisperFrom == null)
			{
				return;
			}

			string targetName = ReadS();
			string text = ReadS();

			// 被魔封
			if (whisperFrom.hasSkillEffect(L1SkillId.STATUS_CHAT_PROHIBITED))
			{
				whisperFrom.sendPackets(new S_ServerMessage(242)); // 你從現在被禁止閒談。
				return;
			}
			// 等級不夠
			if (whisperFrom.Level < Config.WHISPER_CHAT_LEVEL)
			{
				whisperFrom.sendPackets(new S_ServerMessage(404, Config.WHISPER_CHAT_LEVEL.ToString())); // 等級
																										 // %0
																										 // 以下無法使用密談。
				return;
			}
			L1PcInstance whisperTo = L1World.Instance.getPlayer(targetName);
			// 密語對象不存在
			if (whisperTo == null)
			{
				whisperFrom.sendPackets(new S_ServerMessage(73, targetName)); // \f1%0%d
																			  // 不在線上。
				return;
			}
			// 自己跟自己說話
			if (whisperTo == whisperFrom)
			{
				return;
			}
			// 斷絕密語
			if (whisperTo.ExcludingList.contains(whisperFrom.Name))
			{
				whisperFrom.sendPackets(new S_ServerMessage(117, whisperTo.Name)); // %0%s
																				   // 斷絕你的密語。
				return;
			}
			// 關閉密語
			if (!whisperTo.CanWhisper)
			{
				whisperFrom.sendPackets(new S_ServerMessage(205, whisperTo.Name)); // \f1%0%d
																				   // 目前關閉悄悄話。
				return;
			}

			ChatLogTable.Instance.storeChat(whisperFrom, whisperTo, text, 1);
			whisperFrom.sendPackets(new S_ChatPacket(whisperTo, text, Opcodes.S_OPCODE_GLOBALCHAT, 9));
			whisperTo.sendPackets(new S_ChatPacket(whisperFrom, text, Opcodes.S_OPCODE_WHISPERCHAT, 16));
			// GM偷聽密語
			if (Config.GM_OVERHEARD)
			{
				foreach (GameObject visible in L1World.Instance.AllPlayers)
				{
					if (visible is L1PcInstance)
					{
						L1PcInstance GM = (L1PcInstance)visible;
						if (GM.Gm && whisperFrom.Id != GM.Id)
						{
							GM.sendPackets(new S_SystemMessage("" + "【密語】" + whisperFrom.Name + "對" + targetName + ":" + text));
						}
					}
				}
			}
			// GM偷聽密語  end
		}

		public override string Type
		{
			get
			{
				return C_CHAT_WHISPER;
			}
		}
	}

}