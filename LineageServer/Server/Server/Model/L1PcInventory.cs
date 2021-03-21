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
namespace LineageServer.Server.Server.Model
{

	using Random = LineageServer.Server.Server.utils.Random;


	using Config = LineageServer.Server.Config;
	using RaceTicketTable = LineageServer.Server.Server.datatables.RaceTicketTable;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1PetInstance = LineageServer.Server.Server.Model.Instance.L1PetInstance;
	using L1ItemId = LineageServer.Server.Server.Model.identity.L1ItemId;
	using S_AddItem = LineageServer.Server.Server.serverpackets.S_AddItem;
	using S_CharVisualUpdate = LineageServer.Server.Server.serverpackets.S_CharVisualUpdate;
	using S_DeleteInventoryItem = LineageServer.Server.Server.serverpackets.S_DeleteInventoryItem;
	using S_EquipmentWindow = LineageServer.Server.Server.serverpackets.S_EquipmentWindow;
	using S_ItemColor = LineageServer.Server.Server.serverpackets.S_ItemColor;
	using S_ItemStatus = LineageServer.Server.Server.serverpackets.S_ItemStatus;
	using S_OwnCharStatus = LineageServer.Server.Server.serverpackets.S_OwnCharStatus;
	using S_ItemName = LineageServer.Server.Server.serverpackets.S_ItemName;
	using S_ItemAmount = LineageServer.Server.Server.serverpackets.S_ItemAmount;
	using S_PacketBox = LineageServer.Server.Server.serverpackets.S_PacketBox;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using CharactersItemStorage = LineageServer.Server.Server.storage.CharactersItemStorage;
	using L1Item = LineageServer.Server.Server.Templates.L1Item;
	using L1RaceTicket = LineageServer.Server.Server.Templates.L1RaceTicket;

	[Serializable]
	public class L1PcInventory : L1Inventory
	{

		private const long serialVersionUID = 1L;

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1PcInventory).FullName);

		private const int MAX_SIZE = 180;

		private readonly L1PcInstance _owner; // 所有者プレイヤー

		private int _arrowId; // 優先して使用されるアローのItemID

		private int _stingId; // 優先して使用されるスティングのItemID

		public L1PcInventory(L1PcInstance owner)
		{
			_owner = owner;
			_arrowId = 0;
			_stingId = 0;
		}

		public virtual L1PcInstance Owner
		{
			get
			{
				return _owner;
			}
		}

		// 分為242段的重量數值
		public virtual int Weight242
		{
			get
			{
				return calcWeight242(Weight);
			}
		}

		// 242階段的重量數值計算
		public virtual int calcWeight242(int weight)
		{
			int weight242 = 0;
			if (Config.RATE_WEIGHT_LIMIT != 0)
			{
				double maxWeight = _owner.MaxWeight;
				if (weight > maxWeight)
				{
					weight242 = 242;
				}
				else
				{
					double wpTemp = (weight * 100 / maxWeight) * 242.00 / 100.00;
					DecimalFormat df = new DecimalFormat("00.##");
					df.format(wpTemp);
					wpTemp = (long)Math.Round(wpTemp, MidpointRounding.AwayFromZero);
					weight242 = (int)(wpTemp);
				}
			}
			else
			{ // ウェイトレートが０なら重量常に０
				weight242 = 0;
			}
			return weight242;
		}

		public override int checkAddItem(L1ItemInstance item, int count)
		{
			return checkAddItem(item, count, true);
		}

		public virtual int checkAddItem(L1ItemInstance item, int count, bool message)
		{
			if (item == null)
			{
				return -1;
			}
			if (Size > MAX_SIZE || (Size == MAX_SIZE && (!item.Stackable || !checkItem(item.Item.ItemId))))
			{ // 容量確認
				if (message)
				{
					sendOverMessage(263); // \f1一人のキャラクターが持って歩けるアイテムは最大180個までです。
				}
				return SIZE_OVER;
			}

			int weight = Weight + item.Item.Weight * count / 1000 + 1;
			if (weight < 0 || (item.Item.Weight * count / 1000) < 0)
			{
				if (message)
				{
					sendOverMessage(82); // 此物品太重了，所以你無法攜帶。
				}
				return WEIGHT_OVER;
			}
			if (calcWeight242(weight) >= 242)
			{
				if (message)
				{
					sendOverMessage(82); // 此物品太重了，所以你無法攜帶。
				}
				return WEIGHT_OVER;
			}

			L1ItemInstance itemExist = findItemId(item.ItemId);
			if (itemExist != null && (itemExist.Count + count) > MAX_AMOUNT)
			{
				if (message)
				{
					Owner.sendPackets(new S_ServerMessage(166, "所持有的金幣", "超過了2,000,000,000上限。")); // \f1%0が%4%1%3%2
				}
				return AMOUNT_OVER;
			}

			return OK;
		}

		public virtual void sendOverMessage(int message_id)
		{
			// 釣魚中負重訊息變更
			if (_owner.Fishing && message_id == 82)
			{
				message_id = 1518; // 負重太高的狀態下無法進行釣魚。
			}
			_owner.sendPackets(new S_ServerMessage(message_id));
		}

		// 讀取資料庫中的character_items資料表
		public override void loadItems()
		{
			try
			{
				CharactersItemStorage storage = CharactersItemStorage.create();

				foreach (L1ItemInstance item in storage.loadItems(_owner.Id))
				{
					_items.Add(item);

					if (item.Equipped)
					{
						item.Equipped = false;
						setEquipped(item, true, true, false);
					}
					if (item.Item.Type2 == 0 && item.Item.Type == 2)
					{ // light系アイテム
						item.RemainingTime = item.Item.LightFuel;
					}
					/// <summary>
					/// 玩家身上的食人妖精RaceTicket 顯示場次、及選手編號
					/// </summary>
					if (item.ItemId == 40309)
					{
						L1RaceTicket ticket = RaceTicketTable.Instance.getTemplate(item.Id);
						if (ticket != null)
						{
							L1Item temp = (L1Item) item.Item.clone();
							string buf = temp.IdentifiedNameId + " " + ticket.get_round() + "-" + ticket.get_runner_num();
							temp.Name = buf;
							temp.UnidentifiedNameId = buf;
							temp.IdentifiedNameId = buf;
							item.Item = temp;
						}
					}
					L1World.Instance.storeObject(item);
				}
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
		}

		// 對資料庫中的character_items資料表寫入
		public override void insertItem(L1ItemInstance item)
		{
			_owner.sendPackets(new S_AddItem(item));
			if (item.Item.Weight != 0)
			{
				_owner.sendPackets(new S_PacketBox(S_PacketBox.WEIGHT,Weight242));
			}
			try
			{
				CharactersItemStorage storage = CharactersItemStorage.create();
				storage.storeItem(_owner.Id, item);
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
		}

		public const int COL_ALL = 0;

		public const int COL_DURABILITY = 1;

		public const int COL_IS_ID = 2;

		public const int COL_ENCHANTLVL = 4;

		public const int COL_EQUIPPED = 8;

		public const int COL_COUNT = 16;

		public const int COL_DELAY_EFFECT = 32;

		public const int COL_ITEMID = 64;

		public const int COL_CHARGE_COUNT = 128;

		public const int COL_REMAINING_TIME = 256;

		public const int COL_BLESS = 512;

		public const int COL_ATTR_ENCHANT_KIND = 1024;

		public const int COL_ATTR_ENCHANT_LEVEL = 2048;

		public const int COL_ADDHP = 1;

		public const int COL_ADDMP = 2;

		public const int COL_HPR = 4;

		public const int COL_MPR = 8;

		public const int COL_ADDSP = 16;

		public const int COL_M_DEF = 32;

		public const int COL_EARTHMR = 64;

		public const int COL_FIREMR = 128;

		public const int COL_WATERMR = 256;

		public const int COL_WINDMR = 512;

		public override void updateItem(L1ItemInstance item)
		{
			updateItem(item, COL_COUNT);
			if (item.Item.ToBeSavedAtOnce)
			{
				saveItem(item, COL_COUNT);
			}
		}

		/// <summary>
		/// インベントリ内のアイテムの状態を更新する。
		/// </summary>
		/// <param name="item">
		///            - 更新対象のアイテム </param>
		/// <param name="column">
		///            - 更新するステータスの種類 </param>
		public override void updateItem(L1ItemInstance item, int column)
		{
			if (column >= COL_ATTR_ENCHANT_LEVEL)
			{ // 属性強化数
				_owner.sendPackets(new S_ItemStatus(item));
				column -= COL_ATTR_ENCHANT_LEVEL;
			}
			if (column >= COL_ATTR_ENCHANT_KIND)
			{ // 属性強化の種類
				_owner.sendPackets(new S_ItemStatus(item));
				column -= COL_ATTR_ENCHANT_KIND;
			}
			if (column >= COL_BLESS)
			{ // 祝福・封印
				_owner.sendPackets(new S_ItemColor(item));
				column -= COL_BLESS;
			}
			if (column >= COL_REMAINING_TIME)
			{ // 使用可能な残り時間
				_owner.sendPackets(new S_ItemName(item));
				column -= COL_REMAINING_TIME;
			}
			if (column >= COL_CHARGE_COUNT)
			{ // チャージ数
				_owner.sendPackets(new S_ItemName(item));
				column -= COL_CHARGE_COUNT;
			}
			if (column >= COL_ITEMID)
			{ // 別のアイテムになる場合(便箋を開封したときなど)
				_owner.sendPackets(new S_ItemStatus(item));
				_owner.sendPackets(new S_ItemColor(item));
				_owner.sendPackets(new S_PacketBox(S_PacketBox.WEIGHT,Weight242));
				column -= COL_ITEMID;
			}
			if (column >= COL_DELAY_EFFECT)
			{ // 効果ディレイ
				column -= COL_DELAY_EFFECT;
			}
			if (column >= COL_COUNT)
			{ // カウント
				_owner.sendPackets(new S_ItemAmount(item));

				int weight = item.Weight;
				if (weight != item.LastWeight)
				{
					item.LastWeight = weight;
					_owner.sendPackets(new S_ItemStatus(item));
				}
				else
				{
					_owner.sendPackets(new S_ItemName(item));
				}
				if (item.Item.Weight != 0)
				{
					// XXX 242段階のウェイトが変化しない場合は送らなくてよい
					_owner.sendPackets(new S_PacketBox(S_PacketBox.WEIGHT,Weight242));
				}
				column -= COL_COUNT;
			}
			if (column >= COL_EQUIPPED)
			{ // 装備状態
				_owner.sendPackets(new S_ItemName(item));
				column -= COL_EQUIPPED;
			}
			if (column >= COL_ENCHANTLVL)
			{ // エンチャント
				_owner.sendPackets(new S_ItemStatus(item));
				column -= COL_ENCHANTLVL;
			}
			if (column >= COL_IS_ID)
			{ // 確認状態
				_owner.sendPackets(new S_ItemStatus(item));
				_owner.sendPackets(new S_ItemColor(item));
				column -= COL_IS_ID;
			}
			if (column >= COL_DURABILITY)
			{ // 耐久性
				_owner.sendPackets(new S_ItemStatus(item));
				column -= COL_DURABILITY;
			}
		}

		/// <summary>
		/// インベントリ内のアイテムの状態をDBに保存する。
		/// </summary>
		/// <param name="item">
		///            - 更新対象のアイテム </param>
		/// <param name="column">
		///            - 更新するステータスの種類 </param>
		public virtual void saveItem(L1ItemInstance item, int column)
		{
			if (column == 0)
			{
				return;
			}

			try
			{
				CharactersItemStorage storage = CharactersItemStorage.create();
				if (column >= COL_ATTR_ENCHANT_LEVEL)
				{ // 属性強化数
					storage.updateItemAttrEnchantLevel(item);
					column -= COL_ATTR_ENCHANT_LEVEL;
				}
				if (column >= COL_ATTR_ENCHANT_KIND)
				{ // 属性強化の種類
					storage.updateItemAttrEnchantKind(item);
					column -= COL_ATTR_ENCHANT_KIND;
				}
				if (column >= COL_BLESS)
				{ // 祝福・封印
					storage.updateItemBless(item);
					column -= COL_BLESS;
				}
				if (column >= COL_REMAINING_TIME)
				{ // 使用可能な残り時間
					storage.updateItemRemainingTime(item);
					column -= COL_REMAINING_TIME;
				}
				if (column >= COL_CHARGE_COUNT)
				{ // チャージ数
					storage.updateItemChargeCount(item);
					column -= COL_CHARGE_COUNT;
				}
				if (column >= COL_ITEMID)
				{ // 別のアイテムになる場合(便箋を開封したときなど)
					storage.updateItemId(item);
					column -= COL_ITEMID;
				}
				if (column >= COL_DELAY_EFFECT)
				{ // 効果ディレイ
					storage.updateItemDelayEffect(item);
					column -= COL_DELAY_EFFECT;
				}
				if (column >= COL_COUNT)
				{ // カウント
					storage.updateItemCount(item);
					column -= COL_COUNT;
				}
				if (column >= COL_EQUIPPED)
				{ // 装備状態
					storage.updateItemEquipped(item);
					column -= COL_EQUIPPED;
				}
				if (column >= COL_ENCHANTLVL)
				{ // エンチャント
					storage.updateItemEnchantLevel(item);
					column -= COL_ENCHANTLVL;
				}
				if (column >= COL_IS_ID)
				{ // 確認状態
					storage.updateItemIdentified(item);
					column -= COL_IS_ID;
				}
				if (column >= COL_DURABILITY)
				{ // 耐久性
					storage.updateItemDurability(item);
					column -= COL_DURABILITY;
				}
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
		}

		public virtual void saveEnchantAccessory(L1ItemInstance item, int column)
		{ // 飾品強化
			if (column == 0)
			{
				return;
			}

			try
			{
				CharactersItemStorage storage = CharactersItemStorage.create();
				if (column >= COL_WINDMR)
				{
					storage.updateWindMr(item);
					column -= COL_WINDMR;
				}
				if (column >= COL_WATERMR)
				{
					storage.updateWaterMr(item);
					column -= COL_WATERMR;
				}
				if (column >= COL_FIREMR)
				{
					storage.updateFireMr(item);
					column -= COL_FIREMR;
				}
				if (column >= COL_EARTHMR)
				{
					storage.updateEarthMr(item);
					column -= COL_EARTHMR;
				}
				if (column >= COL_M_DEF)
				{
					storage.updateM_Def(item);
					column -= COL_M_DEF;
				}
				if (column >= COL_ADDSP)
				{
					storage.updateaddSp(item);
					column -= COL_ADDSP;
				}
				if (column >= COL_MPR)
				{
					storage.updateMpr(item);
					column -= COL_MPR;
				}
				if (column >= COL_HPR)
				{
					storage.updateHpr(item);
					column -= COL_HPR;
				}
				if (column >= COL_ADDMP)
				{
					storage.updateaddMp(item);
					column -= COL_ADDMP;
				}
				if (column >= COL_ADDHP)
				{
					storage.updateaddHp(item);
					column -= COL_ADDHP;
				}
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
		}

		// ＤＢのcharacter_itemsから削除
		public override void deleteItem(L1ItemInstance item)
		{
			try
			{
				CharactersItemStorage storage = CharactersItemStorage.create();

				storage.deleteItem(item);
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
			}
			if (item.Equipped)
			{
				setEquipped(item, false);
			}
			if (item.Item.Weight != 0)
			{
				_owner.sendPackets(new S_PacketBox(S_PacketBox.WEIGHT,Weight242));
			}
			_owner.sendPackets(new S_DeleteInventoryItem(item));
			_items.Remove(item);
		}

		// アイテムを装着脱着させる（L1ItemInstanceの変更、補正値の設定、character_itemsの更新、パケット送信まで管理）
		public virtual void setEquipped(L1ItemInstance item, bool equipped)
		{
			setEquipped(item, equipped, false, false);
		}

		public virtual void setEquipped(L1ItemInstance item, bool equipped, bool loaded, bool changeWeapon)
		{
			if (item.Equipped != equipped)
			{ // 設定値と違う場合だけ処理
				L1Item temp = item.Item;
				if (equipped)
				{ // 装着
					item.Equipped = true;
					_owner.EquipSlot.set(item);
					Equipped(item, true); //3.63
				}
				else
				{ // 脱着
					if (!loaded)
					{
						// インビジビリティクローク バルログブラッディクローク装備中でインビジ状態の場合はインビジ状態の解除
						if (temp.ItemId == 20077 || temp.ItemId == 20062 || temp.ItemId == 120077)
						{
							if (_owner.Invisble)
							{
								_owner.delInvis();
								return;
							}
						}
					}
					Equipped(item, false); //3.63
					item.Equipped = false;
					_owner.EquipSlot.remove(item);
				}
				if (!loaded)
				{ // 最初の読込時はＤＢパケット関連の処理はしない
					// XXX:意味のないセッター
					_owner.CurrentHp = _owner.CurrentHp;
					_owner.CurrentMp = _owner.CurrentMp;
					updateItem(item, COL_EQUIPPED);
					_owner.sendPackets(new S_OwnCharStatus(_owner));
					if (temp.Type2 == 1 && changeWeapon == false)
					{ // 武器の場合はビジュアル更新。ただし、武器の持ち替えで武器を脱着する時は更新しない
						_owner.sendPackets(new S_CharVisualUpdate(_owner));
						_owner.broadcastPacket(new S_CharVisualUpdate(_owner));
					}
					// _owner.getNetConnection().saveCharToDisk(_owner); //
					// DBにキャラクター情報を書き込む
				}
			}
		}
		public virtual void Equipped(L1ItemInstance item, bool isEq)
		{
		int RingID = item.RingID;
		//3.63　新增裝備欄
		if ((item.Item.Type2 == 2) && (item.Equipped))
		{ // 判斷是否可用裝備
			int items = 0;
			if ((item.Item.Type == 1))
			{
				items = 1;
			}
			else if ((item.Item.Type == 2))
			{
				items = 2;
			}
			else if ((item.Item.Type == 3))
			{
				items = 3;
			}
			else if ((item.Item.Type == 4))
			{
				items = 4;
			}
			else if ((item.Item.Type == 5))
			{
				items = 6;
			}
			else if ((item.Item.Type == 6))
			{
				items = 5;
			}
			else if ((item.Item.Type == 7))
			{
				items = 7;
			}
			else if ((item.Item.Type == 8))
			{
				items = 10;
			}
			else if ((item.Item.Type == 9))
			{
				if (isEq == true)
				{
					//第一顆&二顆戒指
					if (checkRingEquipped(18))
					{
						items = 19;
						item.RingID = 19;
					}
					else
					{
						items = 18;
						item.RingID = 18;
					}
					//第三顆戒指
					if (getTypeEquipped(2, 9) == 3)
					{
						if (!checkRingEquipped(18))
						{
							   items = 18;
							   item.RingID = 18;
						}
						else if (!checkRingEquipped(19))
						{
								items = 19;
								item.RingID = 19;
						}
						else if (!checkRingEquipped(20))
						{
								items = 20;
								item.RingID = 20;
						}
					}
					//第四顆戒指
					if (getTypeEquipped(2, 9) == 4)
					{
						if (!checkRingEquipped(18))
						{
							   items = 18;
							   item.RingID = 18;
						}
						else if (!checkRingEquipped(19))
						{
								items = 19;
								item.RingID = 19;
						}
						else if (!checkRingEquipped(20))
						{
								items = 20;
								item.RingID = 20;
						}
						else if (!checkRingEquipped(21))
						{
								items = 21;
								item.RingID = 21;
						}
					}
				}
				else
				{
					if (RingID == 18)
					{
						items = 18;
						item.RingID = 0;
					}
					else if (RingID == 19)
					{
						items = 19;
						item.RingID = 0;
					}
					else if (RingID == 20)
					{
						items = 20;
						item.RingID = 0;
					}
					else if (RingID == 21)
					{
						items = 21;
						item.RingID = 0;
					}
				}
			}
			else if ((item.Item.Type == 10))
			{
				items = 11;
			}
			else if ((item.Item.Type == 11))
			{
				items = 19;
			}
			else if ((item.Item.Type == 12))
			{
				items = 12;
			}
			else if ((item.Item.Type == 13))
			{
				items = 7;
			}
			else if ((item.Item.Type == 14))
			{
				items = 22;
			}
			else if ((item.Item.Type == 15))
			{
				items = 23;
			}
			else if ((item.Item.Type == 16))
			{
				items = 24;
			}
			else if ((item.Item.Type == 17))
			{
				items = 25;
			}
			else if ((item.Item.Type == 18))
			{
				items = 26;
			}
			else if ((item.Item.Type == 19))
			{
				items = 20;
			}
			else if ((item.Item.Type == 20))
			{
				items = 21;
			}
			_owner.sendPackets(new S_EquipmentWindow(_owner, item.Id,items,isEq));
		}

		if ((item.Item.Type2 == 1) && (item.Equipped))
		{ // 判斷是否可用裝備
			int items = 8;
			_owner.sendPackets(new S_EquipmentWindow(_owner, item.Id,items,isEq));
		}
		//3.63　新增裝備欄
		}
			  // 檢查戒指確認  這是Copy (checkEquipped(int id)) 日方原有程式(前輩寫的…)
		   public virtual bool checkRingEquipped(int id)
		   {
		foreach (object itemObject in _items)
		{
			L1ItemInstance item = (L1ItemInstance) itemObject;
			if (item.RingID == id && item.Equipped)
			{
				return true;
			}
		}
		return false;
		   }
		// 特定のアイテムを装備しているか確認
		public virtual bool checkEquipped(int id)
		{
			foreach (object itemObject in _items)
			{
				L1ItemInstance item = (L1ItemInstance) itemObject;
				if (item.Item.ItemId == id && item.Equipped)
				{
					return true;
				}
			}
			return false;
		}

		// 特定のアイテムを全て装備しているか確認（セットボーナスがあるやつの確認用）
		public virtual bool checkEquipped(int[] ids)
		{
			foreach (int id in ids)
			{
				if (!checkEquipped(id))
				{
					return false;
				}
			}
			return true;
		}

		// 特定のタイプのアイテムを装備している数
		public virtual int getTypeEquipped(int type2, int type)
		{
			int equipeCount = 0;
			foreach (object itemObject in _items)
			{
				L1ItemInstance item = (L1ItemInstance) itemObject;
				if (item.Item.Type2 == type2 && item.Item.Type == type && item.Equipped)
				{
					equipeCount++;
				}
			}
			return equipeCount;
		}

		// 装備している特定のタイプのアイテム
		public virtual L1ItemInstance getItemEquipped(int type2, int type)
		{
			L1ItemInstance equipeitem = null;
			foreach (object itemObject in _items)
			{
				L1ItemInstance item = (L1ItemInstance) itemObject;
				if (item.Item.Type2 == type2 && item.Item.Type == type && item.Equipped)
				{
					equipeitem = item;
					break;
				}
			}
			return equipeitem;
		}

		// 装備しているリング
		public virtual L1ItemInstance[] RingEquipped
		{
			get
			{
				L1ItemInstance[] equipeItem = new L1ItemInstance[2];
				int equipeCount = 0;
				foreach (object itemObject in _items)
				{
					L1ItemInstance item = (L1ItemInstance) itemObject;
					if (item.Item.Type2 == 2 && item.Item.Type == 9 && item.Equipped)
					{
						equipeItem[equipeCount] = item;
						equipeCount++;
						if (equipeCount == 2)
						{
							break;
						}
					}
				}
				return equipeItem;
			}
		}

        public int Arrow { get; internal set; }
        public int Sting { get; internal set; }

        // 変身時に装備できない装備を外す
        public virtual void takeoffEquip(int polyid)
		{
			takeoffWeapon(polyid);
			takeoffArmor(polyid);
		}

		// 変身時に装備できない武器を外す
		private void takeoffWeapon(int polyid)
		{
			if (_owner.Weapon == null)
			{ // 素手
				return;
			}

			bool takeoff = false;
			int weapon_type = _owner.Weapon.Item.Type;
			// 装備出来ない武器を装備してるか？
			takeoff = !L1PolyMorph.isEquipableWeapon(polyid, weapon_type);

			if (takeoff)
			{
				setEquipped(_owner.Weapon, false, false, false);
			}
		}

		// 変身時に装備できない防具を外す
		private void takeoffArmor(int polyid)
		{
			L1ItemInstance armor = null;

			// ヘルムからガーダーまでチェックする
			for (int type = 0; type <= 13; type++)
			{
				// 装備していて、装備不可の場合は外す
				if (getTypeEquipped(2, type) != 0 && !L1PolyMorph.isEquipableArmor(polyid, type))
				{
					if (type == 9)
					{ // リングの場合は、両手分外す
						armor = getItemEquipped(2, type);
						if (armor != null)
						{
							setEquipped(armor, false, false, false);
						}
						armor = getItemEquipped(2, type);
						if (armor != null)
						{
							setEquipped(armor, false, false, false);
						}
					}
					else
					{
						armor = getItemEquipped(2, type);
						if (armor != null)
						{
							setEquipped(armor, false, false, false);
						}
					}
				}
			}
		}

		// 使用するアローの取得
		public virtual L1ItemInstance getArrow()
		{
			return getBullet(0);
		}

		// 使用するスティングの取得
		public virtual L1ItemInstance getSting()
		{
			return getBullet(15);
		}

		private L1ItemInstance getBullet(int type)
		{
			L1ItemInstance bullet;
			int priorityId = 0;
			if (type == 0)
			{
				priorityId = _arrowId; // アロー
			}
			if (type == 15)
			{
				priorityId = _stingId; // スティング
			}
			if (priorityId > 0) // 優先する弾があるか
			{
				bullet = findItemId(priorityId);
				if (bullet != null)
				{
					return bullet;
				}
				else // なくなっていた場合は優先を消す
				{
					if (type == 0)
					{
						_arrowId = 0;
					}
					if (type == 15)
					{
						_stingId = 0;
					}
				}
			}

			foreach (object itemObject in _items) // 弾を探す
			{
				bullet = (L1ItemInstance) itemObject;
				if (bullet.Item.Type == type && bullet.Item.Type2 == 0)
				{
					if (type == 0)
					{
						_arrowId = bullet.Item.ItemId; // 優先にしておく
					}
					if (type == 15)
					{
						_stingId = bullet.Item.ItemId; // 優先にしておく
					}
					return bullet;
				}
			}
			return null;
		}

		// 優先するアローの設定
		public virtual void setArrow(int id)
		{
			_arrowId = id;
		}

		// 優先するスティングの設定
		public virtual void setSting(int id)
		{
			_stingId = id;
		}

		// 装備によるＨＰ自然回復補正
		public virtual int hpRegenPerTick()
		{
			int hpr = 0;
			foreach (object itemObject in _items)
			{
				L1ItemInstance item = (L1ItemInstance) itemObject;
				if (item.Equipped)
				{
					hpr += item.Item.get_addhpr() + item.Hpr;
				}
			}
			return hpr;
		}

		// 装備によるＭＰ自然回復補正
		public virtual int mpRegenPerTick()
		{
			int mpr = 0;
			foreach (object itemObject in _items)
			{
				L1ItemInstance item = (L1ItemInstance) itemObject;
				if (item.Equipped)
				{
					mpr += item.Item.get_addmpr() + item.Mpr;
				}
			}
			return mpr;
		}

		public virtual L1ItemInstance CaoPenalty()
		{
			int rnd = RandomHelper.Next(_items.Count);
			L1ItemInstance penaltyItem = _items[rnd];
			if (penaltyItem.Item.ItemId == L1ItemId.ADENA || !penaltyItem.Item.Tradable)
			{
				return null;
			}
			object[] petlist = _owner.PetList.Values.ToArray();
			foreach (object petObject in petlist)
			{
				if (petObject is L1PetInstance)
				{
					L1PetInstance pet = (L1PetInstance) petObject;
					if (penaltyItem.Id == pet.ItemObjId)
					{
						return null;
					}
				}
			}
			setEquipped(penaltyItem, false);
			return penaltyItem;
		}
	}

}