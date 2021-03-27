﻿
using LineageServer.Clientpackets;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Server
{
    class PacketHandler
    {
        public PacketHandler(ClientThread clientthread)
        {
            _client = clientthread;
        }
        public virtual void handlePacket(byte[] abyte0, L1PcInstance pcInstance)
        {
            int i = abyte0[0] & 0xff;

            switch (i)
            {
                case Opcodes.C_OPCODE_EXCLUDE:
                    new C_Exclude(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_CHARACTERCONFIG:
                    new C_CharcterConfig(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_DOOR:
                    new C_Door(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_TITLE:
                    new C_Title(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_BOARDDELETE:
                    new C_BoardDelete(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_PLEDGE:
                    new C_Pledge(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_CHANGEHEADING:
                    new C_ChangeHeading(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_NPCACTION:
                    new C_NPCAction(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_USESKILL:
                    new C_UseSkill(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_EMBLEMUPLOAD:
                    new C_EmblemUpload(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_TAXRATE:
                    new C_TaxRate(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_EMBLEMDOWNLOAD:
                    new C_EmblemDownload(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_TRADEADDCANCEL:
                    new C_TradeCancel(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_CHANGEWARTIME:
                    new C_ChangeWarTime(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_BOOKMARK:
                    new C_AddBookmark(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_CREATECLAN:
                    new C_CreateClan(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_CLIENTVERSION:
                    new C_ServerVersion(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_DRAWAL:
                    new C_Drawal(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_DEPOSIT:
                    new C_Deposit(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_PROPOSE:
                    new C_Propose(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_SKILLBUY:
                    new C_SkillBuy(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_SHOP:
                    new C_Shop(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_BOARDREAD:
                    new C_BoardRead(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_TRADE:
                    new C_Trade(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_DELETECHAR:
                    new C_DeleteChar(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_KEEPALIVE:
                    new C_KeepALIVE(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_ATTR:
                    new C_Attr(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_LOGINPACKET:
                    new C_AuthLogin(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_BEANFUNLOGINPACKET:
                    new C_AuthLogin(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_RESULT:
                    new C_Result(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_LOGINTOSERVEROK:
                    new C_LoginToServerOK(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_SKILLBUYOK:
                    new C_SkillBuyOK(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_TRADEADDITEM:
                    new C_TradeAddItem(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_ADDBUDDY:
                    new C_AddBuddy(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_CHAT:
                    new C_Chat(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_TRADEADDOK:
                    new C_TradeOK(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_CHECKPK:
                    new C_CheckPK(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_CHANGECHAR:
                    new C_NewCharSelect(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_BUDDYLIST:
                    new C_Buddy(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_DROPITEM:
                    new C_DropItem(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_LEAVEPARTY:
                    new C_LeaveParty(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_PLEDGECONTENT:
                    new C_PledgeContent(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_PLEDGE_RECOMMENDATION:
                    new C_PledgeRecommendation(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_ATTACK:
                case Opcodes.C_OPCODE_ARROWATTACK:
                    new C_Attack(abyte0, _client);
                    break;

                // TODO 翻譯
                // キャラクターのショートカットやインベントリの状態がプレイ中に変動した場合に
                // ショートカットやインベントリの状態を付加してクライアントから送信されてくる
                // 送られてくるタイミングはクライアント終了時
                case Opcodes.C_OPCODE_QUITGAME:
                    break;

                case Opcodes.C_OPCODE_BANCLAN:
                    new C_BanClan(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_BOARD:
                    new C_Board(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_DELETEINVENTORYITEM:
                    new C_DeleteInventoryItem(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_CHATWHISPER:
                    new C_ChatWhisper(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_PARTYLIST:
                    new C_Party(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_PICKUPITEM:
                    new C_PickUpItem(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_WHO:
                    new C_Who(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_GIVEITEM:
                    new C_GiveItem(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_MOVECHAR:
                    new C_MoveChar(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_BOOKMARKDELETE:
                    new C_DeleteBookmark(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_RESTART:
                    new C_Restart(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_LEAVECLANE:
                    new C_LeaveClan(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_NPCTALK:
                    new C_NPCTalk(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_BANPARTY:
                    new C_BanParty(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_DELBUDDY:
                    new C_DelBuddy(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_WAR:
                    new C_War(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_LOGINTOSERVER:
                    new C_LoginToServer(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_PRIVATESHOPLIST:
                    new C_ShopList(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_CHATGLOBAL:
                    new C_Chat(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_JOINCLAN:
                    new C_JoinClan(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_NEWCHAR:
                    new C_CreateChar(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_EXTCOMMAND:
                    new C_ExtraCommand(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_BOARDWRITE:
                    new C_BoardWrite(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_USEITEM:
                    new C_ItemUSe(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_CREATEPARTY:
                    new C_CreateParty(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_ENTERPORTAL:
                    new C_EnterPortal(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_AMOUNT:
                    new C_Amount(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_FIX_WEAPON_LIST:
                    new C_FixWeaponList(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_SELECTLIST:
                    new C_SelectList(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_EXIT_GHOST:
                    new C_ExitGhost(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_CALL:
                    new C_CallPlayer(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_HIRESOLDIER:
                    new C_HireSoldier(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_FISHCLICK:
                    new C_FishClick(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_SELECTTARGET:
                    new C_SelectTarget(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_PETMENU:
                    new C_PetMenu(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_USEPETITEM:
                    new C_UsePetItem(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_TELEPORT:
                    new C_Teleport(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_CAHTPARTY:
                    new C_ChatParty(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_FIGHT:
                    new C_Fight(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_SHIP:
                    new C_Ship(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_MAIL:
                    new C_Mail(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_CHARRESET:
                    new C_CharReset(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_SENDLOCATION:
                    new C_SendLocation(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_WAREHOUSELOCK:
                    new C_WarePassword(abyte0, _client);
                    break;

                case Opcodes.C_OPCODE_RESTARTMENU:
                    new C_RestartMenu(abyte0, _client);
                    break;

                default:
                    // String s = Integer.toHexString(abyte0[0] & 0xff);
                    // _log.warning("用途不明オペコード:データ内容");
                    // _log.warning((new StringBuilder()).append("オペコード ").append(s)
                    // .toString());
                    // _log.warning(new ByteArrayUtil(abyte0).dumpToString());
                    break;
            }
            // _log.warning((new StringBuilder()).append("オペコード
            // ").append(i).toString());
        }

        private readonly ClientThread _client;
    }
}