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
namespace LineageServer.Serverpackets
{
	using FastTable = javolution.util.FastTable;
	using Opcodes = LineageServer.Server.Opcodes;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using System.Collections.Generic;

	/// <summary>
	/// 屬於PacketBox的封包 只是抓出來另外寫
	/// GameStart	進入賽跑的畫面
	/// GameEnd		離開賽跑的畫面
	/// </summary>
	class S_Race : ServerBasePacket
	{
		private const string S_RACE = "[S] S_Race";

		private byte[] _byte = null;

		public const int GameStart = 0x40;
		public const int CountDown = 0x41;
		public const int PlayerInfo = 0x42;
		public const int Lap = 0x43;
		public const int Winner = 0x44;
		public const int GameOver = 0x45;
		public const int GameEnd = 0x46;

		//GameStart// CountDown// GameOver// GameEnd
		public S_Race(int type)
		{
			WriteC(Opcodes.S_OPCODE_PACKETBOX);
			WriteC(type);
			if (type == GameStart)
			{
				WriteC(0x05); //倒數5秒
			}
		}

		public S_Race(List<L1PcInstance> playerList, L1PcInstance pc)
		{
			WriteC(Opcodes.S_OPCODE_PACKETBOX);
			WriteC(PlayerInfo);
			WriteH(playerList.Count); //參賽者人數
			WriteH(playerList.IndexOf(pc)); //名次
			foreach (L1PcInstance player in playerList)
			{
				if (player == null)
				{
					continue;
				}
				WriteS(player.Name);
			}
		}

		public S_Race(int maxLap, int lap)
		{
			WriteC(Opcodes.S_OPCODE_PACKETBOX);
			WriteC(Lap);
			WriteH(maxLap); //最大圈數
			WriteH(lap); //目前圈數
		}

		public S_Race(string winnerName, int time)
		{
			WriteC(Opcodes.S_OPCODE_PACKETBOX);
			WriteC(Winner);
			WriteS(winnerName);
			WriteD(time * 1000);
		}

		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = Bytes;
				}
				return _byte;
			}
		}

		public override string Type
		{
			get
			{
				return S_RACE;
			}
		}
	}

}