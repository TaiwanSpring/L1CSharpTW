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

	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_DoActionGFX = LineageServer.Server.Server.serverpackets.S_DoActionGFX;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;

	/// <summary>
	/// GM指令：執行玩家/NPC動作
	/// </summary>
	public class L1Action : L1CommandExecutor
	{
		private L1Action()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1Action();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				StringTokenizer st = new StringTokenizer(arg);
				int actId = Convert.ToInt32(st.nextToken(), 10);
				pc.sendPackets(new S_DoActionGFX(pc.Id, actId));
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage("請輸入 " + cmdName + " actid。"));
			}
		}
	}

}