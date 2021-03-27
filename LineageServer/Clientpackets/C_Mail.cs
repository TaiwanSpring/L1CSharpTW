using LineageServer.Server.DataSources;
using LineageServer.Server.Model;
using LineageServer.Server.Model.identity;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Server.Templates;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來信件的封包
    /// </summary>
    class C_Mail : ClientBasePacket
    {

        private const string C_MAIL = "[C] C_Mail";

        private static int TYPE_NORMAL_MAIL = 0; // 一般

        private static int TYPE_CLAN_MAIL = 1; // 血盟

        private static int TYPE_MAIL_BOX = 2; // 保管箱

        public C_Mail(byte[] abyte0, ClientThread client) : base(abyte0)
        {

            L1PcInstance pc = client.ActiveChar;
            if (pc == null)
            {
                return;
            }

            int type = ReadC();

            if ((type == 0x00) || (type == 0x01) || (type == 0x02))
            { // 開啟
                pc.sendPackets(new S_Mail(pc, type));
            }
            else if ((type == 0x10) || (type == 0x11) || (type == 0x12))
            { // 讀取
                int mailId = ReadD();
                L1Mail mail = MailTable.getMail(mailId);
                if (mail.ReadStatus == 0)
                {
                    MailTable.Instance.ReadStatus = mailId;
                }
                pc.sendPackets(new S_Mail(mailId, type));
            }
            else if (type == 0x20)
            { // 一般信紙
                if (pc.Inventory.checkItem(40308, 50))
                {
                    pc.Inventory.consumeItem(40308, 50);
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message189));
                    return;
                }
                ReadH(); // 世界寄信次數紀錄
                string receiverName = ReadS();
                sbyte[] text = ReadByte();
                L1PcInstance receiver = L1World.Instance.getPlayer(receiverName);

                if (receiver != null)
                { // 對方在線上
                    if (GetMailSizeByPc(receiver, TYPE_NORMAL_MAIL) >= 40)
                    {
                        pc.sendPackets(new S_Mail(type, false));
                        return;
                    }

                    /* 寄件備份*/
                    int mailId = MailTable.Instance.writeMail(TYPE_NORMAL_MAIL, receiverName, pc, text, pc.Id);
                    pc.sendPackets(new S_Mail(receiver, mailId, true));

                    int mailId2 = MailTable.Instance.writeMail(TYPE_NORMAL_MAIL, receiverName, pc, text, receiver.Id);

                    if (receiver.OnlineStatus == 1)
                    {
                        receiver.sendPackets(new S_Mail(pc, mailId2, false));
                        receiver.sendPackets(new S_SkillSound(receiver.Id, 1091));
                    }
                }
                else
                { // 對方離線中

                    L1PcInstance restorePc = CharacterTable.Instance.restoreCharacter(receiverName);
                    if (restorePc != null)
                    {
                        if (GetMailSizeByPc(restorePc, TYPE_NORMAL_MAIL) >= 40)
                        {
                            pc.sendPackets(new S_Mail(type, false));
                            return;
                        }
                        /* 寄件備份*/
                        int mailId = MailTable.Instance.writeMail(TYPE_NORMAL_MAIL, receiverName, pc, text, pc.Id);
                        pc.sendPackets(new S_Mail(restorePc, mailId, true));

                        MailTable.Instance.writeMail(TYPE_NORMAL_MAIL, receiverName, pc, text, restorePc.Id);
                    }
                    else
                    {
                        pc.sendPackets(new S_ServerMessage(109, receiverName)); // %0という名前の人はいません。
                    }
                }
                pc.sendPackets(new S_Mail(type, true));
            }
            else if (type == 0x21)
            { // 血盟信紙
                if (pc.Clanid > 0)
                {
                    pc.Inventory.consumeItem(40308, 50);
                }
                else
                {
                    pc.sendPackets(new S_ServerMessage(L1SystemMessageId.Message1262));
                    return;
                }
                ReadH();
                string clanName = ReadS();
                sbyte[] text = ReadByte();
                L1Clan clan = L1World.Instance.getClan(clanName);
                if (clan != null)
                {
                    foreach (string name in clan.AllMembers)
                    {
                        L1PcInstance clanPc = L1World.Instance.getPlayer(name);
                        int size = GetMailSizeByPc(clanPc, TYPE_CLAN_MAIL);
                        if (size >= 50)
                        {
                            continue;
                        }
                        MailTable.Instance.writeMail(TYPE_CLAN_MAIL, name, pc, text, clanPc.Id);
                        if (clanPc != null)
                        { // 在線上
                            clanPc.sendPackets(new S_Mail(clanPc, TYPE_CLAN_MAIL));
                            clanPc.sendPackets(new S_SkillSound(clanPc.Id, 1091));
                        }
                    }
                }
            }
            else if ((type == 0x30) || (type == 0x31) || (type == 0x32))
            { // 刪除
                int mailId = ReadD();
                MailTable.Instance.deleteMail(mailId);
                pc.sendPackets(new S_Mail(mailId, type));
            }
            else if (type == 0x60)
            { // 多選刪除
                int count = ReadD();
                for (int i = 0; i < count; i++)
                {
                    int mailId = ReadD();
                    pc.sendPackets(new S_Mail(mailId, (MailTable.getMail(mailId).Type + 0x30)));
                    MailTable.Instance.deleteMail(mailId);
                }
            }
            else if (type == 0x40)
            { // 保管箱儲存
                int mailId = ReadD();
                MailTable.Instance.setMailType(mailId, TYPE_MAIL_BOX);
                pc.sendPackets(new S_Mail(mailId, type));
            }
        }

        private int GetMailSizeByPc(L1PcInstance pc, int type)
        {
            IList<L1Mail> mails = ListFactory.NewList<L1Mail>();
            foreach (L1Mail mail in MailTable.AllMail)
            {
                if (mail.InBoxId == pc.Id)
                {
                    if (mail.Type == type)
                    {
                        mails.Add(mail);
                    }
                }
            }
            return mails.Count;
        }

        public override string Type
        {
            get
            {
                return C_MAIL;
            }
        }
    }

}