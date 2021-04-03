
using LineageServer.Server;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;
using LineageServer.Utils;

namespace LineageServer.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來交易的封包
	/// </summary>
	class C_Trade : ClientBasePacket
	{

		private const string C_TRADE = "[C] C_Trade";
		public C_Trade(byte[] abyte0, ClientThread clientthread) : base(abyte0)
		{
			L1PcInstance player = clientthread.ActiveChar;
			if ((player == null) || player.Ghost)
			{
				return;
			}
			L1PcInstance target = FaceToFace.faceToFace(player, false);
			if (target != null)
			{
				if (!target.Paralyzed)
				{
					player.TradeID = target.Id; // 相手のオブジェクトIDを保存しておく
					target.TradeID = player.Id;
					target.sendPackets(new S_Message_YN(252, player.Name)); // %0%sがあなたとアイテムの取引を望んでいます。取引しますか？（Y/N）
				}
			}
		}

		public override string Type
		{
			get
			{
				return C_TRADE;
			}
		}
	}

}