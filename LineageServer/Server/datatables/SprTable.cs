using LineageServer.DataBase.DataSources;
using LineageServer.Interfaces;
using LineageServer.Utils;
using System.Collections.Generic;
namespace LineageServer.Server.DataTables
{
	class SprTable
	{
		private readonly static IDataSource dataSource =
			Container.Instance.Resolve<IDataSourceFactory>()
			.Factory(Enum.DataSourceTypeEnum.SprAction);

		private static readonly IDictionary<int, Spr> _dataMap = MapFactory.NewMap<int, Spr>();

		private static readonly SprTable _instance = new SprTable();

		private SprTable()
		{
			loadSprAction();
		}

		public static SprTable Instance
		{
			get
			{
				return _instance;
			}
		}

		/// <summary>
		/// spr_actionテーブルをロードする。
		/// </summary>
		public virtual void loadSprAction()
		{
			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

			Spr spr;

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];
				int key = dataSourceRow.getInt(SprAction.Column_spr_id);

				if (_dataMap.ContainsKey(key))
				{
					spr = _dataMap[key];
				}
				else
				{
					spr = new Spr();
					_dataMap[key] = spr;
				}

				int actid = dataSourceRow.getInt(SprAction.Column_act_id);
				int frameCount = dataSourceRow.getInt(SprAction.Column_framecount);
				int frameRate = dataSourceRow.getInt(SprAction.Column_framerate);
				int speed = calcActionSpeed(frameCount, frameRate);

				switch (actid)
				{
					case ActionCodes.ACTION_Walk:
					case ActionCodes.ACTION_SwordWalk:
					case ActionCodes.ACTION_AxeWalk:
					case ActionCodes.ACTION_BowWalk:
					case ActionCodes.ACTION_SpearWalk:
					case ActionCodes.ACTION_StaffWalk:
					case ActionCodes.ACTION_DaggerWalk:
					case ActionCodes.ACTION_TwoHandSwordWalk:
					case ActionCodes.ACTION_EdoryuWalk:
					case ActionCodes.ACTION_ClawWalk:
					case ActionCodes.ACTION_ThrowingKnifeWalk:
						spr.moveSpeed[actid] = speed;
						break;
					case ActionCodes.ACTION_SkillAttack:
						spr.dirSpellSpeed = speed;
						break;
					case ActionCodes.ACTION_SkillBuff:
						spr.nodirSpellSpeed = speed;
						break;
					case ActionCodes.ACTION_Attack:
					case ActionCodes.ACTION_SwordAttack:
					case ActionCodes.ACTION_AxeAttack:
					case ActionCodes.ACTION_BowAttack:
					case ActionCodes.ACTION_SpearAttack:
					case ActionCodes.ACTION_AltAttack:
					case ActionCodes.ACTION_SpellDirectionExtra:
					case ActionCodes.ACTION_StaffAttack:
					case ActionCodes.ACTION_DaggerAttack:
					case ActionCodes.ACTION_TwoHandSwordAttack:
					case ActionCodes.ACTION_EdoryuAttack:
					case ActionCodes.ACTION_ClawAttack:
					case ActionCodes.ACTION_ThrowingKnifeAttack:
						spr.attackSpeed[actid] = speed;
						break;
					case ActionCodes.ACTION_Think:
					case ActionCodes.ACTION_Aggress:
						spr.specialSpeed[actid] = speed;
						break;
					default:
						break;
				}
			}
		}

		/// <summary>
		/// フレーム数とフレームレートからアクションの合計時間(ms)を計算して返す。
		/// </summary>
		private int calcActionSpeed(int frameCount, int frameRate)
		{
			return (int)( frameCount * 40 * ( 24D / frameRate ) );
		}

		/// <summary>
		/// 指定されたsprの攻撃速度を返す。もしsprに指定されたweapon_typeのデータが 設定されていない場合は、1.attackのデータを返す。
		/// </summary>
		/// <param name="sprid">
		///            - 調べるsprのID </param>
		/// <param name="actid">
		///            - 武器の種類を表す値。L1Item.getType1()の返り値 + 1 と一致する </param>
		/// <returns> 指定されたsprの攻撃速度(ms) </returns>
		public virtual int getAttackSpeed(int sprid, int actid)
		{
			if (_dataMap.ContainsKey(sprid))
			{
				if (_dataMap[sprid].attackSpeed.ContainsKey(actid))
				{
					return _dataMap[sprid].attackSpeed[actid];
				}
				else if (actid == ActionCodes.ACTION_Attack)
				{
					return 0;
				}
				else
				{
					return _dataMap[sprid].attackSpeed[ActionCodes.ACTION_Attack];
				}
			}
			return 0;
		}

		public virtual int getMoveSpeed(int sprid, int actid)
		{
			if (_dataMap.ContainsKey(sprid))
			{
				if (_dataMap[sprid].moveSpeed.ContainsKey(actid))
				{
					return _dataMap[sprid].moveSpeed[actid];
				}
				else if (actid == ActionCodes.ACTION_Walk)
				{
					return 0;
				}
				else
				{
					return _dataMap[sprid].moveSpeed[ActionCodes.ACTION_Walk];
				}
			}
			return 0;
		}

		public virtual int getDirSpellSpeed(int sprid)
		{
			if (_dataMap.ContainsKey(sprid))
			{
				return _dataMap[sprid].dirSpellSpeed;
			}
			return 0;
		}

		public virtual int getNodirSpellSpeed(int sprid)
		{
			if (_dataMap.ContainsKey(sprid))
			{
				return _dataMap[sprid].nodirSpellSpeed;
			}
			return 0;
		}

		public virtual int getSpecialSpeed(int sprid, int actid)
		{
			if (_dataMap.ContainsKey(sprid))
			{
				if (_dataMap[sprid].specialSpeed.ContainsKey(actid))
				{
					return _dataMap[sprid].specialSpeed[actid];
				}
				else
				{
					return 1200;
				}
			}
			return 0;
		}

		/// <summary>
		/// Npc 各動作延遲時間 </summary>
		public virtual int getSprSpeed(int sprid, int actid)
		{
			switch (actid)
			{
				case ActionCodes.ACTION_Walk:
				case ActionCodes.ACTION_SwordWalk:
				case ActionCodes.ACTION_AxeWalk:
				case ActionCodes.ACTION_BowWalk:
				case ActionCodes.ACTION_SpearWalk:
				case ActionCodes.ACTION_StaffWalk:
				case ActionCodes.ACTION_DaggerWalk:
				case ActionCodes.ACTION_TwoHandSwordWalk:
				case ActionCodes.ACTION_EdoryuWalk:
				case ActionCodes.ACTION_ClawWalk:
				case ActionCodes.ACTION_ThrowingKnifeWalk:
					// 移動
					return getMoveSpeed(sprid, actid);
				case ActionCodes.ACTION_SkillAttack:
					// 有向施法
					return getDirSpellSpeed(sprid);
				case ActionCodes.ACTION_SkillBuff:
					// 無向施法
					return getNodirSpellSpeed(sprid);
				case ActionCodes.ACTION_Attack:
				case ActionCodes.ACTION_SwordAttack:
				case ActionCodes.ACTION_AxeAttack:
				case ActionCodes.ACTION_BowAttack:
				case ActionCodes.ACTION_SpearAttack:
				case ActionCodes.ACTION_AltAttack:
				case ActionCodes.ACTION_SpellDirectionExtra:
				case ActionCodes.ACTION_StaffAttack:
				case ActionCodes.ACTION_DaggerAttack:
				case ActionCodes.ACTION_TwoHandSwordAttack:
				case ActionCodes.ACTION_EdoryuAttack:
				case ActionCodes.ACTION_ClawAttack:
				case ActionCodes.ACTION_ThrowingKnifeAttack:
					// 攻擊
					return getAttackSpeed(sprid, actid);
				case ActionCodes.ACTION_Think:
				case ActionCodes.ACTION_Aggress:
					// 魔法娃娃表情動作
					return getSpecialSpeed(sprid, actid);
				default:
					break;
			}
			return 0;
		}

		private class Spr
		{
			public readonly IDictionary<int, int> moveSpeed = MapFactory.NewMap<int, int>();

			public readonly IDictionary<int, int> attackSpeed = MapFactory.NewMap<int, int>();

			public readonly IDictionary<int, int> specialSpeed = MapFactory.NewMap<int, int>();

			public int nodirSpellSpeed = 1200;

			public int dirSpellSpeed = 1200;
		}
	}
}