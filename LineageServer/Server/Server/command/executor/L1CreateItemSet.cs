using System;
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
namespace LineageServer.Server.Server.command.executor
{

	using GMCommandsConfig = LineageServer.Server.Server.GMCommandsConfig;
	using ItemTable = LineageServer.Server.Server.datatables.ItemTable;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;
	using L1Item = LineageServer.Server.Server.Templates.L1Item;
	using L1ItemSetItem = LineageServer.Server.Server.Templates.L1ItemSetItem;

	/// <summary>
	/// GM指令：創立套裝
	/// </summary>
	public class L1CreateItemSet : L1CommandExecutor
	{
		private L1CreateItemSet()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1CreateItemSet();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				string name = (new StringTokenizer(arg)).nextToken();
				IList<L1ItemSetItem> list = GMCommandsConfig.ITEM_SETS[name];
				if (list == null)
				{
					pc.sendPackets(new S_SystemMessage(name + " 是未定義的套裝。"));
					return;
				}
				foreach (L1ItemSetItem item in list)
				{
					L1Item temp = ItemTable.Instance.getTemplate(item.Id);
					if (!temp.Stackable && (0 != item.Enchant))
					{
						for (int i = 0; i < item.Amount; i++)
						{
							L1ItemInstance inst = ItemTable.Instance.createItem(item.Id);
							inst.EnchantLevel = item.Enchant;
							pc.Inventory.storeItem(inst);
						}
					}
					else
					{
						pc.Inventory.storeItem(item.Id, item.Amount);
					}
				}
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage("請輸入 .itemset 套裝名稱。"));
			}
		}
	}

}