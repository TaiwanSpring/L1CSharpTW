using LineageServer.Interfaces;
using LineageServer.Server;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using System;
namespace LineageServer.Serverpackets
{
    class S_Pledge : ServerBasePacket
    {

        private const string _S_Pledge = "[S] _S_Pledge";

        private byte[] _byte = null;

        /// <summary>
        /// 盟友查詢 公告視窗 </summary>
        /// <param name="ClanId"> 血盟Id </param>
        public S_Pledge(int ClanId)
        {
            L1Clan clan = ClanTable.Instance.getTemplate(ClanId);
            WriteC(Opcodes.S_OPCODE_PACKETBOX);
            WriteC(S_PacketBox.HTML_PLEDGE_ANNOUNCE);
            WriteS(clan.ClanName);
            WriteS(clan.LeaderName);
            WriteD(clan.EmblemId); // 盟徽id
            WriteD(clan.FoundDate.Millisecond / 1000); // 血盟創立日
            WriteByte(GobalParameters.Encoding.GetBytes(clan.Announcement));
        }

        /// <summary>
        /// 盟友查詢 盟友清單 </summary>
        /// <param name="clanName"> </param>
        /// <exception cref="Exception">  </exception>
        public S_Pledge(L1PcInstance pc)
        {
            L1Clan clan = ClanTable.Instance.getTemplate(pc.Clanid);
            WriteC(Opcodes.S_OPCODE_PACKETBOX);
            WriteC(S_PacketBox.HTML_PLEDGE_MEMBERS);
            WriteH(1);
            WriteC(clan.AllMembers.Length); // 血盟總人數

            // 血盟成員資料
            /* Name/Rank/Level/Notes/MemberId/ClassType */
            foreach (string member in clan.AllMembers)
            {
                L1PcInstance clanMember = Container.Instance.Resolve<ICharacterController>().restoreCharacter(member);
                WriteS(clanMember.Name);
                WriteC(clanMember.ClanRank);
                WriteC(clanMember.Level);
                WriteByte(GobalParameters.Encoding.GetBytes(clanMember.ClanMemberNotes));
                WriteD(clanMember.ClanMemberId);
                WriteC(clanMember.Type);
            }
        }

        /// <summary>
        /// 盟友查詢 寫入備註 </summary>
        /// <param name="name"> 玩家名稱 </param>
        /// <param name="notes"> 備註文字 </param>
        public S_Pledge(string name, string notes)
        {
            WriteC(Opcodes.S_OPCODE_PACKETBOX);
            WriteC(S_PacketBox.HTML_PLEDGE_Write_NOTES);
            WriteS(name);

            WriteByte(GobalParameters.Encoding.GetBytes(notes));
        }

        public override string Type
        {
            get
            {
                return _S_Pledge;
            }
        }
    }

}