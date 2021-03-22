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
namespace LineageServer.Server.Server.Model.Instance
{
	using ActionCodes = LineageServer.Server.Server.ActionCodes;
	using GeneralThreadPool = LineageServer.Server.Server.GeneralThreadPool;
	using IdFactory = LineageServer.Server.Server.IdFactory;
	using SprTable = LineageServer.Server.Server.DataSources.SprTable;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using S_DoActionGFX = LineageServer.Server.Server.serverpackets.S_DoActionGFX;
	using S_DollPack = LineageServer.Server.Server.serverpackets.S_DollPack;
	using S_OwnCharStatus = LineageServer.Server.Server.serverpackets.S_OwnCharStatus;
	using S_SkillIconGFX = LineageServer.Server.Server.serverpackets.S_SkillIconGFX;
	using S_SkillSound = LineageServer.Server.Server.serverpackets.S_SkillSound;
	using L1MagicDoll = LineageServer.Server.Server.Templates.L1MagicDoll;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;
	using Random = LineageServer.Server.Server.utils.Random;

	[Serializable]
	public class L1DollInstance : L1NpcInstance
	{
		private const long serialVersionUID = 1L;

		public const int DOLL_TIME = 1800000;
		private int _itemId;
		private int _itemObjId;
		private int run;
		private bool _isDelete = false;

		// ターゲットがいない場合の処理
		public override bool noTarget()
		{
			if ((_master != null) && !_master.Dead && (_master.MapId == MapId))
			{
				if (Location.getTileLineDistance(_master.Location) > 2)
				{
					int dir = moveDirection(_master.X, _master.Y);
					DirectionMove = dir;
					SleepTime = calcSleepTime(Passispeed, MOVE_SPEED);
				}
				else
				{
					// 魔法娃娃 - 特殊動作
					dollAction();
				}
			}
			else
			{
				_isDelete = true;
				deleteDoll();
				return true;
			}
			return false;
		}

		// 時間計測用
		internal class DollTimer : IRunnableStart
		{
			private readonly L1DollInstance outerInstance;

			public DollTimer(L1DollInstance outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void run()
			{
				if (outerInstance._destroyed)
				{ // 既に破棄されていないかチェック
					return;
				}
				outerInstance.deleteDoll();
			}
		}

		public L1DollInstance(L1Npc template, L1PcInstance master, int itemId, int itemObjId) : base(template)
		{
			Id = IdFactory.Instance.nextId();

			ItemId = itemId;
			ItemObjId = itemObjId;
			GeneralThreadPool.Instance.schedule(new DollTimer(this), DOLL_TIME);

			Master = master;
			X = master.X + RandomHelper.Next(5) - 2;
			Y = master.Y + RandomHelper.Next(5) - 2;
			Map = master.MapId;
			Heading = 5;
			LightSize = template.LightSize;
			MoveSpeed = 1;
			BraveSpeed = 1;

			L1World.Instance.storeObject(this);
			L1World.Instance.addVisibleObject(this);
			foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(this))
			{
				onPerceive(pc);
			}
			master.addDoll(this);
			if (!AiRunning)
			{
				startAI();
			}
			if (L1MagicDoll.isHpRegeneration(_master))
			{
				master.startHpRegenerationByDoll();
			}
			if (L1MagicDoll.isMpRegeneration(_master))
			{
				master.startMpRegenerationByDoll();
			}
			if (L1MagicDoll.isItemMake(_master))
			{
				master.startItemMakeByDoll();
			}
		}

		public virtual void deleteDoll()
		{
			broadcastPacket(new S_SkillSound(Id, 5936));
			if (_master != null && _isDelete)
			{
				L1PcInstance pc = (L1PcInstance) _master;
				pc.sendPackets(new S_SkillIconGFX(56, 0));
				pc.sendPackets(new S_OwnCharStatus(pc));
			}
			if (L1MagicDoll.isHpRegeneration(_master))
			{
				((L1PcInstance) _master).stopHpRegenerationByDoll();
			}
			if (L1MagicDoll.isMpRegeneration(_master))
			{
				((L1PcInstance) _master).stopMpRegenerationByDoll();
			}
			if (L1MagicDoll.isItemMake(_master))
			{
				((L1PcInstance) _master).stopItemMakeByDoll();
			}
			_master.DollList.Remove(Id);
			deleteMe();
		}

		public override void onPerceive(L1PcInstance perceivedFrom)
		{
			// 判斷旅館內是否使用相同鑰匙
			if (perceivedFrom.MapId >= 16384 && perceivedFrom.MapId <= 25088 && perceivedFrom.InnKeyId != _master.InnKeyId)
			{
				return;
			}
			perceivedFrom.addKnownObject(this);
			perceivedFrom.sendPackets(new S_DollPack(this));
		}

		public override void onItemUse()
		{
		}

		public override void onGetItem(L1ItemInstance item)
		{
		}

		public virtual int ItemObjId
		{
			get
			{
				return _itemObjId;
			}
			set
			{
				_itemObjId = value;
			}
		}


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


		// 表情動作
		private void dollAction()
		{
			run = RandomHelper.Next(100) + 1;
			if (run <= 10)
			{
				int actionCode = ActionCodes.ACTION_Aggress; // 67
				if (run <= 5)
				{
					actionCode = ActionCodes.ACTION_Think; // 66
				}

				broadcastPacket(new S_DoActionGFX(Id, actionCode));
				SleepTime = calcSleepTime(SprTable.Instance.getSprSpeed(TempCharGfx,actionCode), MOVE_SPEED);
			}
		}
	}

}