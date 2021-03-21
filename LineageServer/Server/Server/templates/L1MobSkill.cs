using System;

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
namespace LineageServer.Server.Server.Templates
{
	public class L1MobSkill : ICloneable
	{
		public const int TYPE_NONE = 0;

		public const int TYPE_PHYSICAL_ATTACK = 1;

		public const int TYPE_MAGIC_ATTACK = 2;

		public const int TYPE_SUMMON = 3;

		public const int TYPE_POLY = 4;

		public const int CHANGE_TARGET_NO = 0;

		public const int CHANGE_TARGET_COMPANION = 1;

		public const int CHANGE_TARGET_ME = 2;

		public const int CHANGE_TARGET_RANDOM = 3;

		private readonly int skillSize;

		public override L1MobSkill clone()
		{
			try
			{
				return (L1MobSkill)(base.clone());
			}
			catch (CloneNotSupportedException e)
			{
				throw (new InternalError(e.Message));
			}
		}

		public virtual int SkillSize
		{
			get
			{
				return skillSize;
			}
		}

		public L1MobSkill(int sSize)
		{
			skillSize = sSize;

			type = new int[skillSize];
			mpConsume = new int[skillSize];
			triRnd = new int[skillSize];
			triHp = new int[skillSize];
			triCompanionHp = new int[skillSize];
			triRange = new int[skillSize];
			triCount = new int[skillSize];
			changeTarget = new int[skillSize];
			range = new int[skillSize];
			areaWidth = new int[skillSize];
			areaHeight = new int[skillSize];
			leverage = new int[skillSize];
			skillId = new int[skillSize];
			skillArea = new int[skillSize];
			gfxid = new int[skillSize];
			actid = new int[skillSize];
			summon = new int[skillSize];
			summonMin = new int[skillSize];
			summonMax = new int[skillSize];
			polyId = new int[skillSize];
		}

		private int mobid;

		public virtual int get_mobid()
		{
			return mobid;
		}

		public virtual void set_mobid(int i)
		{
			mobid = i;
		}

		private string mobName;

		public virtual string MobName
		{
			get
			{
				return mobName;
			}
			set
			{
				mobName = value;
			}
		}


		/*
		 * スキルのタイプ 0→何もしない、1→物理攻撃、2→魔法攻撃、3→サモン
		 */
		private int[] type;

		public virtual int getType(int idx)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return 0;
			}
			return type[idx];
		}

		public virtual void setType(int idx, int i)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return;
			}
			type[idx] = i;
		}

		/*
		 * 技能範圍設定
		 */
		internal int[] skillArea;

		public virtual int getSkillArea(int idx)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return 0;
			}
			return skillArea[idx];
		}

		public virtual void setSkillArea(int idx, int i)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return;
			}
			skillArea[idx] = i;
		}

		/*
		 * 魔力消耗判斷
		 */
		internal int[] mpConsume;

		public virtual int getMpConsume(int idx)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return 0;
			}
			return mpConsume[idx];
		}

		public virtual void setMpConsume(int idx, int i)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return;
			}
			mpConsume[idx] = i;
		}

		/*
		 * スキル発動条件：ランダムな確率（0%～100%）でスキル発動
		 */
		private int[] triRnd;

		public virtual int getTriggerRandom(int idx)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return 0;
			}
			return triRnd[idx];
		}

		public virtual void setTriggerRandom(int idx, int i)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return;
			}
			triRnd[idx] = i;
		}

		/*
		 * スキル発動条件：HPが%以下で発動
		 */
		internal int[] triHp;

		public virtual int getTriggerHp(int idx)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return 0;
			}
			return triHp[idx];
		}

		public virtual void setTriggerHp(int idx, int i)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return;
			}
			triHp[idx] = i;
		}

		/*
		 * スキル発動条件：同族のHPが%以下で発動
		 */
		internal int[] triCompanionHp;

		public virtual int getTriggerCompanionHp(int idx)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return 0;
			}
			return triCompanionHp[idx];
		}

		public virtual void setTriggerCompanionHp(int idx, int i)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return;
			}
			triCompanionHp[idx] = i;
		}

		/*
		 * スキル発動条件：triRange<0の場合、対象との距離がabs(triRange)以下のとき発動
		 * triRange>0の場合、対象との距離がtriRange以上のとき発動
		 */
		internal int[] triRange;

		public virtual int getTriggerRange(int idx)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return 0;
			}
			return triRange[idx];
		}

		public virtual void setTriggerRange(int idx, int i)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return;
			}
			triRange[idx] = i;
		}

		// distanceが指定idxスキルの発動条件を満たしているか
		public virtual bool isTriggerDistance(int idx, int distance)
		{
			int triggerRange = getTriggerRange(idx);

			if ((triggerRange < 0 && distance <= Math.Abs(triggerRange)) || (triggerRange > 0 && distance >= triggerRange))
			{
				return true;
			}
			return false;
		}

		internal int[] triCount;

		/*
		 * スキル発動条件：スキルの発動回数がtriCount以下のとき発動
		 */
		public virtual int getTriggerCount(int idx)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return 0;
			}
			return triCount[idx];
		}

		public virtual void setTriggerCount(int idx, int i)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return;
			}
			triCount[idx] = i;
		}

		/*
		 * スキル発動時、ターゲットを変更するか
		 */
		internal int[] changeTarget;

		public virtual int getChangeTarget(int idx)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return 0;
			}
			return changeTarget[idx];
		}

		public virtual void setChangeTarget(int idx, int i)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return;
			}
			changeTarget[idx] = i;
		}

		/*
		 * rangeまでの距離ならば攻撃可能、物理攻撃をするならば近接攻撃の場合でも1以上を設定
		 */
		internal int[] range;

		public virtual int getRange(int idx)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return 0;
			}
			return range[idx];
		}

		public virtual void setRange(int idx, int i)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return;
			}
			range[idx] = i;
		}

		/*
		 * 範囲攻撃の横幅、単体攻撃ならば0を設定、範囲攻撃するならば0以上を設定
		 * WidthとHeightの設定は攻撃者からみて横幅をWidth、奥行きをHeightとする。
		 * Widthは+-あるので、1を指定すれば、ターゲットを中心として左右1までが対象となる。
		 */
		internal int[] areaWidth;

		public virtual int getAreaWidth(int idx)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return 0;
			}
			return areaWidth[idx];
		}

		public virtual void setAreaWidth(int idx, int i)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return;
			}
			areaWidth[idx] = i;
		}

		/*
		 * 範囲攻撃の高さ、単体攻撃ならば0を設定、範囲攻撃するならば1以上を設定
		 */
		internal int[] areaHeight;

		public virtual int getAreaHeight(int idx)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return 0;
			}
			return areaHeight[idx];
		}

		public virtual void setAreaHeight(int idx, int i)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return;
			}
			areaHeight[idx] = i;
		}

		/*
		 * ダメージの倍率、1/10で表す。物理攻撃、魔法攻撃共に有効
		 */
		internal int[] leverage;

		public virtual int getLeverage(int idx)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return 0;
			}
			return leverage[idx];
		}

		public virtual void setLeverage(int idx, int i)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return;
			}
			leverage[idx] = i;
		}

		/*
		 * 魔法を使う場合、SkillIdを指定
		 */
		internal int[] skillId;

		public virtual int getSkillId(int idx)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return 0;
			}
			return skillId[idx];
		}

		public virtual void setSkillId(int idx, int i)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return;
			}
			skillId[idx] = i;
		}

		/*
		 * 物理攻撃のモーショングラフィック
		 */
		internal int[] gfxid;

		public virtual int getGfxid(int idx)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return 0;
			}
			return gfxid[idx];
		}

		public virtual void setGfxid(int idx, int i)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return;
			}
			gfxid[idx] = i;
		}

		/*
		 * 物理攻撃のグラフィックのアクションID
		 */
		internal int[] actid;

		public virtual int getActid(int idx)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return 0;
			}
			return actid[idx];
		}

		public virtual void setActid(int idx, int i)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return;
			}
			actid[idx] = i;
		}

		/*
		 * サモンするモンスターのNPCID
		 */
		internal int[] summon;

		public virtual int getSummon(int idx)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return 0;
			}
			return summon[idx];
		}

		public virtual void setSummon(int idx, int i)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return;
			}
			summon[idx] = i;
		}

		/*
		 * サモンするモンスターの最少数
		 */
		internal int[] summonMin;

		public virtual int getSummonMin(int idx)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return 0;
			}
			return summonMin[idx];
		}

		public virtual void setSummonMin(int idx, int i)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return;
			}
			summonMin[idx] = i;
		}

		/*
		 * サモンするモンスターの最大数
		 */
		internal int[] summonMax;

		public virtual int getSummonMax(int idx)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return 0;
			}
			return summonMax[idx];
		}

		public virtual void setSummonMax(int idx, int i)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return;
			}
			summonMax[idx] = i;
		}

		/*
		 * 何に強制変身させるか
		 */
		internal int[] polyId;

		public virtual int getPolyId(int idx)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return 0;
			}
			return polyId[idx];
		}

		public virtual void setPolyId(int idx, int i)
		{
			if (idx < 0 || idx >= SkillSize)
			{
				return;
			}
			polyId[idx] = i;
		}
	}

}