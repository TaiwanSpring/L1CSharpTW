using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class Characters : DataSource
    {
        public const string TableName = "characters";
        public const string Column_account_name = "account_name";
        public const string Column_char_name = "char_name";
        public const string Column_Title = "Title";
        public const string Column_Clanname = "Clanname";
        public const string Column_birthday = "birthday";
        public const string Column_LastPk = "LastPk";
        public const string Column_LastPkForElf = "LastPkForElf";
        public const string Column_DeleteTime = "DeleteTime";
        public const string Column_LastActive = "LastActive";
        public const string Column_objid = "objid";
        public const string Column_level = "level";
        public const string Column_HighLevel = "HighLevel";
        public const string Column_Exp = "Exp";
        public const string Column_MaxHp = "MaxHp";
        public const string Column_CurHp = "CurHp";
        public const string Column_MaxMp = "MaxMp";
        public const string Column_CurMp = "CurMp";
        public const string Column_Ac = "Ac";
        public const string Column_Str = "Str";
        public const string Column_Con = "Con";
        public const string Column_Dex = "Dex";
        public const string Column_Cha = "Cha";
        public const string Column_Intel = "Intel";
        public const string Column_Wis = "Wis";
        public const string Column_Status = "Status";
        public const string Column_Class = "Class";
        public const string Column_Sex = "Sex";
        public const string Column_Type = "Type";
        public const string Column_Heading = "Heading";
        public const string Column_LocX = "LocX";
        public const string Column_LocY = "LocY";
        public const string Column_MapID = "MapID";
        public const string Column_Food = "Food";
        public const string Column_Lawful = "Lawful";
        public const string Column_ClanID = "ClanID";
        public const string Column_ClanRank = "ClanRank";
        public const string Column_BonusStatus = "BonusStatus";
        public const string Column_ElixirStatus = "ElixirStatus";
        public const string Column_ElfAttr = "ElfAttr";
        public const string Column_PKcount = "PKcount";
        public const string Column_PkCountForElf = "PkCountForElf";
        public const string Column_ExpRes = "ExpRes";
        public const string Column_PartnerID = "PartnerID";
        public const string Column_AccessLevel = "AccessLevel";
        public const string Column_OnlineStatus = "OnlineStatus";
        public const string Column_HomeTownID = "HomeTownID";
        public const string Column_Contribution = "Contribution";
        public const string Column_Pay = "Pay";
        public const string Column_HellTime = "HellTime";
        public const string Column_Karma = "Karma";
        public const string Column_OriginalStr = "OriginalStr";
        public const string Column_OriginalCon = "OriginalCon";
        public const string Column_OriginalDex = "OriginalDex";
        public const string Column_OriginalCha = "OriginalCha";
        public const string Column_OriginalInt = "OriginalInt";
        public const string Column_OriginalWis = "OriginalWis";
        public const string Column_AinZone = "AinZone";
        public const string Column_AinPoint = "AinPoint";
        public const string Column_Banned = "Banned";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Characters; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_account_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_char_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_Title, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_Clanname, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_birthday, DbType = DbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_LastPk, DbType = DbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_LastPkForElf, DbType = DbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_DeleteTime, DbType = DbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_LastActive, DbType = DbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_objid, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_level, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_HighLevel, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Exp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_MaxHp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_CurHp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_MaxMp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_CurMp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Ac, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Str, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Con, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Dex, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Cha, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Intel, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Wis, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Status, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Class, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Sex, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Type, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Heading, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_LocX, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_LocY, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_MapID, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Food, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Lawful, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ClanID, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ClanRank, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_BonusStatus, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ElixirStatus, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ElfAttr, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_PKcount, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_PkCountForElf, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ExpRes, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_PartnerID, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_AccessLevel, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_OnlineStatus, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_HomeTownID, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Contribution, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Pay, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_HellTime, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Karma, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_OriginalStr, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_OriginalCon, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_OriginalDex, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_OriginalCha, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_OriginalInt, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_OriginalWis, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_AinZone, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_AinPoint, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Banned, DbType = DbType.Boolean, IsPKey = false},
        };
        public Characters() : base(TableName)
        {
            
        }
    }
}
