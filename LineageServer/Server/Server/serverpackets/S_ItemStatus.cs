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
	using L1ItemInstance = LineageServer.Server.Server.Model.Instance.L1ItemInstance;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_ItemStatus : ServerBasePacket
	{

		private const string S_ITEM_STATUS = "[S] S_ItemStatus";

		/// <summary>
		/// アイテムの名前、状態、特性、重量などの表示を変更する
		/// </summary>
		public S_ItemStatus(L1ItemInstance item)
		{
			writeC(Opcodes.S_OPCODE_ITEMSTATUS);
			writeD(item.Id);
			writeS(item.ViewName);
			writeD(item.Count);
			if (!item.Identified)
			{
				// 未鑑定の場合ステータスを送る必要はない
				writeC(0);
			}
			else
			{
				sbyte[] status = item.StatusBytes;
				writeC(status.Length);
				foreach (sbyte b in status)
				{
					writeC(b);
				}
			}
		}

		public override sbyte[] Content
		{
			get
			{
				return _bao.toByteArray();
			}
		}

		public override string Type
		{
			get
			{
				return S_ITEM_STATUS;
			}
		}
	}

}