private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Characters);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Characters.Column_objid, obj.Objid)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Characters.Column_account_name, obj.AccountName)
.Set(Characters.Column_objid, obj.Objid)
.Set(Characters.Column_char_name, obj.CharName)
.Set(Characters.Column_birthday, obj.Birthday)
.Set(Characters.Column_level, obj.Level)
.Set(Characters.Column_HighLevel, obj.HighLevel)
.Set(Characters.Column_Exp, obj.Exp)
.Set(Characters.Column_MaxHp, obj.MaxHp)
.Set(Characters.Column_CurHp, obj.CurHp)
.Set(Characters.Column_MaxMp, obj.MaxMp)
.Set(Characters.Column_CurMp, obj.CurMp)
.Set(Characters.Column_Ac, obj.Ac)
.Set(Characters.Column_Str, obj.Str)
.Set(Characters.Column_Con, obj.Con)
.Set(Characters.Column_Dex, obj.Dex)
.Set(Characters.Column_Cha, obj.Cha)
.Set(Characters.Column_Intel, obj.Intel)
.Set(Characters.Column_Wis, obj.Wis)
.Set(Characters.Column_Status, obj.Status)
.Set(Characters.Column_Class, obj.Class)
.Set(Characters.Column_Sex, obj.Sex)
.Set(Characters.Column_Type, obj.Type)
.Set(Characters.Column_Heading, obj.Heading)
.Set(Characters.Column_LocX, obj.LocX)
.Set(Characters.Column_LocY, obj.LocY)
.Set(Characters.Column_MapID, obj.MapID)
.Set(Characters.Column_Food, obj.Food)
.Set(Characters.Column_Lawful, obj.Lawful)
.Set(Characters.Column_Title, obj.Title)
.Set(Characters.Column_ClanID, obj.ClanID)
.Set(Characters.Column_Clanname, obj.Clanname)
.Set(Characters.Column_ClanRank, obj.ClanRank)
.Set(Characters.Column_BonusStatus, obj.BonusStatus)
.Set(Characters.Column_ElixirStatus, obj.ElixirStatus)
.Set(Characters.Column_ElfAttr, obj.ElfAttr)
.Set(Characters.Column_PKcount, obj.PKcount)
.Set(Characters.Column_PkCountForElf, obj.PkCountForElf)
.Set(Characters.Column_ExpRes, obj.ExpRes)
.Set(Characters.Column_PartnerID, obj.PartnerID)
.Set(Characters.Column_AccessLevel, obj.AccessLevel)
.Set(Characters.Column_OnlineStatus, obj.OnlineStatus)
.Set(Characters.Column_HomeTownID, obj.HomeTownID)
.Set(Characters.Column_Contribution, obj.Contribution)
.Set(Characters.Column_Pay, obj.Pay)
.Set(Characters.Column_HellTime, obj.HellTime)
.Set(Characters.Column_Banned, obj.Banned)
.Set(Characters.Column_Karma, obj.Karma)
.Set(Characters.Column_LastPk, obj.LastPk)
.Set(Characters.Column_LastPkForElf, obj.LastPkForElf)
.Set(Characters.Column_DeleteTime, obj.DeleteTime)
.Set(Characters.Column_OriginalStr, obj.OriginalStr)
.Set(Characters.Column_OriginalCon, obj.OriginalCon)
.Set(Characters.Column_OriginalDex, obj.OriginalDex)
.Set(Characters.Column_OriginalCha, obj.OriginalCha)
.Set(Characters.Column_OriginalInt, obj.OriginalInt)
.Set(Characters.Column_OriginalWis, obj.OriginalWis)
.Set(Characters.Column_LastActive, obj.LastActive)
.Set(Characters.Column_AinZone, obj.AinZone)
.Set(Characters.Column_AinPoint, obj.AinPoint)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Set(Characters.Column_account_name, obj.AccountName)
.Where(Characters.Column_objid, obj.Objid)
.Set(Characters.Column_char_name, obj.CharName)
.Set(Characters.Column_birthday, obj.Birthday)
.Set(Characters.Column_level, obj.Level)
.Set(Characters.Column_HighLevel, obj.HighLevel)
.Set(Characters.Column_Exp, obj.Exp)
.Set(Characters.Column_MaxHp, obj.MaxHp)
.Set(Characters.Column_CurHp, obj.CurHp)
.Set(Characters.Column_MaxMp, obj.MaxMp)
.Set(Characters.Column_CurMp, obj.CurMp)
.Set(Characters.Column_Ac, obj.Ac)
.Set(Characters.Column_Str, obj.Str)
.Set(Characters.Column_Con, obj.Con)
.Set(Characters.Column_Dex, obj.Dex)
.Set(Characters.Column_Cha, obj.Cha)
.Set(Characters.Column_Intel, obj.Intel)
.Set(Characters.Column_Wis, obj.Wis)
.Set(Characters.Column_Status, obj.Status)
.Set(Characters.Column_Class, obj.Class)
.Set(Characters.Column_Sex, obj.Sex)
.Set(Characters.Column_Type, obj.Type)
.Set(Characters.Column_Heading, obj.Heading)
.Set(Characters.Column_LocX, obj.LocX)
.Set(Characters.Column_LocY, obj.LocY)
.Set(Characters.Column_MapID, obj.MapID)
.Set(Characters.Column_Food, obj.Food)
.Set(Characters.Column_Lawful, obj.Lawful)
.Set(Characters.Column_Title, obj.Title)
.Set(Characters.Column_ClanID, obj.ClanID)
.Set(Characters.Column_Clanname, obj.Clanname)
.Set(Characters.Column_ClanRank, obj.ClanRank)
.Set(Characters.Column_BonusStatus, obj.BonusStatus)
.Set(Characters.Column_ElixirStatus, obj.ElixirStatus)
.Set(Characters.Column_ElfAttr, obj.ElfAttr)
.Set(Characters.Column_PKcount, obj.PKcount)
.Set(Characters.Column_PkCountForElf, obj.PkCountForElf)
.Set(Characters.Column_ExpRes, obj.ExpRes)
.Set(Characters.Column_PartnerID, obj.PartnerID)
.Set(Characters.Column_AccessLevel, obj.AccessLevel)
.Set(Characters.Column_OnlineStatus, obj.OnlineStatus)
.Set(Characters.Column_HomeTownID, obj.HomeTownID)
.Set(Characters.Column_Contribution, obj.Contribution)
.Set(Characters.Column_Pay, obj.Pay)
.Set(Characters.Column_HellTime, obj.HellTime)
.Set(Characters.Column_Banned, obj.Banned)
.Set(Characters.Column_Karma, obj.Karma)
.Set(Characters.Column_LastPk, obj.LastPk)
.Set(Characters.Column_LastPkForElf, obj.LastPkForElf)
.Set(Characters.Column_DeleteTime, obj.DeleteTime)
.Set(Characters.Column_OriginalStr, obj.OriginalStr)
.Set(Characters.Column_OriginalCon, obj.OriginalCon)
.Set(Characters.Column_OriginalDex, obj.OriginalDex)
.Set(Characters.Column_OriginalCha, obj.OriginalCha)
.Set(Characters.Column_OriginalInt, obj.OriginalInt)
.Set(Characters.Column_OriginalWis, obj.OriginalWis)
.Set(Characters.Column_LastActive, obj.LastActive)
.Set(Characters.Column_AinZone, obj.AinZone)
.Set(Characters.Column_AinPoint, obj.AinPoint)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Characters.Column_objid, obj.Objid)
.Execute();


obj.AccountName = dataSourceRow.getString(Characters.Column_account_name);
obj.Objid = dataSourceRow.getString(Characters.Column_objid);
obj.CharName = dataSourceRow.getString(Characters.Column_char_name);
obj.Birthday = dataSourceRow.getString(Characters.Column_birthday);
obj.Level = dataSourceRow.getString(Characters.Column_level);
obj.HighLevel = dataSourceRow.getString(Characters.Column_HighLevel);
obj.Exp = dataSourceRow.getString(Characters.Column_Exp);
obj.MaxHp = dataSourceRow.getString(Characters.Column_MaxHp);
obj.CurHp = dataSourceRow.getString(Characters.Column_CurHp);
obj.MaxMp = dataSourceRow.getString(Characters.Column_MaxMp);
obj.CurMp = dataSourceRow.getString(Characters.Column_CurMp);
obj.Ac = dataSourceRow.getString(Characters.Column_Ac);
obj.Str = dataSourceRow.getString(Characters.Column_Str);
obj.Con = dataSourceRow.getString(Characters.Column_Con);
obj.Dex = dataSourceRow.getString(Characters.Column_Dex);
obj.Cha = dataSourceRow.getString(Characters.Column_Cha);
obj.Intel = dataSourceRow.getString(Characters.Column_Intel);
obj.Wis = dataSourceRow.getString(Characters.Column_Wis);
obj.Status = dataSourceRow.getString(Characters.Column_Status);
obj.Class = dataSourceRow.getString(Characters.Column_Class);
obj.Sex = dataSourceRow.getString(Characters.Column_Sex);
obj.Type = dataSourceRow.getString(Characters.Column_Type);
obj.Heading = dataSourceRow.getString(Characters.Column_Heading);
obj.LocX = dataSourceRow.getString(Characters.Column_LocX);
obj.LocY = dataSourceRow.getString(Characters.Column_LocY);
obj.MapID = dataSourceRow.getString(Characters.Column_MapID);
obj.Food = dataSourceRow.getString(Characters.Column_Food);
obj.Lawful = dataSourceRow.getString(Characters.Column_Lawful);
obj.Title = dataSourceRow.getString(Characters.Column_Title);
obj.ClanID = dataSourceRow.getString(Characters.Column_ClanID);
obj.Clanname = dataSourceRow.getString(Characters.Column_Clanname);
obj.ClanRank = dataSourceRow.getString(Characters.Column_ClanRank);
obj.BonusStatus = dataSourceRow.getString(Characters.Column_BonusStatus);
obj.ElixirStatus = dataSourceRow.getString(Characters.Column_ElixirStatus);
obj.ElfAttr = dataSourceRow.getString(Characters.Column_ElfAttr);
obj.PKcount = dataSourceRow.getString(Characters.Column_PKcount);
obj.PkCountForElf = dataSourceRow.getString(Characters.Column_PkCountForElf);
obj.ExpRes = dataSourceRow.getString(Characters.Column_ExpRes);
obj.PartnerID = dataSourceRow.getString(Characters.Column_PartnerID);
obj.AccessLevel = dataSourceRow.getString(Characters.Column_AccessLevel);
obj.OnlineStatus = dataSourceRow.getString(Characters.Column_OnlineStatus);
obj.HomeTownID = dataSourceRow.getString(Characters.Column_HomeTownID);
obj.Contribution = dataSourceRow.getString(Characters.Column_Contribution);
obj.Pay = dataSourceRow.getString(Characters.Column_Pay);
obj.HellTime = dataSourceRow.getString(Characters.Column_HellTime);
obj.Banned = dataSourceRow.getString(Characters.Column_Banned);
obj.Karma = dataSourceRow.getString(Characters.Column_Karma);
obj.LastPk = dataSourceRow.getString(Characters.Column_LastPk);
obj.LastPkForElf = dataSourceRow.getString(Characters.Column_LastPkForElf);
obj.DeleteTime = dataSourceRow.getString(Characters.Column_DeleteTime);
obj.OriginalStr = dataSourceRow.getString(Characters.Column_OriginalStr);
obj.OriginalCon = dataSourceRow.getString(Characters.Column_OriginalCon);
obj.OriginalDex = dataSourceRow.getString(Characters.Column_OriginalDex);
obj.OriginalCha = dataSourceRow.getString(Characters.Column_OriginalCha);
obj.OriginalInt = dataSourceRow.getString(Characters.Column_OriginalInt);
obj.OriginalWis = dataSourceRow.getString(Characters.Column_OriginalWis);
obj.LastActive = dataSourceRow.getString(Characters.Column_LastActive);
obj.AinZone = dataSourceRow.getString(Characters.Column_AinZone);
obj.AinPoint = dataSourceRow.getString(Characters.Column_AinPoint);

