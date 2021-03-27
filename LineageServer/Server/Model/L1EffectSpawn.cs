using System;
using System.Text;

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
namespace LineageServer.Server.Model
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static l1j.server.server.model.skill.L1SkillId.FIRE_WALL;


	using IdFactory = LineageServer.Server.IdFactory;
	using NpcTable = LineageServer.Server.DataSources.NpcTable;
	using SkillsTable = LineageServer.Server.DataSources.SkillsTable;
	using L1EffectInstance = LineageServer.Server.Model.Instance.L1EffectInstance;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using L1Map = LineageServer.Server.Model.map.L1Map;
	using L1WorldMap = LineageServer.Server.Model.map.L1WorldMap;
	using S_NPCPack = LineageServer.Serverpackets.S_NPCPack;
	using L1Npc = LineageServer.Server.Templates.L1Npc;

	// Referenced classes of package l1j.server.server.model:
	// L1EffectSpawn

	public class L1EffectSpawn
	{

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static readonly Logger _log = Logger.GetLogger(typeof(L1EffectSpawn).FullName);

		private static L1EffectSpawn _instance;

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: private java.lang.reflect.Constructor<?> _constructor;
		private System.Reflection.ConstructorInfo<object> _constructor;

		private L1EffectSpawn()
		{
		}

		public static L1EffectSpawn Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new L1EffectSpawn();
				}
				return _instance;
			}
		}

		/// <summary>
		/// エフェクトオブジェクトを生成し設置する
		/// </summary>
		/// <param name="npcId">
		///            エフェクトNPCのテンプレートID </param>
		/// <param name="time">
		///            存在時間(ms) </param>
		/// <param name="locX">
		///            設置する座標X </param>
		/// <param name="locY">
		///            設置する座標Y </param>
		/// <param name="mapId">
		///            設置するマップのID </param>
		/// <returns> 生成されたエフェクトオブジェクト </returns>
		public virtual L1EffectInstance spawnEffect(int npcId, int time, int locX, int locY, short mapId)
		{
			return spawnEffect(npcId, time, locX, locY, mapId, null, 0);
		}

		public virtual L1EffectInstance spawnEffect(int npcId, int time, int locX, int locY, short mapId, L1PcInstance user, int skiiId)
		{
			L1Npc template = NpcTable.Instance.getTemplate(npcId);
			L1EffectInstance effect = null;

			if (template == null)
			{
				return null;
			}

			string className = (new StringBuilder()).Append("l1j.server.server.model.Instance.").Append(template.Impl).Append("Instance").ToString();

			try
			{
				_constructor = Type.GetType(className).GetConstructors()[0];
				object[] obj = new object[] {template};
				effect = (L1EffectInstance) _constructor.Invoke(obj);

				effect.Id = IdFactory.Instance.nextId();
				effect.GfxId = template.get_gfxid();
				effect.X = locX;
				effect.Y = locY;
				effect.HomeX = locX;
				effect.HomeY = locY;
				effect.Heading = 0;
				effect.Map = mapId;
				effect.User = user;
				effect.SkillId = skiiId;
				L1World.Instance.storeObject(effect);
				L1World.Instance.addVisibleObject(effect);

				foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(effect))
				{
					effect.addKnownObject(pc);
					pc.addKnownObject(effect);
					pc.sendPackets(new S_NPCPack(effect));
					pc.broadcastPacket(new S_NPCPack(effect));
				}
				L1NpcDeleteTimer timer = new L1NpcDeleteTimer(effect, time);
				timer.begin();
			}
			catch (Exception e)
			{
				_log.Error(e);
			}

			return effect;
		}

		public virtual void doSpawnFireWall(L1Character cha, int targetX, int targetY)
		{
			L1Npc firewall = NpcTable.Instance.getTemplate(81157); // ファイアーウォール
			int duration = SkillsTable.Instance.getTemplate(FIRE_WALL).BuffDuration;

			if (firewall == null)
			{
				throw new System.NullReferenceException("FireWall data not found:npcid=81157");
			}

			L1Character @base = cha;
			for (int i = 0; i < 8; i++)
			{
				int a = @base.targetDirection(targetX, targetY);
				int x = @base.X;
				int y = @base.Y;
				if (a == 1)
				{
					x++;
					y--;
				}
				else if (a == 2)
				{
					x++;
				}
				else if (a == 3)
				{
					x++;
					y++;
				}
				else if (a == 4)
				{
					y++;
				}
				else if (a == 5)
				{
					x--;
					y++;
				}
				else if (a == 6)
				{
					x--;
				}
				else if (a == 7)
				{
					x--;
					y--;
				}
				else if (a == 0)
				{
					y--;
				}
				if (!@base.isAttackPosition(x, y, 1))
				{
					x = @base.X;
					y = @base.Y;
				}
				L1Map map = L1WorldMap.Instance.getMap(cha.MapId);
				if (!map.isArrowPassable(x, y, cha.Heading))
				{
					break;
				}

				L1EffectInstance effect = spawnEffect(81157, duration * 1000, x, y, cha.MapId);
				if (effect == null)
				{
					break;
				}
				foreach (GameObject objects in L1World.Instance.getVisibleObjects(effect, 0))
				{
					if (objects is L1EffectInstance)
					{
						L1EffectInstance npc = (L1EffectInstance) objects;
						if (npc.NpcTemplate.get_npcId() == 81157)
						{
							npc.deleteMe();
						}
					}
				}
				if ((targetX == x) && (targetY == y))
				{
					break;
				}
				@base = effect;
			}

		}
	}

}