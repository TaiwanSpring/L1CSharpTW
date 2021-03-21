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
	using L1DwarfInventory = LineageServer.Server.Server.Model.L1DwarfInventory;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;
	using L1Item = LineageServer.Server.Server.Templates.L1Item;

	public class L1Present : L1CommandExecutor
	{
		private L1Present()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1Present();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				StringTokenizer st = new StringTokenizer(arg);
				string account = st.nextToken();
				int itemid = Convert.ToInt32(st.nextToken(), 10);
				int enchant = Convert.ToInt32(st.nextToken(), 10);
				int count = Convert.ToInt32(st.nextToken(), 10);

				L1Item temp = ItemTable.Instance.getTemplate(itemid);
				if (temp == null)
				{
					pc.sendPackets(new S_SystemMessage("不存在的道具編號。"));
					return;
				}

				L1DwarfInventory.present(account, itemid, enchant, count);
				pc.sendPackets(new S_SystemMessage(temp.IdentifiedNameId + "數量" + count + "個發送出去了。", true));
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage("請輸入 : " + ".present 帳號 道具編號 數量 強化等級。（* 等於所有帳號）"));
			}
		}
	}

}