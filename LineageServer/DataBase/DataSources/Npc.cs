using System.Data;
using LineageServer.Enum;
using MySql.Data.MySqlClient;

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
            new ColumnInfo() { Column = Column_name, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_nameid, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_note, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_impl, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_size, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_family, MySqlDbType = MySqlDbType.Text, IsPKey = false},
            new ColumnInfo() { Column = Column_npcid, MySqlDbType = MySqlDbType.Int32, IsPKey = true},
            new ColumnInfo() { Column = Column_gfxid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_lvl, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ac, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_str, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_con, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_dex, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_wis, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_intel, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mr, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_exp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_lawful, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_weakAttr, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_ranged, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_passispeed, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_atkspeed, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_alt_atk_speed, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_atk_magic_speed, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_sub_magic_speed, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_undead, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_poison_atk, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_paralysis_atk, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_agrofamily, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_agrogfxid1, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_agrogfxid2, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_digestitem, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hprinterval, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hpr, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mprinterval, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_mpr, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_randomlevel, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_randomhp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_randommp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_randomac, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_randomexp, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_randomlawful, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_damage_reduction, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_bowActId, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_karma, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_transform_id, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_transform_gfxid, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_change_head, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_hascastle, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_spawnlist_door, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_count_map, MySqlDbType = MySqlDbType.Int32, IsPKey = false},
            new ColumnInfo() { Column = Column_tamable, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_agro, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_agrososc, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_agrocoi, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_picupitem, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_bravespeed, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_teleport, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_hard, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_doppel, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_IsTU, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_IsErase, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_light_size, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_amount_fixed, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
            new ColumnInfo() { Column = Column_cant_resurrect, MySqlDbType = MySqlDbType.Bit, IsPKey = false},
        };
        public Npc() : base(TableName)
        {
            
        }
    }
}
