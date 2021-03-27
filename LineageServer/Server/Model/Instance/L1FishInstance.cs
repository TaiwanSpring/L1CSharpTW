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
namespace LineageServer.Server.Model.Instance
{

	using S_ChangeHeading = LineageServer.Serverpackets.S_ChangeHeading;
	using S_DoActionGFX = LineageServer.Serverpackets.S_DoActionGFX;
	using S_NPCPack = LineageServer.Serverpackets.S_NPCPack;
	using L1Npc = LineageServer.Server.Templates.L1Npc;
	using Random = LineageServer.Utils.Random;

	[Serializable]
	public class L1FishInstance : L1NpcInstance
	{

		private const long serialVersionUID = 1L;
		private fishTimer _fishTimer;

		public L1FishInstance(L1Npc template) : base(template)
		{
			_fishTimer = new fishTimer(this, this);
			Timer timer = new Timer(true);
			timer.scheduleAtFixedRate(_fishTimer, 1000, (RandomHelper.Next(30, 30) * 1000));
		}

		public override void onPerceive(L1PcInstance perceivedFrom)
		{
			perceivedFrom.addKnownObject(this);
			perceivedFrom.sendPackets(new S_NPCPack(this));
		}

		private class fishTimer : TimerTask
		{
			private readonly L1FishInstance outerInstance;


			internal L1FishInstance _fish;

			public fishTimer(L1FishInstance outerInstance, L1FishInstance fish)
			{
				this.outerInstance = outerInstance;
				_fish = fish;
			}

			public override void run()
			{
				if (_fish != null)
				{
					_fish.Heading = RandomHelper.Next(8); // 隨機面向
					_fish.broadcastPacket(new S_ChangeHeading(_fish)); // 更新面向
					_fish.broadcastPacket(new S_DoActionGFX(_fish.Id, 0)); // 動作
				}
				else
				{
					cancel();
				}
			}
		}

	}

}