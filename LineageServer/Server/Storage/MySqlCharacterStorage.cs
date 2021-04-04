using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Server.Model.Instance;
using System;
namespace LineageServer.Server.Storage
{
    class MySqlCharacterStorage : ICharacterStorage
    {
        private readonly static IDataSource dataSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.Characters);
        public virtual L1PcInstance loadCharacter(string charName)
        {
            L1PcInstance pc = null;
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Select()
            .Where(Characters.Column_char_name, charName)
            .Execute();

            IDataSource clanMembersSource =
            Container.Instance.Resolve<IDataSourceFactory>()
            .Factory(Enum.DataSourceTypeEnum.ClanMembers);

            if (dataSourceRow.HaveData)
            {
                pc = new L1PcInstance();
                pc.AccountName = dataSourceRow.getString(Characters.Column_account_name);
                pc.Id = dataSourceRow.getInt(Characters.Column_objid);
                pc.Name = dataSourceRow.getString(Characters.Column_char_name);
                pc.Birthday = dataSourceRow.getTimestamp(Characters.Column_birthday);
                pc.HighLevel = dataSourceRow.getInt(Characters.Column_HighLevel);
                pc.Exp = dataSourceRow.getLong(Characters.Column_Exp);
                pc.addBaseMaxHp(dataSourceRow.getShort(Characters.Column_MaxHp));
                short currentHp = dataSourceRow.getShort(Characters.Column_CurHp);
                if (currentHp < 1)
                {
                    currentHp = 1;
                }
                pc.CurrentHpDirect = currentHp;
                pc.Dead = false;
                pc.Status = 0;
                pc.addBaseMaxMp(dataSourceRow.getShort(Characters.Column_MaxMp));
                pc.CurrentMpDirect = dataSourceRow.getShort(Characters.Column_CurMp);
                pc.addBaseStr(dataSourceRow.getByte(Characters.Column_Str));
                pc.addBaseCon(dataSourceRow.getByte(Characters.Column_Con));
                pc.addBaseDex(dataSourceRow.getByte(Characters.Column_Dex));
                pc.addBaseCha(dataSourceRow.getByte(Characters.Column_Cha));
                pc.addBaseInt(dataSourceRow.getByte(Characters.Column_Intel));
                pc.addBaseWis(dataSourceRow.getByte(Characters.Column_Wis));
                int status = dataSourceRow.getInt(Characters.Column_Status);
                pc.CurrentWeapon = status;
                int classId = dataSourceRow.getInt(Characters.Column_Class);
                pc.ClassId = classId;
                pc.TempCharGfx = classId;
                pc.GfxId = classId;
                pc.set_sex(dataSourceRow.getInt(Characters.Column_Sex));
                pc.Type = dataSourceRow.getInt(Characters.Column_Type);
                int head = dataSourceRow.getInt(Characters.Column_Heading);
                if (head > 7)
                {
                    head = 0;
                }
                pc.Heading = head;
                pc.X = dataSourceRow.getInt(Characters.Column_LocX);
                pc.Y = dataSourceRow.getInt(Characters.Column_LocY);
                pc.MapId = dataSourceRow.getShort(Characters.Column_MapID);
                pc.set_food(dataSourceRow.getInt(Characters.Column_Food));
                pc.Lawful = dataSourceRow.getInt(Characters.Column_Lawful);
                pc.Title = dataSourceRow.getString(Characters.Column_Title);
                pc.Clanid = dataSourceRow.getInt(Characters.Column_ClanID);
                pc.Clanname = dataSourceRow.getString(Characters.Column_Clanname);
                pc.ClanRank = dataSourceRow.getInt(Characters.Column_ClanRank);
                pc.BonusStats = dataSourceRow.getInt(Characters.Column_BonusStatus);
                pc.ElixirStats = dataSourceRow.getInt(Characters.Column_ElixirStatus);
                pc.ElfAttr = dataSourceRow.getInt(Characters.Column_ElfAttr);
                pc.set_PKcount(dataSourceRow.getInt(Characters.Column_PKcount));
                pc.PkCountForElf = dataSourceRow.getInt(Characters.Column_PkCountForElf);
                pc.ExpRes = dataSourceRow.getInt(Characters.Column_ExpRes);
                pc.PartnerId = dataSourceRow.getInt(Characters.Column_PartnerID);
                pc.AccessLevel = dataSourceRow.getShort(Characters.Column_AccessLevel);

                if (pc.AccessLevel >= 200)
                {
                    pc.Gm = true;
                    pc.Monitor = false;
                }
                else if (pc.AccessLevel >= 100)
                {
                    pc.Gm = false;
                    pc.Monitor = true;
                }
                else
                {
                    pc.Gm = false;
                    pc.Monitor = false;
                }
                pc.OnlineStatus = dataSourceRow.getInt(Characters.Column_OnlineStatus);
                pc.HomeTownId = dataSourceRow.getInt(Characters.Column_HomeTownID);
                pc.Contribution = dataSourceRow.getInt(Characters.Column_Contribution);
                pc.Pay = dataSourceRow.getInt(Characters.Column_Pay); // 村莊福利金 此欄位由 HomeTownTimeController 處理 update
                pc.HellTime = dataSourceRow.getInt(Characters.Column_HellTime);
                pc.Banned = dataSourceRow.getBoolean(Characters.Column_Banned);
                pc.Karma = dataSourceRow.getInt(Characters.Column_Karma);
                pc.LastPk = dataSourceRow.getTimestamp(Characters.Column_LastPk);
                pc.LastPkForElf = dataSourceRow.getTimestamp(Characters.Column_LastPkForElf);
                pc.DeleteTime = dataSourceRow.getTimestamp(Characters.Column_DeleteTime);
                pc.OriginalStr = dataSourceRow.getInt(Characters.Column_OriginalStr);
                pc.OriginalCon = dataSourceRow.getInt(Characters.Column_OriginalCon);
                pc.OriginalDex = dataSourceRow.getInt(Characters.Column_OriginalDex);
                pc.OriginalCha = dataSourceRow.getInt(Characters.Column_OriginalCha);
                pc.OriginalInt = dataSourceRow.getInt(Characters.Column_OriginalInt);
                pc.OriginalWis = dataSourceRow.getInt(Characters.Column_OriginalWis);
                pc.LastActive = dataSourceRow.getTimestamp(Characters.Column_LastActive); // TODO 殷海薩的祝福
                pc.AinZone = dataSourceRow.getInt(Characters.Column_AinZone); // TODO 殷海薩的祝福
                pc.AinPoint = dataSourceRow.getInt(Characters.Column_AinPoint); // TODO 殷海薩的祝福                
                pc.refresh();
                pc.MoveSpeed = 0;
                pc.BraveSpeed = 0;
                pc.GmInvis = false;

                if (pc.Clanid > 0)
                {
                    IDataSourceRow clanMember = clanMembersSource.NewRow();
                    clanMember.Select()
                          .Where(ClanMembers.Column_char_id, pc.Id)
                          .Execute();
                    if (clanMember.HaveData)
                    {
                        pc.ClanMemberId = clanMember.getInt(ClanMembers.Column_index_id);
                        pc.ClanMemberNotes = clanMember.getString(ClanMembers.Column_notes);
                    }
                }
                return pc;
            }
            else
            {
                return null;
            }
        }

        public virtual void createCharacter(L1PcInstance pc)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Insert()
            .Set(Characters.Column_account_name, pc.AccountName)
            .Set(Characters.Column_objid, pc.Id)
            .Set(Characters.Column_char_name, pc.Name)
            .Set(Characters.Column_birthday, pc.Birthday)
            .Set(Characters.Column_level, pc.Level)
            .Set(Characters.Column_HighLevel, pc.HighLevel)
            .Set(Characters.Column_Exp, pc.Exp)
            .Set(Characters.Column_MaxHp, pc.MaxHp)
            .Set(Characters.Column_CurHp, pc.CurrentHp)
            .Set(Characters.Column_MaxMp, pc.BaseMaxMp)
            .Set(Characters.Column_CurMp, pc.CurrentMp)
            .Set(Characters.Column_Ac, pc.Ac)
            .Set(Characters.Column_Str, pc.BaseStr)
            .Set(Characters.Column_Con, pc.BaseCon)
            .Set(Characters.Column_Dex, pc.BaseDex)
            .Set(Characters.Column_Cha, pc.BaseCha)
            .Set(Characters.Column_Intel, pc.getInt())
            .Set(Characters.Column_Wis, pc.BaseWis)
            .Set(Characters.Column_Status, pc.Status)
            .Set(Characters.Column_Class, pc.ClassId)
            .Set(Characters.Column_Sex, pc.get_sex())
            .Set(Characters.Column_Type, pc.Type)
            .Set(Characters.Column_Heading, pc.Heading)
            .Set(Characters.Column_LocX, pc.X)
            .Set(Characters.Column_LocY, pc.Y)
            .Set(Characters.Column_MapID, pc.MapId)
            .Set(Characters.Column_Food, pc.get_food())
            .Set(Characters.Column_Lawful, pc.Lawful)
            .Set(Characters.Column_Title, pc.Title)
            .Set(Characters.Column_ClanID, pc.Clanid)
            .Set(Characters.Column_Clanname, pc.Clanname)
            .Set(Characters.Column_ClanRank, pc.ClanRank)
            .Set(Characters.Column_BonusStatus, pc.BonusStats)
            .Set(Characters.Column_ElixirStatus, pc.ElixirStats)
            .Set(Characters.Column_ElfAttr, pc.ElfAttr)
            .Set(Characters.Column_PKcount, pc.get_PKcount())
            .Set(Characters.Column_PkCountForElf, pc.PkCountForElf)
            .Set(Characters.Column_ExpRes, pc.ExpRes)
            .Set(Characters.Column_PartnerID, pc.PartnerId)
            .Set(Characters.Column_AccessLevel, pc.AccessLevel)
            .Set(Characters.Column_OnlineStatus, pc.OnlineStatus)
            .Set(Characters.Column_HomeTownID, pc.HomeTownId)
            .Set(Characters.Column_Contribution, pc.Contribution)
            .Set(Characters.Column_Pay, pc.Pay)
            .Set(Characters.Column_HellTime, pc.HellTime)
            .Set(Characters.Column_Banned, pc.Banned)
            .Set(Characters.Column_Karma, pc.Karma)
            .Set(Characters.Column_LastPk, pc.LastPk)
            .Set(Characters.Column_LastPkForElf, pc.LastPkForElf)
            .Set(Characters.Column_DeleteTime, pc.DeleteTime)
            .Set(Characters.Column_OriginalStr, pc.OriginalStr)
            .Set(Characters.Column_OriginalCon, pc.OriginalCon)
            .Set(Characters.Column_OriginalDex, pc.OriginalDex)
            .Set(Characters.Column_OriginalCha, pc.OriginalCha)
            .Set(Characters.Column_OriginalInt, pc.OriginalInt)
            .Set(Characters.Column_OriginalWis, pc.OriginalWis)
            .Set(Characters.Column_LastActive, pc.LastActive)
            .Set(Characters.Column_AinZone, pc.AinZone)
            .Set(Characters.Column_AinPoint, pc.AinPoint)
            .Execute();
        }
        public virtual void deleteCharacter(string accountName, string charName)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Select()
            .Where(Characters.Column_account_name, accountName)
            .Where(Characters.Column_char_name, charName)
            .Execute();
            if (dataSourceRow.HaveData)
            {
                int charId = dataSourceRow.getInt(Characters.Column_objid);
                Container.Instance.Resolve<IDataSourceFactory>()
                    .Factory(Enum.DataSourceTypeEnum.CharacterBuddys).NewRow()
                    .Delete()
                    .Where(CharacterBuddys.Column_char_id, charId)
                    .Execute();

                Container.Instance.Resolve<IDataSourceFactory>()
                  .Factory(Enum.DataSourceTypeEnum.CharacterBuff).NewRow()
                  .Delete()
                  .Where(CharacterBuff.Column_char_obj_id, charId)
                  .Execute();

                Container.Instance.Resolve<IDataSourceFactory>()
                  .Factory(Enum.DataSourceTypeEnum.CharacterConfig).NewRow()
                  .Delete()
                  .Where(CharacterConfig.Column_object_id, charId)
                  .Execute();

                Container.Instance.Resolve<IDataSourceFactory>()
                .Factory(Enum.DataSourceTypeEnum.CharacterItems).NewRow()
                .Delete()
                .Where(CharacterItems.Column_char_id, charId)
                .Execute();

                Container.Instance.Resolve<IDataSourceFactory>()
               .Factory(Enum.DataSourceTypeEnum.CharacterQuests).NewRow()
               .Delete()
               .Where(CharacterQuests.Column_char_id, charId)
               .Execute();

                Container.Instance.Resolve<IDataSourceFactory>()
               .Factory(Enum.DataSourceTypeEnum.CharacterSkills).NewRow()
               .Delete()
               .Where(CharacterSkills.Column_char_obj_id, charId)
               .Execute();

                Container.Instance.Resolve<IDataSourceFactory>()
                    .Factory(Enum.DataSourceTypeEnum.CharacterTeleport).NewRow()
                    .Delete()
                    .Where(CharacterTeleport.Column_char_id, charId)
                    .Execute();

                Container.Instance.Resolve<IDataSourceFactory>()
                    .Factory(Enum.DataSourceTypeEnum.ClanMembers).NewRow()
                    .Delete()
                    .Where(ClanMembers.Column_char_id, charId)
                    .Execute();

                Container.Instance.Resolve<IDataSourceFactory>()
                    .Factory(Enum.DataSourceTypeEnum.Characters).NewRow()
                    .Delete()
                    .Where(Characters.Column_objid, charId)
                    .Execute();
            }
        }
        public virtual void storeCharacter(L1PcInstance pc)
        {
            IDataSourceRow dataSourceRow = dataSource.NewRow();
            dataSourceRow.Update()
            .Where(Characters.Column_objid, pc.Id)
            //.Set(Characters.Column_char_name, pc.Name)
            //.Set(Characters.Column_birthday, pc.Birthday)
            .Set(Characters.Column_level, pc.Level)
            .Set(Characters.Column_HighLevel, pc.HighLevel)
            .Set(Characters.Column_Exp, pc.Exp)
            .Set(Characters.Column_MaxHp, pc.MaxHp)
            .Set(Characters.Column_CurHp, pc.CurrentHp)
            .Set(Characters.Column_MaxMp, pc.BaseMaxMp)
            .Set(Characters.Column_CurMp, pc.CurrentMp)
            .Set(Characters.Column_Ac, pc.Ac)
            .Set(Characters.Column_Str, pc.BaseStr)
            .Set(Characters.Column_Con, pc.BaseCon)
            .Set(Characters.Column_Dex, pc.BaseDex)
            .Set(Characters.Column_Cha, pc.BaseCha)
            .Set(Characters.Column_Intel, pc.getInt())
            .Set(Characters.Column_Wis, pc.BaseWis)
            .Set(Characters.Column_Status, pc.Status)
            .Set(Characters.Column_Class, pc.ClassId)
            .Set(Characters.Column_Sex, pc.get_sex())
            .Set(Characters.Column_Type, pc.Type)
            .Set(Characters.Column_Heading, pc.Heading)
            .Set(Characters.Column_LocX, pc.X)
            .Set(Characters.Column_LocY, pc.Y)
            .Set(Characters.Column_MapID, pc.MapId)
            .Set(Characters.Column_Food, pc.get_food())
            .Set(Characters.Column_Lawful, pc.Lawful)
            .Set(Characters.Column_Title, pc.Title)
            .Set(Characters.Column_ClanID, pc.Clanid)
            .Set(Characters.Column_Clanname, pc.Clanname)
            .Set(Characters.Column_ClanRank, pc.ClanRank)
            .Set(Characters.Column_BonusStatus, pc.BonusStats)
            .Set(Characters.Column_ElixirStatus, pc.ElixirStats)
            .Set(Characters.Column_ElfAttr, pc.ElfAttr)
            .Set(Characters.Column_PKcount, pc.get_PKcount())
            .Set(Characters.Column_PkCountForElf, pc.PkCountForElf)
            .Set(Characters.Column_ExpRes, pc.ExpRes)
            .Set(Characters.Column_PartnerID, pc.PartnerId)
            .Set(Characters.Column_AccessLevel, pc.AccessLevel)
            .Set(Characters.Column_OnlineStatus, pc.OnlineStatus)
            .Set(Characters.Column_HomeTownID, pc.HomeTownId)
            .Set(Characters.Column_Contribution, pc.Contribution)
            .Set(Characters.Column_Pay, pc.Pay)
            .Set(Characters.Column_HellTime, pc.HellTime)
            .Set(Characters.Column_Banned, pc.Banned)
            .Set(Characters.Column_Karma, pc.Karma)
            .Set(Characters.Column_LastPk, pc.LastPk)
            .Set(Characters.Column_LastPkForElf, pc.LastPkForElf)
            .Set(Characters.Column_DeleteTime, pc.DeleteTime)
            //.Set(Characters.Column_OriginalStr, pc.OriginalStr)
            // .Set(Characters.Column_OriginalCon, pc.OriginalCon)
            //.Set(Characters.Column_OriginalDex, pc.OriginalDex)
            //.Set(Characters.Column_OriginalCha, pc.OriginalCha)
            //.Set(Characters.Column_OriginalInt, pc.OriginalInt)
            // .Set(Characters.Column_OriginalWis, pc.OriginalWis)
            .Set(Characters.Column_LastActive, pc.LastActive)
            .Set(Characters.Column_AinZone, pc.AinZone)
            .Set(Characters.Column_AinPoint, pc.AinPoint)
            .Execute();
        }

    }

}