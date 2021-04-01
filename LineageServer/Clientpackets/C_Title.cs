using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來變更稱號的封包
    /// </summary>
    class C_Title : ClientBasePacket
    {

        private const string C_TITLE = "[C] C_Title";
        public C_Title(byte[] abyte0, ClientThread clientthread) : base(abyte0)
        {

            L1PcInstance pc = clientthread.ActiveChar;
            if (pc == null)
            {
                return;
            }

            string charName = ReadS();
            string title = ReadS();

            if (charName.Length == 0 || title.Length == 0)
            {
                // \f1次のように入力してください：「/title \f0キャラクター名 呼称\f1」
                pc.sendPackets(new S_ServerMessage(196));
                return;
            }
            L1PcInstance target = Container.Instance.Resolve<IGameWorld>().getPlayer(charName);
            if (target == null)
            {
                return;
            }

            if (pc.Gm)
            {
                changeTitle(target, title);
                return;
            }

            if (isClanLeader(pc))
            { // 血盟主
                if (pc.Id == target.Id)
                { // 自己
                    if (pc.Level < 10)
                    {
                        // \f1血盟員の場合、呼称を持つにはレベル10以上でなければなりません。
                        pc.sendPackets(new S_ServerMessage(197));
                        return;
                    }
                    changeTitle(pc, title);
                }
                else
                { // 他人
                    if (pc.Clanid != target.Clanid)
                    {
                        // \f1血盟員でなければ他人に呼称を与えることはできません。
                        pc.sendPackets(new S_ServerMessage(199));
                        return;
                    }
                    if (target.Level < 10)
                    {
                        // \f1%0のレベルが10未満なので呼称を与えることはできません。
                        pc.sendPackets(new S_ServerMessage(202, charName));
                        return;
                    }
                    changeTitle(target, title);
                    L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
                    if (clan != null)
                    {
                        foreach (L1PcInstance clanPc in clan.OnlineClanMember)
                        {
                            // \f1%0が%1に「%2」という呼称を与えました。
                            clanPc.sendPackets(new S_ServerMessage(203, pc.Name, charName, title));
                        }
                    }
                }
            }
            else
            {
                if (pc.Id == target.Id)
                { // 自分
                    if (pc.Clanid != 0 && !Config.CHANGE_TITLE_BY_ONESELF)
                    {
                        // \f1血盟員に呼称を与えられるのはプリンスとプリンセスだけです。
                        pc.sendPackets(new S_ServerMessage(198));
                        return;
                    }
                    if (target.Level < 40)
                    {
                        // \f1血盟員ではないのに呼称を持つには、レベル40以上でなければなりません。
                        pc.sendPackets(new S_ServerMessage(200));
                        return;
                    }
                    changeTitle(pc, title);
                }
                else
                { // 他人
                    if (pc.Crown)
                    { // 連合に所属した君主
                        if (pc.Clanid == target.Clanid)
                        {
                            // \f1%0はあなたの血盟ではありません。
                            pc.sendPackets(new S_ServerMessage(201, pc.Clanname));
                            return;
                        }
                    }
                }
            }
        }

        private void changeTitle(L1PcInstance pc, string title)
        {
            int objectId = pc.Id;
            pc.Title = title;
            pc.sendPackets(new S_CharTitle(objectId, title));
            pc.broadcastPacket(new S_CharTitle(objectId, title));
            pc.Save(); // 儲存玩家的資料到資料庫中
        }

        private bool isClanLeader(L1PcInstance pc)
        {
            bool isClanLeader = false;
            if (pc.Clanid != 0)
            { // 有血盟
                L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(pc.Clanname);
                if (clan != null)
                {
                    if (pc.Crown && pc.Id == clan.LeaderId)
                    { // 君主、かつ、血盟主
                        isClanLeader = true;
                    }
                }
            }
            return isClanLeader;
        }

        public override string Type
        {
            get
            {
                return C_TITLE;
            }
        }

    }

}