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
namespace LineageServer.Server.Server.serverpackets
{
	using Opcodes = LineageServer.Server.Server.Opcodes;
	using L1GameTimeClock = LineageServer.Server.Server.Model.Gametime.L1GameTimeClock;

	public class S_GameTime : ServerBasePacket
	{
		public S_GameTime(int time)
		{
			buildPacket(time);
		}

		public S_GameTime()
		{
			int time = L1GameTimeClock.Instance.currentTime().Seconds;
			buildPacket(time);
		}

		private void buildPacket(int time)
		{
			writeC(Opcodes.S_OPCODE_GAMETIME);
			writeD(time);
		}

		public override sbyte[] Content
		{
			get
			{
				return Bytes;
			}
		}
	}

}