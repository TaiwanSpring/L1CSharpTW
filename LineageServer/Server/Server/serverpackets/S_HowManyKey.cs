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
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;

	public class S_HowManyKey : ServerBasePacket
	{

		/*
		 * 『來源:伺服器』<位址:136>{長度:40}(時間:-473598622)
		 *  0000: 88 09 3e 00 00 dc 00 00 00 01 00 00 00 01 00 00 ..>.............
		 *  0010: 00 08 00 00 00 00 00 69 6e 6e 32 00 00 02 00 24 .......inn2....$
		 *  0020: 34 35 30 00 32 32 30 00 450.220.
		 */
		public S_HowManyKey(L1NpcInstance npc, int price, int min, int max, string htmlId)
		{
			WriteC(Opcodes.S_OPCODE_INPUTAMOUNT);
			WriteD(npc.Id);
			WriteD(price); // 價錢
			WriteD(min); // 起始數量
			WriteD(min); // 起始數量
			WriteD(max); // 購買上限
			WriteH(0); // ?
			WriteS(htmlId); // 對話檔檔名
			WriteC(0); // ?
			WriteH(0x02); // WriteS 數量
			WriteS(npc.Name); // 顯示NPC名稱
			WriteS(price.ToString()); // 顯示價錢
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