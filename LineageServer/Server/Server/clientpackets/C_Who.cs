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
namespace LineageServer.Server.Server.Clientpackets
{
	using Config = LineageServer.Server.Config;
	using ClientThread = LineageServer.Server.Server.ClientThread;
	using GetNowTime = LineageServer.Server.Server.GetNowTime;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_SystemMessage = LineageServer.Server.Server.serverpackets.S_SystemMessage;
	using S_WhoAmount = LineageServer.Server.Server.serverpackets.S_WhoAmount;
	using S_WhoCharinfo = LineageServer.Server.Server.serverpackets.S_WhoCharinfo;
	using L1GameReStart = LineageServer.william.L1GameReStart;

	// Referenced classes of package l1j.server.server.clientpackets:
	// ClientBasePacket

	/// <summary>
	/// 處理收到由客戶端傳來查詢線上人數的封包
	/// </summary>
	class C_Who : ClientBasePacket
	{

		private const string C_WHO = "[C] C_Who";

		public C_Who(sbyte[] decrypt, ClientThread client) : base(decrypt)
		{
			string amount = (L1World.Instance.AllPlayers.Count).ToString();
			L1PcInstance pc = client.ActiveChar;
			if (pc == null)
			{
				return;
			}

			string s = readS();
			L1PcInstance find = L1World.Instance.getPlayer(s);


			if (find != null)
			{
				S_WhoCharinfo s_whocharinfo = new S_WhoCharinfo(find);
				pc.sendPackets(s_whocharinfo);
			}
			else
			{
				if (Config.ALT_WHO_COMMAND || pc.Gm)
				{
					switch (Config.ALT_WHO_TYPE)
					{
					case 1: // 對話視窗顯示
					pc.sendPackets(new S_SystemMessage("經驗值:" + Config.RATE_XP + "倍 掉寶:" + Config.RATE_DROP_ITEMS + "倍 金幣:" + Config.RATE_DROP_ADENA + "倍。"));
					pc.sendPackets(new S_SystemMessage("友好:" + Config.RATE_KARMA + "倍 正義:" + Config.RATE_LA + "倍 。"));
					pc.sendPackets(new S_SystemMessage("武器:" + Config.ENCHANT_CHANCE_WEAPON + "%/防具:" + Config.ENCHANT_CHANCE_ARMOR + "% 屬性卷:" + Config.ATTR_ENCHANT_CHANCE + "%。"));
						//TODO 今天日期
					int Mon = GetNowTime.GetNowMonth(); //TODO 月份錯誤補正
					pc.sendPackets(new S_SystemMessage("今天:" + GetNowTime.GetNowYear() + "年" + (Mon + 1) + "月" + GetNowTime.GetNowDay() + "日" + GetNowTime.GetNowWeek() + "。"));
					//TODO 目前時間
					pc.sendPackets(new S_SystemMessage("現在時間(24h):" + GetNowTime.GetNowHour() + "時" + GetNowTime.GetNowMinute() + "分" + GetNowTime.GetNowSecond() + "秒。"));
					int second = L1GameReStart.Instance.GetRemnant();
					pc.sendPackets(new S_SystemMessage("伺服器重啟時間還有:" + (second / 60) / 60 + "時" + (second / 60) % 60 + "分" + second % 60 + "秒。"));
					//TODO 線上資訊
					S_WhoAmount s_whoamount = new S_WhoAmount(amount);
					pc.sendPackets(s_whoamount);
					break;
					}
				}
			}
		}

		public override string Type
		{
			get
			{
				return C_WHO;
			}
		}
	}

}