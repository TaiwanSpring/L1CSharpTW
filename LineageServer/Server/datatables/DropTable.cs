using LineageServer.Interfaces;
using LineageServer.Models;
using LineageServer.Server.Model;
using LineageServer.Server.Model.identity;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;
using LineageServer.Serverpackets;
using LineageServer.Utils;
using System.Collections.Generic;

namespace LineageServer.Server.DataTables
{
	class DropTable
	{
		private readonly static IDataSource dataSource =
			 Container.Instance.Resolve<IDataSourceFactory>()
			 .Factory(Enum.DataSourceTypeEnum.Droplist);


		private static readonly ILogger _log = Logger.GetLogger(nameof(DropTable));

		private static DropTable _instance;

		private readonly IDictionary<int, IList<L1Drop>> _droplists; //每個怪物的掉落列表

		public static DropTable Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new DropTable();
				}
				return _instance;
			}
		}

		private DropTable()
		{
			_droplists = allDropList();
		}

		private IDictionary<int, IList<L1Drop>> allDropList()
		{
			IDictionary<int, IList<L1Drop>> droplistMap = MapFactory.NewMap<int, IList<L1Drop>>();

			IList<IDataSourceRow> dataSourceRows = dataSource.Select().Query();

			for (int i = 0; i < dataSourceRows.Count; i++)
			{
				IDataSourceRow dataSourceRow = dataSourceRows[i];
				int mobId = dataSourceRow.getInt("mobId");
				int itemId = dataSourceRow.getInt("itemId");
				int min = dataSourceRow.getInt("min");
				int max = dataSourceRow.getInt("max");
				int chance = dataSourceRow.getInt("chance");
				int enchantlvl = dataSourceRow.getInt("enchantlvl");

				L1Drop drop = new L1Drop(mobId, itemId, min, max, chance, enchantlvl);

				if (droplistMap.ContainsKey(drop.Mobid))
				{
					droplistMap[drop.Mobid].Add(drop);
				}
				else
				{
					droplistMap.Add(drop.Mobid, new List<L1Drop>() { drop });
				}
			}
			return droplistMap;
		}

		// インベントリにドロップを設定
		public virtual void setDrop(L1NpcInstance npc, L1Inventory inventory)
		{
			// ドロップリストの取得
			int mobId = npc.NpcTemplate.get_npcId();
			IList<L1Drop> dropList = _droplists[mobId];
			if (dropList == null)
			{
				return;
			}

			// レート取得
			double droprate = Config.RATE_DROP_ITEMS;
			if (droprate <= 0)
			{
				droprate = 0;
			}
			double adenarate = Config.RATE_DROP_ADENA;
			if (adenarate <= 0)
			{
				adenarate = 0;
			}
			if (( droprate <= 0 ) && ( adenarate <= 0 ))
			{
				return;
			}

			int itemId;
			int itemCount;
			int addCount;
			int randomChance;
			int enchantlvl;
			L1ItemInstance item;

			foreach (L1Drop drop in dropList)
			{
				// ドロップアイテムの取得
				itemId = drop.Itemid;
				if (( adenarate == 0 ) && ( itemId == L1ItemId.ADENA ))
				{
					continue; // アデナレート０でドロップがアデナの場合はスルー
				}

				// ドロップチャンス判定
				randomChance = RandomHelper.Next(0xf4240) + 1;
				double rateOfMapId = MapsTable.Instance.getDropRate(npc.MapId);
				double rateOfItem = DropItemTable.Instance.getDropRate(itemId);
				if (( droprate == 0 ) || ( drop.Chance * droprate * rateOfMapId * rateOfItem < randomChance ))
				{
					continue;
				}

				// ドロップ個数を設定
				double amount = DropItemTable.Instance.getDropAmount(itemId);
				int min = (int)( drop.Min * amount );
				int max = (int)( drop.Max * amount );

				itemCount = min;
				addCount = max - min + 1;
				if (addCount > 1)
				{
					itemCount += RandomHelper.Next(addCount);
				}
				if (itemId == L1ItemId.ADENA)
				{ // ドロップがアデナの場合はアデナレートを掛ける
					itemCount *= (int)adenarate;
				}
				if (itemCount < 0)
				{
					itemCount = 0;
				}
				if (itemCount > 2000000000)
				{
					itemCount = 2000000000;
				}

				enchantlvl = drop.Enchantlvl;

				// アイテムの生成
				item = ItemTable.Instance.createItem(itemId);
				item.Count = itemCount;
				item.EnchantLevel = enchantlvl;

				// アイテム格納
				inventory.storeItem(item);
			}
		}

		// ドロップを分配
		public virtual void dropShare(L1NpcInstance npc, IList<L1Character> acquisitorList, IList<int> hateList)
		{
			L1Inventory inventory = npc.Inventory;
			if (inventory.Size == 0)
			{
				return;
			}
			if (acquisitorList.Count != hateList.Count)
			{
				return;
			}
			// ヘイトの合計を取得
			int totalHate = 0;
			L1Character acquisitor;
			for (int i = hateList.Count - 1; i >= 0; i--)
			{
				acquisitor = acquisitorList[i];
				if (( Config.AUTO_LOOT == 2 ) && ( ( acquisitor is L1SummonInstance ) || ( acquisitor is L1PetInstance ) ))
				{
					acquisitorList.RemoveAt(i);
					hateList.RemoveAt(i);
				}
				else if (( acquisitor != null ) && ( acquisitor.MapId == npc.MapId ) && ( acquisitor.Location.getTileLineDistance(npc.Location) <= Config.LOOTING_RANGE ))
				{
					totalHate += hateList[i];
				}
				else
				{ // nullだったり死んでたり遠かったら排除
					acquisitorList.RemoveAt(i);
					hateList.RemoveAt(i);
				}
			}

			// ドロップの分配
			L1ItemInstance item;
			L1Inventory targetInventory = null;
			L1PcInstance player;
			L1PcInstance[] partyMember;
			int randomInt;
			int chanceHate;
			int itemId;
			for (int i = inventory.Size; i > 0; i--)
			{
				item = inventory.Items[0];
				itemId = item.ItemId;
				bool isGround = false;
				bool isPartyShare = false; //組隊自動分配
				if (( item.Item.Type2 == 0 ) && ( item.Item.Type == 2 ))
				{ // light系アイテム
					item.NowLighting = false;
				}

				if (( ( Config.AUTO_LOOT != 0 ) || ( itemId == L1ItemId.ADENA ) ) && ( totalHate > 0 ))
				{ // オートルーティングかアデナで取得者がいる場合
					randomInt = RandomHelper.Next(totalHate);
					chanceHate = 0;
					for (int j = hateList.Count - 1; j >= 0; j--)
					{
						chanceHate += hateList[j];
						if (chanceHate > randomInt)
						{
							acquisitor = acquisitorList[j];
							if (( itemId >= 40131 ) && ( itemId <= 40135 ))
							{
								if (!( acquisitor is L1PcInstance ) || ( hateList.Count > 1 ))
								{
									targetInventory = null;
									break;
								}
								player = (L1PcInstance)acquisitor;
								if (player.Quest.get_step(L1Quest.QUEST_LYRA) != 1)
								{
									targetInventory = null;
									break;
								}
							}
							if (acquisitor.Inventory.checkAddItem(item, item.Count) == L1Inventory.OK)
							{
								targetInventory = acquisitor.Inventory;
								if (acquisitor is L1PcInstance)
								{
									player = (L1PcInstance)acquisitor;
									L1ItemInstance l1iteminstance = player.Inventory.findItemId(L1ItemId.ADENA); // 所持アデナをチェック
									if (( l1iteminstance != null ) && ( l1iteminstance.Count > 2000000000 ))
									{
										targetInventory = Container.Instance.Resolve<IGameWorld>().getInventory(acquisitor.X, acquisitor.Y, acquisitor.MapId); // 持てないので足元に落とす
										isGround = true;
										player.sendPackets(new S_ServerMessage(166, "所持しているアデナ", "2,000,000,000を超過しています。")); // \f1%0が%4%1%3%2
									}
									else
									{
										if (player.InParty)
										{ // パーティの場合
											partyMember = player.Party.Members;
											//組隊自動分配 (item.getCount() > 現場隊員人數才分配，分配後剩餘數量 otherCount 歸第一順位隊員所有(應該是殺死怪的人))
											if (player.PartyType == 1)
											{
												int partySize = 0;
												int memberItemCount = 0;
												foreach (L1PcInstance member in partyMember)
												{
													if (member != null && member.MapId == npc.MapId && member.CurrentHp > 0 && !member.Dead)
													{
														partySize++;
													}
												}
												if (partySize > 1 && item.Count >= partySize)
												{
													memberItemCount = item.Count / partySize;
													int otherCount = item.Count - memberItemCount * partySize;
													if (otherCount > 0)
													{
														item.Count = memberItemCount + otherCount;
													}
													foreach (L1PcInstance member in partyMember)
													{
														if (member != null && member.MapId == npc.MapId && member.CurrentHp > 0 && !member.Dead)
														{
															member.Inventory.storeItem(itemId, memberItemCount);
															foreach (L1PcInstance pc in player.Party.Members)
															{
																pc.sendPackets(new S_ServerMessage(813, npc.Name, item.LogName, member.Name));
															}
															if (otherCount > 0)
															{
																item.Count = memberItemCount;
																otherCount = 0;
															}
														}
													}
													inventory.removeItem(item, item.Count);
													isPartyShare = true;
												}
												else
												{
													foreach (L1PcInstance pc in player.Party.Members)
													{
														pc.sendPackets(new S_ServerMessage(813, npc.Name, item.LogName, player.Name));
													}
												}
											}
											else
											{
												foreach (L1PcInstance element in partyMember)
												{
													element.sendPackets(new S_ServerMessage(813, npc.Name, item.LogName, player.Name));
												}
											}
										}
										else
										{
											// ソロの場合
											player.sendPackets(new S_ServerMessage(143, npc.Name, item.LogName)); // \f1%0が%1をくれました。
										}
									}
								}
							}
							else
							{
								targetInventory = Container.Instance.Resolve<IGameWorld>().getInventory(acquisitor.X, acquisitor.Y, acquisitor.MapId); // 持てないので足元に落とす
								isGround = true;
							}
							break;
						}
					}
				}
				else
				{ // ノンオートルーティング
					IList<int> dirList = ListFactory.NewList<int>();
					for (int j = 0; j < 8; j++)
					{
						dirList.Add(j);
					}
					int x = 0;
					int y = 0;
					int dir = 0;
					do
					{
						if (dirList.Count == 0)
						{
							x = 0;
							y = 0;
							break;
						}
						randomInt = RandomHelper.Next(dirList.Count);
						dir = dirList[randomInt];
						dirList.RemoveAt(randomInt);
						switch (dir)
						{
							case 0:
								x = 0;
								y = -1;
								break;
							case 1:
								x = 1;
								y = -1;
								break;
							case 2:
								x = 1;
								y = 0;
								break;
							case 3:
								x = 1;
								y = 1;
								break;
							case 4:
								x = 0;
								y = 1;
								break;
							case 5:
								x = -1;
								y = 1;
								break;
							case 6:
								x = -1;
								y = 0;
								break;
							case 7:
								x = -1;
								y = -1;
								break;
						}
					} while (!npc.Map.isPassable(npc.X, npc.Y, dir));
					targetInventory = Container.Instance.Resolve<IGameWorld>().getInventory(npc.X + x, npc.Y + y, npc.MapId);
					isGround = true;
				}
				if (( itemId >= 40131 ) && ( itemId <= 40135 ))
				{
					if (isGround || ( targetInventory == null ))
					{
						inventory.removeItem(item, item.Count);
						continue;
					}
				}
				inventory.tradeItem(item, item.Count, targetInventory);
			}
			npc.turnOnOffLight();
		}

	}
}