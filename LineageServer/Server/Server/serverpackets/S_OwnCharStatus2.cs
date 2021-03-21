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
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket, S_SendInvOnLogin

	public class S_OwnCharStatus2 : ServerBasePacket
	{

		/// <summary>
		/// 角色六大素質+負重更新<br> </summary>
		/// <param name="pc"> </param>
		/// <param name="type"> 0:不檢查重複的屬性  1:檢查重複的屬性次數 </param>
		public S_OwnCharStatus2(L1PcInstance pc, int type)
		{
			if (type == 0)
			{
				buildPacket(pc);
			}
			else if (type == 1)
			{
				int[] status = new int[] {pc.Str, pc.Int, pc.Wis, pc.Dex, pc.Con, pc.Cha};
				for (int i = 0; i <= status.Length; i++)
				{
					for (int j = i + 1; j <= status.Length; j++)
					{
						buildPacket(pc);
					}
				}
			}

		}

		/// <summary>
		/// 更新六項能力值以及負重 </summary>
		private void buildPacket(L1PcInstance pc)
		{
			writeC(Opcodes.S_OPCODE_OWNCHARSTATUS2);
			writeC(pc.Str);
			writeC(pc.Int);
			writeC(pc.Wis);
			writeC(pc.Dex);
			writeC(pc.Con);
			writeC(pc.Cha);
			writeC(pc.Inventory.Weight242);
		}

		public override sbyte[] Content
		{
			get
			{
				return Bytes;
			}
		}

		public override string Type
		{
			get
			{
				return "[C] S_OwnCharStatus2";
			}
		}
	}

}