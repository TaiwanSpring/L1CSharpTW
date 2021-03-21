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
	using L1Clan = LineageServer.Server.Server.Model.L1Clan;
	using L1World = LineageServer.Server.Server.Model.L1World;
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;

	public class S_RetrievePledgeList : ServerBasePacket
	{
		public S_RetrievePledgeList(int objid, L1PcInstance pc)
		{
			L1Clan clan = L1World.Instance.getClan(pc.Clanname);
			if (clan == null)
			{
				return;
			}

			if (clan.WarehouseUsingChar != 0 && clan.WarehouseUsingChar != pc.Id) // 自キャラ以外がクラン倉庫使用中
			{
				pc.sendPackets(new S_ServerMessage(209)); // \f1他の血盟員が倉庫を使用中です。しばらく経ってから利用してください。
				return;
			}

			if (pc.Inventory.Size < 180)
			{
				int size = clan.DwarfForClanInventory.Size;
				if (size > 0)
				{
					clan.WarehouseUsingChar = pc.Id; // クラン倉庫をロック
					writeC(Opcodes.S_OPCODE_SHOWRETRIEVELIST);
					writeD(objid);
					writeH(size);
					writeC(5); // 血盟倉庫
					foreach (object itemObject in clan.DwarfForClanInventory.Items)
					{
						L1ItemInstance item = (L1ItemInstance) itemObject;
						writeD(item.Id);
						writeC(item.Item.UseType);
						writeH(item.get_gfxid());
						writeC(item.Bless);
						writeD(item.Count);
						writeC(item.Identified ? 1 : 0);
						writeS(item.ViewName);
					}
					writeD(0x0000001e); // 金幣30
				}
				else
				{
					pc.sendPackets(new S_ServerMessage(1625));
				}
			}
			else
			{
				pc.sendPackets(new S_ServerMessage(263)); // \f1一人のキャラクターが持って歩けるアイテムは最大180個までです。
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public byte[] getContent() throws java.io.IOException
		public override sbyte[] Content
		{
			get
			{
				return Bytes;
			}
		}
	}

}