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

	using ActionCodes = LineageServer.Server.Server.ActionCodes;
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using S_DoActionGFX = LineageServer.Server.Server.serverpackets.S_DoActionGFX;

	public class L1NpcDeleteTimer : TimerTask
	{
		public L1NpcDeleteTimer(L1NpcInstance npc, int timeMillis)
		{
			_npc = npc;
			_timeMillis = timeMillis;
		}

		public override void run()
		{
			// 龍之門扉存在時間到時
			if (_npc != null)
			{
				if (_npc.NpcId == 81273 || _npc.NpcId == 81274 || _npc.NpcId == 81275 || _npc.NpcId == 81276 || _npc.NpcId == 81277)
				{
					if (_npc.NpcId == 81277)
					{ // 隱匿的巨龍谷入口關閉
						L1DragonSlayer.Instance.HiddenDragonValleyStstus = 0;
					}
					// 結束屠龍副本
					L1DragonSlayer.Instance.setPortalPack(_npc.PortalNumber, null);
					L1DragonSlayer.Instance.endDragonPortal(_npc.PortalNumber);
					// 門扉消失動作
					_npc.Status = ActionCodes.ACTION_Die;
					_npc.broadcastPacket(new S_DoActionGFX(_npc.Id, ActionCodes.ACTION_Die));
				}
				_npc.deleteMe();
				cancel();
			}
		}

		public virtual void begin()
		{
			Timer timer = new Timer();
			timer.schedule(this, _timeMillis);
		}

		private readonly L1NpcInstance _npc;

		private readonly int _timeMillis;
	}

}