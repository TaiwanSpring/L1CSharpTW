using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到客戶端傳來角色升級/出生的封包
    /// </summary>
    class C_CharReset : ClientBasePacket
    {
        private const string C_CHAR_RESET = "[C] C_CharReset";
        private static ILogger _log = Logger.GetLogger(nameof(C_CharReset));

        /// <summary>
        /// //配置完初期點數 按確定 127.0.0.1 Request Work ID : 120 0000: 78 01 0d 0a 0b 0a 12
        /// 0d
        /// 
        /// //提升10及 127.0.0.1 Request Work ID : 120 0000: 78 02 07 00 //提升1及
        /// 127.0.0.1 Request Work ID : 120 0000: 78 02 00 04
        /// 
        /// //提升完等級 127.0.0.1 Request Work ID : 120 0000: 78 02 08 00 x...
        /// 
        /// //萬能藥 127.0.0.1 Request Work ID : 120 0000: 78 03 23 0a 0b 17 12 0d
        /// </summary>

        public C_CharReset(byte[] abyte0, ClientThread clientthread) : base(abyte0)
        {
            if (clientthread.ActiveChar == null)
            {
                return;
            }

            L1PcInstance pc = clientthread.ActiveChar;

            int stage = ReadC();

            if (stage == 0x01)
            { // 0x01:キャラクター初期化
                int str = ReadC();
                int intel = ReadC();
                int wis = ReadC();
                int dex = ReadC();
                int con = ReadC();
                int cha = ReadC();
                int hp = CalcInitHpMp.calcInitHp(pc);
                int mp = CalcInitHpMp.calcInitMp(pc);
                pc.sendPackets(new S_OwnCharStatus2(pc, 0));
                /// <summary>
                /// 『來源:伺服器』<位址:64>{長度:8}(時間:1233793211)
                ///  0000:  40 04 00 00 04 01 8b df   @.......
                ///  尚未知的封包
                /// </summary>
                pc.sendPackets(new S_CharReset(pc, 1, hp, mp, 10, str, intel, wis, dex, con, cha));
                initCharStatus(pc, hp, mp, str, intel, wis, dex, con, cha);
                Container.Instance.Resolve<ICharacterController>().saveCharStatus(pc);
            }
            else if (stage == 0x02)
            {
                // 0x02:ステータス再分配
                int type2 = ReadC();
                if (type2 == 0x00)
                { // 0x00:Lv1UP
                    setLevelUp(pc, 1);
                }
                else if (type2 == 0x07)
                { // 0x07:Lv10UP
                    if (pc.TempMaxLevel - pc.TempLevel < 10)
                    {
                        return;
                    }
                    setLevelUp(pc, 10);
                }
                else if (type2 == 0x01)
                {
                    pc.addBaseStr(1);
                    setLevelUp(pc, 1);
                }
                else if (type2 == 0x02)
                {
                    pc.addBaseInt(1);
                    setLevelUp(pc, 1);
                }
                else if (type2 == 0x03)
                {
                    pc.addBaseWis(1);
                    setLevelUp(pc, 1);
                }
                else if (type2 == 0x04)
                {
                    pc.addBaseDex(1);
                    setLevelUp(pc, 1);
                }
                else if (type2 == 0x05)
                {
                    pc.addBaseCon(1);
                    setLevelUp(pc, 1);
                }
                else if (type2 == 0x06)
                {
                    pc.addBaseCha(1);
                    setLevelUp(pc, 1);
                }
                else if (type2 == 0x08)
                {
                    switch (ReadC())
                    {
                        case 1:
                            pc.addBaseStr(1);
                            break;
                        case 2:
                            pc.addBaseInt(1);
                            break;
                        case 3:
                            pc.addBaseWis(1);
                            break;
                        case 4:
                            pc.addBaseDex(1);
                            break;
                        case 5:
                            pc.addBaseCon(1);
                            break;
                        case 6:
                            pc.addBaseCha(1);
                            break;
                    }
                    if (pc.ElixirStats > 0)
                    {
                        pc.sendPackets(new S_CharReset(pc.ElixirStats));
                        return;
                    }
                    saveNewCharStatus(pc);
                }
            }
            else if (stage == 0x03)
            {
                pc.addBaseStr((byte)(ReadC() - pc.BaseStr));
                pc.addBaseInt((byte)(ReadC() - pc.BaseInt));
                pc.addBaseWis((byte)(ReadC() - pc.BaseWis));
                pc.addBaseDex((byte)(ReadC() - pc.BaseDex));
                pc.addBaseCon((byte)(ReadC() - pc.BaseCon));
                pc.addBaseCha((byte)(ReadC() - pc.BaseCha));
                saveNewCharStatus(pc);
            }
        }

        private void saveNewCharStatus(L1PcInstance pc)
        {
            pc.InCharReset = false;
            if (pc.OriginalAc > 0)
            {
                pc.addAc(pc.OriginalAc);
            }
            if (pc.OriginalMr > 0)
            {
                pc.addMr(0 - pc.OriginalMr);
            }
            pc.refresh();
            pc.CurrentHp = pc.getMaxHp();
            pc.CurrentMp = pc.getMaxMp();
            if (pc.TempMaxLevel != pc.Level)
            {
                pc.Level = pc.TempMaxLevel;
                pc.Exp = ExpTable.getExpByLevel(pc.TempMaxLevel);
            }
            if (pc.Level > 50)
            {
                pc.BonusStats = pc.Level - 50;
            }
            else
            {
                pc.BonusStats = 0;
            }
            pc.sendPackets(new S_OwnCharStatus(pc));
            L1ItemInstance item = pc.Inventory.findItemId(49142); // 希望のロウソク
            if (item != null)
            {
                try
                {
                    pc.Inventory.removeItem(item, 1);
                    pc.Save(); // 儲存玩家的資料到資料庫中
                }
                catch (Exception e)
                {
                    _log.Error(e);
                }
            }
            L1Teleport.teleport(pc, 32628, 32772, (short)4, 4, false);
        }

        private void initCharStatus(L1PcInstance pc, int hp, int mp, int str, int intel, int wis, int dex, int con, int cha)
        {
            pc.addBaseMaxHp((short)(hp - pc.BaseMaxHp));
            pc.addBaseMaxMp((short)(mp - pc.BaseMaxMp));
            pc.addBaseStr((byte)(str - pc.BaseStr));
            pc.addBaseInt((byte)(intel - pc.BaseInt));
            pc.addBaseWis((byte)(wis - pc.BaseWis));
            pc.addBaseDex((byte)(dex - pc.BaseDex));
            pc.addBaseCon((byte)(con - pc.BaseCon));
            pc.addBaseCha((byte)(cha - pc.BaseCha));
        }

        private void setLevelUp(L1PcInstance pc, int addLv)
        {
            pc.TempLevel = pc.TempLevel + addLv;
            for (int i = 0; i < addLv; i++)
            {
                short randomHp = CalcStat.calcStatHp(pc.Type, pc.BaseMaxHp, pc.BaseCon, pc.OriginalHpup);
                short randomMp = CalcStat.calcStatMp(pc.Type, pc.BaseMaxMp, pc.BaseWis, pc.OriginalMpup);
                pc.addBaseMaxHp(randomHp);
                pc.addBaseMaxMp(randomMp);
            }
            int newAc = CalcStat.calcAc(pc.TempLevel, pc.BaseDex);
            pc.sendPackets(new S_CharReset(pc, pc.TempLevel, pc.BaseMaxHp, pc.BaseMaxMp, newAc, pc.BaseStr, pc.BaseInt, pc.BaseWis, pc.BaseDex, pc.BaseCon, pc.BaseCha));
        }

        public override string Type
        {
            get
            {
                return C_CHAR_RESET;
            }
        }

    }

}