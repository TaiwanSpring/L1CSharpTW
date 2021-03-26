using System.Collections.Generic;

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
namespace LineageServer.Server.Server.Model
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.SHAPE_CHANGE;

	using PolyTable = LineageServer.Server.Server.DataSources.PolyTable;
	using SprTable = LineageServer.Server.Server.DataSources.SprTable;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1MonsterInstance = LineageServer.Server.Server.Model.Instance.L1MonsterInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1NpcDefaultAction = LineageServer.Server.Server.Model.Npc.Action.L1NpcDefaultAction;
	using S_ChangeShape = LineageServer.Server.Server.serverpackets.S_ChangeShape;
	using S_CharVisualUpdate = LineageServer.Server.Server.serverpackets.S_CharVisualUpdate;
	using S_CloseList = LineageServer.Server.Server.serverpackets.S_CloseList;
	using S_NpcChangeShape = LineageServer.Server.Server.serverpackets.S_NpcChangeShape;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using S_SkillIconGFX = LineageServer.Server.Server.serverpackets.S_SkillIconGFX;
	using Maps = LineageServer.Server.Server.Utils.collections.Maps;

	// Referenced classes of package l1j.server.server.model:
	// L1PcInstance

	public class L1PolyMorph
	{
		// weapon equip bit
		private const int DAGGER_EQUIP = 1;

		private const int SWORD_EQUIP = 2;

		private const int TWOHANDSWORD_EQUIP = 4;

		private const int AXE_EQUIP = 8;

		private const int SPEAR_EQUIP = 16;

		private const int STAFF_EQUIP = 32;

		private const int EDORYU_EQUIP = 64;

		private const int CLAW_EQUIP = 128;

		private const int BOW_EQUIP = 256; // ガントレット含む

		private const int KIRINGKU_EQUIP = 512;

		private const int CHAINSWORD_EQUIP = 1024;

		// armor equip bit
		private const int HELM_EQUIP = 1;

		private const int AMULET_EQUIP = 2;

		private const int EARRING_EQUIP = 4;

		private const int TSHIRT_EQUIP = 8;

		private const int ARMOR_EQUIP = 16;

		private const int CLOAK_EQUIP = 32;

		private const int BELT_EQUIP = 64;

		private const int SHIELD_EQUIP = 128;

		private const int GLOVE_EQUIP = 256;

		private const int RING_EQUIP = 512;

		private const int BOOTS_EQUIP = 1024;

		private const int GUARDER_EQUIP = 2048;

		// 変身の原因を示すbit
		public const int MORPH_BY_ITEMMAGIC = 1;

		public const int MORPH_BY_GM = 2;

		public const int MORPH_BY_NPC = 4; // 占星術師ケプリシャ以外のNPC

		public const int MORPH_BY_KEPLISHA = 8;

		public const int MORPH_BY_LOGIN = 0;

		private static readonly IDictionary<int, int> weaponFlgMap = Maps.newMap();
		static L1PolyMorph()
		{
			weaponFlgMap[1] = SWORD_EQUIP;
			weaponFlgMap[2] = DAGGER_EQUIP;
			weaponFlgMap[3] = TWOHANDSWORD_EQUIP;
			weaponFlgMap[4] = BOW_EQUIP;
			weaponFlgMap[5] = SPEAR_EQUIP;
			weaponFlgMap[6] = AXE_EQUIP;
			weaponFlgMap[7] = STAFF_EQUIP;
			weaponFlgMap[8] = BOW_EQUIP;
			weaponFlgMap[9] = BOW_EQUIP;
			weaponFlgMap[10] = BOW_EQUIP;
			weaponFlgMap[11] = CLAW_EQUIP;
			weaponFlgMap[12] = EDORYU_EQUIP;
			weaponFlgMap[13] = BOW_EQUIP;
			weaponFlgMap[14] = SPEAR_EQUIP;
			weaponFlgMap[15] = AXE_EQUIP;
			weaponFlgMap[16] = STAFF_EQUIP;
			weaponFlgMap[17] = KIRINGKU_EQUIP;
			weaponFlgMap[18] = CHAINSWORD_EQUIP;
			weaponFlgMap[19] = KIRINGKU_EQUIP;
			armorFlgMap[1] = HELM_EQUIP;
			armorFlgMap[2] = ARMOR_EQUIP;
			armorFlgMap[3] = TSHIRT_EQUIP;
			armorFlgMap[4] = CLOAK_EQUIP;
			armorFlgMap[5] = GLOVE_EQUIP;
			armorFlgMap[6] = BOOTS_EQUIP;
			armorFlgMap[7] = SHIELD_EQUIP;
			armorFlgMap[8] = AMULET_EQUIP;
			armorFlgMap[9] = RING_EQUIP;
			armorFlgMap[10] = BELT_EQUIP;
			armorFlgMap[12] = EARRING_EQUIP;
			armorFlgMap[13] = GUARDER_EQUIP;
		}

		private static readonly IDictionary<int, int> armorFlgMap = Maps.newMap();

		private int _id;

		private string _name;

		private int _polyId;

		private int _minLevel;

		private int _weaponEquipFlg;

		private int _armorEquipFlg;

		private bool _canUseSkill;

		private int _causeFlg;

		public L1PolyMorph(int id, string name, int polyId, int minLevel, int weaponEquipFlg, int armorEquipFlg, bool canUseSkill, int causeFlg)
		{
			_id = id;
			_name = name;
			_polyId = polyId;
			_minLevel = minLevel;
			_weaponEquipFlg = weaponEquipFlg;
			_armorEquipFlg = armorEquipFlg;
			_canUseSkill = canUseSkill;
			_causeFlg = causeFlg;
		}

		public virtual int Id
		{
			get
			{
				return _id;
			}
		}

		public virtual string Name
		{
			get
			{
				return _name;
			}
		}

		public virtual int PolyId
		{
			get
			{
				return _polyId;
			}
		}

		public virtual int MinLevel
		{
			get
			{
				return _minLevel;
			}
		}

		public virtual int WeaponEquipFlg
		{
			get
			{
				return _weaponEquipFlg;
			}
		}

		public virtual int ArmorEquipFlg
		{
			get
			{
				return _armorEquipFlg;
			}
		}

		public virtual bool canUseSkill()
		{
			return _canUseSkill;
		}

		public virtual int CauseFlg
		{
			get
			{
				return _causeFlg;
			}
		}

		public static void handleCommands(L1PcInstance pc, string s)
		{
			if ((pc == null) || pc.Dead)
			{
				return;
			}
			L1PolyMorph poly = PolyTable.Instance.getTemplate(s);
			if ((poly != null) || s.Equals("none"))
			{
				if (s.Equals("none"))
				{
					if ((pc.TempCharGfx == 6034) || (pc.TempCharGfx == 6035))
					{
					}
					else
					{
						pc.removeSkillEffect(L1SkillId.SHAPE_CHANGE);
						pc.sendPackets(new S_CloseList(pc.Id));
					}
				}
				else if ((pc.Level >= poly.MinLevel) || pc.Gm)
				{
					if ((pc.TempCharGfx == 6034) || (pc.TempCharGfx == 6035))
					{
						pc.sendPackets(new S_ServerMessage(181));
						// \f1そのようなモンスターには変身できません。
					}
					else
					{
						doPoly(pc, poly.PolyId, 7200, MORPH_BY_ITEMMAGIC);
						pc.sendPackets(new S_CloseList(pc.Id));
					}
				}
				else
				{
					pc.sendPackets(new S_ServerMessage(181)); // \f1そのようなモンスターには変身できません。
				}
			}
		}

		// 變身
		public static void doPoly(L1Character cha, int polyId, int timeSecs, int cause)
		{
			doPoly(cha, polyId, timeSecs, cause, true);
		}

		// 變身
		public static void doPoly(L1Character cha, int polyId, int timeSecs, int cause, bool cantPolyMessage)
		{
			if ((cha == null) || cha.Dead)
			{
				return;
			}
			if (cha is L1PcInstance)
			{
				L1PcInstance pc = (L1PcInstance) cha;
				if (pc.MapId == 5124 || pc.MapId == 5300 || pc.MapId == 5301)
				{ // 釣魚池
					if (cantPolyMessage)
					{
						pc.sendPackets(new S_ServerMessage(1170)); // 這裡不可以變身。
					}
					else
					{
						pc.sendPackets(new S_ServerMessage(79));
					}
					return;
				}
				if ((pc.TempCharGfx == 6034) || (pc.TempCharGfx == 6035) || !isMatchCause(polyId, cause))
				{
					if (cantPolyMessage)
					{
						pc.sendPackets(new S_ServerMessage(181)); // \f1無法變成你指定的怪物。
					}
					else
					{
						pc.sendPackets(new S_ServerMessage(79));
					}
					return;
				}
				pc.killSkillEffectTimer(SHAPE_CHANGE);
				pc.setSkillEffect(SHAPE_CHANGE, timeSecs * 1000);
				if (pc.TempCharGfx != polyId)
				{
					L1ItemInstance weapon = pc.Weapon;
					bool weaponTakeoff = (weapon != null && !isEquipableWeapon(polyId, weapon.Item.Type));
					if (weaponTakeoff)
					{ // 解除武器時
						pc.CurrentWeapon = 0;
					}
					pc.TempCharGfx = polyId;
					pc.sendPackets(new S_ChangeShape(pc.Id, polyId, pc.CurrentWeapon));
					if (pc.GmInvis)
					{ // GM隱身
					}
					else if (pc.Invisble)
					{ // 一般隱身
						pc.broadcastPacketForFindInvis(new S_ChangeShape(pc.Id, polyId, pc.CurrentWeapon), true);
					}
					else
					{
						pc.broadcastPacket(new S_ChangeShape(pc.Id, polyId, pc.CurrentWeapon));
					}
					pc.Inventory.takeoffEquip(polyId); // 是否將裝備的武器強制解除。
				}
				pc.sendPackets(new S_SkillIconGFX(35, timeSecs));
			}
			else if (cha is L1MonsterInstance)
			{ // 怪物變身
				L1MonsterInstance mob = (L1MonsterInstance) cha;
				mob.killSkillEffectTimer(SHAPE_CHANGE);
				mob.setSkillEffect(SHAPE_CHANGE, timeSecs * 1000);
				if (mob.TempCharGfx != polyId)
				{
					mob.TempCharGfx = polyId;
					int npcStatus = L1NpcDefaultAction.Instance.getStatus(polyId);
					mob.Status = npcStatus;
					if (npcStatus == 20)
					{ // 弓類
						mob.PolyAtkRanged = 10;
						mob.PolyArrowGfx = 66;
					}
					else if (npcStatus == 24 || polyId == 95 || polyId == 146)
					{ // 矛類、夏洛伯、楊果理恩
						mob.PolyAtkRanged = 2;
						mob.PolyArrowGfx = 0;
					}
					else
					{
						mob.PolyAtkRanged = 1;
						mob.PolyArrowGfx = 0;
					}
					mob.Passispeed = SprTable.Instance.getSprSpeed(polyId, mob.Status); // 移動速度
					mob.Atkspeed = SprTable.Instance.getSprSpeed(polyId, mob.Status + 1); // 攻擊速度
					mob.broadcastPacket(new S_NpcChangeShape(mob.Id, polyId, mob.Lawful, mob.Status)); // 更新NPC外觀
				}
			}
		}

		/// <summary>
		/// 3.80c 個人商店變身 </summary>
		/// <param name="cha"> </param>
		/// <param name="polyIndex"> 1-8 </param>
		public static void doPolyPraivateShop(L1Character cha, int polyIndex)
		{
			if ((cha == null) || cha.Dead)
			{
				return;
			}
			if (cha is L1PcInstance)
			{
				L1PcInstance pc = (L1PcInstance) cha;
				/// <summary>
				/// 3.80 個人商店變身清單 </summary>
				int[] PolyList = new int[] {11479, 11427, 10047, 9688, 11322, 10069, 10034, 10032};
				if (pc.TempCharGfx != PolyList[polyIndex - 1])
				{
					L1ItemInstance weapon = pc.Weapon;
					bool weaponTakeoff = (weapon != null && !isEquipableWeapon(PolyList[polyIndex - 1], weapon.Item.Type));
					if (weaponTakeoff)
					{ // 解除武器時
						pc.CurrentWeapon = 0;
					}
					pc.TempCharGfx = PolyList[polyIndex - 1];
					pc.sendPackets(new S_ChangeShape(pc.Id, PolyList[polyIndex - 1], 0x46));
					if (pc.GmInvis)
					{ // GM隱身
					}
					else if (pc.Invisble)
					{ // 一般隱身
						pc.broadcastPacketForFindInvis(new S_ChangeShape(pc.Id,PolyList[polyIndex - 1], 0x46), true);
					}
					else
					{
						pc.broadcastPacket(new S_ChangeShape(pc.Id,PolyList[polyIndex - 1], 0x46));
					}
					pc.Inventory.takeoffEquip(PolyList[polyIndex - 1]); // 是否將裝備的武器強制解除。
				}
				pc.sendPackets(new S_SkillIconGFX(PolyList[polyIndex - 1]));
				pc.sendPackets(new S_CharVisualUpdate(pc, 0x46));
				pc.broadcastPacket(new S_CharVisualUpdate(pc, 0x46));
			}
		}

		/// <summary>
		/// 3.80c 個人商店 取消變身 </summary>
		/// <param name="cha"> </param>
		public static void undoPolyPrivateShop(L1Character cha)
		{
			if (cha is L1PcInstance)
			{
				L1PcInstance pc = (L1PcInstance) cha;
				int classId = pc.ClassId;
				pc.TempCharGfx = classId;
				if (!pc.Dead)
				{
					pc.sendPackets(new S_ChangeShape(pc.Id, classId, pc.CurrentWeapon));
					pc.broadcastPacket(new S_ChangeShape(pc.Id, classId, pc.CurrentWeapon));
					pc.sendPackets(new S_SkillIconGFX(classId));
					pc.sendPackets(new S_CharVisualUpdate(pc, pc.CurrentWeapon));
					pc.broadcastPacket(new S_CharVisualUpdate(pc, pc.CurrentWeapon));
				}
			}

		}

		// 解除變身
		public static void undoPoly(L1Character cha)
		{
			if (cha is L1PcInstance)
			{
				L1PcInstance pc = (L1PcInstance) cha;
				int classId = pc.ClassId;
				pc.TempCharGfx = classId;
				if (!pc.Dead)
				{
					pc.sendPackets(new S_ChangeShape(pc.Id, classId, pc.CurrentWeapon));
					pc.broadcastPacket(new S_ChangeShape(pc.Id, classId, pc.CurrentWeapon));
				}
			}
			else if (cha is L1MonsterInstance)
			{
				L1MonsterInstance mob = (L1MonsterInstance) cha;
				int gfxId = mob.GfxId;
				mob.TempCharGfx = 0;
				mob.Status = L1NpcDefaultAction.Instance.getStatus(gfxId);
				mob.PolyAtkRanged = -1;
				mob.PolyArrowGfx = 0;
				mob.Passispeed = SprTable.Instance.getSprSpeed(gfxId, mob.Status); // 移動速度
				mob.Atkspeed = SprTable.Instance.getSprSpeed(gfxId, mob.Status + 1); // 攻擊速度
				mob.broadcastPacket(new S_NpcChangeShape(mob.Id, gfxId, mob.Lawful, mob.Status)); // 更新NPC外觀
			}
		}

		// 指定したpolyIdがweapontTypeの武器を装備出来るか？
		public static bool isEquipableWeapon(int polyId, int weaponType)
		{
			L1PolyMorph poly = PolyTable.Instance.getTemplate(polyId);
			if (poly == null)
			{
				return true;
			}

			int? flg = weaponFlgMap[weaponType];
			if (flg != null)
			{
				return 0 != (poly.WeaponEquipFlg & flg.Value);
			}
			return true;
		}

		// 指定したpolyIdがarmorTypeの防具を装備出来るか？
		public static bool isEquipableArmor(int polyId, int armorType)
		{
			L1PolyMorph poly = PolyTable.Instance.getTemplate(polyId);
			if (poly == null)
			{
				return true;
			}

			int? flg = armorFlgMap[armorType];
			if (flg != null)
			{
				return 0 != (poly.ArmorEquipFlg & flg.Value);
			}
			return true;
		}

		// 指定したpolyIdが何によって変身し、それが変身させられるか？
		public static bool isMatchCause(int polyId, int cause)
		{
			L1PolyMorph poly = PolyTable.Instance.getTemplate(polyId);
			if (poly == null)
			{
				return true;
			}
			if (cause == MORPH_BY_LOGIN)
			{
				return true;
			}

			return 0 != (poly.CauseFlg & cause);
		}
	}

}