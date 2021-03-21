using LineageServer.Model;
using LineageServer.Server.Server.datatables;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;
using LineageServer.Server.Server.Templates;
using System;
using System.Collections.Generic;
namespace LineageServer.Server.Server.Command.Executor
{
	/// <summary>
	/// GM指令：創立套裝
	/// </summary>
	class L1CreateItemSet : IL1CommandExecutor
	{

		public void Execute(L1PcInstance pc, string cmdName, string arg)
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