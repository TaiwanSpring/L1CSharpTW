using LineageServer.Models;
using LineageServer.Server.DataTables;
using LineageServer.Server.Model.skill;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LineageServer.Server.Model.Instance
{
	class L1PetInstance : L1NpcInstance
	{

		private const long serialVersionUID = 1L;

		private int _dir;

		// ターゲットがいない場合の処理
		public override bool noTarget()
		{
			switch (_currentPetStatus)
			{
				case 3: // 休息
					return true;
				case 4: // 散開
					if (( _petMaster != null ) && ( _petMaster.MapId == MapId ) && ( Location.getTileLineDistance(_petMaster.Location) < 5 ))
					{
						_dir = targetReverseDirection(_petMaster.X, _petMaster.Y);
						_dir = checkObject(X, Y, MapId, _dir);
						DirectionMove = _dir;
						SleepTime = calcSleepTime(Passispeed, MOVE_SPEED);
					}
					else
					{ // 距離主人 5格以上休息
						_currentPetStatus = 3;
						return true;
					}
					return false;
				case 5: // 警戒
					if (( Math.Abs(HomeX - X) > 1 ) || ( Math.Abs(HomeY - Y) > 1 ))
					{
						int dir = moveDirection(HomeX, HomeY);
						if (dir == -1)
						{
							HomeX = X;
							HomeY = Y;
						}
						else
						{
							DirectionMove = dir;
							SleepTime = calcSleepTime(Passispeed, MOVE_SPEED);
						}
					}
					return false;
				case 7: // 哨子呼叫
					if (( _petMaster != null ) && ( _petMaster.MapId == MapId ) && ( Location.getTileLineDistance(_petMaster.Location) <= 1 ))
					{
						_currentPetStatus = 3;
						return true;
					}
					int locx = _petMaster.X + RandomHelper.Next(1);
					int locy = _petMaster.Y + RandomHelper.Next(1);
					_dir = moveDirection(locx, locy);
					if (_dir == -1)
					{
						_currentPetStatus = 3;
						return true;
					}
					DirectionMove = _dir;
					SleepTime = calcSleepTime(Passispeed, MOVE_SPEED);
					return false;
				default:
					if (( _petMaster != null ) && ( _petMaster.MapId == MapId ))
					{
						if (Location.getTileLineDistance(_petMaster.Location) > 2)
						{
							_dir = moveDirection(_petMaster.X, _petMaster.Y);
							DirectionMove = _dir;
							SleepTime = calcSleepTime(Passispeed, MOVE_SPEED);
						}
					}
					else
					{ // 與主人走失則休息
						_currentPetStatus = 3;
						return true;
					}
					return false;
			}
		}

		/// <summary>
		/// 領出寵物 </summary>
		public L1PetInstance(L1Npc template, L1PcInstance master, L1Pet l1pet) : base(template)
		{

			_petMaster = master;
			_itemObjId = l1pet.get_itemobjid();
			_type = PetTypeTable.Instance.get(template.get_npcId());

			// ステータスを上書き
			Id = l1pet.get_objid();
			Name = l1pet.get_name();
			Level = l1pet.get_level();
			// HPMPはMAXとする
			MaxHp = l1pet.get_hp();
			CurrentHpDirect = l1pet.get_hp();
			MaxMp = l1pet.get_mp();
			CurrentMpDirect = l1pet.get_mp();
			Exp = l1pet.get_exp();
			ExpPercent = ExpTable.getExpPercentage(l1pet.get_level(), l1pet.get_exp());
			Lawful = l1pet.get_lawful();
			TempLawful = l1pet.get_lawful();
			set_food(l1pet.get_food());
			// 執行飽食度計時器
			startFoodTimer(this);

			Master = master;
			X = master.X + RandomHelper.Next(5) - 2;
			Y = master.Y + RandomHelper.Next(5) - 2;
			MapId = master.MapId;
			Heading = 5;
			LightSize = template.LightSize;

			_currentPetStatus = 3;

			L1World.Instance.storeObject(this);
			L1World.Instance.addVisibleObject(this);
			foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(this))
			{
				onPerceive(pc);
			}
			master.addPet(this);
		}

		/// <summary>
		/// 馴養寵物 </summary>
		public L1PetInstance(L1NpcInstance target, L1PcInstance master, int itemid) : base(null)
		{

			_petMaster = master;
			_itemObjId = itemid;
			_type = PetTypeTable.Instance.get(target.NpcTemplate.get_npcId());

			// ステータスを上書き
			Id = IdFactory.Instance.nextId();
			setting_template(target.NpcTemplate);
			CurrentHpDirect = target.CurrentHp;
			CurrentMpDirect = target.CurrentMp;
			Exp = 750; // Lv.5のEXP
			ExpPercent = 0;
			Lawful = 0;
			TempLawful = 0;
			set_food(50); // 飽食度：普通
			startFoodTimer(this); // 執行飽食度計時器

			Master = master;
			X = target.X;
			Y = target.Y;
			MapId = target.MapId;
			Heading = target.Heading;
			LightSize = target.LightSize;
			Petcost = 6;
			Inventory = target.Inventory;
			target.Inventory = null;

			_currentPetStatus = 3;
			/* 修正馴養後回血&回魔 */
			stopHpRegeneration();
			if (MaxHp > CurrentHp)
			{
				startHpRegeneration();
			}
			stopMpRegeneration();
			if (MaxMp > CurrentMp)
			{
				startMpRegeneration();
			}
			target.deleteMe();
			L1World.Instance.storeObject(this);
			L1World.Instance.addVisibleObject(this);
			foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(this))
			{
				onPerceive(pc);
			}

			master.addPet(this);
			PetTable.Instance.storeNewPet(target, Id, itemid);
		}

		// 攻撃でＨＰを減らすときはここを使用
		public override void receiveDamage(L1Character attacker, int damage)
		{
			if (CurrentHp > 0)
			{
				if (damage > 0)
				{ // 回復の場合は攻撃しない。
					setHate(attacker, 0); // ペットはヘイト無し
					removeSkillEffect(L1SkillId.FOG_OF_SLEEPING);
				}

				if (( attacker is L1PcInstance ) && ( damage > 0 ))
				{
					L1PcInstance player = (L1PcInstance)attacker;
					player.PetTarget = this;
				}

				if (attacker is L1PetInstance)
				{
					L1PetInstance pet = (L1PetInstance)attacker;
					// 目標在安區、攻擊者在安區、NOPVP
					if (( ZoneType == 1 ) || ( pet.ZoneType == 1 ))
					{
						damage = 0;
					}
				}
				else if (attacker is L1SummonInstance)
				{
					L1SummonInstance summon = (L1SummonInstance)attacker;
					// 目標在安區、攻擊者在安區、NOPVP
					if (( ZoneType == 1 ) || ( summon.ZoneType == 1 ))
					{
						damage = 0;
					}
				}

				int newHp = CurrentHp - damage;
				if (newHp <= 0)
				{
					death(attacker);
				}
				else
				{
					CurrentHp = newHp;
				}
			}
			else if (!Dead)
			{ // 念のため
				death(attacker);
			}
		}

		public virtual void death(L1Character lastAttacker)
		{
			lock (this)
			{
				if (!Dead)
				{
					Dead = true;
					// 停止飽食度計時器
					stopFoodTimer(this);
					Status = ActionCodes.ACTION_Die;
					CurrentHp = 0;

					Map.setPassable(Location, true);
					broadcastPacket(new S_DoActionGFX(Id, ActionCodes.ACTION_Die));
				}
			}
		}

		/// <summary>
		/// 寵物進化 </summary>
		public virtual void evolvePet(int new_itemobjid)
		{

			L1Pet l1pet = PetTable.Instance.getTemplate(_itemObjId);
			if (l1pet == null)
			{
				return;
			}

			int newNpcId = _type.NpcIdForEvolving;
			int evolvItem = _type.EvolvItemId;
			// 取得進化前最大血魔
			int tmpMaxHp = MaxHp;
			int tmpMaxMp = MaxMp;

			transform(newNpcId);
			_type = PetTypeTable.Instance.get(newNpcId);

			Level = 1;
			// 進化後血魔減半
			MaxHp = tmpMaxHp / 2;
			MaxMp = tmpMaxMp / 2;
			CurrentHpDirect = MaxHp;
			CurrentMpDirect = MaxMp;
			Exp = 0;
			ExpPercent = 0;
			Inventory.consumeItem(evolvItem, 1); // 吃掉進化道具

			// 將原寵物身上道具移交到進化後的寵物身上
			GameObject obj = L1World.Instance.findObject(l1pet.get_objid());
			if (( obj != null ) && ( obj is L1NpcInstance ))
			{
				L1PetInstance new_pet = (L1PetInstance)obj;
				L1Inventory new_petInventory = new_pet.Inventory;
				IList<L1ItemInstance> itemList = Inventory.Items;
				foreach (object itemObject in itemList)
				{
					L1ItemInstance item = (L1ItemInstance)itemObject;
					if (item == null)
					{
						continue;
					}
					if (item.Equipped)
					{ // 裝備中
						item.Equipped = false;
						L1PetItem petItem = PetItemTable.Instance.getTemplate(item.ItemId);
						if (petItem.UseType == 1)
						{ // 牙齒
							Weapon = null;
							new_pet.usePetWeapon(this, item);
						}
						else if (petItem.UseType == 0)
						{ // 盔甲
							Armor = null;
							new_pet.usePetArmor(this, item);
						}
					}
					if (new_pet.Inventory.checkAddItem(item, item.Count) == L1Inventory.OK)
					{
						Inventory.tradeItem(item, item.Count, new_petInventory);
					}
					else
					{ // 掉落地面
						new_petInventory = L1World.Instance.getInventory(X, Y, MapId);
						Inventory.tradeItem(item, item.Count, new_petInventory);
					}
				}
				new_pet.broadcastPacket(new S_SkillSound(new_pet.Id, 2127)); // 升級光芒
			}

			// 刪除原寵物資料
			PetTable.Instance.deletePet(_itemObjId);

			// 紀錄新寵物資料
			l1pet.set_itemobjid(new_itemobjid);
			l1pet.set_npcid(newNpcId);
			l1pet.set_name(Name);
			l1pet.set_level(Level);
			l1pet.set_hp(MaxHp);
			l1pet.set_mp(MaxMp);
			l1pet.set_exp((int)Exp);
			l1pet.set_food(get_food());

			PetTable.Instance.storeNewPet(this, Id, new_itemobjid);

			_itemObjId = new_itemobjid;
			// 執行飽食度計時器
			if (( obj != null ) && ( obj is L1NpcInstance ))
			{
				L1PetInstance new_pet = (L1PetInstance)obj;
				startFoodTimer(new_pet);
			}
		}

		/// <summary>
		/// 解放寵物 </summary>
		public virtual void liberate()
		{
			L1MonsterInstance monster = new L1MonsterInstance(NpcTemplate);
			monster.Id = IdFactory.Instance.nextId();

			monster.X = X;
			monster.Y = Y;
			monster.MapId = MapId;
			monster.Heading = Heading;
			monster.set_storeDroped(true);
			monster.Inventory = Inventory;
			Inventory = null;
			monster.Level = Level;
			monster.MaxHp = MaxHp;
			monster.CurrentHpDirect = CurrentHp;
			monster.MaxMp = MaxMp;
			monster.CurrentMpDirect = CurrentMp;

			_petMaster.PetList.Remove(Id);
			if (_petMaster.PetList.Count == 0)
			{
				_petMaster.sendPackets(new S_PetCtrlMenu(_master, monster, false)); // 關閉寵物控制圖形介面
			}

			deleteMe();

			// DBとPetTableから削除し、ペットアミュも破棄
			_petMaster.Inventory.removeItem(_itemObjId, 1);
			PetTable.Instance.deletePet(_itemObjId);

			L1World.Instance.storeObject(monster);
			L1World.Instance.addVisibleObject(monster);
			foreach (L1PcInstance pc in L1World.Instance.getRecognizePlayer(monster))
			{
				onPerceive(pc);
			}
		}

		// 收集寵物身上道具
		public virtual void collect(bool isDepositnpc)
		{
			L1Inventory targetInventory = _petMaster.Inventory;
			IList<L1ItemInstance> itemList = Inventory.Items;
			foreach (object itemObject in itemList)
			{
				L1ItemInstance item = (L1ItemInstance)itemObject;
				if (item == null)
				{
					continue;
				}
				if (item.Equipped)
				{ // 裝備中
					if (!isDepositnpc)
					{ // 非寄放寵物
						continue;
					}
					else
					{
						L1PetItem petItem = PetItemTable.Instance.getTemplate(item.ItemId);
						if (petItem.UseType == 1)
						{ // 牙齒
							Weapon = null;
						}
						else if (petItem.UseType == 0)
						{ // 盔甲
							Armor = null;
						}
						item.Equipped = false;
					}
				}
				if (_petMaster.Inventory.checkAddItem(item, item.Count) == L1Inventory.OK)
				{
					Inventory.tradeItem(item, item.Count, targetInventory);
					_petMaster.sendPackets(new S_ServerMessage(143, Name, item.LogName));
				}
				else
				{ // 掉落地面
					targetInventory = L1World.Instance.getInventory(X, Y, MapId);
					Inventory.tradeItem(item, item.Count, targetInventory);
				}
			}
		}

		// 重登時寵物身上道具掉落地面
		public virtual void dropItem()
		{
			L1Inventory targetInventory = L1World.Instance.getInventory(X, Y, MapId);
			IList<L1ItemInstance> items = _inventory.Items;
			int size = _inventory.Size;
			for (int i = 0; i < size; i++)
			{
				L1ItemInstance item = items[0];
				if (item.Equipped)
				{ // 裝備中
					L1PetItem petItem = PetItemTable.Instance.getTemplate(item.ItemId);
					if (petItem.UseType == 1)
					{ // 牙齒
						Weapon = null;
					}
					else if (petItem.UseType == 0)
					{ // 盔甲
						Armor = null;
					}
					item.Equipped = false;
				}
				_inventory.tradeItem(item, item.Count, targetInventory);
			}
		}

		// 哨子呼叫寵物
		public virtual void call()
		{
			int id = _type.getMessageId(L1PetType.getMessageNumber(Level));
			if (id != 0 && !Dead)
			{
				if (get_food() == 0)
				{
					id = _type.DefyMessageId;
				}
				broadcastPacket(new S_NpcChatPacket(this, "$" + id, 0));
			}

			if (get_food() > 0)
			{
				CurrentPetStatus = 7; // 前往主人身邊並休息
			}
			else
			{
				CurrentPetStatus = 3; // 休息
			}
		}

		public virtual L1Character Target
		{
			set
			{
				if (( value != null ) && ( ( _currentPetStatus == 1 ) || ( _currentPetStatus == 2 ) || ( _currentPetStatus == 5 ) ) && ( get_food() > 0 ))
				{
					setHate(value, 0);
					if (!AiRunning)
					{
						startAI();
					}
				}
			}
		}

		public virtual L1Character MasterTarget
		{
			set
			{
				if (( value != null ) && ( ( _currentPetStatus == 1 ) || ( _currentPetStatus == 5 ) ) && ( get_food() > 0 ))
				{
					setHate(value, 0);
					if (!AiRunning)
					{
						startAI();
					}
				}
			}
		}

		public override void onPerceive(L1PcInstance perceivedFrom)
		{
			perceivedFrom.addKnownObject(this);
			perceivedFrom.sendPackets(new S_PetPack(this, perceivedFrom)); // ペット系オブジェクト認識
			if (Dead)
			{
				perceivedFrom.sendPackets(new S_DoActionGFX(Id, ActionCodes.ACTION_Die));
			}
		}

		public override void onAction(L1PcInstance pc)
		{
			onAction(pc, 0);
		}

		public override void onAction(L1PcInstance pc, int skillId)
		{
			L1Character cha = Master;
			L1PcInstance master = (L1PcInstance)cha;
			if (master.Teleport)
			{ // テレポート処理中
				return;
			}
			if (ZoneType == 1)
			{ // 攻撃される側がセーフティーゾーン
				L1Attack attack_mortion = new L1Attack(pc, this, skillId); // 攻撃モーション送信
				attack_mortion.action();
				return;
			}

			if (pc.checkNonPvP(pc, this))
			{
				return;
			}

			L1Attack attack = new L1Attack(pc, this, skillId);
			if (attack.calcHit())
			{
				attack.calcDamage();
			}
			attack.action();
			attack.commit();
		}

		public override void onTalkAction(L1PcInstance player)
		{
			if (Dead)
			{
				return;
			}
			if (_petMaster == player)
			{
				player.sendPackets(new S_PetMenuPacket(this, ExpPercent));
				L1Pet l1pet = PetTable.Instance.getTemplate(_itemObjId);
				// XXX ペットに話しかけるたびにDBに書き込む必要はない
				if (l1pet != null)
				{
					l1pet.set_exp((int)Exp);
					l1pet.set_level(Level);
					l1pet.set_hp(MaxHp);
					l1pet.set_mp(MaxMp);
					l1pet.set_food(get_food());
					PetTable.Instance.storePet(l1pet); // DBに書き込み
				}
			}
		}

		public override void onFinalAction(L1PcInstance player, string action)
		{
			int status = actionType(action);
			if (status == 0)
			{
				return;
			}
			if (status == 6)
			{
				L1PcInstance petMaster = (L1PcInstance)_master;
				liberate(); // ペットの解放
							// 更新寵物控制介面
				var petList = petMaster.PetList.Values.ToArray();
				foreach (var petObject in petList)
				{
					if (petObject is L1SummonInstance)
					{
						L1SummonInstance summon = (L1SummonInstance)petObject;
						petMaster.sendPackets(new S_SummonPack(summon, petMaster));
						return;
					}
					else if (petObject is L1PetInstance)
					{
						L1PetInstance pet = (L1PetInstance)petObject;
						petMaster.sendPackets(new S_PetPack(pet, petMaster));
						return;
					}
				}
			}
			else
			{
				// 同じ主人のペットの状態をすべて更新
				object[] petList = _petMaster.PetList.Values.ToArray();
				foreach (object petObject in petList)
				{
					if (petObject is L1PetInstance)
					{ // 寵物
						L1PetInstance pet = (L1PetInstance)petObject;
						if (( _petMaster != null ) && ( _petMaster.Level >= pet.Level ) && pet.get_food() > 0)
						{
							pet.CurrentPetStatus = status;
						}
						else
						{
							if (!pet.Dead)
							{
								L1PetType type = PetTypeTable.Instance.get(pet.NpcTemplate.get_npcId());
								int id = type.DefyMessageId;
								if (id != 0)
								{
									pet.broadcastPacket(new S_NpcChatPacket(pet, "$" + id, 0));
								}
							}
						}
					}
					else if (petObject is L1SummonInstance)
					{ // 召喚獸
						L1SummonInstance summon = (L1SummonInstance)petObject;
						summon.set_currentPetStatus(status);
					}
				}
			}
		}

		public override void onItemUse()
		{
			if (!Actived)
			{
				useItem(USEITEM_HASTE, 100); // １００％の確率でヘイストポーション使用
			}
			if (CurrentHp * 100 / MaxHp < 40)
			{ // ＨＰが４０％きったら
				useItem(USEITEM_HEAL, 100); // １００％の確率で回復ポーション使用
			}
		}

		public override void onGetItem(L1ItemInstance item)
		{
			if (NpcTemplate.get_digestitem() > 0)
			{
				DigestItem = item;
			}
			Array.Sort(healPotions);
			Array.Sort(haestPotions);
			if (Array.BinarySearch(healPotions, item.Item.ItemId) >= 0)
			{
				if (CurrentHp != MaxHp)
				{
					useItem(USEITEM_HEAL, 100);
				}
			}
			else if (Array.BinarySearch(haestPotions, item.Item.ItemId) >= 0)
			{
				useItem(USEITEM_HASTE, 100);
			}
		}

		private int actionType(string action)
		{
			int status = 0;
			if (action == "aggressive")
			{ // 攻撃態勢
				status = 1;
			}
			else if (action == "defensive")
			{ // 防御態勢
				status = 2;
			}
			else if (action == "stay")
			{ // 休憩
				status = 3;
			}
			else if (action == "extend")
			{ // 配備
				status = 4;
			}
			else if (action == "alert")
			{ // 警戒
				status = 5;
			}
			else if (action == "dismiss")
			{ // 解散
				status = 6;
			}
			else if (action == "getitem")
			{ // 収集
				collect(false);
			}
			return status;
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

				if (MaxHp > CurrentHp)
				{
					startHpRegeneration();
				}

				if (_petMaster != null)
				{
					int HpRatio = 100 * currentHp / MaxHp;
					L1PcInstance Master = _petMaster;
					Master.sendPackets(new S_HPMeter(Id, HpRatio));
				}
			}
		}

		public override int CurrentMp
		{
			set
			{
				int currentMp = value;
				if (currentMp >= MaxMp)
				{
					currentMp = MaxMp;
				}
				CurrentMpDirect = currentMp;

				if (MaxMp > CurrentMp)
				{
					startMpRegeneration();
				}
			}
		}

		public virtual int CurrentPetStatus
		{
			set
			{
				_currentPetStatus = value;
				if (_currentPetStatus == 5)
				{
					HomeX = X;
					HomeY = Y;
				}
				if (_currentPetStatus == 7)
				{
					allTargetClear();
				}

				if (_currentPetStatus == 3)
				{
					allTargetClear();
				}
				else
				{
					if (!AiRunning)
					{
						startAI();
					}
				}
			}
			get
			{
				return _currentPetStatus;
			}
		}


		public virtual int ItemObjId
		{
			get
			{
				return _itemObjId;
			}
		}

		public virtual int ExpPercent
		{
			set
			{
				_expPercent = value;
			}
			get
			{
				return _expPercent;
			}
		}


		private L1ItemInstance _weapon;

		public virtual L1ItemInstance Weapon
		{
			set
			{
				_weapon = value;
			}
			get
			{
				return _weapon;
			}
		}


		private L1ItemInstance _armor;

		public virtual L1ItemInstance Armor
		{
			set
			{
				_armor = value;
			}
			get
			{
				return _armor;
			}
		}


		private int _hitByWeapon;

		public virtual int HitByWeapon
		{
			set
			{
				_hitByWeapon = value;
			}
			get
			{
				return _hitByWeapon;
			}
		}


		private int _damageByWeapon;

		public virtual int DamageByWeapon
		{
			set
			{
				_damageByWeapon = value;
			}
			get
			{
				return _damageByWeapon;
			}
		}


		private int _currentPetStatus;

		private L1PcInstance _petMaster;

		private int _itemObjId;

		private L1PetType _type;

		private int _expPercent;

		public virtual L1PetType PetType
		{
			get
			{
				return _type;
			}
		}

		// 寵物飽食度計時器
		private L1PetFood _petFood;

		public virtual void startFoodTimer(L1PetInstance pet)
		{
			_petFood = new L1PetFood(pet, _itemObjId);
			RunnableExecuter.Instance.scheduleAtFixedRate(_petFood, 1000, 200000); // 每 X秒減少
		}

		public virtual void stopFoodTimer(L1PetInstance pet)
		{
			if (_petFood != null)
			{
				_petFood.cancel();
				_petFood = null;
			}
		}

		// 使用寵物裝備
		public virtual void usePetWeapon(L1PetInstance pet, L1ItemInstance weapon)
		{
			if (pet.Weapon == null)
			{
				setPetWeapon(pet, weapon);
			}
			else
			{ // 既に何かを装備している場合、前の装備をはずす
				if (pet.Weapon == weapon)
				{
					removePetWeapon(pet, pet.Weapon);
				}
				else
				{
					removePetWeapon(pet, pet.Weapon);
					setPetWeapon(pet, weapon);
				}
			}
		}

		public virtual void usePetArmor(L1PetInstance pet, L1ItemInstance armor)
		{
			if (pet.Armor == null)
			{
				setPetArmor(pet, armor);
			}
			else
			{ // 既に何かを装備している場合、前の装備をはずす
				if (pet.Armor == armor)
				{
					removePetArmor(pet, pet.Armor);
				}
				else
				{
					removePetArmor(pet, pet.Armor);
					setPetArmor(pet, armor);
				}
			}
		}

		private void setPetWeapon(L1PetInstance pet, L1ItemInstance weapon)
		{
			int itemId = weapon.Item.ItemId;
			L1PetItem petItem = PetItemTable.Instance.getTemplate(itemId);
			if (petItem == null)
			{
				return;
			}

			pet.HitByWeapon = petItem.HitModifier;
			pet.DamageByWeapon = petItem.DamageModifier;
			pet.addStr(petItem.AddStr);
			pet.addCon(petItem.AddCon);
			pet.addDex(petItem.AddDex);
			pet.addInt(petItem.AddInt);
			pet.addWis(petItem.AddWis);
			pet.addMaxHp(petItem.AddHp);
			pet.addMaxMp(petItem.AddMp);
			pet.addSp(petItem.AddSp);
			pet.addMr(petItem.AddMr);

			pet.Weapon = weapon;
			weapon.Equipped = true;
		}

		private void removePetWeapon(L1PetInstance pet, L1ItemInstance weapon)
		{
			int itemId = weapon.Item.ItemId;
			L1PetItem petItem = PetItemTable.Instance.getTemplate(itemId);
			if (petItem == null)
			{
				return;
			}

			pet.HitByWeapon = 0;
			pet.DamageByWeapon = 0;
			pet.addStr(-petItem.AddStr);
			pet.addCon(-petItem.AddCon);
			pet.addDex(-petItem.AddDex);
			pet.addInt(-petItem.AddInt);
			pet.addWis(-petItem.AddWis);
			pet.addMaxHp(-petItem.AddHp);
			pet.addMaxMp(-petItem.AddMp);
			pet.addSp(-petItem.AddSp);
			pet.addMr(-petItem.AddMr);

			pet.Weapon = null;
			weapon.Equipped = false;
		}

		private void setPetArmor(L1PetInstance pet, L1ItemInstance armor)
		{
			int itemId = armor.Item.ItemId;
			L1PetItem petItem = PetItemTable.Instance.getTemplate(itemId);
			if (petItem == null)
			{
				return;
			}

			pet.addAc(petItem.AddAc);
			pet.addStr(petItem.AddStr);
			pet.addCon(petItem.AddCon);
			pet.addDex(petItem.AddDex);
			pet.addInt(petItem.AddInt);
			pet.addWis(petItem.AddWis);
			pet.addMaxHp(petItem.AddHp);
			pet.addMaxMp(petItem.AddMp);
			pet.addSp(petItem.AddSp);
			pet.addMr(petItem.AddMr);

			pet.Armor = armor;
			armor.Equipped = true;
		}

		private void removePetArmor(L1PetInstance pet, L1ItemInstance armor)
		{
			int itemId = armor.Item.ItemId;
			L1PetItem petItem = PetItemTable.Instance.getTemplate(itemId);
			if (petItem == null)
			{
				return;
			}

			pet.addAc(-petItem.AddAc);
			pet.addStr(-petItem.AddStr);
			pet.addCon(-petItem.AddCon);
			pet.addDex(-petItem.AddDex);
			pet.addInt(-petItem.AddInt);
			pet.addWis(-petItem.AddWis);
			pet.addMaxHp(-petItem.AddHp);
			pet.addMaxMp(-petItem.AddMp);
			pet.addSp(-petItem.AddSp);
			pet.addMr(-petItem.AddMr);

			pet.Armor = null;
			armor.Equipped = false;
		}
	}

}