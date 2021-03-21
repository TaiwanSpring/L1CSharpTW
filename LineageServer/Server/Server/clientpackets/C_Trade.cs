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
	using ClientThread = LineageServer.Server.Server.ClientThread;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using S_Message_YN = LineageServer.Server.Server.serverpackets.S_Message_YN;
	using FaceToFace = LineageServer.Server.Server.utils.FaceToFace;

	// Referenced classes of package l1j.server.server.clientpackets:
	// ClientBasePacket

	/// <summary>
	/// 處理收到由客戶端傳來交易的封包
	/// </summary>
	class C_Trade : ClientBasePacket
	{

		private const string C_TRADE = "[C] C_Trade";

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public C_Trade(byte abyte0[], l1j.server.server.ClientThread clientthread) throws Exception
		public C_Trade(sbyte[] abyte0, ClientThread clientthread) : base(abyte0)
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