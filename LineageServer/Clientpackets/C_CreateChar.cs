using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來建立角色的封包
    /// </summary>
    class C_CreateChar : ClientBasePacket
    {
        private static ILogger _log = Logger.GetLogger(nameof(C_CreateChar));

        private const string C_CREATE_CHAR = "[C] C_CreateChar";

        private static readonly int[] ORIGINAL_STR = new int[] { 13, 16, 11, 8, 12, 13, 11 };

        private static readonly int[] ORIGINAL_DEX = new int[] { 10, 12, 12, 7, 15, 11, 10 };

        private static readonly int[] ORIGINAL_CON = new int[] { 10, 14, 12, 12, 8, 14, 12 };

        private static readonly int[] ORIGINAL_WIS = new int[] { 11, 9, 12, 12, 10, 12, 12 };

        private static readonly int[] ORIGINAL_CHA = new int[] { 13, 12, 9, 8, 9, 8, 8 };

        private static readonly int[] ORIGINAL_INT = new int[] { 10, 8, 12, 12, 11, 11, 12 };

        private static readonly int[] ORIGINAL_AMOUNT = new int[] { 8, 4, 7, 16, 10, 6, 10 };

        private static readonly string CLIENT_LANGUAGE_CODE = Config.CLIENT_LANGUAGE_CODE;
        public C_CreateChar(byte[] abyte0, ClientThread client) : base(abyte0)
        {
            L1PcInstance pc = new L1PcInstance();
            string name = ReadS();

            Account account = Account.Load(client.AccountName);
            int characterSlot = account.CharacterSlot;
            int maxAmount = Config.DEFAULT_CHARACTER_SLOT + characterSlot;

            name = name.Replace("\\s", "");
            name = name.Replace("　", "");
            if (name.Length == 0)
            {
                S_CharCreateStatus s_charcreatestatus = new S_CharCreateStatus(S_CharCreateStatus.REASON_INVALID_NAME);
                client.SendPacket(s_charcreatestatus);
                return;
            }

            if (isInvalidName(name))
            {
                S_CharCreateStatus s_charcreatestatus = new S_CharCreateStatus(S_CharCreateStatus.REASON_INVALID_NAME);
                client.SendPacket(s_charcreatestatus);
                return;
            }

            if (Container.Instance.Resolve<ICharacterController>().doesCharNameExist(name))
            {
                S_CharCreateStatus s_charcreatestatus1 = new S_CharCreateStatus(S_CharCreateStatus.REASON_ALREADY_EXSISTS);
                client.SendPacket(s_charcreatestatus1);
                return;
            }

            if (client.Account.CountCharacters() >= maxAmount)
            {
                S_CharCreateStatus s_charcreatestatus1 = new S_CharCreateStatus(S_CharCreateStatus.REASON_WRONG_AMOUNT);
                client.SendPacket(s_charcreatestatus1);
                return;
            }

            pc.Name = name;
            pc.Type = ReadC();
            pc.set_sex(ReadC());
            pc.addBaseStr(ReadC());
            pc.addBaseDex(ReadC());
            pc.addBaseCon(ReadC());
            pc.addBaseWis(ReadC());
            pc.addBaseCha(ReadC());
            pc.addBaseInt(ReadC());

            bool isStatusError = false;
            int originalStr = ORIGINAL_STR[pc.Type];
            int originalDex = ORIGINAL_DEX[pc.Type];
            int originalCon = ORIGINAL_CON[pc.Type];
            int originalWis = ORIGINAL_WIS[pc.Type];
            int originalCha = ORIGINAL_CHA[pc.Type];
            int originalInt = ORIGINAL_INT[pc.Type];
            int originalAmount = ORIGINAL_AMOUNT[pc.Type];

            if ((pc.BaseStr < originalStr) ||
                (pc.BaseDex < originalDex) ||
                (pc.BaseCon < originalCon) ||
                (pc.BaseWis < originalWis) ||
                (pc.BaseCha < originalCha) ||
                (pc.BaseInt < originalInt) ||
                (pc.BaseStr > originalStr + originalAmount) ||
                (pc.BaseDex > originalDex + originalAmount) ||
                (pc.BaseCon > originalCon + originalAmount) ||
                (pc.BaseWis > originalWis + originalAmount) ||
                (pc.BaseCha > originalCha + originalAmount) ||
                (pc.BaseInt > originalInt + originalAmount))
            {
                isStatusError = true;
            }

            int statusAmount = pc.getDex() + pc.getCha() + pc.getCon()
                + pc.getInt() + pc.getStr() + pc.getWis();

            if ((statusAmount != 75) || isStatusError)
            {
                _log.Info($"{client.Account} create character have wrong value");
                S_CharCreateStatus s_charcreatestatus3 = new S_CharCreateStatus(S_CharCreateStatus.REASON_WRONG_AMOUNT);
                client.SendPacket(s_charcreatestatus3);
                return;
            }

            _log.Info("charname: " + pc.Name + " classId: " + pc.ClassId);
            S_CharCreateStatus s_charcreatestatus2 = new S_CharCreateStatus(S_CharCreateStatus.REASON_OK);
            client.SendPacket(s_charcreatestatus2);
            initNewChar(client, pc);
        }

        private static readonly int[] MALE_LIST = new int[] { 0, 61, 138, 734, 2786, 6658, 6671 };

        private static readonly int[] FEMALE_LIST = new int[] { 1, 48, 37, 1186, 2796, 6661, 6650 };

        /* 台灣伺服器 3.80C 新手村*/
        private const int LOCX = 32689;

        private const int LOCY = 32842;

        private const short MAPID = 2005;
        private static void initNewChar(ClientThread client, L1PcInstance pc)
        {
            pc.Id = Container.Instance.Resolve<IIdFactory>().nextId();
            pc.setBirthday();
            if (pc.get_sex() == 0)
            {
                pc.ClassId = MALE_LIST[pc.Type];
            }
            else
            {
                pc.ClassId = FEMALE_LIST[pc.Type];
            }
            pc.X = LOCX;
            pc.Y = LOCY;
            pc.Map = Container.Instance.Resolve<IWorldMap>().getMap(MAPID);
            pc.Heading = 0;
            pc.Lawful = 0;

            int initHp = CalcInitHpMp.calcInitHp(pc);
            int initMp = CalcInitHpMp.calcInitMp(pc);
            pc.addBaseMaxHp((short)initHp);
            pc.CurrentHp = (short)initHp;
            pc.addBaseMaxMp((short)initMp);
            pc.CurrentMp = (short)initMp;
            pc.resetBaseAc();
            pc.Title = "";
            pc.Clanid = 0;
            pc.ClanRank = 0;
            pc.set_food(40);
            pc.AccessLevel = (short)0;
            pc.Gm = false;
            pc.Monitor = false;
            pc.GmInvis = false;
            pc.Exp = 0;
            pc.HighLevel = 0;
            pc.Status = 0;
            pc.Clanname = "";
            pc.BonusStats = 0;
            pc.ElixirStats = 0;
            pc.resetBaseMr();
            pc.ElfAttr = 0;
            pc.set_PKcount(0);
            pc.PkCountForElf = 0;
            pc.ExpRes = 0;
            pc.PartnerId = 0;
            pc.OnlineStatus = 0;
            pc.HomeTownId = 0;
            pc.Contribution = 0;
            pc.Banned = false;
            pc.Karma = 0;
            if (pc.Wizard)
            { // WIZ
                pc.sendPackets(new S_AddSkill(3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                int object_id = pc.Id;
                L1Skills l1skills = SkillsTable.Instance.getTemplate(4); // EB
                string skill_name = l1skills.Name;
                int skill_id = l1skills.SkillId;
                SkillsTable.Instance.spellMastery(object_id, skill_id, skill_name, 0, 0); // 儲存魔法資料到資料庫中
            }
            BeginnerController.Instance.GiveItem(pc);
            pc.AccountName = client.AccountName;
            Container.Instance.Resolve<ICharacterController>().storeNewCharacter(pc);
            S_NewCharPacket s_newcharpacket = new S_NewCharPacket(pc);
            client.SendPacket(s_newcharpacket);
            Container.Instance.Resolve<ICharacterController>().saveCharStatus(pc);
            pc.refresh();
        }

        private static bool isAlphaNumeric(string s)
        {
            bool flag = true;
            char[] ac = s.ToCharArray();
            int i = 0;
            do
            {
                if (i >= ac.Length)
                {
                    break;
                }
                if (!char.IsLetterOrDigit(ac[i]))
                {
                    flag = false;
                    break;
                }
                i++;
            } while (true);
            return flag;
        }

        private static bool isInvalidName(string name)
        {


            // TODO:Check the badNameList is working well ?
            if (BadNamesList.Instance.isBadName(name))
            {
                return true;
            }


            if (isAlphaNumeric(name))
            {
                return false;
            }

            // XXX - 規則還沒確定
            // 雙字節字符或5個字符以上或整個超過 12個字節就視為一個無效的名稱
            int numOfNameBytes = 0;
            try
            {
                numOfNameBytes = GobalParameters.Encoding.GetByteCount(name);
            }
            catch (Exception e)
            {
                _log.Error(e);
                return false;
            }
            if ((5 < (numOfNameBytes - name.Length)) || (12 < numOfNameBytes))
            {
                return false;
            }

            if (BadNamesList.Instance.isBadName(name))
            {
                return false;
            }
            return true;
        }

        public override string Type
        {
            get
            {
                return C_CREATE_CHAR;
            }
        }
    }

}