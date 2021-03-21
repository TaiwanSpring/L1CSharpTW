using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Clientpackets
{
	/// <summary>
	/// 處理收到由客戶端傳來交易OK的封包
	/// </summary>
	class C_TradeOK : ClientBasePacket
	{

		private const string C_TRADE_CANCEL = "[C] C_TradeOK";
		public C_TradeOK(sbyte[] abyte0, ClientThread clientthread) : base(abyte0)
		{
			L1PcInstance player = clientthread.ActiveChar;
			if (player == null)
			{
				return;
			}

			L1PcInstance trading_partner = (L1PcInstance) L1World.Instance.findObject(player.TradeID);
			if (trading_partner != null)
			{
				player.TradeOk = true;

				if (player.TradeOk && trading_partner.TradeOk) // 同時都壓OK
				{
					// 檢查身上的空間是否還有 (180 - 16)
					if ((player.Inventory.Size < (180 - 16)) && (trading_partner.Inventory.Size < (180 - 16))) // お互いのアイテムを相手に渡す
					{
						L1Trade trade = new L1Trade();
						trade.TradeOK(player);
					}
					else // お互いのアイテムを手元に戻す
					{
						player.sendPackets(new S_ServerMessage(263)); // \f1一人のキャラクターが持って歩けるアイテムは最大180個までです。
						trading_partner.sendPackets(new S_ServerMessage(263)); // \f1一人のキャラクターが持って歩けるアイテムは最大180個までです。
						L1Trade trade = new L1Trade();
						trade.TradeCancel(player);
					}
				}
			}
		}

		public override string Type
		{
			get
			{
				return C_TRADE_CANCEL;
			}
		}

	}

}