private readonly static IDataSource dataSource = Container.Instance.Resolve<IDataSourceFactory>().Factory(Enum.DataSourceTypeEnum.Npc);
IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

for (int i = 0; i < dataSourceRows.Count; i++)
{
    IDataSourceRow dataSourceRow = dataSourceRows[i];
}
IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Select()
.Where(Npc.Column_npcid, obj.Npcid)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Insert()
.Set(Npc.Column_npcid, obj.Npcid)
.Set(Npc.Column_name, obj.Name)
.Set(Npc.Column_nameid, obj.Nameid)
.Set(Npc.Column_note, obj.Note)
.Set(Npc.Column_impl, obj.Impl)
.Set(Npc.Column_gfxid, obj.Gfxid)
.Set(Npc.Column_lvl, obj.Lvl)
.Set(Npc.Column_hp, obj.Hp)
.Set(Npc.Column_mp, obj.Mp)
.Set(Npc.Column_ac, obj.Ac)
.Set(Npc.Column_str, obj.Str)
.Set(Npc.Column_con, obj.Con)
.Set(Npc.Column_dex, obj.Dex)
.Set(Npc.Column_wis, obj.Wis)
.Set(Npc.Column_intel, obj.Intel)
.Set(Npc.Column_mr, obj.Mr)
.Set(Npc.Column_exp, obj.Exp)
.Set(Npc.Column_lawful, obj.Lawful)
.Set(Npc.Column_size, obj.Size)
.Set(Npc.Column_weakAttr, obj.WeakAttr)
.Set(Npc.Column_ranged, obj.Ranged)
.Set(Npc.Column_tamable, obj.Tamable)
.Set(Npc.Column_passispeed, obj.Passispeed)
.Set(Npc.Column_atkspeed, obj.Atkspeed)
.Set(Npc.Column_alt_atk_speed, obj.AltAtkSpeed)
.Set(Npc.Column_atk_magic_speed, obj.AtkMagicSpeed)
.Set(Npc.Column_sub_magic_speed, obj.SubMagicSpeed)
.Set(Npc.Column_undead, obj.Undead)
.Set(Npc.Column_poison_atk, obj.PoisonAtk)
.Set(Npc.Column_paralysis_atk, obj.ParalysisAtk)
.Set(Npc.Column_agro, obj.Agro)
.Set(Npc.Column_agrososc, obj.Agrososc)
.Set(Npc.Column_agrocoi, obj.Agrocoi)
.Set(Npc.Column_family, obj.Family)
.Set(Npc.Column_agrofamily, obj.Agrofamily)
.Set(Npc.Column_agrogfxid1, obj.Agrogfxid1)
.Set(Npc.Column_agrogfxid2, obj.Agrogfxid2)
.Set(Npc.Column_picupitem, obj.Picupitem)
.Set(Npc.Column_digestitem, obj.Digestitem)
.Set(Npc.Column_bravespeed, obj.Bravespeed)
.Set(Npc.Column_hprinterval, obj.Hprinterval)
.Set(Npc.Column_hpr, obj.Hpr)
.Set(Npc.Column_mprinterval, obj.Mprinterval)
.Set(Npc.Column_mpr, obj.Mpr)
.Set(Npc.Column_teleport, obj.Teleport)
.Set(Npc.Column_randomlevel, obj.Randomlevel)
.Set(Npc.Column_randomhp, obj.Randomhp)
.Set(Npc.Column_randommp, obj.Randommp)
.Set(Npc.Column_randomac, obj.Randomac)
.Set(Npc.Column_randomexp, obj.Randomexp)
.Set(Npc.Column_randomlawful, obj.Randomlawful)
.Set(Npc.Column_damage_reduction, obj.DamageReduction)
.Set(Npc.Column_hard, obj.Hard)
.Set(Npc.Column_doppel, obj.Doppel)
.Set(Npc.Column_IsTU, obj.IsTU)
.Set(Npc.Column_IsErase, obj.IsErase)
.Set(Npc.Column_bowActId, obj.BowActId)
.Set(Npc.Column_karma, obj.Karma)
.Set(Npc.Column_transform_id, obj.TransformId)
.Set(Npc.Column_transform_gfxid, obj.TransformGfxid)
.Set(Npc.Column_light_size, obj.LightSize)
.Set(Npc.Column_amount_fixed, obj.AmountFixed)
.Set(Npc.Column_change_head, obj.ChangeHead)
.Set(Npc.Column_cant_resurrect, obj.CantResurrect)
.Set(Npc.Column_hascastle, obj.Hascastle)
.Set(Npc.Column_spawnlist_door, obj.SpawnlistDoor)
.Set(Npc.Column_count_map, obj.CountMap)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Update()
.Where(Npc.Column_npcid, obj.Npcid)
.Set(Npc.Column_name, obj.Name)
.Set(Npc.Column_nameid, obj.Nameid)
.Set(Npc.Column_note, obj.Note)
.Set(Npc.Column_impl, obj.Impl)
.Set(Npc.Column_gfxid, obj.Gfxid)
.Set(Npc.Column_lvl, obj.Lvl)
.Set(Npc.Column_hp, obj.Hp)
.Set(Npc.Column_mp, obj.Mp)
.Set(Npc.Column_ac, obj.Ac)
.Set(Npc.Column_str, obj.Str)
.Set(Npc.Column_con, obj.Con)
.Set(Npc.Column_dex, obj.Dex)
.Set(Npc.Column_wis, obj.Wis)
.Set(Npc.Column_intel, obj.Intel)
.Set(Npc.Column_mr, obj.Mr)
.Set(Npc.Column_exp, obj.Exp)
.Set(Npc.Column_lawful, obj.Lawful)
.Set(Npc.Column_size, obj.Size)
.Set(Npc.Column_weakAttr, obj.WeakAttr)
.Set(Npc.Column_ranged, obj.Ranged)
.Set(Npc.Column_tamable, obj.Tamable)
.Set(Npc.Column_passispeed, obj.Passispeed)
.Set(Npc.Column_atkspeed, obj.Atkspeed)
.Set(Npc.Column_alt_atk_speed, obj.AltAtkSpeed)
.Set(Npc.Column_atk_magic_speed, obj.AtkMagicSpeed)
.Set(Npc.Column_sub_magic_speed, obj.SubMagicSpeed)
.Set(Npc.Column_undead, obj.Undead)
.Set(Npc.Column_poison_atk, obj.PoisonAtk)
.Set(Npc.Column_paralysis_atk, obj.ParalysisAtk)
.Set(Npc.Column_agro, obj.Agro)
.Set(Npc.Column_agrososc, obj.Agrososc)
.Set(Npc.Column_agrocoi, obj.Agrocoi)
.Set(Npc.Column_family, obj.Family)
.Set(Npc.Column_agrofamily, obj.Agrofamily)
.Set(Npc.Column_agrogfxid1, obj.Agrogfxid1)
.Set(Npc.Column_agrogfxid2, obj.Agrogfxid2)
.Set(Npc.Column_picupitem, obj.Picupitem)
.Set(Npc.Column_digestitem, obj.Digestitem)
.Set(Npc.Column_bravespeed, obj.Bravespeed)
.Set(Npc.Column_hprinterval, obj.Hprinterval)
.Set(Npc.Column_hpr, obj.Hpr)
.Set(Npc.Column_mprinterval, obj.Mprinterval)
.Set(Npc.Column_mpr, obj.Mpr)
.Set(Npc.Column_teleport, obj.Teleport)
.Set(Npc.Column_randomlevel, obj.Randomlevel)
.Set(Npc.Column_randomhp, obj.Randomhp)
.Set(Npc.Column_randommp, obj.Randommp)
.Set(Npc.Column_randomac, obj.Randomac)
.Set(Npc.Column_randomexp, obj.Randomexp)
.Set(Npc.Column_randomlawful, obj.Randomlawful)
.Set(Npc.Column_damage_reduction, obj.DamageReduction)
.Set(Npc.Column_hard, obj.Hard)
.Set(Npc.Column_doppel, obj.Doppel)
.Set(Npc.Column_IsTU, obj.IsTU)
.Set(Npc.Column_IsErase, obj.IsErase)
.Set(Npc.Column_bowActId, obj.BowActId)
.Set(Npc.Column_karma, obj.Karma)
.Set(Npc.Column_transform_id, obj.TransformId)
.Set(Npc.Column_transform_gfxid, obj.TransformGfxid)
.Set(Npc.Column_light_size, obj.LightSize)
.Set(Npc.Column_amount_fixed, obj.AmountFixed)
.Set(Npc.Column_change_head, obj.ChangeHead)
.Set(Npc.Column_cant_resurrect, obj.CantResurrect)
.Set(Npc.Column_hascastle, obj.Hascastle)
.Set(Npc.Column_spawnlist_door, obj.SpawnlistDoor)
.Set(Npc.Column_count_map, obj.CountMap)
.Execute();


IDataSourceRow dataSourceRow = dataSource.NewRow();
dataSourceRow.Delete()
.Where(Npc.Column_npcid, obj.Npcid)
.Execute();


obj.Npcid = dataSourceRow.getString(Npc.Column_npcid);
obj.Name = dataSourceRow.getString(Npc.Column_name);
obj.Nameid = dataSourceRow.getString(Npc.Column_nameid);
obj.Note = dataSourceRow.getString(Npc.Column_note);
obj.Impl = dataSourceRow.getString(Npc.Column_impl);
obj.Gfxid = dataSourceRow.getString(Npc.Column_gfxid);
obj.Lvl = dataSourceRow.getString(Npc.Column_lvl);
obj.Hp = dataSourceRow.getString(Npc.Column_hp);
obj.Mp = dataSourceRow.getString(Npc.Column_mp);
obj.Ac = dataSourceRow.getString(Npc.Column_ac);
obj.Str = dataSourceRow.getString(Npc.Column_str);
obj.Con = dataSourceRow.getString(Npc.Column_con);
obj.Dex = dataSourceRow.getString(Npc.Column_dex);
obj.Wis = dataSourceRow.getString(Npc.Column_wis);
obj.Intel = dataSourceRow.getString(Npc.Column_intel);
obj.Mr = dataSourceRow.getString(Npc.Column_mr);
obj.Exp = dataSourceRow.getString(Npc.Column_exp);
obj.Lawful = dataSourceRow.getString(Npc.Column_lawful);
obj.Size = dataSourceRow.getString(Npc.Column_size);
obj.WeakAttr = dataSourceRow.getString(Npc.Column_weakAttr);
obj.Ranged = dataSourceRow.getString(Npc.Column_ranged);
obj.Tamable = dataSourceRow.getString(Npc.Column_tamable);
obj.Passispeed = dataSourceRow.getString(Npc.Column_passispeed);
obj.Atkspeed = dataSourceRow.getString(Npc.Column_atkspeed);
obj.AltAtkSpeed = dataSourceRow.getString(Npc.Column_alt_atk_speed);
obj.AtkMagicSpeed = dataSourceRow.getString(Npc.Column_atk_magic_speed);
obj.SubMagicSpeed = dataSourceRow.getString(Npc.Column_sub_magic_speed);
obj.Undead = dataSourceRow.getString(Npc.Column_undead);
obj.PoisonAtk = dataSourceRow.getString(Npc.Column_poison_atk);
obj.ParalysisAtk = dataSourceRow.getString(Npc.Column_paralysis_atk);
obj.Agro = dataSourceRow.getString(Npc.Column_agro);
obj.Agrososc = dataSourceRow.getString(Npc.Column_agrososc);
obj.Agrocoi = dataSourceRow.getString(Npc.Column_agrocoi);
obj.Family = dataSourceRow.getString(Npc.Column_family);
obj.Agrofamily = dataSourceRow.getString(Npc.Column_agrofamily);
obj.Agrogfxid1 = dataSourceRow.getString(Npc.Column_agrogfxid1);
obj.Agrogfxid2 = dataSourceRow.getString(Npc.Column_agrogfxid2);
obj.Picupitem = dataSourceRow.getString(Npc.Column_picupitem);
obj.Digestitem = dataSourceRow.getString(Npc.Column_digestitem);
obj.Bravespeed = dataSourceRow.getString(Npc.Column_bravespeed);
obj.Hprinterval = dataSourceRow.getString(Npc.Column_hprinterval);
obj.Hpr = dataSourceRow.getString(Npc.Column_hpr);
obj.Mprinterval = dataSourceRow.getString(Npc.Column_mprinterval);
obj.Mpr = dataSourceRow.getString(Npc.Column_mpr);
obj.Teleport = dataSourceRow.getString(Npc.Column_teleport);
obj.Randomlevel = dataSourceRow.getString(Npc.Column_randomlevel);
obj.Randomhp = dataSourceRow.getString(Npc.Column_randomhp);
obj.Randommp = dataSourceRow.getString(Npc.Column_randommp);
obj.Randomac = dataSourceRow.getString(Npc.Column_randomac);
obj.Randomexp = dataSourceRow.getString(Npc.Column_randomexp);
obj.Randomlawful = dataSourceRow.getString(Npc.Column_randomlawful);
obj.DamageReduction = dataSourceRow.getString(Npc.Column_damage_reduction);
obj.Hard = dataSourceRow.getString(Npc.Column_hard);
obj.Doppel = dataSourceRow.getString(Npc.Column_doppel);
obj.IsTU = dataSourceRow.getString(Npc.Column_IsTU);
obj.IsErase = dataSourceRow.getString(Npc.Column_IsErase);
obj.BowActId = dataSourceRow.getString(Npc.Column_bowActId);
obj.Karma = dataSourceRow.getString(Npc.Column_karma);
obj.TransformId = dataSourceRow.getString(Npc.Column_transform_id);
obj.TransformGfxid = dataSourceRow.getString(Npc.Column_transform_gfxid);
obj.LightSize = dataSourceRow.getString(Npc.Column_light_size);
obj.AmountFixed = dataSourceRow.getString(Npc.Column_amount_fixed);
obj.ChangeHead = dataSourceRow.getString(Npc.Column_change_head);
obj.CantResurrect = dataSourceRow.getString(Npc.Column_cant_resurrect);
obj.Hascastle = dataSourceRow.getString(Npc.Column_hascastle);
obj.SpawnlistDoor = dataSourceRow.getString(Npc.Column_spawnlist_door);
obj.CountMap = dataSourceRow.getString(Npc.Column_count_map);

