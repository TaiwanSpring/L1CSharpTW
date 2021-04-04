using LineageServer.Interfaces;
using LineageServer.Server;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using System;
using System.Collections.Generic;
namespace LineageServer.Serverpackets
{
	/// <summary>
	/// スキルアイコンや遮断リストの表示など複数の用途に使われるパケットのクラス
	/// </summary>
	class S_PacketBox : ServerBasePacket
	{
		private const string S_PACKETBOX = "[S] S_PacketBox";

		private byte[] _byte = null;

		// *** S_107 sub code list ***
		// 1:Kent 2:Orc 3:WW 4:Giran 5:Heine 6:Dwarf 7:Aden 8:Diad 9:城名9 ...
		/// <summary>
		/// C(id) H(?): %sの攻城戦が始まりました。 </summary>
		public const int MSG_WAR_BEGIN = 0;

		/// <summary>
		/// C(id) H(?): %sの攻城戦が終了しました。 </summary>
		public const int MSG_WAR_END = 1;

		/// <summary>
		/// C(id) H(?): %sの攻城戦が進行中です。 </summary>
		public const int MSG_WAR_GOING = 2;

		/// <summary>
		/// -: 城の主導権を握りました。 (音楽が変わる) </summary>
		public const int MSG_WAR_INITIATIVE = 3;

		/// <summary>
		/// -: 城を占拠しました。 </summary>
		public const int MSG_WAR_OCCUPY = 4;

		/// <summary>
		/// ?: 決闘が終りました。 (音楽が変わる) </summary>
		public const int MSG_DUEL = 5;

		/// <summary>
		/// C(count): SMSの送信に失敗しました。 / 全部で%d件送信されました。 </summary>
		public const int MSG_SMS_SENT = 6;

		/// <summary>
		/// -: 祝福の中、2人は夫婦として結ばれました。 (音楽が変わる) </summary>
		public const int MSG_MARRIED = 9;

		/// <summary>
		/// C(weight): 重量(30段階) </summary>
		public const int WEIGHT = 10;

		/// <summary>
		/// C(food): 満腹度(30段階) </summary>
		public const int FOOD = 11;

		/// <summary>
		/// C(0) C(level): このアイテムは%dレベル以下のみ使用できます。 (0~49以外は表示されない) </summary>
		public const int MSG_LEVEL_OVER = 12;

		/// <summary>
		/// UB情報HTML </summary>
		public const int HTML_UB = 14;

		/// <summary>
		/// C(id)<br>
		/// 1:身に込められていた精霊の力が空気の中に溶けて行くのを感じました。<br>
		/// 2:体の隅々に火の精霊力が染みこんできます。<br>
		/// 3:体の隅々に水の精霊力が染みこんできます。<br>
		/// 4:体の隅々に風の精霊力が染みこんできます。<br>
		/// 5:体の隅々に地の精霊力が染みこんできます。<br>
		/// </summary>
		public const int MSG_ELF = 15;

		/// <summary>
		/// C(count) S(name)...: 遮断リスト複数追加 </summary>
		public const int ADD_EXCLUDE2 = 17;

		/// <summary>
		/// S(name): 遮断リスト追加 </summary>
		public const int ADD_EXCLUDE = 18;

		/// <summary>
		/// S(name): 遮断解除 </summary>
		public const int REM_EXCLUDE = 19;

		/// <summary>
		/// スキルアイコン </summary>
		public const int ICONS1 = 20;

		/// <summary>
		/// スキルアイコン </summary>
		public const int ICONS2 = 21;

		/// <summary>
		/// オーラ系のスキルアイコン </summary>
		public const int ICON_AURA = 22;

		/// <summary>
		/// S(name): タウンリーダーに%sが選ばれました。 </summary>
		public const int MSG_TOWN_LEADER = 23;

		/// <summary>
		/// 血盟推薦 接受玩家加入 </summary>
		public const int HTML_PLEDGE_RECOMMENDATION_ACCEPT = 25;
		/// <summary>
		/// C(id): あなたのランクが%sに変更されました。<br>
		/// id - 1:見習い 2:一般 3:ガーディアン
		/// </summary>
		public const int MSG_RANK_CHANGED = 27;

		/// <summary>
		/// D(?) S(name) S(clanname): %s血盟の%sがラスタバド軍を退けました。 </summary>
		public const int MSG_WIN_LASTAVARD = 30;

		/// <summary>
		/// -: \f1気分が良くなりました。 </summary>
		public const int MSG_FEEL_GOOD = 31;

		/// <summary>
		/// 不明。C_30パケットが飛ぶ </summary>
		public const int SOMETHING1 = 33;

		/// <summary>
		/// H(time): ブルーポーションのアイコンが表示される。 </summary>
		public const int ICON_BLUEPOTION = 34;

		/// <summary>
		/// H(time): 変身のアイコンが表示される。 </summary>
		public const int ICON_POLYMORPH = 35;

		/// <summary>
		/// H(time): チャット禁止のアイコンが表示される。 </summary>
		public const int ICON_CHATBAN = 36;

		/// <summary>
		/// 不明。C_7パケットが飛ぶ。C_7はペットのメニューを開いたときにも飛ぶ。 </summary>
		public const int SOMETHING2 = 37;

		/// <summary>
		/// 血盟情報のHTMLが表示される </summary>
		public const int HTML_CLAN1 = 38;

		/// <summary>
		/// H(time): イミュのアイコンが表示される </summary>
		public const int ICON_I2H = 40;

		/// <summary>
		/// キャラクターのゲームオプション、ショートカット情報などを送る </summary>
		public const int CHARACTER_CONFIG = 41;

		/// <summary>
		/// キャラクター選択画面に戻る </summary>
		public const int LOGOUT = 42;

		/// <summary>
		/// 戦闘中に再始動することはできません。 </summary>
		public const int MSG_CANT_LOGOUT = 43;

		/// <summary>
		/// C(count) D(time) S(name) S(info):<br>
		/// [CALL] ボタンのついたウィンドウが表示される。これはBOTなどの不正者チェックに
		/// 使われる機能らしい。名前をダブルクリックするとC_RequestWhoが飛び、クライアントの
		/// フォルダにbot_list.txtが生成される。名前を選択して+キーを押すと新しいウィンドウが開く。
		/// </summary>
		public const int CALL_SOMETHING = 45;

		/// <summary>
		/// C(id): バトル コロシアム、カオス大戦がー<br>
		/// id - 1:開始します 2:取り消されました 3:終了します
		/// </summary>
		public const int MSG_COLOSSEUM = 49;

		/// <summary>
		/// 血盟情報のHTML </summary>
		public const int HTML_CLAN2 = 51;

		/// <summary>
		/// 料理ウィンドウを開く </summary>
		public const int COOK_WINDOW = 52;

		/// <summary>
		/// C(type) H(time): 料理アイコンが表示される </summary>
		public const int ICON_COOKING = 53;

		/// <summary>
		/// 魚がかかったグラフィックが表示される </summary>
		public const int FISHING = 55;

		/// <summary>
		/// 魔法娃娃狀態圖示 </summary>
		public const int ICON_MAGIC_DOLL = 56;

		/// <summary>
		/// 戰爭結束佔領公告<br>
		/// C(count) S(name)<br> 
		/// </summary>
		public const int MSG_WAR_OCCUPY_ALL = 79;

		/// <summary>
		/// 攻城戰進行中 </summary>
		public const int MSG_WAR_IS_GOING_ALL = 80; //TODO
		/// <summary>
		/// 經驗值加成（殷海薩的祝福） </summary>
		public const int EXPBLESS = 82;
		/// <summary>
		/// 閃避率 正 </summary>
		public const int DODGE_RATE_PLUS = 88;

		/// <summary>
		/// 閃避率 負 </summary>
		public const int DODGE_RATE_MINUS = 101;

		/// <summary>
		/// Updating </summary>
		public const int UPDATE_OLD_PART_MEMBER = 104;

		/// <summary>
		/// 3.3 組隊系統(更新新加入的隊員信息) </summary>
		public const int PATRY_UPDATE_MEMBER = 105;

		/// <summary>
		/// 3.3組隊系統(委任新隊長) </summary>
		public const int PATRY_SET_MASTER = 106;

		/// <summary>
		/// 3.3 組隊系統(更新隊伍信息,所有隊員) </summary>
		public const int PATRY_MEMBERS = 110;

		/// <summary>
		/// 3.8血盟倉庫使用紀錄 </summary>
		public const int HTML_CLAN_WARHOUSE_RECORD = 117;

		/// <summary>
		/// 3.8 地圖倒數計時器 </summary>
		public const int MAP_TIMER = 153;

		/// <summary>
		/// 3.8 地圖剩餘時間 </summary>
		public const int MAP_TIME = 159;

		/// <summary>
		/// 3.8 血盟查詢盟友 (顯示公告) </summary>
		public const int HTML_PLEDGE_ANNOUNCE = 167;

		/// <summary>
		/// 3.8 血盟查詢盟友 (寫入公告) </summary>
		public const int HTML_PLEDGE_REALEASE_ANNOUNCE = 168;

		/// <summary>
		/// 3.8 血盟查詢盟友 (寫入備註) </summary>
		public const int HTML_PLEDGE_Write_NOTES = 169;

		/// <summary>
		/// 3.8 血盟查詢盟友 (顯示盟友) </summary>
		public const int HTML_PLEDGE_MEMBERS = 170;

		/// <summary>
		/// 3.8 血盟查詢盟友 (顯示上線盟友) </summary>
		public const int HTML_PLEDGE_ONLINE_MEMBERS = 171;

		/// <summary>
		/// 3.8 血盟 識別盟徽狀態 </summary>
		public const int PLEDGE_EMBLEM_STATUS = 173;

		/// <summary>
		/// 3.8 村莊便利傳送 </summary>
		public const int TOWN_TELEPORT = 176;


		public S_PacketBox(int subCode)
		{
			WriteC(Opcodes.S_OPCODE_PACKETBOX);
			WriteC(subCode);

			switch (subCode)
			{
				case MSG_WAR_INITIATIVE:
				case MSG_WAR_OCCUPY:
				case MSG_MARRIED:
				case MSG_FEEL_GOOD:
				case MSG_CANT_LOGOUT:
				case LOGOUT:
				case FISHING:
					break;
				case CALL_SOMETHING:
					callSomething();
					goto default;
				default:
					break;
			}
		}

		public S_PacketBox(int subCode, L1PcInstance pc)
		{
			WriteC(Opcodes.S_OPCODE_PACKETBOX);
			WriteC(subCode);
			switch (subCode)
			{
				case TOWN_TELEPORT:
					WriteC(0x01);
					WriteH(pc.X);
					WriteH(pc.Y);
					break;
			}

		}

		public S_PacketBox(int subCode, int value)
		{
			WriteC(Opcodes.S_OPCODE_PACKETBOX);
			WriteC(subCode);

			switch (subCode)
			{
				case ICON_BLUEPOTION:
				case ICON_CHATBAN:
				case ICON_I2H:
				case ICON_POLYMORPH:
				case MAP_TIMER:
					WriteH(value); // time
					break;
				case MSG_WAR_BEGIN:
				case MSG_WAR_END:
				case MSG_WAR_GOING:
					WriteC(value); // castle id
					WriteH(0); // ?
					break;
				case MSG_SMS_SENT:
				case WEIGHT:
				case FOOD:
					WriteC(value);
					break;
				case MSG_ELF: // 忽然全身充滿了%s的靈力。
				case MSG_COLOSSEUM: // 大圓形競技場，混沌的大戰開始！結束！取消！
					WriteC(value); // msg id
					WriteC(0);
					break;
				case MSG_LEVEL_OVER:
					WriteC(0); // ?
					WriteC(value); // 0-49以外は表示されない
					break;
				case COOK_WINDOW:
					WriteC(0xdb); // ?
					WriteC(0x31);
					WriteC(0xdf);
					WriteC(0x02);
					WriteC(0x01);
					WriteC(value); // level
					break;
				case EXPBLESS:
					WriteC(value); // %值為0 ~ 200
					break;
				case DODGE_RATE_PLUS: // + 閃避率
					WriteC(value);
					WriteC(0x00);
					break;
				case DODGE_RATE_MINUS: // - 閃避率
					WriteC(value);
					break;
				case 21: // 狀態圖示
					WriteC(0x00);
					WriteC(0x00);
					WriteC(0x00);
					WriteC(value); // 閃避圖示 (幻術:鏡像、黑妖:闇影閃避)
					break;
				case PLEDGE_EMBLEM_STATUS:
					WriteC(1);
					if (value == 0)
					{ // 0:關閉 1:開啟
						WriteC(0);
					}
					else if (value == 1)
					{
						WriteC(1);
					}
					WriteD(0x00);
					break;
				default:
					break;
			}
		}

		public S_PacketBox(int subCode, int type, int time)
		{
			WriteC(Opcodes.S_OPCODE_PACKETBOX);
			WriteC(subCode);

			switch (subCode)
			{
				case ICON_COOKING:
					if (type == 54)
					{ // 象牙塔妙藥
						WriteC(0x12);
						WriteC(0x0c);
						WriteC(0x0c);
						WriteC(0x07);
						WriteC(0x12);
						WriteC(0x08);
						WriteH(0x0000); // 飽和度 值:2000，飽和度100%
						WriteC(type); // 類型
						WriteC(0x2a);
						WriteH(time); // 時間
						WriteC(0x0); // 負重度 值:242，負重度100%
					}
					else if (type != 7)
					{
						WriteC(0x12);
						WriteC(0x0b);
						WriteC(0x0c);
						WriteC(0x0b);
						WriteC(0x0f);
						WriteC(0x08);
						WriteH(0x0000); // 飽和度 值:2000，飽和度100%
						WriteC(type); // 類型
						WriteC(0x24);
						WriteH(time); // 時間
						WriteC(0x00); // 負重度 值:242，負重度100%
					}
					else
					{
						WriteC(0x12);
						WriteC(0x0b);
						WriteC(0x0c);
						WriteC(0x0b);
						WriteC(0x0f);
						WriteC(0x08);
						WriteH(0x0000); // 飽和度 值:2000，飽和度100%
						WriteC(type); // 類型
						WriteC(0x26);
						WriteH(time); // 時間
						WriteC(0x00); // 負重度 值:240，負重度100%
					}
					break;
				case MSG_DUEL:
					WriteD(type); // 相手のオブジェクトID
					WriteD(time); // 自分のオブジェクトID
					break;
				case ICON_MAGIC_DOLL:
					if (type == 32)
					{ // 愛心圖示
						WriteH(time);
						WriteC(type);
						WriteC(12);
					}
					else
					{ // 魔法娃娃圖示
						WriteH(time);
						WriteC(0);
						WriteC(0);
					}
					break;
				default:
					break;
			}
		}

		public S_PacketBox(int subCode, string name)
		{
			WriteC(Opcodes.S_OPCODE_PACKETBOX);
			WriteC(subCode);

			switch (subCode)
			{
				case ADD_EXCLUDE:
				case REM_EXCLUDE:
				case MSG_TOWN_LEADER:
				case HTML_PLEDGE_REALEASE_ANNOUNCE:
					WriteS(name);
					break;
				default:
					break;
			}
		}

		public S_PacketBox(int subCode, int id, string name, string clanName)
		{
			WriteC(Opcodes.S_OPCODE_PACKETBOX);
			WriteC(subCode);

			switch (subCode)
			{
				case MSG_WIN_LASTAVARD:
					WriteD(id); // クランIDか何か？
					WriteS(name);
					WriteS(clanName);
					break;

				default:
					break;
			}
		}

		public S_PacketBox(int subCode, int rank, string name)
		{
			WriteC(Opcodes.S_OPCODE_PACKETBOX);
			WriteC(subCode);

			switch (subCode)
			{
				case MSG_RANK_CHANGED: // 你的階級變更為%s
					WriteC(rank);
					WriteS(name);
					break;
			}

		}

		public S_PacketBox(int subCode, object[] names)
		{
			WriteC(Opcodes.S_OPCODE_PACKETBOX);
			WriteC(subCode);

			switch (subCode)
			{
				case ADD_EXCLUDE2:
					WriteC(names.Length);
					foreach (object name in names)
					{
						WriteS(name.ToString());
					}
					break;
				case MSG_WAR_OCCUPY_ALL:
					WriteC(names.Length);
					foreach (object name in names)
					{
						WriteS(name.ToString());
					}
					break;
				case MSG_WAR_IS_GOING_ALL:
					WriteC(names.Length);
					foreach (object name in names)
					{
						WriteS(name.ToString());
					}
					break;
				case HTML_PLEDGE_ONLINE_MEMBERS:
					WriteH(names.Length);
					foreach (object name in names)
					{
						L1PcInstance pc = (L1PcInstance)name;
						WriteS(pc.Name);
					}
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// 3.80C 地圖入場剩餘時間 </summary>
		/// <param name="subCode"> MAP_TIME </param>
		/// <param name="names"> 地圖名稱 </param>
		/// <param name="value"> 時間 </param>
		public S_PacketBox(int subCode, object[] names, int[] value)
		{
			WriteC(Opcodes.S_OPCODE_PACKETBOX);
			WriteC(subCode);

			switch (subCode)
			{
				case MAP_TIME:
					WriteD(names.Length);
					int i = 1;
					foreach (object name in names)
					{
						WriteD(i);
						WriteS(name.ToString());
						WriteD(Convert.ToInt32(value[i - 1]));
						i++;
					}
					break;
				default:
					break;
			}
		}

		private void callSomething()
		{
			WriteC(Container.Instance.Resolve<IGameWorld>().AllPlayers.Count);

			DateTime dateTime = new DateTime(1970, 01, 01, 09, 0, 0);

			foreach (var pc in Container.Instance.Resolve<IGameWorld>().AllPlayers)
			{
				Account acc = Account.Load(pc.AccountName);

				// 時間情報 とりあえずログイン時間を入れてみる
				if (acc == null)
				{
					WriteD(0);
				}
				else
				{
					//DateTime cal = DateTime.getInstance(TimeZone.getTimeZone(Config.TIME_ZONE));
					//long lastactive = (acc.LastActive - DateTime.MinValue);
					//cal.TimeInMillis = lastactive;
					//cal.set(DateTime.YEAR, 1970);
					//int time = (int)(cal.Ticks / 1000);

					WriteD((int)( ( acc.LastActive - dateTime ).TotalMilliseconds / 1000 )); // JST 1970 1/1 09:00 が基準
				}

				// キャラ情報
				WriteS(pc.Name); // 半角12字まで
				WriteS(pc.Clanname); // []内に表示される文字列。半角12字まで
			}
		}

		public override string Type
		{
			get
			{
				return S_PACKETBOX;
			}
		}
	}

}