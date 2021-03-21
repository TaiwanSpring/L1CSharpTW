﻿using System;

/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Server.storage.mysql
{

    using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
    using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
    using CharacterStorage = LineageServer.Server.Server.storage.CharacterStorage;
    using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;

    public class MySqlCharacterStorage : CharacterStorage
    {
        //JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
        private static Logger _log = Logger.getLogger(typeof(MySqlCharacterStorage).FullName);

        public virtual L1PcInstance loadCharacter(string charName)
        {
            L1PcInstance pc = null;
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            PreparedStatement pstm2 = null;
            ResultSet rs = null;
            try
            {

                con = L1DatabaseFactory.Instance.Connection;
                pstm = con.prepareStatement("SELECT * FROM characters WHERE char_name=?");
                pstm.setString(1, charName);
                rs = pstm.executeQuery();
                if (!rs.next())
                {
                    /*
					 * SELECTが結果を返さなかった。
					 */
                    return null;
                }
                pc = new L1PcInstance();
                pc.AccountName = rs.getString("account_name");
                pc.Id = rs.getInt("objid");
                pc.Name = rs.getString("char_name");
                pc.Birthday = rs.getTimestamp("birthday");
                pc.HighLevel = rs.getInt("HighLevel");
                pc.Exp = rs.getLong("Exp");
                pc.addBaseMaxHp(rs.getShort("MaxHp"));
                short currentHp = rs.getShort("CurHp");
                if (currentHp < 1)
                {
                    currentHp = 1;
                }
                pc.CurrentHpDirect = currentHp;
                pc.Dead = false;
                pc.Status = 0;
                pc.addBaseMaxMp(rs.getShort("MaxMp"));
                pc.CurrentMpDirect = rs.getShort("CurMp");
                pc.addBaseStr(rs.getByte("Str"));
                pc.addBaseCon(rs.getByte("Con"));
                pc.addBaseDex(rs.getByte("Dex"));
                pc.addBaseCha(rs.getByte("Cha"));
                pc.addBaseInt(rs.getByte("Intel"));
                pc.addBaseWis(rs.getByte("Wis"));
                int status = rs.getInt("Status");
                pc.CurrentWeapon = status;
                int classId = rs.getInt("Class");
                pc.ClassId = classId;
                pc.TempCharGfx = classId;
                pc.GfxId = classId;
                pc.set_sex(rs.getInt("Sex"));
                pc.Type = rs.getInt("Type");
                int head = rs.getInt("Heading");
                if (head > 7)
                {
                    head = 0;
                }
                pc.Heading = head;
                pc.X = rs.getInt("locX");
                pc.Y = rs.getInt("locY");
                pc.Map = rs.getShort("MapID");
                pc.set_food(rs.getInt("Food"));
                pc.Lawful = rs.getInt("Lawful");
                pc.Title = rs.getString("Title");
                pc.Clanid = rs.getInt("ClanID");
                pc.Clanname = rs.getString("Clanname");
                pc.ClanRank = rs.getInt("ClanRank");
                pc.BonusStats = rs.getInt("BonusStatus");
                pc.ElixirStats = rs.getInt("ElixirStatus");
                pc.ElfAttr = rs.getInt("ElfAttr");
                pc.set_PKcount(rs.getInt("PKcount"));
                pc.PkCountForElf = rs.getInt("PkCountForElf");
                pc.ExpRes = rs.getInt("ExpRes");
                pc.PartnerId = rs.getInt("PartnerID");
                pc.AccessLevel = rs.getShort("AccessLevel");
                if (pc.AccessLevel == 200)
                {
                    pc.Gm = true;
                    pc.Monitor = false;
                }
                else if (pc.AccessLevel == 100)
                {
                    pc.Gm = false;
                    pc.Monitor = true;
                }
                else
                {
                    pc.Gm = false;
                    pc.Monitor = false;
                }
                pc.OnlineStatus = rs.getInt("OnlineStatus");
                pc.HomeTownId = rs.getInt("HomeTownID");
                pc.Contribution = rs.getInt("Contribution");
                pc.Pay = rs.getInt("Pay"); // 村莊福利金 此欄位由 HomeTownTimeController 處理 update
                pc.HellTime = rs.getInt("HellTime");
                pc.Banned = rs.getBoolean("Banned");
                pc.Karma = rs.getInt("Karma");
                pc.LastPk = rs.getTimestamp("LastPk");
                pc.LastPkForElf = rs.getTimestamp("LastPkForElf");
                pc.DeleteTime = rs.getTimestamp("DeleteTime");
                pc.OriginalStr = rs.getInt("OriginalStr");
                pc.OriginalCon = rs.getInt("OriginalCon");
                pc.OriginalDex = rs.getInt("OriginalDex");
                pc.OriginalCha = rs.getInt("OriginalCha");
                pc.OriginalInt = rs.getInt("OriginalInt");
                pc.OriginalWis = rs.getInt("OriginalWis");
                pc.LastActive = rs.getTimestamp("LastActive"); // TODO 殷海薩的祝福
                pc.AinZone = rs.getInt("AinZone"); // TODO 殷海薩的祝福
                pc.AinPoint = rs.getInt("AinPoint"); // TODO 殷海薩的祝福
                rs.close();

                pc.refresh();
                pc.MoveSpeed = 0;
                pc.BraveSpeed = 0;
                pc.GmInvis = false;

                if (pc.Clanid > 0)
                {
                    pstm2 = con.prepareStatement("SELECT * FROM clan_members WHERE char_id=?");
                    pstm2.setInt(1, pc.Id);
                    rs = pstm2.executeQuery();
                    if (!rs.next())
                    {
                        return null;
                    }
                    pc.ClanMemberId = rs.getInt("index_id");
                    pc.ClanMemberNotes = rs.getString("notes");
                }
                _log.finest("restored char data: ");
            }
            catch (SQLException e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
                return null;
            }
            finally
            {
                SQLUtil.close(rs);
                SQLUtil.close(pstm);
                SQLUtil.close(con);
            }
            return pc;
        }

        public virtual void createCharacter(L1PcInstance pc)
        {
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            try
            {
                int i = 0;
                con = L1DatabaseFactory.Instance.Connection;
                pstm = con.prepareStatement("INSERT INTO characters SET account_name=?,objid=?,char_name=?,birthday=?,level=?,HighLevel=?,Exp=?,MaxHp=?,CurHp=?,MaxMp=?,CurMp=?,Ac=?,Str=?,Con=?,Dex=?,Cha=?,Intel=?,Wis=?,Status=?,Class=?,Sex=?,Type=?,Heading=?,LocX=?,LocY=?,MapID=?,Food=?,Lawful=?,Title=?,ClanID=?,Clanname=?,ClanRank=?,BonusStatus=?,ElixirStatus=?,ElfAttr=?,PKcount=?,PkCountForElf=?,ExpRes=?,PartnerID=?,AccessLevel=?,OnlineStatus=?,HomeTownID=?,Contribution=?,Pay=?,HellTime=?,Banned=?,Karma=?,LastPk=?,LastPkForElf=?,DeleteTime=?");
                pstm.setString(++i, pc.AccountName);
                pstm.setInt(++i, pc.Id);
                pstm.setString(++i, pc.Name);
                pstm.setInt(++i, pc.SimpleBirthday);
                pstm.setInt(++i, pc.Level);
                pstm.setInt(++i, pc.HighLevel);
                pstm.setLong(++i, pc.Exp);
                pstm.setInt(++i, pc.BaseMaxHp);
                int hp = pc.CurrentHp;
                if (hp < 1)
                {
                    hp = 1;
                }
                pstm.setInt(++i, hp);
                pstm.setInt(++i, pc.BaseMaxMp);
                pstm.setInt(++i, pc.CurrentMp);
                pstm.setInt(++i, pc.Ac);
                pstm.setInt(++i, pc.BaseStr);
                pstm.setInt(++i, pc.BaseCon);
                pstm.setInt(++i, pc.BaseDex);
                pstm.setInt(++i, pc.BaseCha);
                pstm.setInt(++i, pc.BaseInt);
                pstm.setInt(++i, pc.BaseWis);
                pstm.setInt(++i, pc.CurrentWeapon);
                pstm.setInt(++i, pc.ClassId);
                pstm.setInt(++i, pc.get_sex());
                pstm.setInt(++i, pc.Type);
                pstm.setInt(++i, pc.Heading);
                pstm.setInt(++i, pc.X);
                pstm.setInt(++i, pc.Y);
                pstm.setInt(++i, pc.MapId);
                pstm.setInt(++i, pc.get_food());
                pstm.setInt(++i, pc.Lawful);
                pstm.setString(++i, pc.Title);
                pstm.setInt(++i, pc.Clanid);
                pstm.setString(++i, pc.Clanname);
                pstm.setInt(++i, pc.ClanRank);
                pstm.setInt(++i, pc.BonusStats);
                pstm.setInt(++i, pc.ElixirStats);
                pstm.setInt(++i, pc.ElfAttr);
                pstm.setInt(++i, pc.get_PKcount());
                pstm.setInt(++i, pc.PkCountForElf);
                pstm.setInt(++i, pc.ExpRes);
                pstm.setInt(++i, pc.PartnerId);
                pstm.setShort(++i, pc.AccessLevel);
                pstm.setInt(++i, pc.OnlineStatus);
                pstm.setInt(++i, pc.HomeTownId);
                pstm.setInt(++i, pc.Contribution);
                pstm.setInt(++i, 0);
                pstm.setInt(++i, pc.HellTime);
                pstm.setBoolean(++i, pc.Banned);
                pstm.setInt(++i, pc.Karma);
                pstm.setTimestamp(++i, pc.LastPk);
                pstm.setTimestamp(++i, pc.LastPkForElf);
                pstm.setTimestamp(++i, pc.DeleteTime);

                pstm.execute();

                _log.finest("stored char data: " + pc.Name);
            }
            catch (SQLException e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
            finally
            {
                SQLUtil.close(pstm);
                SQLUtil.close(con);
            }
        }
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: @Override public void deleteCharacter(String accountName, String charName)throws Exception
        public virtual void deleteCharacter(string accountName, string charName)
        {
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            ResultSet rs = null;
            try
            {
                con = L1DatabaseFactory.Instance.Connection;
                pstm = con.prepareStatement("SELECT * FROM characters WHERE account_name=? AND char_name=?");
                pstm.setString(1, accountName);
                pstm.setString(2, charName);
                rs = pstm.executeQuery();
                if (!rs.next())
                {
                    /*
					 * SELECTが値を返していない
					 * 存在しないか、あるいは別のアカウントが所有しているキャラクター名が指定されたということになる。
					 */
                    _log.warning("invalid delete char request: account=" + accountName + " char=" + charName);
                    throw new Exception("could not delete character");
                }
                pstm.close();

                pstm = con.prepareStatement("DELETE FROM character_buddys WHERE char_id IN (SELECT objid FROM characters WHERE char_name = ?)");
                pstm.setString(1, charName);
                pstm.execute();
                pstm.close();

                pstm = con.prepareStatement("DELETE FROM character_buff WHERE char_obj_id IN (SELECT objid FROM characters WHERE char_name = ?)");
                pstm.setString(1, charName);
                pstm.execute();
                pstm.close();

                pstm = con.prepareStatement("DELETE FROM character_config WHERE object_id IN (SELECT objid FROM characters WHERE char_name = ?)");
                pstm.setString(1, charName);
                pstm.execute();
                pstm.close();

                pstm = con.prepareStatement("DELETE FROM character_items WHERE char_id IN (SELECT objid FROM characters WHERE char_name = ?)");
                pstm.setString(1, charName);
                pstm.execute();
                pstm.close();

                pstm = con.prepareStatement("DELETE FROM character_quests WHERE char_id IN (SELECT objid FROM characters WHERE char_name = ?)");
                pstm.setString(1, charName);
                pstm.execute();
                pstm.close();

                pstm = con.prepareStatement("DELETE FROM character_skills WHERE char_obj_id IN (SELECT objid FROM characters WHERE char_name = ?)");
                pstm.setString(1, charName);
                pstm.execute();
                pstm.close();

                pstm = con.prepareStatement("DELETE FROM character_teleport WHERE char_id IN (SELECT objid FROM characters WHERE char_name = ?)");
                pstm.setString(1, charName);
                pstm.execute();
                pstm.close();

                pstm = con.prepareStatement("DELETE FROM characters WHERE char_name=?");
                pstm.setString(1, charName);
                pstm.execute();
                pstm.close();

                pstm = con.prepareStatement("DELETE FROM clan_members WHERE char_id IN (SELECT objid FROM characters WHERE char_name = ?)");
                pstm.setString(1, charName);
                pstm.execute();
                pstm.close();
            }
            catch (SQLException e)
            {
                throw e;
            }
            finally
            {
                SQLUtil.close(rs);
                SQLUtil.close(pstm);
                SQLUtil.close(con);

            }
        }

        public virtual void storeCharacter(L1PcInstance pc)
        {
            IDataBaseConnection con = null;
            PreparedStatement pstm = null;
            try
            {
                int i = 0;
                con = L1DatabaseFactory.Instance.Connection;
                pstm = con.prepareStatement("UPDATE characters SET level=?,HighLevel=?,Exp=?,MaxHp=?,CurHp=?,MaxMp=?,CurMp=?,Ac=?,Str=?,Con=?,Dex=?,Cha=?,Intel=?,Wis=?,Status=?,Class=?,Sex=?,Type=?,Heading=?,LocX=?,LocY=?,MapID=?,Food=?,Lawful=?,Title=?,ClanID=?,Clanname=?,ClanRank=?,BonusStatus=?,ElixirStatus=?,ElfAttr=?,PKcount=?,PkCountForElf=?,ExpRes=?,PartnerID=?,AccessLevel=?,OnlineStatus=?,HomeTownID=?,Contribution=?,HellTime=?,Banned=?,Karma=?,LastPk=?,LastPkForElf=?,DeleteTime=?,LastActive=?,AinZone=?,AinPoint=? WHERE objid=?");
                pstm.setInt(++i, pc.Level);
                pstm.setInt(++i, pc.HighLevel);
                pstm.setLong(++i, pc.Exp);
                pstm.setInt(++i, pc.BaseMaxHp);
                int hp = pc.CurrentHp;
                if (hp < 1)
                {
                    hp = 1;
                }
                pstm.setInt(++i, hp);
                pstm.setInt(++i, pc.BaseMaxMp);
                pstm.setInt(++i, pc.CurrentMp);
                pstm.setInt(++i, pc.Ac);
                pstm.setInt(++i, pc.BaseStr);
                pstm.setInt(++i, pc.BaseCon);
                pstm.setInt(++i, pc.BaseDex);
                pstm.setInt(++i, pc.BaseCha);
                pstm.setInt(++i, pc.BaseInt);
                pstm.setInt(++i, pc.BaseWis);
                pstm.setInt(++i, pc.CurrentWeapon);
                pstm.setInt(++i, pc.ClassId);
                pstm.setInt(++i, pc.get_sex());
                pstm.setInt(++i, pc.Type);
                pstm.setInt(++i, pc.Heading);
                pstm.setInt(++i, pc.X);
                pstm.setInt(++i, pc.Y);
                pstm.setInt(++i, pc.MapId);
                pstm.setInt(++i, pc.get_food());
                pstm.setInt(++i, pc.Lawful);
                pstm.setString(++i, pc.Title);
                pstm.setInt(++i, pc.Clanid);
                pstm.setString(++i, pc.Clanname);
                pstm.setInt(++i, pc.ClanRank);
                pstm.setInt(++i, pc.BonusStats);
                pstm.setInt(++i, pc.ElixirStats);
                pstm.setInt(++i, pc.ElfAttr);
                pstm.setInt(++i, pc.get_PKcount());
                pstm.setInt(++i, pc.PkCountForElf);
                pstm.setInt(++i, pc.ExpRes);
                pstm.setInt(++i, pc.PartnerId);
                pstm.setShort(++i, pc.AccessLevel);
                pstm.setInt(++i, pc.OnlineStatus);
                pstm.setInt(++i, pc.HomeTownId);
                pstm.setInt(++i, pc.Contribution);
                pstm.setInt(++i, pc.HellTime);
                pstm.setBoolean(++i, pc.Banned);
                pstm.setInt(++i, pc.Karma);
                pstm.setTimestamp(++i, pc.LastPk);
                pstm.setTimestamp(++i, pc.LastPkForElf);
                pstm.setTimestamp(++i, pc.DeleteTime);
                pstm.setTimestamp(++i, new Timestamp(DateTimeHelper.CurrentUnixTimeMillis())); // TODO 角色登出時間殷海薩的祝福
                pstm.setInt(++i, pc.AinZone); // TODO 殷海薩的祝福
                pstm.setInt(++i, pc.AinPoint); // TODO 殷海薩的祝福
                pstm.setInt(++i, pc.Id);
                pstm.execute();
                pstm.close();

                _log.finest("stored char data:" + pc.Name);
            }
            catch (SQLException e)
            {
                _log.log(Enum.Level.Server, e.Message, e);
            }
            finally
            {
                SQLUtil.close(pstm);
                SQLUtil.close(con);
            }
        }

    }

}