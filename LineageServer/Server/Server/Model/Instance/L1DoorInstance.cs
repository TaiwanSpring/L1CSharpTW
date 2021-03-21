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
namespace LineageServer.Server.Server.Model.Instance
{
	using ActionCodes = LineageServer.Server.Server.ActionCodes;
	using NpcTable = LineageServer.Server.Server.datatables.NpcTable;
	using L1Attack = LineageServer.Server.Server.Model.L1Attack;
	using L1Character = LineageServer.Server.Server.Model.L1Character;
	using L1Location = LineageServer.Server.Server.Model.L1Location;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using S_DoActionGFX = LineageServer.Server.Server.serverpackets.S_DoActionGFX;
	using S_Door = LineageServer.Server.Server.serverpackets.S_Door;
	using S_DoorPack = LineageServer.Server.Server.serverpackets.S_DoorPack;
	using S_RemoveObject = LineageServer.Server.Server.serverpackets.S_RemoveObject;
	using L1DoorGfx = LineageServer.Server.Server.Templates.L1DoorGfx;
	using L1Npc = LineageServer.Server.Server.Templates.L1Npc;

	[Serializable]
	public class L1DoorInstance : L1NpcInstance
	{

		private const long serialVersionUID = 1L;
		private const int DOOR_NPC_ID = 81158;

		public L1DoorInstance(L1Npc template) : base(template)
		{
		}

		public L1DoorInstance(int doorId, L1DoorGfx gfx, L1Location loc, int hp, int keeper, bool isOpening) : base(NpcTable.Instance.getTemplate(DOOR_NPC_ID))
		{
			DoorId = doorId;
			MaxHp = hp;
			CurrentHp = hp;
			GfxId = gfx.GfxId;
			Location = loc;
			HomeX = loc.X;
			HomeY = loc.Y;
			Direction = gfx.Direction;
			int baseLoc = gfx.Direction == 0 ? loc.X : loc.Y;
			LeftEdgeLocation = baseLoc + gfx.LeftEdgeOffset;
			RightEdgeLocation = baseLoc + gfx.RightEdgeOffset;
			KeeperId = keeper;
			if (isOpening)
			{
				open();
			}
		}

		public override void onAction(L1PcInstance pc)
		{
			if (MaxHp == 0)
			{ // 破壊不可能なドアは対象外
				return;
			}

			if (CurrentHp > 0 && !Dead)
			{
				L1Attack attack = new L1Attack(pc, this);
				if (attack.calcHit())
				{
					attack.calcDamage();
					attack.addPcPoisonAttack(pc, this);
					attack.addChaserAttack();
				}
				attack.action();
				attack.commit();
			}
		}

		public override void onPerceive(L1PcInstance perceivedFrom)
		{
			perceivedFrom.addKnownObject(this);
			perceivedFrom.sendPackets(new S_DoorPack(this));
			sendDoorPacket(perceivedFrom);
		}

		public override void deleteMe()
		{
			Dead = true;
			sendDoorPacket(null);

			_destroyed = true;
			if (Inventory != null)
			{
				Inventory.clearItems();
			}
			allTargetClear();
			_master = null;
			L1World.Instance.removeVisibleObject(this);
			L1World.Instance.removeObject(this);
			foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(this))
			{
				pc.removeKnownObject(this);
				pc.sendPackets(new S_RemoveObject(this));
			}
			removeAllKnownObjects();
		}

		public override void receiveDamage(L1Character attacker, int damage)
		{
			if (MaxHp == 0)
			{ // 破壊不可能なドアは対象外
				return;
			}
			if (CurrentHp <= 0 || Dead)
			{
				return;
			}

			int newHp = CurrentHp - damage;
			if (newHp <= 0 && !Dead)
			{
				die();
				return;
			}

			CurrentHpDirect = newHp;
			updateStatus();
		}

		private void updateStatus()
		{
			int newStatus = 0;
			if ((MaxHp * 1 / 6) > CurrentHp)
			{
				newStatus = ActionCodes.ACTION_DoorAction5;
			}
			else if ((MaxHp * 2 / 6) > CurrentHp)
			{
				newStatus = ActionCodes.ACTION_DoorAction4;
			}
			else if ((MaxHp * 3 / 6) > CurrentHp)
			{
				newStatus = ActionCodes.ACTION_DoorAction3;
			}
			else if ((MaxHp * 4 / 6) > CurrentHp)
			{
				newStatus = ActionCodes.ACTION_DoorAction2;
			}
			else if ((MaxHp * 5 / 6) > CurrentHp)
			{
				newStatus = ActionCodes.ACTION_DoorAction1;
			}
			if (Status == newStatus)
			{
				return;
			}
			Status = newStatus;
			broadcastPacket(new S_DoActionGFX(Id, newStatus));
		}

		public override int CurrentHp
		{
			set
			{
				int currentHp = value;
				if (currentHp >= MaxHp)
				{
					currentHp = MaxHp;
				}
				CurrentHpDirect = currentHp;
			}
		}

		private void die()
		{
			CurrentHpDirect = 0;
			Dead = true;
			Status = ActionCodes.ACTION_DoorDie;

			System.Collections.IDictionary.setPassable(Location, true);

			broadcastPacket(new S_DoActionGFX(Id, ActionCodes.ACTION_DoorDie));
			sendDoorPacket(null);
		}

		public virtual void sendDoorPacket(L1PcInstance pc)
		{
			int entranceX = EntranceX;
			int entranceY = EntranceY;
			int leftEdgeLocation = LeftEdgeLocation;
			int rightEdgeLocation = RightEdgeLocation;

			int size = rightEdgeLocation - leftEdgeLocation;
			if (size == 0)
			{ // 1マス分の幅のドア
				sendPacket(pc, entranceX, entranceY);
			}
			else
			{ // 2マス分以上の幅があるドア
				if (Direction == 0)
				{ // ／向き
					for (int x = leftEdgeLocation; x <= rightEdgeLocation; x++)
					{
						sendPacket(pc, x, entranceY);
					}
				}
				else
				{ // ＼向き
					for (int y = leftEdgeLocation; y <= rightEdgeLocation; y++)
					{
						sendPacket(pc, entranceX, y);
					}
				}
			}
		}

		private bool Passable
		{
			get
			{
				return Dead || OpenStatus == ActionCodes.ACTION_Open;
			}
		}

		private void sendPacket(L1PcInstance pc, int x, int y)
		{
			S_Door packet = new S_Door(x, y, Direction, Passable);
			if (pc != null)
			{ // onPerceive()経由の場合
				// 開いている場合は通行不可パケット送信不要
				if (OpenStatus == ActionCodes.ACTION_Close)
				{
					pc.sendPackets(packet);
				}
			}
			else
			{
				broadcastPacket(packet);
			}
		}

		public virtual void open()
		{
			if (Dead || OpenStatus == ActionCodes.ACTION_Open)
			{
				return;
			}

			OpenStatus = ActionCodes.ACTION_Open;
			broadcastPacket(new S_DoorPack(this));
			broadcastPacket(new S_DoActionGFX(Id, ActionCodes.ACTION_Open));
			sendDoorPacket(null);
		}

		public virtual void close()
		{
			if (Dead || OpenStatus == ActionCodes.ACTION_Close)
			{
				return;
			}

			OpenStatus = ActionCodes.ACTION_Close;
			broadcastPacket(new S_DoorPack(this));
			broadcastPacket(new S_DoActionGFX(Id, ActionCodes.ACTION_Close));
			sendDoorPacket(null);
		}

		public virtual void repairGate()
		{
			if (MaxHp <= 1)
			{
				return;
			}

			Dead = false;
			CurrentHp = MaxHp;
			Status = 0;
			OpenStatus = ActionCodes.ACTION_Open;
			close();
		}

		private int _doorId = 0;

		public virtual int DoorId
		{
			get
			{
				return _doorId;
			}
			set
			{
				_doorId = value;
			}
		}


		private int _direction = 0; // ドアの向き

		public virtual int Direction
		{
			get
			{
				return _direction;
			}
			set
			{
				if (value != 0 && value != 1)
				{
					throw new System.ArgumentException();
				}
				_direction = value;
			}
		}


		public virtual int EntranceX
		{
			get
			{
				int entranceX = 0;
				if (Direction == 0)
				{ // ／向き
					entranceX = X;
				}
				else
				{ // ＼向き
					entranceX = X - 1;
				}
				return entranceX;
			}
		}

		public virtual int EntranceY
		{
			get
			{
				int entranceY = 0;
				if (Direction == 0)
				{ // ／向き
					entranceY = Y + 1;
				}
				else
				{ // ＼向き
					entranceY = Y;
				}
				return entranceY;
			}
		}

		private int _leftEdgeLocation = 0; // ドアの左端の座標(ドアの向きからX軸orY軸を決定する)

		public virtual int LeftEdgeLocation
		{
			get
			{
				return _leftEdgeLocation;
			}
			set
			{
				_leftEdgeLocation = value;
			}
		}


		private int _rightEdgeLocation = 0; // ドアの右端の座標(ドアの向きからX軸orY軸を決定する)

		public virtual int RightEdgeLocation
		{
			get
			{
				return _rightEdgeLocation;
			}
			set
			{
				_rightEdgeLocation = value;
			}
		}


		private int _openStatus = ActionCodes.ACTION_Close;

		public virtual int OpenStatus
		{
			get
			{
				return _openStatus;
			}
			set
			{
				if (value != ActionCodes.ACTION_Open && value != ActionCodes.ACTION_Close)
				{
					throw new System.ArgumentException();
				}
				_openStatus = value;
			}
		}


		private int _keeperId = 0;

		public virtual int KeeperId
		{
			get
			{
				return _keeperId;
			}
			set
			{
				_keeperId = value;
			}
		}

	}

}