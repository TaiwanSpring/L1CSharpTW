using System.Collections.Generic;

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
	using L1NpcInstance = LineageServer.Server.Model.Instance.L1NpcInstance;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;
	using L1PetInstance = LineageServer.Server.Model.Instance.L1PetInstance;
	using ListFactory = LineageServer.Utils.ListFactory;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_PetList : ServerBasePacket
	{

		private const string S_PETLIST = "[S] S_PetList";

		private byte[] _byte = null;

		public S_PetList(int npcObjId, L1PcInstance pc)
		{
			buildPacket(npcObjId, pc);
		}

		private void buildPacket(int npcObjId, L1PcInstance pc)
		{
			IList<L1ItemInstance> amuletList = ListFactory.newList();
			// 判斷身上是否有寵物項圈！
			foreach (L1ItemInstance item in pc.Inventory.Items)
			{
				if ((item.Item.ItemId == 40314) || (item.Item.ItemId == 40316))
				{
					if (!isWithdraw(pc, item))
					{
						amuletList.Add(item);
					}
				}
			}

			if (amuletList.Count != 0)
			{
				WriteC(Opcodes.S_OPCODE_SHOWRETRIEVELIST);
				WriteD(npcObjId);
				WriteH(amuletList.Count);
				WriteC(0x0c);
				foreach (L1ItemInstance item in amuletList)
				{
					WriteD(item.Id);
					WriteC(0x00);
					WriteH(item.get_gfxid());
					WriteC(item.Bless);
					WriteD(item.Count);
					WriteC(item.Identified ? 1 : 0);
					WriteS(item.ViewName);
				}
			}
			else
			{
				return;
			}
			WriteD(0x00000073); // Price
		}

		private bool isWithdraw(L1PcInstance pc, L1ItemInstance item)
		{
			foreach (L1NpcInstance petNpc in pc.PetList.Values)
			{
				if (petNpc is L1PetInstance)
				{
					L1PetInstance pet = (L1PetInstance) petNpc;
					if (item.Id == pet.ItemObjId)
					{
						return true;
					}
				}
			}
			return false;
		}

		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = Bytes;
				}
				return _byte;
			}
		}

		public override string Type
		{
			get
			{
				return S_PETLIST;
			}
		}
	}

}