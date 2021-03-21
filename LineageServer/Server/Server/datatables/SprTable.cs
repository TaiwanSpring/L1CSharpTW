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
namespace LineageServer.Server.Server.datatables
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_Aggress;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_AltAttack;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_Attack;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_AxeAttack;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_AxeWalk;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_BowAttack;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_BowWalk;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_ClawAttack;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_ClawWalk;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_DaggerAttack;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_DaggerWalk;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_EdoryuAttack;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_EdoryuWalk;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_SkillAttack;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_SkillBuff;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_SpearAttack;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_SpearWalk;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_SpellDirectionExtra;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_StaffAttack;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_StaffWalk;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_SwordAttack;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_SwordWalk;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_Think;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_ThrowingKnifeAttack;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_ThrowingKnifeWalk;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_TwoHandSwordAttack;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_TwoHandSwordWalk;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.ActionCodes.ACTION_Walk;


	using L1DatabaseFactory = LineageServer.Server.L1DatabaseFactory;
	using SQLUtil = LineageServer.Server.Server.utils.SQLUtil;
	using Maps = LineageServer.Server.Server.utils.collections.Maps;

	public class SprTable
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(SprTable).FullName);

		private class Spr
		{
			internal readonly IDictionary<int, int> moveSpeed = Maps.newMap();

			internal readonly IDictionary<int, int> attackSpeed = Maps.newMap();

			internal readonly IDictionary<int, int> specialSpeed = Maps.newMap();

			internal int nodirSpellSpeed = 1200;

			internal int dirSpellSpeed = 1200;
		}

		private static readonly IDictionary<int, Spr> _dataMap = Maps.newMap();

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
			Connection con = null;
			PreparedStatement pstm = null;
			ResultSet rs = null;
			Spr spr = null;
			try
			{
				con = L1DatabaseFactory.Instance.Connection;
				pstm = con.prepareStatement("SELECT * FROM spr_action");
				rs = pstm.executeQuery();
				while (rs.next())
				{
					int key = rs.getInt("spr_id");
					if (!_dataMap.ContainsKey(key))
					{
						spr = new Spr();
						_dataMap[key] = spr;
					}
					else
					{
						spr = _dataMap[key];
					}

					int actid = rs.getInt("act_id");
					int frameCount = rs.getInt("framecount");
					int frameRate = rs.getInt("framerate");
					int speed = calcActionSpeed(frameCount, frameRate);

					switch (actid)
					{
						case ACTION_Walk:
						case ACTION_SwordWalk:
						case ACTION_AxeWalk:
						case ACTION_BowWalk:
						case ACTION_SpearWalk:
						case ACTION_StaffWalk:
						case ACTION_DaggerWalk:
						case ACTION_TwoHandSwordWalk:
						case ACTION_EdoryuWalk:
						case ACTION_ClawWalk:
						case ACTION_ThrowingKnifeWalk:
							spr.moveSpeed[actid] = speed;
							break;
						case ACTION_SkillAttack:
							spr.dirSpellSpeed = speed;
							break;
						case ACTION_SkillBuff:
							spr.nodirSpellSpeed = speed;
							break;
						case ACTION_Attack:
						case ACTION_SwordAttack:
						case ACTION_AxeAttack:
						case ACTION_BowAttack:
						case ACTION_SpearAttack:
						case ACTION_AltAttack:
						case ACTION_SpellDirectionExtra:
						case ACTION_StaffAttack:
						case ACTION_DaggerAttack:
						case ACTION_TwoHandSwordAttack:
						case ACTION_EdoryuAttack:
						case ACTION_ClawAttack:
						case ACTION_ThrowingKnifeAttack:
							spr.attackSpeed[actid] = speed;
							break;
						case ACTION_Think:
						case ACTION_Aggress:
							spr.specialSpeed[actid] = speed;
							break;
						default:
							break;
					}
				}
			}
			catch (SQLException e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			finally
			{
				SQLUtil.close(rs);
				SQLUtil.close(pstm);
				SQLUtil.close(con);
			}
			_log.config("SPRデータ " + _dataMap.Count + "件ロード");
		}

		/// <summary>
		/// フレーム数とフレームレートからアクションの合計時間(ms)を計算して返す。
		/// </summary>
		private int calcActionSpeed(int frameCount, int frameRate)
		{
			return (int)(frameCount * 40 * (24D / frameRate));
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
				else if (actid == ACTION_Attack)
				{
					return 0;
				}
				else
				{
					return _dataMap[sprid].attackSpeed[ACTION_Attack];
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
				else if (actid == ACTION_Walk)
				{
					return 0;
				}
				else
				{
					return _dataMap[sprid].moveSpeed[ACTION_Walk];
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
				case ACTION_Walk:
				case ACTION_SwordWalk:
				case ACTION_AxeWalk:
				case ACTION_BowWalk:
				case ACTION_SpearWalk:
				case ACTION_StaffWalk:
				case ACTION_DaggerWalk:
				case ACTION_TwoHandSwordWalk:
				case ACTION_EdoryuWalk:
				case ACTION_ClawWalk:
				case ACTION_ThrowingKnifeWalk:
					// 移動
					return getMoveSpeed(sprid, actid);
				case ACTION_SkillAttack:
					// 有向施法
					return getDirSpellSpeed(sprid);
				case ACTION_SkillBuff:
					// 無向施法
					return getNodirSpellSpeed(sprid);
				case ACTION_Attack:
				case ACTION_SwordAttack:
				case ACTION_AxeAttack:
				case ACTION_BowAttack:
				case ACTION_SpearAttack:
				case ACTION_AltAttack:
				case ACTION_SpellDirectionExtra:
				case ACTION_StaffAttack:
				case ACTION_DaggerAttack:
				case ACTION_TwoHandSwordAttack:
				case ACTION_EdoryuAttack:
				case ACTION_ClawAttack:
				case ACTION_ThrowingKnifeAttack:
					// 攻擊
					return getAttackSpeed(sprid, actid);
				case ACTION_Think:
				case ACTION_Aggress:
					// 魔法娃娃表情動作
					return getSpecialSpeed(sprid, actid);
				default:
					break;
			}
			return 0;
		}
	}

}