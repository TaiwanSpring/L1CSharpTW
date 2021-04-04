using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_HowManyKey : ServerBasePacket
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
    }
}