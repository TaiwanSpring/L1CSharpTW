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
namespace LineageServer.Server.Server.serverpackets
{

	using Config = LineageServer.Server.Config;
	using Opcodes = LineageServer.Server.Server.Opcodes;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_WarTime : ServerBasePacket
	{
		private const string S_WAR_TIME = "[S] S_WarTime";

		public S_WarTime(DateTime cal)
		{
			// 1997/01/01 17:00を基点としている
			DateTime base_cal = new DateTime();
			base_cal = new DateTime(1997, 0, 1, 17, 0, 0);
			long base_millis = base_cal.Ticks;
			long millis = cal.Ticks;
			long diff = millis - base_millis;
			diff -= 1200 * 60 * 1000; // 誤差修正
			diff = diff / 60000; // 分以下切捨て
			// timeは1加算すると3:02（182分）進む
			int time = (int)(diff / 182);

			// writeDの直前のwriteCで時間の調節ができる
			// 0.7倍した時間だけ縮まるが
			// 1つ調整するとその次の時間が広がる？
			writeC(Opcodes.S_OPCODE_WARTIME);
			writeH(6); // リストの数（6以上は無効）
			writeS(Config.TIME_ZONE); // 時間の後ろの（）内に表示される文字列
			writeC(0); // ?
			writeC(0); // ?
			writeC(0);
			writeD(time);
			writeC(0);
			writeD(time - 1);
			writeC(0);
			writeD(time - 2);
			writeC(0);
			writeD(time - 3);
			writeC(0);
			writeD(time - 4);
			writeC(0);
			writeD(time - 5);
			writeC(0);
		}

		public override sbyte[] Content
		{
			get
			{
				return Bytes;
			}
		}

		public override string Type
		{
			get
			{
				return S_WAR_TIME;
			}
		}
	}

}