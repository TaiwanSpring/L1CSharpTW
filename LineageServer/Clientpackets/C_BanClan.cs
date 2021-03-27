using LineageServer.Interfaces;
using LineageServer.Server.DataSources;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理從客戶端傳來脫離血盟的封包
    /// </summary>
    class C_BanClan : ClientBasePacket
    {

        private const string C_BAN_CLAN = "[C] C_BanClan";
        private static ILogger _log = Logger.GetLogger(nameof(C_BanClan));

        public C_BanClan(byte[] abyte0, ClientThread clientthread) : base(abyte0)
        {

            L1PcInstance pc = clientthread.ActiveChar;
            if (pc == null)
            {
                return;
            }

            string s = ReadS();

            L1Clan clan = L1World.Instance.getClan(pc.Clanname);
            if (clan != null)
            {
                string[] clanMemberName = clan.AllMembers;
                int i;
                if (pc.Crown && pc.Id == clan.LeaderId)
                { // 王族，或者已經創立血盟
                    for (i = 0; i < clanMemberName.Length; i++)
                    {
                        if (pc.Name.ToLower().Equals(s.ToLower()))
                        { // 是血盟創立者
                            return;
                        }
                    }
                    L1PcInstance tempPc = L1World.Instance.getPlayer(s);
                    if (tempPc != null)
                    { // 玩家在線上
                        if (tempPc.Clanid == pc.Clanid)
                        { // 確定同血盟
                            tempPc.Clanid = 0;
                            tempPc.Clanname = "";
                            tempPc.ClanRank = 0;
                            tempPc.ClanMemberId = 0;
                            tempPc.Save(); // 儲存玩家的資料到資料庫中
                            clan.delMemberName(tempPc.Name);
                            ClanMembersTable.Instance.deleteMember(tempPc.Id);
                            tempPc.sendPackets(new S_ServerMessage(238, pc.Clanname)); // 你被 %0 血盟驅逐了。
                            pc.sendPackets(new S_ServerMessage(240, tempPc.Name)); // %0%o 被你從你的血盟驅逐了。
                        }
                        else
                        {
                            pc.sendPackets(new S_ServerMessage(109, s)); // 沒有叫%0的人。
                        }
                    }
                    else
                    { // 玩家離線中
                        try
                        {
                            L1PcInstance restorePc = CharacterTable.Instance.restoreCharacter(s);
                            if (restorePc != null && restorePc.Clanid == pc.Clanid)
                            { // 確定同血盟
                                restorePc.Clanid = 0;
                                restorePc.Clanname = "";
                                restorePc.ClanRank = 0;
                                restorePc.ClanMemberId = 0;
                                restorePc.Save(); // 儲存玩家的資料到資料庫中
                                clan.delMemberName(restorePc.Name);
                                ClanMembersTable.Instance.deleteMember(restorePc.Id);
                                pc.sendPackets(new S_ServerMessage(240, restorePc.Name)); // %0%o 被你從你的血盟驅逐了。
                            }
                            else
                            {
                                pc.sendPackets(new S_ServerMessage(109, s)); // %0%o 被你從你的血盟驅逐了。
                            }
                        }
                        catch (Exception e)
                        {
                            _log.Error(e);
                        }
                    }
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(518)); // 血盟君主才可使用此命令。
                }
            }
        }

        public override string Type
        {
            get
            {
                return C_BAN_CLAN;
            }
        }
    }

}