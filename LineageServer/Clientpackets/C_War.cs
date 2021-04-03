using LineageServer.Interfaces;
using LineageServer.Server;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using System.Collections.Generic;
namespace LineageServer.Clientpackets
{
    /// <summary>
    /// TODO 翻譯，好多 處理收到由客戶端傳來盟戰的封包
    /// </summary>
    class C_War : ClientBasePacket
    {

        private const string C_WAR = "[C] C_War";

        public C_War(byte[] abyte0, ClientThread clientthread) : base(abyte0)
        {

            L1PcInstance player = clientthread.ActiveChar;
            if (player == null)
            {
                return;
            }

            int type = ReadC();
            string s = ReadS();

            string playerName = player.Name;
            string clanName = player.Clanname;
            int clanId = player.Clanid;

            if (!player.Crown)
            { // 不是王族
                player.sendPackets(new S_ServerMessage(478)); // \f1プリンスとプリンセスのみ戦争を布告できます。
                return;
            }
            if (clanId == 0)
            { // 沒有血盟
                player.sendPackets(new S_ServerMessage(272)); // \f1戦争するためにはまず血盟を創設しなければなりません。
                return;
            }
            L1Clan clan = Container.Instance.Resolve<IGameWorld>().getClan(clanName);
            if (clan == null)
            { // 找不到血盟
                return;
            }

            if (player.Id != clan.LeaderId)
            { // 血盟主
                player.sendPackets(new S_ServerMessage(478)); // \f1プリンスとプリンセスのみ戦争を布告できます。
                return;
            }

            if (clanName.ToLower().Equals(s.ToLower()))
            { // 自クランを指定
                return;
            }

            L1Clan enemyClan = null;
            string enemyClanName = null;
            foreach (L1Clan checkClan in Container.Instance.Resolve<IGameWorld>().AllClans)
            { // 取得所有的血盟
                if (checkClan.ClanName.ToLower().Equals(s.ToLower()))
                {
                    enemyClan = checkClan;
                    enemyClanName = checkClan.ClanName;
                    break;
                }
            }
            if (enemyClan == null)
            { // 相手のクランが見つからなかった
                return;
            }

            bool inWar = false;
            IList<L1War> warList = Container.Instance.Resolve<IGameWorld>().WarList; // 取得所有的盟戰
            foreach (L1War war in warList)
            {
                if (war.CheckClanInWar(clanName))
                { // 檢查是否在盟戰中
                    if (type == 0)
                    { // 宣戰公告
                        player.sendPackets(new S_ServerMessage(234)); // \f1あなたの血盟はすでに戦争中です。
                        return;
                    }
                    inWar = true;
                    break;
                }
            }
            if (!inWar && ((type == 2) || (type == 3)))
            { // 自クランが戦争中以外で、降伏または終結
                return;
            }

            if (clan.CastleId != 0)
            { // 有城堡
                if (type == 0)
                { // 宣戰公告
                    player.sendPackets(new S_ServerMessage(474)); // あなたはすでに城を所有しているので、他の城を取ることは出来ません。
                    return;
                }
                else if ((type == 2) || (type == 3))
                { // 投降、或是結束
                    return;
                }
            }

            if ((enemyClan.CastleId == 0) && (player.Level <= 15))
            {
                player.sendPackets(new S_ServerMessage(232)); // \f1レベル15以下の君主は宣戦布告できません。
                return;
            }

            if ((enemyClan.CastleId != 0) && (player.Level < 25))
            {
                player.sendPackets(new S_ServerMessage(475)); // 攻城戦を宣言するにはレベル25に達していなければなりません。
                return;
            }

            if (enemyClan.CastleId != 0)
            { // 相手クランが城主
                int castle_id = enemyClan.CastleId;
                if (Container.Instance.Resolve<IWarController>().isNowWar(castle_id))
                { // 戦争時間内
                    L1PcInstance[] clanMember = clan.OnlineClanMember;
                    foreach (L1PcInstance element in clanMember)
                    {
                        if (L1CastleLocation.checkInWarArea(castle_id, element))
                        {
                            player.sendPackets(new S_ServerMessage(477)); // あなたを含む全ての血盟員が城の外に出なければ攻城戦は宣言できません。
                            return;
                        }
                    }
                    bool enemyInWar = false;
                    foreach (L1War war in warList)
                    {
                        if (war.CheckClanInWar(enemyClanName))
                        { // 相手クランが既に戦争中
                            if (type == 0)
                            { // 宣戦布告
                                war.DeclareWar(clanName, enemyClanName);
                                war.AddAttackClan(clanName);
                            }
                            else if ((type == 2) || (type == 3))
                            {
                                if (!war.CheckClanInSameWar(clanName, enemyClanName))
                                { // 自クランと相手クランが別の戦争
                                    return;
                                }
                                if (type == 2)
                                { // 降伏
                                    war.SurrenderWar(clanName, enemyClanName);
                                }
                                else if (type == 3)
                                { // 終結
                                    war.CeaseWar(clanName, enemyClanName);
                                }
                            }
                            enemyInWar = true;
                            break;
                        }
                    }
                    if (!enemyInWar && (type == 0))
                    { // 相手クランが戦争中以外で、宣戦布告
                        L1War war = new L1War();
                        war.handleCommands(1, clanName, enemyClanName); // 攻城戦開始
                    }
                }
                else
                { // 戦争時間外
                    if (type == 0)
                    { // 宣戦布告
                        player.sendPackets(new S_ServerMessage(476)); // まだ攻城戦の時間ではありません。
                    }
                }
            }
            else
            { // 相手クランが城主ではない
                bool enemyInWar = false;
                foreach (L1War war in warList)
                {
                    if (war.CheckClanInWar(enemyClanName))
                    { // 相手クランが既に戦争中
                        if (type == 0)
                        { // 宣戦布告
                            player.sendPackets(new S_ServerMessage(236, enemyClanName)); // %0血盟があなたの血盟との戦争を拒絶しました。
                            return;
                        }
                        else if ((type == 2) || (type == 3))
                        { // 降伏または終結
                            if (!war.CheckClanInSameWar(clanName, enemyClanName))
                            { // 自クランと相手クランが別の戦争
                                return;
                            }
                        }
                        enemyInWar = true;
                        break;
                    }
                }
                if (!enemyInWar && ((type == 2) || (type == 3)))
                { // 相手クランが戦争中以外で、降伏または終結
                    return;
                }

                // 攻城戦ではない場合、相手の血盟主の承認が必要
                L1PcInstance enemyLeader = Container.Instance.Resolve<IGameWorld>().getPlayer(enemyClan.LeaderName);

                if (enemyLeader == null)
                { // 相手の血盟主が見つからなかった
                    player.sendPackets(new S_ServerMessage(218, enemyClanName)); // \f1%0血盟の君主は現在ワールドに居ません。
                    return;
                }

                if (type == 0)
                { // 宣戦布告
                    enemyLeader.TempID = player.Id; // 相手のオブジェクトIDを保存しておく
                    enemyLeader.sendPackets(new S_Message_YN(217, clanName, playerName)); // %0血盟の%1があなたの血盟との戦争を望んでいます。戦争に応じますか？（Y/N）
                }
                else if (type == 2)
                { // 降伏
                    enemyLeader.TempID = player.Id; // 相手のオブジェクトIDを保存しておく
                    enemyLeader.sendPackets(new S_Message_YN(221, clanName)); // %0血盟が降伏を望んでいます。受け入れますか？（Y/N）
                }
                else if (type == 3)
                { // 終結
                    enemyLeader.TempID = player.Id; // 相手のオブジェクトIDを保存しておく
                    enemyLeader.sendPackets(new S_Message_YN(222, clanName)); // %0血盟が戦争の終結を望んでいます。終結しますか？（Y/N）
                }
            }
        }

        public override string Type
        {
            get
            {
                return C_WAR;
            }
        }

    }

}