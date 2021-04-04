using LineageServer.Server;
using System;
using System.Globalization;

namespace LineageServer.Serverpackets
{
    class S_WarTime : ServerBasePacket
    {
        private const string S_WAR_TIME = "[S] S_WarTime";

        public S_WarTime(DateTime cal)
        {
            // 1997/01/01 17:00を基点としている
            DateTime base_cal = new DateTime();
            base_cal = new DateTime(1997, 0, 1, 17, 0, 0);
            long base_millis = base_cal.Ticks;
            long millis = cal.Ticks;
            long diff = millis - base_millis;
            diff -= 1200 * 60 * 1000; // 誤差修正
            diff = diff / 60000; // 分以下切捨て
                                 // timeは1加算すると3:02（182分）進む
            int time = (int)(diff / 182);

            // WriteDの直前のWriteCで時間の調節ができる
            // 0.7倍した時間だけ縮まるが
            // 1つ調整するとその次の時間が広がる？
            WriteC(Opcodes.S_OPCODE_WARTIME);
            WriteH(6); // リストの数（6以上は無効）
            WriteS(CultureInfo.CurrentCulture.DisplayName); // 時間の後ろの（）内に表示される文字列
            WriteC(0); // ?
            WriteC(0); // ?
            WriteC(0);
            WriteD(time);
            WriteC(0);
            WriteD(time - 1);
            WriteC(0);
            WriteD(time - 2);
            WriteC(0);
            WriteD(time - 3);
            WriteC(0);
            WriteD(time - 4);
            WriteC(0);
            WriteD(time - 5);
            WriteC(0);
        }

        public override string Type
        {
            get
            {
                return S_WAR_TIME;
            }
        }
    }

}