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
namespace LineageServer.Server.Server.command.executor
{

	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1ItemId = LineageServer.Server.Server.Model.identity.L1ItemId;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;

	/// <summary>
	/// GM指令：增加金幣
	/// </summary>
	public class L1Adena : L1CommandExecutor
	{
		private L1Adena()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1Adena();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				StringTokenizer stringtokenizer = new StringTokenizer(arg);
				int count = int.Parse(stringtokenizer.nextToken());

				L1ItemInstance adena = pc.Inventory.storeItem(L1ItemId.ADENA, count);
				if (adena != null)
				{
					pc.sendPackets(new S_SystemMessage((new StringBuilder()).Append(count).Append(" 金幣產生。").ToString()));
				}
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage((new StringBuilder()).Append("請輸入 .adena 數量||.金幣  數量。").ToString()));
			}
		}
	}

}