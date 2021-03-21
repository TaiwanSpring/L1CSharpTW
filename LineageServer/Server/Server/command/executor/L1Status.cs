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
namespace LineageServer.Server.Server.command.executor
{

	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_Lawful = LineageServer.Server.Server.serverpackets.S_Lawful;
	using S_OwnCharStatus = LineageServer.Server.Server.serverpackets.S_OwnCharStatus;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;

	public class L1Status : L1CommandExecutor
	{
		private L1Status()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1Status();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				StringTokenizer st = new StringTokenizer(arg);
				string char_name = st.nextToken();
				string param = st.nextToken();
				int value = int.Parse(st.nextToken());

				L1PcInstance target = null;
				if (char_name.Equals("me", StringComparison.OrdinalIgnoreCase))
				{
					target = pc;
				}
				else
				{
					target = L1World.Instance.getPlayer(char_name);
				}

				if (target == null)
				{
					pc.sendPackets(new S_ServerMessage(73, char_name)); // \f1%0はゲームをしていません。
					return;
				}

				// -- not use DB --
				if (param.Equals("AC", StringComparison.OrdinalIgnoreCase))
				{
					target.addAc((sbyte)(value - target.Ac));
				}
				else if (param.Equals("MR", StringComparison.OrdinalIgnoreCase))
				{
					target.addMr((short)(value - target.Mr));
				}
				else if (param.Equals("HIT", StringComparison.OrdinalIgnoreCase))
				{
					target.addHitup((short)(value - target.Hitup));
				}
				else if (param.Equals("DMG", StringComparison.OrdinalIgnoreCase))
				{
					target.addDmgup((short)(value - target.Dmgup));
					// -- use DB --
				}
				else
				{
					if (param.Equals("HP", StringComparison.OrdinalIgnoreCase))
					{
						target.addBaseMaxHp((short)(value - target.BaseMaxHp));
						target.CurrentHpDirect = target.MaxHp;
					}
					else if (param.Equals("MP", StringComparison.OrdinalIgnoreCase))
					{
						target.addBaseMaxMp((short)(value - target.BaseMaxMp));
						target.CurrentMpDirect = target.MaxMp;
					}
					else if (param.Equals("LAWFUL", StringComparison.OrdinalIgnoreCase))
					{
						target.Lawful = value;
						S_Lawful s_lawful = new S_Lawful(target.Id, target.Lawful);
						target.sendPackets(s_lawful);
						target.broadcastPacket(s_lawful);
					}
					else if (param.Equals("KARMA", StringComparison.OrdinalIgnoreCase))
					{
						target.Karma = value;
					}
					else if (param.Equals("GM", StringComparison.OrdinalIgnoreCase))
					{
						if (value > 200)
						{
							value = 200;
						}
						target.AccessLevel = (short) value;
						target.sendPackets(new S_SystemMessage("リスタートすれば、GMに昇格されています。"));
					}
					else if (param.Equals("STR", StringComparison.OrdinalIgnoreCase))
					{
						target.addBaseStr((sbyte)(value - target.BaseStr));
					}
					else if (param.Equals("CON", StringComparison.OrdinalIgnoreCase))
					{
						target.addBaseCon((sbyte)(value - target.BaseCon));
					}
					else if (param.Equals("DEX", StringComparison.OrdinalIgnoreCase))
					{
						target.addBaseDex((sbyte)(value - target.BaseDex));
					}
					else if (param.Equals("INT", StringComparison.OrdinalIgnoreCase))
					{
						target.addBaseInt((sbyte)(value - target.BaseInt));
					}
					else if (param.Equals("WIS", StringComparison.OrdinalIgnoreCase))
					{
						target.addBaseWis((sbyte)(value - target.BaseWis));
					}
					else if (param.Equals("CHA", StringComparison.OrdinalIgnoreCase))
					{
						target.addBaseCha((sbyte)(value - target.BaseCha));
					}
					else
					{
						pc.sendPackets(new S_SystemMessage("狀態 " + param + " 不明。"));
						return;
					}
					target.Save(); // DBにキャラクター情報を書き込む
				}
				target.sendPackets(new S_OwnCharStatus(target));
				pc.sendPackets(new S_SystemMessage(target.Name + " 的" + param + "值" + value + "被變更了。"));
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage("請輸入: " + cmdName + " 玩家名稱|me 屬性 變更值 。"));
			}
		}
	}

}