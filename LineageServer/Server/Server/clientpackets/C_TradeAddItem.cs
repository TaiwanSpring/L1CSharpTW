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
	using L1Inventory = LineageServer.Server.Server.Model.L1Inventory;
	using L1Trade = LineageServer.Server.Server.Model.L1Trade;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1DollInstance = LineageServer.Server.Server.Model.Instance.L1DollInstance;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;
	using L1PetInstance = LineageServer.Server.Server.Model.Instance.L1PetInstance;
	using S_ServerMessage = LineageServer.Server.Server.serverpackets.S_ServerMessage;

	// Referenced classes of package l1j.server.server.clientpackets:
	// ClientBasePacket

	/// <summary>
	/// 處理收到由客戶端傳來增加交易物品的封包
	/// </summary>
	class C_TradeAddItem : ClientBasePacket
	{
		private const string C_TRADE_ADD_ITEM = "[C] C_TradeAddItem";

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public C_TradeAddItem(byte abyte0[], l1j.server.server.ClientThread client) throws Exception
		public C_TradeAddItem(sbyte[] abyte0, ClientThread client) : base(abyte0)
		{

			L1PcInstance pc = client.ActiveChar;
			if (pc == null)
			{
				return;
			}

			int itemid = readD();
			int itemcount = readD();

			L1Trade trade = new L1Trade();
			L1ItemInstance item = pc.Inventory.getItem(itemid);
			if (!item.Item.Tradable)
			{
				pc.sendPackets(new S_ServerMessage(210, item.Item.Name)); // \f1%0は捨てたりまたは他人に讓ることができません。
				return;
			}
			if (item.Bless >= 128)
			{ // 封印的裝備
				// \f1%0は捨てたりまたは他人に讓ることができません。
				pc.sendPackets(new S_ServerMessage(210, item.Item.Name));
				return;
			}
			// 使用中的寵物項鍊 - 無法交易
			foreach (L1NpcInstance petNpc in pc.PetList.Values)
			{
				if (petNpc is L1PetInstance)
				{
					L1PetInstance pet = (L1PetInstance) petNpc;
					if (item.Id == pet.ItemObjId)
					{
						pc.sendPackets(new S_ServerMessage(1187)); // 寵物項鍊正在使用中。
						return;
					}
				}
			}
			// 使用中的魔法娃娃 - 無法交易
			foreach (L1DollInstance doll in pc.DollList.Values)
			{
				if (doll.ItemObjId == item.Id)
				{
					pc.sendPackets(new S_ServerMessage(1181)); // 這個魔法娃娃目前正在使用中。
					return;
				}
			}

			L1PcInstance tradingPartner = (L1PcInstance) L1World.Instance.findObject(pc.TradeID);
			if (tradingPartner == null)
			{
				return;
			}
			if (pc.TradeOk)
			{
				return;
			}
			if (tradingPartner.Inventory.checkAddItem(item, itemcount) != L1Inventory.OK)
			{ // 檢查容量與重量
				tradingPartner.sendPackets(new S_ServerMessage(270)); // \f1持っているものが重くて取引できません。
				pc.sendPackets(new S_ServerMessage(271)); // \f1相手が物を持ちすぎていて取引できません。
				return;
			}

			trade.TradeAddItem(pc, itemid, itemcount);
		}

		public override string Type
		{
			get
			{
				return C_TRADE_ADD_ITEM;
			}
		}
	}

}