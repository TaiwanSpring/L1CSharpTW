using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

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
            new ColumnInfo() { Column = Column_account_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_char_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_Title, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_Clanname, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_birthday, MySqlDbType = MySqlDbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_LastPk, MySqlDbType = MySqlDbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_LastPkForElf, MySqlDbType = MySqlDbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_DeleteTime, MySqlDbType = MySqlDbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_LastActive, MySqlDbType = MySqlDbType.DateTime, IsPKey = false},
            new ColumnInfo() { Column = Column_objid, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_level, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_HighLevel, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Exp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_MaxHp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_CurHp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_MaxMp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_CurMp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Ac, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Str, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Con, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Dex, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Cha, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Intel, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Wis, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Status, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Class, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Sex, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Type, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Heading, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_LocX, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_LocY, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_MapID, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Food, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Lawful, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ClanID, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ClanRank, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_BonusStatus, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ElixirStatus, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ElfAttr, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_PKcount, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_PkCountForElf, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ExpRes, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_PartnerID, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_AccessLevel, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_OnlineStatus, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_HomeTownID, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Contribution, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Pay, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_HellTime, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Karma, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_OriginalStr, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_OriginalCon, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_OriginalDex, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_OriginalCha, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_OriginalInt, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_OriginalWis, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_AinZone, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_AinPoint, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_Banned, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
        };
        public Characters() : base(TableName)
        {
            
        }
    }
}
