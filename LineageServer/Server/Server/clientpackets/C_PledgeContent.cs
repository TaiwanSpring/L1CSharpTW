using LineageServer.Server.Server.DataSources;
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Clientpackets
{
	class C_PledgeContent : ClientBasePacket
	{
		private const string C_PledgeContent_Conflict = "[C] C_PledgeContent";

		public C_PledgeContent(sbyte[] decrypt, ClientThread client) : base(decrypt)
		{
			L1PcInstance pc = client.ActiveChar;
			if (pc == null)
			{
				return;
			}

			if (pc.Clanid == 0)
			{
				return;
			}

			int data = readC();

			if (data == 15)
			{ // 寫入血盟公告
				// 讀取公告字串封包
				string announce = readS();
				/* 取出L1Clan物件 */
				L1Clan clan = ClanTable.Instance.getTemplate(pc.Clanid);
				/* 更新公告 */
				clan.Announcement = announce;
				/* 更新L1Clan物件 */
				ClanTable.Instance.updateClan(clan);
				/* 送出血盟公告封包 */
				pc.sendPackets(new S_PacketBox(S_PacketBox.HTML_PLEDGE_REALEASE_ANNOUNCE, announce));
			}
			else if (data == 16)
			{ // 寫入個人備註
				// 讀取備註字串封包
				string notes = readS();
				/* 更新角色備註資料 */
				pc.ClanMemberNotes = notes;
				/* 寫入備註資料到資料庫 */
				ClanMembersTable.Instance.updateMemberNotes(pc, notes);
				/* 送出寫入備註更新封包 */
				pc.sendPackets(new S_Pledge(pc.Name, notes));
			}
		}

		public override string Type
		{
			get
			{
				return C_PledgeContent_Conflict;
			}
		}

	}

}