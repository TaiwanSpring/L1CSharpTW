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
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_Message_YN = LineageServer.Server.Server.serverpackets.S_Message_YN;
	using S_SkillSound = LineageServer.Server.Server.serverpackets.S_SkillSound;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;

	public class L1Ress : L1CommandExecutor
	{
		private L1Ress()
		{
		}

		public static L1CommandExecutor Instance
		{
			get
			{
				return new L1Ress();
			}
		}

		public virtual void execute(L1PcInstance pc, string cmdName, string arg)
		{
			try
			{
				int objid = pc.Id;
				pc.sendPackets(new S_SkillSound(objid, 759));
				pc.broadcastPacket(new S_SkillSound(objid, 759));
				pc.CurrentHp = pc.MaxHp;
				pc.CurrentMp = pc.MaxMp;
				foreach (L1PcInstance tg in L1World.Instance.getVisiblePlayer(pc))
				{
					if ((tg.CurrentHp == 0) && tg.Dead)
					{
						tg.sendPackets(new S_SystemMessage("GM給予了重生。"));
						tg.broadcastPacket(new S_SkillSound(tg.Id, 3944));
						tg.sendPackets(new S_SkillSound(tg.Id, 3944));
						// 祝福された 復活スクロールと同じ効果
						tg.TempID = objid;
						tg.sendPackets(new S_Message_YN(322, "")); // また復活したいですか？（Y/N）
					}
					else
					{
						tg.sendPackets(new S_SystemMessage("GM給予了治療。"));
						tg.broadcastPacket(new S_SkillSound(tg.Id, 832));
						tg.sendPackets(new S_SkillSound(tg.Id, 832));
						tg.CurrentHp = tg.MaxHp;
						tg.CurrentMp = tg.MaxMp;
					}
				}
			}
			catch (Exception)
			{
				pc.sendPackets(new S_SystemMessage(cmdName + " 指令錯誤"));
			}
		}
	}

}