namespace LineageServer.Server.Server.Templates
{
	using ItemTable = LineageServer.Server.Server.DataSources.ItemTable;
	using MagicDollTable = LineageServer.Server.Server.DataSources.MagicDollTable;
	using L1Character = LineageServer.Server.Server.Model.L1Character;
	using L1DollInstance = LineageServer.Server.Server.Model.Instance.L1DollInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_SkillSound = LineageServer.Server.Server.serverpackets.S_SkillSound;
	using Random = LineageServer.Server.Server.utils.Random;

	public class L1MagicDoll
	{

		public static int getHitAddByDoll(L1Character _master)
		{ // 近距離的命中率增加
			int s = 0;
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					s += doll.Hit;
				}
			}
			return s;
		}

		public static int getDamageAddByDoll(L1Character _master)
		{ // 近距離的攻擊力增加
			int s = 0;
			int chance = RandomHelper.Next(100) + 1;
			bool isAdd = false;
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					if (doll.DmgChance > 0 && !isAdd)
					{ // 額外傷害發動機率
						if (doll.DmgChance >= chance)
						{
							s += doll.Dmg;
							isAdd = true;
						}
					}
					else if (doll.Dmg != 0)
					{ // 額外傷害
						s += doll.Dmg;
					}
				}
			}
			if (isAdd)
			{
				if (_master is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) _master;
					pc.sendPackets(new S_SkillSound(_master.Id, 6319));
				}
				_master.broadcastPacket(new S_SkillSound(_master.Id, 6319));
			}
			return s;
		}

		public static int getDamageReductionByDoll(L1Character _master)
		{ // 傷害減免
			int s = 0;
			int chance = RandomHelper.Next(100) + 1;
			bool isReduction = false;
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					if (doll.DmgReductionChance > 0 && !isReduction)
					{ // 傷害減免發動機率
						if (doll.DmgReductionChance >= chance)
						{
							s += doll.DmgReduction;
							isReduction = true;
						}
					}
					else if (doll.DmgReduction != 0)
					{ // 傷害減免
						s += doll.DmgReduction;
					}
				}
			}
			if (isReduction)
			{
				if (_master is L1PcInstance)
				{
					L1PcInstance pc = (L1PcInstance) _master;
					pc.sendPackets(new S_SkillSound(_master.Id, 6320));
				}
				_master.broadcastPacket(new S_SkillSound(_master.Id, 6320));
			}
			return s;
		}

		public static int getDamageEvasionByDoll(L1Character _master)
		{ // 傷害迴避
			int chance = RandomHelper.Next(100) + 1;
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					if (doll.DmgEvasionChance >= chance)
					{ // 傷害迴避發動機率
						if (_master is L1PcInstance)
						{
							L1PcInstance pc = (L1PcInstance) _master;
							pc.sendPackets(new S_SkillSound(_master.Id, 6320));
						}
						_master.broadcastPacket(new S_SkillSound(_master.Id, 6320));
						return 1;
					}
				}
			}
			return 0;
		}

		public static int getBowHitAddByDoll(L1Character _master)
		{ // 弓的命中率增加
			int s = 0;
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					s += doll.BowHit;
				}
			}
			return s;
		}

		public static int getBowDamageByDoll(L1Character _master)
		{ // 弓的攻擊力增加
			int s = 0;
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					s += doll.BowDmg;
				}
			}
			return s;
		}

		public static int getAcByDoll(L1Character _master)
		{ // 防禦力增加
			int s = 0;
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					s += doll.Ac;
				}
			}
			return s;
		}

		public static int getRegistStoneByDoll(L1Character _master)
		{ // 石化耐性增加
			int s = 0;
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					s += doll.RegistStone;
				}
			}
			return s;
		}

		public static int getRegistStunByDoll(L1Character _master)
		{ // 昏迷耐性增加
			int s = 0;
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					s += doll.RegistStun;
				}
			}
			return s;
		}

		public static int getRegistSustainByDoll(L1Character _master)
		{ // 支撐耐性增加
			int s = 0;
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					s += doll.RegistSustain;
				}
			}
			return s;
		}

		public static int getRegistBlindByDoll(L1Character _master)
		{ // 闇黑耐性增加
			int s = 0;
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					s += doll.RegistBlind;
				}
			}
			return s;
		}

		public static int getRegistFreezeByDoll(L1Character _master)
		{ // 寒冰耐性增加
			int s = 0;
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					s += doll.RegistFreeze;
				}
			}
			return s;
		}

		public static int getRegistSleepByDoll(L1Character _master)
		{ // 睡眠耐性增加
			int s = 0;
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					s += doll.RegistSleep;
				}
			}
			return s;
		}

		public static int getWeightReductionByDoll(L1Character _master)
		{ // 負重減輕
			int s = 0;
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					s += doll.WeightReduction;
				}
			}
			return s;
		}

		public static int getHprByDoll(L1Character _master)
		{ // 體力回覆量
			int s = 0;
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					if (!doll.HprTime && doll.Hpr != 0)
					{
						s += doll.Hpr;
					}
				}
			}
			return s;
		}

		public static int getMprByDoll(L1Character _master)
		{ // 魔力回覆量
			int s = 0;
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					if (!doll.MprTime && doll.Mpr != 0)
					{
						s += doll.Mpr;
					}
				}
			}
			return s;
		}

		public static bool isItemMake(L1Character _master)
		{
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					L1Item item = ItemTable.Instance.getTemplate((doll.MakeItemId));
					if (item != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static int getMakeItemId(L1Character _master)
		{ // 獲得道具
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					L1Item item = ItemTable.Instance.getTemplate((doll.MakeItemId));
					if (item != null)
					{
						return item.ItemId;
					}
				}
			}
			return 0;
		}

		public static bool isHpRegeneration(L1Character _master)
		{ // 回血判斷 (時間固定性)
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					if (doll.HprTime && doll.Hpr != 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static int getHpByDoll(L1Character _master)
		{ // 體力回覆量 (時間固定性)
			int s = 0;
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					if (doll.HprTime && doll.Hpr != 0)
					{
						s += doll.Hpr;
					}
				}
			}
			return s;
		}

		public static bool isMpRegeneration(L1Character _master)
		{ // 回魔判斷 (時間固定性)
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					if (doll.MprTime && doll.Mpr != 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static int getMpByDoll(L1Character _master)
		{ // 魔力回覆量 (時間固定性)
			int s = 0;

			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll != null)
				{
					if (doll.MprTime && doll.Mpr != 0)
					{
						s += doll.Mpr;
					}
				}
			}
			return s;
		}

		public static int getEffectByDoll(L1Character _master, sbyte type)
		{ // 效果
			int chance = RandomHelper.Next(100) + 1;
			foreach (L1DollInstance dollIns in _master.DollList.Values)
			{
				L1MagicDoll doll = MagicDollTable.Instance.getTemplate(dollIns.ItemId);
				if (doll.EffectChance > chance)
				{
					if (doll != null)
					{
						if (doll.Effect == type)
						{
						return type;
						}
					}
				}
			}
			return 0;
		}

		private int _itemId;
		private int _dollId;
		private int _ac;
		private int _hpr;
		private int _mpr;
		private bool _hprTime;
		private bool _mprTime;
		private int _dmg;
		private int _bowDmg;
		private int _dmgChance;
		private int _hit;
		private int _bowHit;
		private int _dmgReduction;
		private int _dmgReductionChance;
		private int _dmgEvasionChance;
		private int _weightReduction;
		private int _registStun;
		private int _registStone;
		private int _registSleep;
		private int _registFreeze;
		private int _registSustain;
		private int _registBlind;
		private int _makeItemId;
		private sbyte _effect;
		private int _EffectChance;

		public virtual int ItemId
		{
			get
			{
				return _itemId;
			}
			set
			{
				_itemId = value;
			}
		}


		public virtual int DollId
		{
			get
			{
				return _dollId;
			}
			set
			{
				_dollId = value;
			}
		}


		public virtual int Ac
		{
			get
			{
				return _ac;
			}
			set
			{
				_ac = value;
			}
		}


		public virtual int Hpr
		{
			get
			{
				return _hpr;
			}
			set
			{
				_hpr = value;
			}
		}


		public virtual int Mpr
		{
			get
			{
				return _mpr;
			}
			set
			{
				_mpr = value;
			}
		}


		public virtual bool HprTime
		{
			get
			{
				return _hprTime;
			}
			set
			{
				_hprTime = value;
			}
		}


		public virtual bool MprTime
		{
			get
			{
				return _mprTime;
			}
			set
			{
				_mprTime = value;
			}
		}


		public virtual int Dmg
		{
			get
			{
				return _dmg;
			}
			set
			{
				_dmg = value;
			}
		}


		public virtual int BowDmg
		{
			get
			{
				return _bowDmg;
			}
			set
			{
				_bowDmg = value;
			}
		}


		public virtual int DmgChance
		{
			get
			{
				return _dmgChance;
			}
			set
			{
				_dmgChance = value;
			}
		}


		public virtual int Hit
		{
			get
			{
				return _hit;
			}
			set
			{
				_hit = value;
			}
		}


		public virtual int BowHit
		{
			get
			{
				return _bowHit;
			}
			set
			{
				_bowHit = value;
			}
		}


		public virtual int DmgReduction
		{
			get
			{
				return _dmgReduction;
			}
			set
			{
				_dmgReduction = value;
			}
		}


		public virtual int DmgReductionChance
		{
			get
			{
				return _dmgReductionChance;
			}
			set
			{
				_dmgReductionChance = value;
			}
		}


		public virtual int DmgEvasionChance
		{
			get
			{
				return _dmgEvasionChance;
			}
			set
			{
				_dmgEvasionChance = value;
			}
		}


		public virtual int WeightReduction
		{
			get
			{
				return _weightReduction;
			}
			set
			{
				_weightReduction = value;
			}
		}


		public virtual int RegistStun
		{
			get
			{
				return _registStun;
			}
			set
			{
				_registStun = value;
			}
		}


		public virtual int RegistStone
		{
			get
			{
				return _registStone;
			}
			set
			{
				_registStone = value;
			}
		}


		public virtual int RegistSleep
		{
			get
			{
				return _registSleep;
			}
			set
			{
				_registSleep = value;
			}
		}


		public virtual int RegistFreeze
		{
			get
			{
				return _registFreeze;
			}
			set
			{
				_registFreeze = value;
			}
		}


		public virtual int RegistSustain
		{
			get
			{
				return _registSustain;
			}
			set
			{
				_registSustain = value;
			}
		}


		public virtual int RegistBlind
		{
			get
			{
				return _registBlind;
			}
			set
			{
				_registBlind = value;
			}
		}


		public virtual int MakeItemId
		{
			get
			{
				return _makeItemId;
			}
			set
			{
				_makeItemId = value;
			}
		}


		public virtual sbyte Effect
		{
			get
			{
				return _effect;
			}
			set
			{
				_effect = value;
			}
		}

		public virtual int EffectChance
		{
			get
			{
				return _EffectChance;
			}
			set
			{
				_EffectChance = value;
			}
		}


	}

}