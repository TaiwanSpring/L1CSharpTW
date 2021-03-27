namespace LineageServer.Server.Model.skill
{
	public class L1SkillId
	{
		readonly static int[] allBuffSkill = new int[]
		{
			 LIGHT,
			 DECREASE_WEIGHT,
			 PHYSICAL_ENCHANT_DEX,
			 MEDITATION,
			 PHYSICAL_ENCHANT_STR,
			 BLESS_WEAPON,
			 BERSERKERS,
			 IMMUNE_TO_HARM,
			 ADVANCE_SPIRIT,
			 REDUCTION_ARMOR,
			 BOUNCE_ATTACK,
			 SOLID_CARRIAGE,
			 ENCHANT_VENOM,
			 BURNING_SPIRIT,
			 VENOM_RESIST,
			 DOUBLE_BRAKE,
			 UNCANNY_DODGE,
			 DRESS_EVASION,
			 GLOWING_AURA,
			 BRAVE_AURA,
			 RESIST_MAGIC,
			 CLEAR_MIND,
			 ELEMENTAL_PROTECTION,
			 AQUA_PROTECTER,
			 BURNING_WEAPON,
			 IRON_SKIN,
			 EXOTIC_VITALIZE,
			 WATER_LIFE,
			 ELEMENTAL_FIRE,
			 SOUL_OF_FLAME,
			 ADDITIONAL_FIRE
		};
		public static int[] GetAllBuffers()
		{
			return allBuffSkill;
		}

		/// <summary>
		/// 魔法開頭 </summary>
		public const int SKILLS_BEGIN = 1;

		/*
		 * Regular Magic Lv1-10
		 */
		/// <summary>
		/// 法師魔法 (初級治癒術) </summary>
		public const int HEAL = 1; // E: LESSER_HEAL

		/// <summary>
		/// 法師魔法 (日光術) </summary>
		public const int LIGHT = 2;

		/// <summary>
		/// 法師魔法 (防護罩) </summary>
		public const int SHIELD = 3;

		/// <summary>
		/// 法師魔法 (光箭) </summary>
		public const int ENERGY_BOLT = 4;

		/// <summary>
		/// 法師魔法 (指定傳送) </summary>
		public const int TELEPORT = 5;

		/// <summary>
		/// 法師魔法 (冰箭) </summary>
		public const int ICE_DAGGER = 6;

		/// <summary>
		/// 法師魔法 (風刃) </summary>
		public const int WIND_CUTTER = 7; // E: WIND_SHURIKEN

		/// <summary>
		/// 法師魔法 (神聖武器) </summary>
		public const int HOLY_WEAPON = 8;

		/// <summary>
		/// 法師魔法 (解毒術) </summary>
		public const int CURE_POISON = 9;

		/// <summary>
		/// 法師魔法 (寒冷戰慄) </summary>
		public const int CHILL_TOUCH = 10;

		/// <summary>
		/// 法師魔法 (毒咒) </summary>
		public const int CURSE_POISON = 11;

		/// <summary>
		/// 法師魔法 (擬似魔法武器) </summary>
		public const int ENCHANT_WEAPON = 12;

		/// <summary>
		/// 法師魔法 (無所遁形術) </summary>
		public const int DETECTION = 13;

		/// <summary>
		/// 法師魔法 (負重強化) </summary>
		public const int DECREASE_WEIGHT = 14;

		/// <summary>
		/// 法師魔法 (火箭) </summary>
		public const int FIRE_ARROW = 15;

		/// <summary>
		/// 法師魔法 (地獄之牙) </summary>
		public const int STALAC = 16;

		/// <summary>
		/// 法師魔法 (極光雷電) </summary>
		public const int LIGHTNING = 17;

		/// <summary>
		/// 法師魔法 (起死回生術) </summary>
		public const int TURN_UNDEAD = 18;

		/// <summary>
		/// 法師魔法 (中級治癒術) </summary>
		public const int EXTRA_HEAL = 19; // E: HEAL

		/// <summary>
		/// 法師魔法 (闇盲咒術) </summary>
		public const int CURSE_BLIND = 20;

		/// <summary>
		/// 法師魔法 (鎧甲護持) </summary>
		public const int BLESSED_ARMOR = 21;

		/// <summary>
		/// 法師魔法 (寒冰氣息) </summary>
		public const int FROZEN_CLOUD = 22;

		/// <summary>
		/// 法師魔法 (能量感測) </summary>
		public const int WEAK_ELEMENTAL = 23; // E: REVEAL_WEAKNESS

		// none = 24
		/// <summary>
		/// 法師魔法 (燃燒的火球) </summary>
		public const int FIREBALL = 25;

		/// <summary>
		/// 法師魔法 (通暢氣脈術) </summary>
		public const int PHYSICAL_ENCHANT_DEX = 26; // E: ENCHANT_DEXTERITY

		/// <summary>
		/// 法師魔法 (壞物術) </summary>
		public const int WEAPON_BREAK = 27;

		/// <summary>
		/// 法師魔法 (吸血鬼之吻) </summary>
		public const int VAMPIRIC_TOUCH = 28;

		/// <summary>
		/// 法師魔法 (緩速術) </summary>
		public const int SLOW = 29;

		/// <summary>
		/// 法師魔法 (岩牢) </summary>
		public const int EARTH_JAIL = 30;

		/// <summary>
		/// 法師魔法 (魔法屏障) </summary>
		public const int COUNTER_MAGIC = 31;

		/// <summary>
		/// 法師魔法 (冥想術) </summary>
		public const int MEDITATION = 32;

		/// <summary>
		/// 法師魔法 (木乃伊的詛咒) </summary>
		public const int CURSE_PARALYZE = 33;

		/// <summary>
		/// 法師魔法 (極道落雷) </summary>
		public const int CALL_LIGHTNING = 34;

		/// <summary>
		/// 法師魔法 (高級治癒術) </summary>
		public const int GREATER_HEAL = 35;

		/// <summary>
		/// 法師魔法 (迷魅術) </summary>
		public const int TAMING_MONSTER = 36; // E: TAME_MONSTER

		/// <summary>
		/// 法師魔法 (聖潔之光) </summary>
		public const int REMOVE_CURSE = 37;

		/// <summary>
		/// 法師魔法 (冰錐) </summary>
		public const int CONE_OF_COLD = 38;

		/// <summary>
		/// 法師魔法 (魔力奪取) </summary>
		public const int MANA_DRAIN = 39;

		/// <summary>
		/// 法師魔法 (黑闇之影) </summary>
		public const int DARKNESS = 40;

		/// <summary>
		/// 法師魔法 (造屍術) </summary>
		public const int CREATE_ZOMBIE = 41;

		/// <summary>
		/// 法師魔法 (體魄強健術) </summary>
		public const int PHYSICAL_ENCHANT_STR = 42; // E: ENCHANT_MIGHTY

		/// <summary>
		/// 法師魔法 (加速術) </summary>
		public const int HASTE = 43;

		/// <summary>
		/// 法師魔法 (魔法相消術) </summary>
		public const int CANCELLATION = 44; // E: CANCEL MAGIC

		/// <summary>
		/// 法師魔法 (地裂術) </summary>
		public const int ERUPTION = 45;

		/// <summary>
		/// 法師魔法 (烈炎術) </summary>
		public const int SUNBURST = 46;

		/// <summary>
		/// 法師魔法 (弱化術) </summary>
		public const int WEAKNESS = 47;

		/// <summary>
		/// 法師魔法 (祝福魔法武器) </summary>
		public const int BLESS_WEAPON = 48;

		/// <summary>
		/// 法師魔法 (體力回復術) </summary>
		public const int HEAL_ALL = 49; // E: HEAL_PLEDGE

		/// <summary>
		/// 法師魔法 (冰矛圍籬) </summary>
		public const int ICE_LANCE = 50;

		/// <summary>
		/// 法師魔法 (召喚術) </summary>
		public const int SUMMON_MONSTER = 51;

		/// <summary>
		/// 法師魔法 (神聖疾走) </summary>
		public const int HOLY_WALK = 52;

		/// <summary>
		/// 法師魔法 (龍捲風) </summary>
		public const int TORNADO = 53;

		/// <summary>
		/// 法師魔法 (強力加速術) </summary>
		public const int GREATER_HASTE = 54;

		/// <summary>
		/// 法師魔法 (狂暴術) </summary>
		public const int BERSERKERS = 55;

		/// <summary>
		/// 法師魔法 (疾病術) </summary>
		public const int DISEASE = 56;

		/// <summary>
		/// 法師魔法 (全部治癒術) </summary>
		public const int FULL_HEAL = 57;

		/// <summary>
		/// 法師魔法 (火牢) </summary>
		public const int FIRE_WALL = 58;

		/// <summary>
		/// 法師魔法 (冰雪暴) </summary>
		public const int BLIZZARD = 59;

		/// <summary>
		/// 法師魔法 (隱身術) </summary>
		public const int INVISIBILITY = 60;

		/// <summary>
		/// 法師魔法 (返生術) </summary>
		public const int RESURRECTION = 61;

		/// <summary>
		/// 法師魔法 (震裂術) </summary>
		public const int EARTHQUAKE = 62;

		/// <summary>
		/// 法師魔法 (治癒能量風暴) </summary>
		public const int LIFE_STREAM = 63;

		/// <summary>
		/// 法師魔法 (魔法封印) </summary>
		public const int SILENCE = 64;

		/// <summary>
		/// 法師魔法 (雷霆風暴) </summary>
		public const int LIGHTNING_STORM = 65;

		/// <summary>
		/// 法師魔法 (沉睡之霧) </summary>
		public const int FOG_OF_SLEEPING = 66;

		/// <summary>
		/// 法師魔法 (變形術) </summary>
		public const int SHAPE_CHANGE = 67; // E: POLYMORPH

		/// <summary>
		/// 法師魔法 (聖結界) </summary>
		public const int IMMUNE_TO_HARM = 68;

		/// <summary>
		/// 法師魔法 (集體傳送術) </summary>
		public const int MASS_TELEPORT = 69;

		/// <summary>
		/// 法師魔法 (火風暴) </summary>
		public const int FIRE_STORM = 70;

		/// <summary>
		/// 法師魔法 (藥水霜化術) </summary>
		public const int DECAY_POTION = 71;

		/// <summary>
		/// 法師魔法 (強力無所遁形術) </summary>
		public const int COUNTER_DETECTION = 72;

		/// <summary>
		/// 法師魔法 (創造魔法武器) </summary>
		public const int CREATE_MAGICAL_WEAPON = 73;

		/// <summary>
		/// 法師魔法 (流星雨) </summary>
		public const int METEOR_STRIKE = 74;

		/// <summary>
		/// 法師魔法 (終極返生術) </summary>
		public const int GREATER_RESURRECTION = 75;

		/// <summary>
		/// 法師魔法 (集體緩速術) </summary>
		public const int MASS_SLOW = 76;

		/// <summary>
		/// 法師魔法 (究極光裂術) </summary>
		public const int DISINTEGRATE = 77; // E: DESTROY

		/// <summary>
		/// 法師魔法 (絕對屏障) </summary>
		public const int ABSOLUTE_BARRIER = 78;

		/// <summary>
		/// 法師魔法 (靈魂昇華) </summary>
		public const int ADVANCE_SPIRIT = 79;

		/// <summary>
		/// 法師魔法 (冰雪颶風) </summary>
		public const int FREEZING_BLIZZARD = 80;

		// none = 81 - 86
		/*
		 * Knight skills
		 */
		/// <summary>
		/// 騎士魔法 (衝擊之暈) </summary>
		public const int SHOCK_STUN = 87; // E: STUN_SHOCK

		/// <summary>
		/// 騎士魔法 (增幅防禦) </summary>
		public const int REDUCTION_ARMOR = 88;

		/// <summary>
		/// 騎士魔法 (尖刺盔甲) </summary>
		public const int BOUNCE_ATTACK = 89;

		/// <summary>
		/// 騎士魔法 (堅固防護) </summary>
		public const int SOLID_CARRIAGE = 90;

		/// <summary>
		/// 騎士魔法 (反擊屏障) </summary>
		public const int COUNTER_BARRIER = 91;

		// none = 92-96
		/*
		 * Dark Spirit Magic
		 */
		/// <summary>
		/// 黑暗妖精魔法 (暗隱術) </summary>
		public const int BLIND_HIDING = 97;

		/// <summary>
		/// 黑暗妖精魔法 (附加劇毒) </summary>
		public const int ENCHANT_VENOM = 98;

		/// <summary>
		/// 黑暗妖精魔法 (影之防護) </summary>
		public const int SHADOW_ARMOR = 99;

		/// <summary>
		/// 黑暗妖精魔法 (提煉魔石) </summary>
		public const int BRING_STONE = 100;

		/// <summary>
		/// 黑暗妖精魔法 (行走加速) </summary>
		public const int MOVING_ACCELERATION = 101; // E: PURIFY_STONE

		/// <summary>
		/// 黑暗妖精魔法 (燃燒鬥志) </summary>
		public const int BURNING_SPIRIT = 102;

		/// <summary>
		/// 黑暗妖精魔法 (暗黑盲咒) </summary>
		public const int DARK_BLIND = 103;

		/// <summary>
		/// 黑暗妖精魔法 (毒性抵抗) </summary>
		public const int VENOM_RESIST = 104;

		/// <summary>
		/// 黑暗妖精魔法 (雙重破壞) </summary>
		public const int DOUBLE_BRAKE = 105;

		/// <summary>
		/// 黑暗妖精魔法 (暗影閃避) </summary>
		public const int UNCANNY_DODGE = 106;

		/// <summary>
		/// 黑暗妖精魔法 (暗影之牙) </summary>
		public const int SHADOW_FANG = 107;

		/// <summary>
		/// 黑暗妖精魔法 (會心一擊) </summary>
		public const int FINAL_BURN = 108;

		/// <summary>
		/// 黑暗妖精魔法 (力量提升) </summary>
		public const int DRESS_MIGHTY = 109;

		/// <summary>
		/// 黑暗妖精魔法 (敏捷提升) </summary>
		public const int DRESS_DEXTERITY = 110;

		/// <summary>
		/// 黑暗妖精魔法 (閃避提升) </summary>
		public const int DRESS_EVASION = 111;

		// none = 112
		/*
		 * Royal Magic
		 */
		/// <summary>
		/// 王族魔法 (精準目標) </summary>
		public const int TRUE_TARGET = 113;

		/// <summary>
		/// 王族魔法 (激勵士氣) </summary>
		public const int GLOWING_AURA = 114;

		/// <summary>
		/// 王族魔法 (鋼鐵士氣) </summary>
		public const int SHINING_AURA = 115;

		/// <summary>
		/// 王族魔法 (呼喚盟友) </summary>
		public const int CALL_CLAN = 116; // E: CALL_PLEDGE_MEMBER

		/// <summary>
		/// 王族魔法 (衝擊士氣) </summary>
		public const int BRAVE_AURA = 117;

		/// <summary>
		/// 王族魔法 (援護盟友) </summary>
		public const int RUN_CLAN = 118;

		// unknown = 119 - 120
		// none = 121 - 128
		/*
		 * Spirit Magic
		 */
		/// <summary>
		/// 妖精魔法 (魔法防禦) </summary>
		public const int RESIST_MAGIC = 129;

		/// <summary>
		/// 妖精魔法 (心靈轉換) </summary>
		public const int BODY_TO_MIND = 130;

		/// <summary>
		/// 妖精魔法 (世界樹的呼喚) </summary>
		public const int TELEPORT_TO_MATHER = 131;

		/// <summary>
		/// 妖精魔法 (三重矢) </summary>
		public const int TRIPLE_ARROW = 132;

		/// <summary>
		/// 妖精魔法 (弱化屬性) </summary>
		public const int ELEMENTAL_FALL_DOWN = 133;

		/// <summary>
		/// 妖精魔法 (鏡反射) </summary>
		public const int COUNTER_MIRROR = 134;

		// none = 135 - 136
		/// <summary>
		/// 妖精魔法 (淨化精神) </summary>
		public const int CLEAR_MIND = 137;

		/// <summary>
		/// 妖精魔法 (屬性防禦) </summary>
		public const int RESIST_ELEMENTAL = 138;

		// none = 139 - 144
		/// <summary>
		/// 妖精魔法 (釋放元素) </summary>
		public const int RETURN_TO_NATURE = 145;

		/// <summary>
		/// 妖精魔法 (魂體轉換) </summary>
		public const int BLOODY_SOUL = 146; // E: BLOOD_TO_SOUL

		/// <summary>
		/// 妖精魔法 (單屬性防禦) </summary>
		public const int ELEMENTAL_PROTECTION = 147; // E:PROTECTION_FROM_ELEMENTAL

		/// <summary>
		/// 妖精魔法 (火焰武器) </summary>
		public const int FIRE_WEAPON = 148;

		/// <summary>
		/// 妖精魔法 (風之神射) </summary>
		public const int WIND_SHOT = 149;

		/// <summary>
		/// 妖精魔法 (風之疾走) </summary>
		public const int WIND_WALK = 150;

		/// <summary>
		/// 妖精魔法 (大地防護) </summary>
		public const int EARTH_SKIN = 151;

		/// <summary>
		/// 妖精魔法 (地面障礙) </summary>
		public const int ENTANGLE = 152;

		/// <summary>
		/// 妖精魔法 (魔法消除) </summary>
		public const int ERASE_MAGIC = 153;

		/// <summary>
		/// 妖精魔法 (召喚屬性精靈) </summary>
		public const int LESSER_ELEMENTAL = 154; // E:SUMMON_LESSER_ELEMENTAL

		/// <summary>
		/// 妖精魔法 (烈炎氣息) </summary>
		public const int FIRE_BLESS = 155; // E: BLESS_OF_FIRE

		/// <summary>
		/// 妖精魔法 (暴風之眼) </summary>
		public const int STORM_EYE = 156; // E: EYE_OF_STORM

		/// <summary>
		/// 妖精魔法 (大地屏障) </summary>
		public const int EARTH_BIND = 157;

		/// <summary>
		/// 妖精魔法 (生命之泉) </summary>
		public const int NATURES_TOUCH = 158;

		/// <summary>
		/// 妖精魔法 (大地的祝福) </summary>
		public const int EARTH_BLESS = 159; // E: BLESS_OF_EARTH

		/// <summary>
		/// 妖精魔法 (水之防護) </summary>
		public const int AQUA_PROTECTER = 160;

		/// <summary>
		/// 妖精魔法 (封印禁地) </summary>
		public const int AREA_OF_SILENCE = 161;

		/// <summary>
		/// 妖精魔法 (召喚強力屬性精靈) </summary>
		public const int GREATER_ELEMENTAL = 162; // E:SUMMON_GREATER_ELEMENTAL

		/// <summary>
		/// 妖精魔法 (烈炎武器) </summary>
		public const int BURNING_WEAPON = 163;

		/// <summary>
		/// 妖精魔法 (生命的祝福) </summary>
		public const int NATURES_BLESSING = 164;

		/// <summary>
		/// 妖精魔法 (生命呼喚) </summary>
		public const int CALL_OF_NATURE = 165; // E: NATURES_MIRACLE

		/// <summary>
		/// 妖精魔法 (暴風神射) </summary>
		public const int STORM_SHOT = 166;

		/// <summary>
		/// 妖精魔法 (風之枷鎖) </summary>
		public const int WIND_SHACKLE = 167;

		/// <summary>
		/// 妖精魔法 (鋼鐵防護) </summary>
		public const int IRON_SKIN = 168;

		/// <summary>
		/// 妖精魔法 (體能激發) </summary>
		public const int EXOTIC_VITALIZE = 169;

		/// <summary>
		/// 妖精魔法 (水之元氣) </summary>
		public const int WATER_LIFE = 170;

		/// <summary>
		/// 妖精魔法 (屬性之火) </summary>
		public const int ELEMENTAL_FIRE = 171;

		/// <summary>
		/// 妖精魔法 (暴風疾走) </summary>
		public const int STORM_WALK = 172;

		/// <summary>
		/// 妖精魔法 (污濁之水) </summary>
		public const int POLLUTE_WATER = 173;

		/// <summary>
		/// 妖精魔法 (精準射擊) </summary>
		public const int STRIKER_GALE = 174;

		/// <summary>
		/// 妖精魔法 (烈焰之魂) </summary>
		public const int SOUL_OF_FLAME = 175;

		/// <summary>
		/// 妖精魔法 (能量激發) </summary>
		public const int ADDITIONAL_FIRE = 176;

		// none = 177-180
		/*
		 * Dragon Knight skills
		 */
		/// <summary>
		/// 龍騎士魔法 (龍之護鎧) </summary>
		public const int DRAGON_SKIN = 181;

		/// <summary>
		/// 龍騎士魔法 (燃燒擊砍) </summary>
		public const int BURNING_SLASH = 182;

		/// <summary>
		/// 龍騎士魔法 (護衛毀滅) </summary>
		public const int GUARD_BRAKE = 183;

		/// <summary>
		/// 龍騎士魔法 (岩漿噴吐) </summary>
		public const int MAGMA_BREATH = 184;

		/// <summary>
		/// 龍騎士魔法 (覺醒：安塔瑞斯) </summary>
		public const int AWAKEN_ANTHARAS = 185;

		/// <summary>
		/// 龍騎士魔法 (血之渴望) </summary>
		public const int BLOODLUST = 186;

		/// <summary>
		/// 龍騎士魔法 (屠宰者) </summary>
		public const int FOE_SLAYER = 187;

		/// <summary>
		/// 龍騎士魔法 (恐懼無助) </summary>
		public const int RESIST_FEAR = 188;

		/// <summary>
		/// 龍騎士魔法 (衝擊之膚) </summary>
		public const int SHOCK_SKIN = 189;

		/// <summary>
		/// 龍騎士魔法 (覺醒：法利昂) </summary>
		public const int AWAKEN_FAFURION = 190;

		/// <summary>
		/// 龍騎士魔法 (致命身軀) </summary>
		public const int MORTAL_BODY = 191;

		/// <summary>
		/// 龍騎士魔法 (奪命之雷) </summary>
		public const int THUNDER_GRAB = 192;

		/// <summary>
		/// 龍騎士魔法 (驚悚死神) </summary>
		public const int HORROR_OF_DEATH = 193;

		/// <summary>
		/// 龍騎士魔法 (寒冰噴吐) </summary>
		public const int FREEZING_BREATH = 194;

		/// <summary>
		/// 龍騎士魔法 (覺醒：巴拉卡斯) </summary>
		public const int AWAKEN_VALAKAS = 195;

		// none = 196-200
		/*
		 * Illusionist Magic
		 */
		/// <summary>
		/// 幻術士魔法 (鏡像) </summary>
		public const int MIRROR_IMAGE = 201;

		/// <summary>
		/// 幻術士魔法 (混亂) </summary>
		public const int CONFUSION = 202;

		/// <summary>
		/// 幻術士魔法 (暴擊) </summary>
		public const int SMASH = 203;

		/// <summary>
		/// 幻術士魔法 (幻覺：歐吉) </summary>
		public const int ILLUSION_OGRE = 204;

		/// <summary>
		/// 幻術士魔法 (立方：燃燒) </summary>
		public const int CUBE_IGNITION = 205;

		/// <summary>
		/// 幻術士魔法 (專注) </summary>
		public const int CONCENTRATION = 206;

		/// <summary>
		/// 幻術士魔法 (心靈破壞) </summary>
		public const int MIND_BREAK = 207;

		/// <summary>
		/// 幻術士魔法 (骷髏毀壞) </summary>
		public const int BONE_BREAK = 208;

		/// <summary>
		/// 幻術士魔法 (幻覺：巫妖) </summary>
		public const int ILLUSION_LICH = 209;

		/// <summary>
		/// 幻術士魔法 (立方：地裂) </summary>
		public const int CUBE_QUAKE = 210;

		/// <summary>
		/// 幻術士魔法 (耐力) </summary>
		public const int PATIENCE = 211;

		/// <summary>
		/// 幻術士魔法 (幻想) </summary>
		public const int PHANTASM = 212;

		/// <summary>
		/// 幻術士魔法 (武器破壞者) </summary>
		public const int ARM_BREAKER = 213;

		/// <summary>
		/// 幻術士魔法 (幻覺：鑽石高侖) </summary>
		public const int ILLUSION_DIA_GOLEM = 214;

		/// <summary>
		/// 幻術士魔法 (立方：衝擊) </summary>
		public const int CUBE_SHOCK = 215;

		/// <summary>
		/// 幻術士魔法 (洞察) </summary>
		public const int INSIGHT = 216;

		/// <summary>
		/// 幻術士魔法 (恐慌) </summary>
		public const int PANIC = 217;

		/// <summary>
		/// 幻術士魔法 (疼痛的歡愉) </summary>
		public const int JOY_OF_PAIN = 218;

		/// <summary>
		/// 幻術士魔法 (幻覺：化身) </summary>
		public const int ILLUSION_AVATAR = 219;

		/// <summary>
		/// 幻術士魔法 (立方：和諧) </summary>
		public const int CUBE_BALANCE = 220;

		public const int SKILLS_END = 220;

		/*
		 * Status
		 */
		public const int STATUS_BEGIN = 1000;

		/// <summary>
		/// 二段加速 </summary>
		public const int STATUS_BRAVE = 1000;

		/// <summary>
		/// 一段加速 </summary>
		public const int STATUS_HASTE = 1001;

		public const int STATUS_BLUE_POTION = 1002;

		public const int STATUS_UNDERWATER_BREATH = 1003;

		public const int STATUS_WISDOM_POTION = 1004;

		public const int STATUS_CHAT_PROHIBITED = 1005;

		public const int STATUS_POISON = 1006;

		public const int STATUS_POISON_SILENCE = 1007;

		public const int STATUS_POISON_PARALYZING = 1008;

		public const int STATUS_POISON_PARALYZED = 1009;

		public const int STATUS_CURSE_PARALYZING = 1010;

		public const int STATUS_CURSE_PARALYZED = 1011;

		public const int STATUS_FLOATING_EYE = 1012;

		public const int STATUS_HOLY_WATER = 1013;

		public const int STATUS_HOLY_MITHRIL_POWDER = 1014;

		public const int STATUS_HOLY_WATER_OF_EVA = 1015;

		public const int STATUS_ELFBRAVE = 1016;

		public const int STATUS_RIBRAVE = 1017;

		/// <summary>
		/// 立方：燃燒(友方) </summary>
		public const int STATUS_CUBE_IGNITION_TO_ALLY = 1018;

		/// <summary>
		/// 立方：燃燒(敵方) </summary>
		public const int STATUS_CUBE_IGNITION_TO_ENEMY = 1019;

		/// <summary>
		/// 立方：地裂(友方) </summary>
		public const int STATUS_CUBE_QUAKE_TO_ALLY = 1020;

		/// <summary>
		/// 立方：地裂(敵方) </summary>
		public const int STATUS_CUBE_QUAKE_TO_ENEMY = 1021;

		/// <summary>
		/// 立方：衝擊(友方) </summary>
		public const int STATUS_CUBE_SHOCK_TO_ALLY = 1022;

		/// <summary>
		/// 立方：衝擊(敵方) </summary>
		public const int STATUS_CUBE_SHOCK_TO_ENEMY = 1023;

		public const int STATUS_MR_REDUCTION_BY_CUBE_SHOCK = 1024;

		/// <summary>
		/// 立方：和諧 </summary>
		public const int STATUS_CUBE_BALANCE = 1025;

		/// <summary>
		/// 超級加速 </summary>
		public const int STATUS_BRAVE2 = 1026;

		/// <summary>
		/// 三段加速 </summary>
		public const int STATUS_THIRD_SPEED = 1027;

		public const int STATUS_END = 1027;

		public const int GMSTATUS_BEGIN = 2000;

		public const int GMSTATUS_INVISIBLE = 2000;

		public const int GMSTATUS_HPBAR = 2001;

		public const int GMSTATUS_SHOWTRAPS = 2002;

		public const int GMSTATUS_FINDINVIS = 2003;

		public const int GMSTATUS_END = 2003;

		public const int COOKING_NOW = 2999;

		public const int COOKING_BEGIN = 3000;

		/// <summary>
		/// 漂浮之眼肉排 </summary>
		public const int COOKING_1_0_N = 3000;

		/// <summary>
		/// 烤熊肉 </summary>
		public const int COOKING_1_1_N = 3001;

		/// <summary>
		/// 煎餅 </summary>
		public const int COOKING_1_2_N = 3002;

		/// <summary>
		/// 烤螞蟻腿起司 </summary>
		public const int COOKING_1_3_N = 3003;

		/// <summary>
		/// 水果沙拉 </summary>
		public const int COOKING_1_4_N = 3004;

		/// <summary>
		/// 水果糖醋肉 </summary>
		public const int COOKING_1_5_N = 3005;

		/// <summary>
		/// 烤山豬肉串 </summary>
		public const int COOKING_1_6_N = 3006;

		/// <summary>
		/// 蘑菇湯 </summary>
		public const int COOKING_1_7_N = 3007;

		/// <summary>
		/// 特別的漂浮之眼肉排 </summary>
		public const int COOKING_1_0_S = 3008;

		/// <summary>
		/// 特別的烤熊肉 </summary>
		public const int COOKING_1_1_S = 3009;

		/// <summary>
		/// 特別的煎餅 </summary>
		public const int COOKING_1_2_S = 3010;

		/// <summary>
		/// 特別的烤螞蟻腿起司 </summary>
		public const int COOKING_1_3_S = 3011;

		/// <summary>
		/// 特別的水果沙拉 </summary>
		public const int COOKING_1_4_S = 3012;

		/// <summary>
		/// 特別的水果糖醋肉 </summary>
		public const int COOKING_1_5_S = 3013;

		/// <summary>
		/// 特別的烤山豬肉串 </summary>
		public const int COOKING_1_6_S = 3014;

		/// <summary>
		/// 特別的蘑菇湯 </summary>
		public const int COOKING_1_7_S = 3015;

		/// <summary>
		/// 魚子醬 </summary>
		public const int COOKING_2_0_N = 3016;

		/// <summary>
		/// 鱷魚肉排 </summary>
		public const int COOKING_2_1_N = 3017;

		/// <summary>
		/// 龍龜蛋餅乾 </summary>
		public const int COOKING_2_2_N = 3018;

		/// <summary>
		/// 烤奇異鸚鵡 </summary>
		public const int COOKING_2_3_N = 3019;

		/// <summary>
		/// 毒蠍串燒 </summary>
		public const int COOKING_2_4_N = 3020;

		/// <summary>
		/// 燉伊萊克頓 </summary>
		public const int COOKING_2_5_N = 3021;

		/// <summary>
		/// 蜘蛛腿串燒 </summary>
		public const int COOKING_2_6_N = 3022;

		/// <summary>
		/// 蟹肉湯 </summary>
		public const int COOKING_2_7_N = 3023;

		/// <summary>
		/// 特別的魚子醬 </summary>
		public const int COOKING_2_0_S = 3024;

		/// <summary>
		/// 特別的鱷魚肉排 </summary>
		public const int COOKING_2_1_S = 3025;

		/// <summary>
		/// 特別的龍龜蛋餅乾 </summary>
		public const int COOKING_2_2_S = 3026;

		/// <summary>
		/// 特別的烤奇異鸚鵡 </summary>
		public const int COOKING_2_3_S = 3027;

		/// <summary>
		/// 特別的毒蠍串燒 </summary>
		public const int COOKING_2_4_S = 3028;

		/// <summary>
		/// 特別的燉伊萊克頓 </summary>
		public const int COOKING_2_5_S = 3029;

		/// <summary>
		/// 特別的蜘蛛腿串燒 </summary>
		public const int COOKING_2_6_S = 3030;

		/// <summary>
		/// 特別的蟹肉湯 </summary>
		public const int COOKING_2_7_S = 3031;

		/// <summary>
		/// 烤奎斯坦修的螯 </summary>
		public const int COOKING_3_0_N = 3032;

		/// <summary>
		/// 烤格利芬肉 </summary>
		public const int COOKING_3_1_N = 3033;

		/// <summary>
		/// 亞力安的尾巴肉排 </summary>
		public const int COOKING_3_2_N = 3034;

		/// <summary>
		/// 烤巨王龜肉 </summary>
		public const int COOKING_3_3_N = 3035;

		/// <summary>
		/// 幼龍翅膀串燒 </summary>
		public const int COOKING_3_4_N = 3036;

		/// <summary>
		/// 烤飛龍肉 </summary>
		public const int COOKING_3_5_N = 3037;

		/// <summary>
		/// 燉深海魚肉 </summary>
		public const int COOKING_3_6_N = 3038;

		/// <summary>
		/// 邪惡蜥蜴蛋湯 </summary>
		public const int COOKING_3_7_N = 3039;

		/// <summary>
		/// 特別的烤奎斯坦修的螯 </summary>
		public const int COOKING_3_0_S = 3040;

		/// <summary>
		/// 特別的烤格利芬肉 </summary>
		public const int COOKING_3_1_S = 3041;

		/// <summary>
		/// 特別的亞力安的尾巴肉排 </summary>
		public const int COOKING_3_2_S = 3042;

		/// <summary>
		/// 特別的烤巨王龜肉 </summary>
		public const int COOKING_3_3_S = 3043;

		/// <summary>
		/// 特別的幼龍翅膀串燒 </summary>
		public const int COOKING_3_4_S = 3044;

		/// <summary>
		/// 特別的烤飛龍肉 </summary>
		public const int COOKING_3_5_S = 3045;

		/// <summary>
		/// 特別的燉深海魚肉 </summary>
		public const int COOKING_3_6_S = 3046;

		/// <summary>
		/// 特別的邪惡蜥蜴蛋湯 </summary>
		public const int COOKING_3_7_S = 3047;

		/// <summary>
		/// 象牙塔妙藥 </summary>
		public const int COOKING_WONDER_DRUG = 3048;

		public const int COOKING_END = 3048;

		public const int STATUS_FREEZE = 10071;

		public const int CURSE_PARALYZE2 = 10101;

		// 編號待修正 (可攻擊炎魔、火焰之影狀態)
		public const int STATUS_CURSE_BARLOG = 1015;

		public const int STATUS_CURSE_YAHEE = 1014;

		// 相消無法消除的狀態

		public const int EFFECT_BEGIN = 4001;

		/// <summary>
		/// 神力藥水150% </summary>
		public const int EFFECT_POTION_OF_EXP_150 = 4001;

		/// <summary>
		/// 神力藥水175% </summary>
		public const int EFFECT_POTION_OF_EXP_175 = 4002;

		/// <summary>
		/// 神力藥水200% </summary>
		public const int EFFECT_POTION_OF_EXP_200 = 4003;

		/// <summary>
		/// 神力藥水225% </summary>
		public const int EFFECT_POTION_OF_EXP_225 = 4004;

		/// <summary>
		/// 神力藥水250% </summary>
		public const int EFFECT_POTION_OF_EXP_250 = 4005;

		/// <summary>
		/// 媽祖的祝福 </summary>
		public const int EFFECT_BLESS_OF_MAZU = 4006;

		/// <summary>
		/// 戰鬥藥水 </summary>
		public const int EFFECT_POTION_OF_BATTLE = 4007;

		/// <summary>
		/// 體力增強卷軸 </summary>
		public const int EFFECT_STRENGTHENING_HP = 4008;

		/// <summary>
		/// 魔力增強卷軸 </summary>
		public const int EFFECT_STRENGTHENING_MP = 4009;

		/// <summary>
		/// 強化戰鬥卷軸 </summary>
		public const int EFFECT_ENCHANTING_BATTLE = 4010;

		/// <summary>
		/// 安塔瑞斯的血痕 </summary>
		public const int EFFECT_BLOODSTAIN_OF_ANTHARAS = 4011;

		/// <summary>
		/// 法利昂的血痕 </summary>
		public const int EFFECT_BLOODSTAIN_OF_FAFURION = 4012;

		public const int EFFECT_MAGIC_STONE_A_1 = 4013; // 1階(近戰)

		public const int EFFECT_MAGIC_STONE_A_2 = 4014; // 2階(近戰)

		public const int EFFECT_MAGIC_STONE_A_3 = 4015; // 3階(近戰)

		public const int EFFECT_MAGIC_STONE_A_4 = 4016; // 4階(近戰)

		public const int EFFECT_MAGIC_STONE_A_5 = 4017; // 5階(近戰)

		public const int EFFECT_MAGIC_STONE_A_6 = 4018; // 6階(近戰)

		public const int EFFECT_MAGIC_STONE_A_7 = 4019; // 7階(近戰)

		public const int EFFECT_MAGIC_STONE_A_8 = 4020; // 8階(近戰)

		public const int EFFECT_MAGIC_STONE_A_9 = 4021; // 9階(近戰)

		public const int EFFECT_MAGIC_STONE_B_1 = 4022; // 1階(遠攻)

		public const int EFFECT_MAGIC_STONE_B_2 = 4023; // 2階(遠攻)

		public const int EFFECT_MAGIC_STONE_B_3 = 4024; // 3階(遠攻)

		public const int EFFECT_MAGIC_STONE_B_4 = 4025; // 4階(遠攻)

		public const int EFFECT_MAGIC_STONE_B_5 = 4026; // 5階(遠攻)

		public const int EFFECT_MAGIC_STONE_B_6 = 4027; // 6階(遠攻)

		public const int EFFECT_MAGIC_STONE_B_7 = 4028; // 7階(遠攻)

		public const int EFFECT_MAGIC_STONE_B_8 = 4029; // 8階(遠攻)

		public const int EFFECT_MAGIC_STONE_B_9 = 4030; // 9階(遠攻)

		public const int EFFECT_MAGIC_STONE_C_1 = 4031; // 1階(恢復)

		public const int EFFECT_MAGIC_STONE_C_2 = 4032; // 2階(恢復)

		public const int EFFECT_MAGIC_STONE_C_3 = 4033; // 3階(恢復)

		public const int EFFECT_MAGIC_STONE_C_4 = 4034; // 4階(恢復)

		public const int EFFECT_MAGIC_STONE_C_5 = 4035; // 5階(恢復)

		public const int EFFECT_MAGIC_STONE_C_6 = 4036; // 6階(恢復)

		public const int EFFECT_MAGIC_STONE_C_7 = 4037; // 7階(恢復)

		public const int EFFECT_MAGIC_STONE_C_8 = 4038; // 8階(恢復)

		public const int EFFECT_MAGIC_STONE_C_9 = 4039; // 9階(恢復)

		public const int EFFECT_MAGIC_STONE_D_1 = 4040; // 1階(防禦)

		public const int EFFECT_MAGIC_STONE_D_2 = 4041; // 2階(防禦)

		public const int EFFECT_MAGIC_STONE_D_3 = 4042; // 3階(防禦)

		public const int EFFECT_MAGIC_STONE_D_4 = 4043; // 4階(防禦)

		public const int EFFECT_MAGIC_STONE_D_5 = 4044; // 5階(防禦)

		public const int EFFECT_MAGIC_STONE_D_6 = 4045; // 6階(防禦)

		public const int EFFECT_MAGIC_STONE_D_7 = 4046; // 7階(防禦)

		public const int EFFECT_MAGIC_STONE_D_8 = 4047; // 8階(防禦)

		public const int EFFECT_MAGIC_STONE_D_9 = 4048; // 9階(防禦)

		/// <summary>
		/// 地龍之魔眼 </summary>
		public const int EFFECT_MAGIC_EYE_OF_AHTHARTS = 4049;

		/// <summary>
		/// 水龍之魔眼 </summary>
		public const int EFFECT_MAGIC_EYE_OF_FAFURION = 4050;

		/// <summary>
		/// 風龍之魔眼 </summary>
		public const int EFFECT_MAGIC_EYE_OF_LINDVIOR = 4051;

		/// <summary>
		/// 火龍之魔眼 </summary>
		public const int EFFECT_MAGIC_EYE_OF_VALAKAS = 4052;

		/// <summary>
		/// 誕生之魔眼 </summary>
		public const int EFFECT_MAGIC_EYE_OF_BIRTH = 4053;

		/// <summary>
		/// 形象之魔眼 </summary>
		public const int EFFECT_MAGIC_EYE_OF_FIGURE = 4054;

		/// <summary>
		/// 生命之魔眼 </summary>
		public const int EFFECT_MAGIC_EYE_OF_LIFE = 4055;

		/// <summary>
		/// 卡瑞的祝福 </summary>
		public const int EFFECT_BLESS_OF_CRAY = 4056;

		/// <summary>
		/// 莎爾的祝福 </summary>
		public const int EFFECT_BLESS_OF_SAELL = 4057;

		public const int EFFECT_END = 4057;

		// 特殊狀態
		public const int SPECIAL_EFFECT_BEGIN = 5001;

		/// <summary>
		/// 鎖鏈劍 (弱點曝光 LV1) * </summary>
		public const int SPECIAL_EFFECT_WEAKNESS_LV1 = 5001;

		/// <summary>
		/// 鎖鏈劍 (弱點曝光 LV2) * </summary>
		public const int SPECIAL_EFFECT_WEAKNESS_LV2 = 5002;

		/// <summary>
		/// 鎖鏈劍 (弱點曝光 LV3) * </summary>
		public const int SPECIAL_EFFECT_WEAKNESS_LV3 = 5003;

		/// <summary>
		/// 骷髏毀壞 (發動) * </summary>
		public const int BONE_BREAK_START = 5004;

		/// <summary>
		/// 骷髏毀壞 (結束) * </summary>
		public const int BONE_BREAK_END = 5005;

		/// <summary>
		/// 混亂 (發動中) </summary>
		public const int CONFUSION_ING = 5006;

		/// <summary>
		/// 奪命之雷 (發動) </summary>
		public const int THUNDER_GRAB_START = 5007;

		/// <summary>
		/// 破壞之密藥 </summary>
		public const int SECRET_MEDICINE_OF_DESTRUCTION = 5008;

		public const int SPECIAL_EFFECT_END = 5008;

		// 戰鬥特化狀態
		/// <summary>
		/// 新手保護(遭遇的守護) * </summary>
		public const int STATUS_NOVICE = 8000;

		public const int MAGIC_STONE = 6001;

		/// <summary>
		/// 殷海薩的祝福 </summary>
		public const int EXP_POTION = 7013;

		public const int MAGIC_STONE_END = 6036;

		// 怪物增加
		/// <summary>
		/// 亞力安冰矛圍籬 * </summary>
		public const int ICE_LANCE_COCKATRICE = 15003;

		/// <summary>
		/// 邪惡蜥蜴冰矛圍籬 * </summary>
		public const int ICE_LANCE_BASILISK = 15004;

		public const int POLY_EFFECT = 15566;

		public const int SPEED_EFFECT = 18333;
		/// <summary>
		/// 毒霧，前方3X3
		/// </summary>
		public const int AREA_POISON = 20011;
	}

}