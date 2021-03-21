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

	using ItemTable = LineageServer.Server.Server.datatables.ItemTable;
	using L1Inventory = LineageServer.Server.Server.Model.L1Inventory;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;
	using L1Item = LineageServer.Server.Server.Templates.L1Item;

	/// <summary>
	/// GM指令：創立道具
	/// </summary>
	public class L1CreateItem : L1CommandExecutor
	{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		private static Logger _log = Logger.getLogger(typeof(L1CreateItem).FullName);

		private L1CreateItem()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1CreateItem();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				StringTokenizer st = new StringTokenizer(arg);
				string nameid = st.nextToken();
				int count = 1;
				if (st.hasMoreTokens())
				{
					count = int.Parse(st.nextToken());
				}
				int enchant = 0;
				if (st.hasMoreTokens())
				{
					enchant = int.Parse(st.nextToken());
				}
				int isId = 0;
				if (st.hasMoreTokens())
				{
					isId = int.Parse(st.nextToken());
				}
				string attr = "";
				if (st.hasMoreTokens())
				{
					attr = st.nextToken();
				}
				int attLevel = 0;
				if (st.hasMoreTokens())
				{
					attLevel = int.Parse(st.nextToken());
				}
				int itemid = 0;
				try
				{
					itemid = int.Parse(nameid);
				}
				catch (System.FormatException)
				{
					itemid = ItemTable.Instance.findItemIdByNameWithoutSpace(nameid);
					if (itemid == 0)
					{
						pc.sendPackets(new S_SystemMessage("找不到符合條件項目。"));
						return;
					}
				}
				L1Item temp = ItemTable.Instance.getTemplate(itemid);
				if (temp != null)
				{
					if (temp.Stackable)
					{
						L1ItemInstance item = ItemTable.Instance.createItem(itemid);
						item.EnchantLevel = 0;
						item.Count = count;
						if (isId == 1)
						{
							item.Identified = true;
						}
						if (pc.Inventory.checkAddItem(item, count) == L1Inventory.OK)
						{
							pc.Inventory.storeItem(item);
							pc.sendPackets(new S_ServerMessage(403, item.LogName + "(ID:" + itemid + ")"));
						}
					}
					else
					{
						L1ItemInstance item = null;
						int createCount;
						for (createCount = 0; createCount < count; createCount++)
						{
							item = ItemTable.Instance.createItem(itemid);
							item.EnchantLevel = enchant;
							if ((item.Item.Type2 == 2) && (item.Item.Type >= 8 && item.Item.Type <= 12))
							{ // 飾品類
								bool award = false;
								for (int i = 0; i < enchant; i++)
								{
									if (i == 5)
									{
										award = true;
									}
									else
									{
										award = false;
									}
									switch (item.Item.Grade)
									{
										case 0: // 上等
											// 四屬性 +1
											item.FireMr = item.FireMr + 1;
											item.WaterMr = item.WaterMr + 1;
											item.EarthMr = item.EarthMr + 1;
											item.WindMr = item.WindMr + 1;
											// LV6 額外獎勵：體力與魔力回復量 +1
											if (award)
											{
												item.Hpr = item.Hpr + 1;
												item.Mpr = item.Mpr + 1;
											}
											break;
										case 1: // 中等
											// HP +2
											item.setaddHp(item.getaddHp() + 2);
											// LV6 額外獎勵：魔防 +1
											if (award)
											{
												item.M_Def = item.M_Def + 1;
											}
											break;
										case 2: // 初等
											// MP +1
											item.setaddMp(item.getaddMp() + 1);
											// LV6 額外獎勵：魔攻 +1
											if (award)
											{
												item.setaddSp(item.getaddSp() + 1);
											}
											break;
										case 3: // 特等
											// 功能台版未實裝。
											break;
										default:
											break;
									}
								}
							}
							else if (item.Item.Type2 == 1)
							{ // 武器類
								if (attr.Equals("地", StringComparison.OrdinalIgnoreCase) || attr.Equals("1", StringComparison.OrdinalIgnoreCase))
								{
									if (attLevel > 0 && attLevel <= 3)
									{
										item.AttrEnchantKind = 1;
										item.AttrEnchantLevel = attLevel;
									}
								}
								else if (attr.Equals("火", StringComparison.OrdinalIgnoreCase) || attr.Equals("2", StringComparison.OrdinalIgnoreCase))
								{
									if (attLevel > 0 && attLevel <= 3)
									{
										item.AttrEnchantKind = 2;
										item.AttrEnchantLevel = attLevel;
									}
								}
								else if (attr.Equals("水", StringComparison.OrdinalIgnoreCase) || attr.Equals("4", StringComparison.OrdinalIgnoreCase))
								{
									if (attLevel > 0 && attLevel <= 3)
									{
										item.AttrEnchantKind = 4;
										item.AttrEnchantLevel = attLevel;
									}
								}
								else if (attr.Equals("風", StringComparison.OrdinalIgnoreCase) || attr.Equals("8", StringComparison.OrdinalIgnoreCase))
								{
									if (attLevel > 0 && attLevel <= 3)
									{
										item.AttrEnchantKind = 8;
										item.AttrEnchantLevel = attLevel;
									}
								}
							}
							if (isId == 1)
							{
								item.Identified = true;
							}
							if (pc.Inventory.checkAddItem(item, 1) == L1Inventory.OK)
							{
								pc.Inventory.storeItem(item);
							}
							else
							{
								break;
							}
						}
						if (createCount > 0)
						{
							pc.sendPackets(new S_ServerMessage(403, item.LogName + "(ID:" + itemid + ")"));
						}
					}
				}
				else
				{
					pc.sendPackets(new S_SystemMessage("指定的道具編號不存在"));
				}
			}
			catch (Exception e)
			{
				_log.log(Enum.Level.Server, e.Message, e);
				pc.sendPackets(new S_SystemMessage("請輸入 .item itemid|name [數量] [強化等級] [鑑定狀態] [武器屬性] [屬性等級]。"));
			}
		}
	}

}