using System.Data;
using LineageServer.Enum;
namespace LineageServer.DataBase.DataSources
{
    class Npc : DataSource
    {
        public const string TableName = "npc";
        public const string Column_name = "name";
        public const string Column_nameid = "nameid";
        public const string Column_note = "note";
        public const string Column_impl = "impl";
        public const string Column_size = "size";
        public const string Column_family = "family";
        public const string Column_npcid = "npcid";
        public const string Column_gfxid = "gfxid";
        public const string Column_lvl = "lvl";
        public const string Column_hp = "hp";
        public const string Column_mp = "mp";
        public const string Column_ac = "ac";
        public const string Column_str = "str";
        public const string Column_con = "con";
        public const string Column_dex = "dex";
        public const string Column_wis = "wis";
        public const string Column_intel = "intel";
        public const string Column_mr = "mr";
        public const string Column_exp = "exp";
        public const string Column_lawful = "lawful";
        public const string Column_weakAttr = "weakAttr";
        public const string Column_ranged = "ranged";
        public const string Column_passispeed = "passispeed";
        public const string Column_atkspeed = "atkspeed";
        public const string Column_alt_atk_speed = "alt_atk_speed";
        public const string Column_atk_magic_speed = "atk_magic_speed";
        public const string Column_sub_magic_speed = "sub_magic_speed";
        public const string Column_undead = "undead";
        public const string Column_poison_atk = "poison_atk";
        public const string Column_paralysis_atk = "paralysis_atk";
        public const string Column_agrofamily = "agrofamily";
        public const string Column_agrogfxid1 = "agrogfxid1";
        public const string Column_agrogfxid2 = "agrogfxid2";
        public const string Column_digestitem = "digestitem";
        public const string Column_hprinterval = "hprinterval";
        public const string Column_hpr = "hpr";
        public const string Column_mprinterval = "mprinterval";
        public const string Column_mpr = "mpr";
        public const string Column_randomlevel = "randomlevel";
        public const string Column_randomhp = "randomhp";
        public const string Column_randommp = "randommp";
        public const string Column_randomac = "randomac";
        public const string Column_randomexp = "randomexp";
        public const string Column_randomlawful = "randomlawful";
        public const string Column_damage_reduction = "damage_reduction";
        public const string Column_bowActId = "bowActId";
        public const string Column_karma = "karma";
        public const string Column_transform_id = "transform_id";
        public const string Column_transform_gfxid = "transform_gfxid";
        public const string Column_change_head = "change_head";
        public const string Column_hascastle = "hascastle";
        public const string Column_spawnlist_door = "spawnlist_door";
        public const string Column_count_map = "count_map";
        public const string Column_tamable = "tamable";
        public const string Column_agro = "agro";
        public const string Column_agrososc = "agrososc";
        public const string Column_agrocoi = "agrocoi";
        public const string Column_picupitem = "picupitem";
        public const string Column_bravespeed = "bravespeed";
        public const string Column_teleport = "teleport";
        public const string Column_hard = "hard";
        public const string Column_doppel = "doppel";
        public const string Column_IsTU = "IsTU";
        public const string Column_IsErase = "IsErase";
        public const string Column_light_size = "light_size";
        public const string Column_amount_fixed = "amount_fixed";
        public const string Column_cant_resurrect = "cant_resurrect";
        public override DataSourceTypeEnum DataSourceType { get { return DataSourceTypeEnum.Npc; } }
        protected override ColumnInfo[] ColumnInfos { get { return columnInfos; } }
        private static readonly ColumnInfo[] columnInfos = new ColumnInfo[]
        {
            new ColumnInfo() { Column = Column_name, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_nameid, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_note, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_impl, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_size, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_family, DbType = DbType.String, IsPKey = false},
            new ColumnInfo() { Column = Column_npcid, DbType = DbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_gfxid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_lvl, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ac, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_str, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_con, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_dex, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_wis, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_intel, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mr, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_exp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_lawful, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_weakAttr, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ranged, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_passispeed, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_atkspeed, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_alt_atk_speed, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_atk_magic_speed, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_sub_magic_speed, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_undead, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_poison_atk, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_paralysis_atk, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_agrofamily, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_agrogfxid1, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_agrogfxid2, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_digestitem, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hprinterval, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hpr, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mprinterval, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mpr, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_randomlevel, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_randomhp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_randommp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_randomac, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_randomexp, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_randomlawful, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_damage_reduction, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_bowActId, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_karma, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_transform_id, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_transform_gfxid, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_change_head, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hascastle, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_spawnlist_door, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_count_map, DbType = DbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_tamable, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_agro, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_agrososc, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_agrocoi, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_picupitem, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_bravespeed, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_teleport, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_hard, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_doppel, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_IsTU, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_IsErase, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_light_size, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_amount_fixed, DbType = DbType.Boolean, IsPKey = false},
            new ColumnInfo() { Column = Column_cant_resurrect, DbType = DbType.Boolean, IsPKey = false},
        };
        public Npc() : base(TableName)
        {
            
        }
    }
}
