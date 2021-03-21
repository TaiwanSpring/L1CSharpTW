using System;
using System.Threading;

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
	using Config = LineageServer.Server.Config;
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1Map = LineageServer.Server.Server.Model.map.L1Map;
	using S_SkillSound = LineageServer.Server.Server.serverpackets.S_SkillSound;
	using S_Teleport = LineageServer.Server.Server.serverpackets.S_Teleport;
	using Teleportation = LineageServer.Server.Server.utils.Teleportation;

	public class L1Teleport
	{

		// テレポートスキルの種類
		public const int TELEPORT = 0;

		public const int CHANGE_POSITION = 1;

		public const int ADVANCED_MASS_TELEPORT = 2;

		public const int CALL_CLAN = 3;

		// 順番にteleport(白), change position e(青), ad mass teleport e(赤), call clan(緑)
		public static readonly int[] EFFECT_SPR = new int[] {169, 2235, 2236, 2281};

		public static readonly int[] EFFECT_TIME = new int[] {280, 440, 440, 1120};

		private L1Teleport()
		{
		}

		public static void teleport(L1PcInstance pc, L1Location loc, int head, bool effectable)
		{
			teleport(pc, loc.X, loc.Y, (short) loc.MapId, head, effectable, TELEPORT);
		}

		public static void teleport(L1PcInstance pc, L1Location loc, int head, bool effectable, int skillType)
		{
			teleport(pc, loc.X, loc.Y, (short) loc.MapId, head, effectable, skillType);
		}

		public static void teleport(L1PcInstance pc, int x, int y, short mapid, int head, bool effectable)
		{
			teleport(pc, x, y, mapid, head, effectable, TELEPORT);
		}

		public static void teleport(L1PcInstance pc, int x, int y, short mapId, int head, bool effectable, int skillType)
		{
			// 瞬移, 取消交易
			if (pc.TradeID != 0)
			{
				L1Trade trade = new L1Trade();
				trade.TradeCancel(pc);
			}

			//pc.sendPackets(new S_Paralysis(S_Paralysis.TYPE_TELEPORT_UNLOCK, false));

			// エフェクトの表示
			if (effectable && ((skillType >= 0) && (skillType <= EFFECT_SPR.Length)))
			{
				S_SkillSound packet = new S_SkillSound(pc.Id, EFFECT_SPR[skillType]);
				pc.sendPackets(packet);
				pc.broadcastPacket(packet);

				// テレポート以外のsprはキャラが消えないので見た目上送っておきたいが
				// 移動中だった場合クラ落ちすることがある
				// if (skillType != TELEPORT) {
				// pc.sendPackets(new S_DeleteNewObject(pc));
				// pc.broadcastPacket(new S_DeleteObjectFromScreen(pc));
				// }

				try
				{
					Thread.Sleep((int)(EFFECT_TIME[skillType] * 0.7));
				}
				catch (Exception)
				{
				}
			}

			pc.TeleportX = x;
			pc.TeleportY = y;
			pc.TeleportMapId = mapId;
			pc.TeleportHeading = head;
			if (Config.SEND_PACKET_BEFORE_TELEPORT)
			{
				pc.sendPackets(new S_Teleport(pc));
			}
			else
			{
				Teleportation.actionTeleportation(pc);
			}
		}

		/*
		 * targetキャラクターのdistanceで指定したマス分前にテレポートする。指定されたマスがマップでない場合何もしない。
		 */
		public static void teleportToTargetFront(L1Character cha, L1Character target, int distance)
		{
			int locX = target.X;
			int locY = target.Y;
			int heading = target.Heading;
			L1Map map = target.Map;
			short mapId = target.MapId;

			// ターゲットの向きからテレポート先の座標を決める。
			switch (heading)
			{
				case 1:
					locX += distance;
					locY -= distance;
					break;

				case 2:
					locX += distance;
					break;

				case 3:
					locX += distance;
					locY += distance;
					break;

				case 4:
					locY += distance;
					break;

				case 5:
					locX -= distance;
					locY += distance;
					break;

				case 6:
					locX -= distance;
					break;

				case 7:
					locX -= distance;
					locY -= distance;
					break;

				case 0:
					locY -= distance;
					break;

				default:
					break;

			}

			if (map.isPassable(locX, locY))
			{
				if (cha is L1PcInstance)
				{
					teleport((L1PcInstance) cha, locX, locY, mapId, cha.Heading, true);
				}
				else if (cha is L1NpcInstance)
				{
					((L1NpcInstance) cha).teleport(locX, locY, cha.Heading);
				}
			}
		}

		public static void randomTeleport(L1PcInstance pc, bool effectable)
		{
			// まだ本サーバのランテレ処理と違うところが結構あるような・・・
			L1Location newLocation = pc.Location.randomLocation(200, true);
			int newX = newLocation.X;
			int newY = newLocation.Y;
			short mapId = (short) newLocation.MapId;

			L1Teleport.teleport(pc, newX, newY, mapId, 5, effectable);
		}
	}

}