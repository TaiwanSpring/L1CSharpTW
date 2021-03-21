﻿using System;

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

	using L1PolyMorph = LineageServer.Server.Server.Model.L1PolyMorph;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;

	public class L1Poly : L1CommandExecutor
	{
		private L1Poly()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1Poly();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				StringTokenizer st = new StringTokenizer(arg);
				string name = st.nextToken();
				int polyid = int.Parse(st.nextToken());

				L1PcInstance tg = L1World.Instance.getPlayer(name);

				if (tg == null)
				{
					pc.sendPackets(new S_ServerMessage(73, name)); // \f1%0はゲームをしていません。
				}
				else
				{
					try
					{
						L1PolyMorph.doPoly(tg, polyid, 7200, L1PolyMorph.MORPH_BY_GM);
					}
					catch (Exception)
					{
						pc.sendPackets(new S_SystemMessage("請輸入 .poly 玩家名稱 變身代碼。"));
					}
				}
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage(cmdName + " 請輸入  玩家名稱 變身代碼。"));
			}
		}
	}

}