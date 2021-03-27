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

	using Opcodes = LineageServer.Server.Opcodes;
	using L1ItemInstance = LineageServer.Server.Model.Instance.L1ItemInstance;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;

	public class S_RetrieveList : ServerBasePacket
	{
		public S_RetrieveList(int objid, L1PcInstance pc)
		{
			if (pc.Inventory.Size < 180)
			{
				int size = pc.DwarfInventory.Size;
				if (size > 0)
				{
					WriteC(Opcodes.S_OPCODE_SHOWRETRIEVELIST);
					WriteD(objid);
					WriteH(size);
					WriteC(3); // 個人倉庫
					foreach (object itemObject in pc.DwarfInventory.Items)
					{
						L1ItemInstance item = (L1ItemInstance) itemObject;
						WriteD(item.Id);
						WriteC(item.Item.UseType); // 道具:0 武器:1  防具:2...
						WriteH(item.get_gfxid());
						WriteC(item.Bless);
						WriteD(item.Count);
						WriteC(item.Identified ? 1 : 0);
						WriteS(item.ViewName);
					}
					WriteD(0x0000001e); // 金幣30
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