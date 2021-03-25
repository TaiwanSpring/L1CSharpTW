using LineageServer.Server.Server.Model.Instance;
using System;
using System.IO;
namespace LineageServer.Server.Server.utils
{
    /// <summary>
    /// Log Recorder 紀錄存取
    /// 
    /// @author ibm
    /// </summary>
    class LogRecorder
    {
        /* BufferedWriter */
        internal static StreamWriter @out;

        /// <summary>
        /// base </summary>
        public static void writeLog(string messenge)
        {
            //try
            //{
            //    @out = new StreamWriter(new FileStream("log\\Log.log", true));
            //    @out.Write(messenge + "\r\n");
            //    @out.Close();
            //}
            //catch (IOException e)
            //{
            //    System.Console.WriteLine("以下是錯誤訊息: " + e.Message);
            //}
        }

        /// <summary>
        /// Weapon Log
        /// </summary>
        /// <param name="pc">
        ///            玩家 </param>
        public static void writeWeaponLog(string messenge, L1PcInstance pc)
        {
            //try
            //{
            //    File WeaponLog = new File("log\\WeaponLog.log");
            //    if (WeaponLog.createNewFile())
            //    {
            //        @out = new StreamWriter(new StreamWriter("log\\WeaponLog.log", false));
            //        @out.Write("※以下是[衝升武器]的所有紀錄※" + "\r\n");
            //        @out.Close();
            //    }
            //    @out = new StreamWriter(new StreamWriter("log\\WeaponLog.log", true)); // true:從尾端寫起
            //    @out.Write("\r\n"); // 每次填寫資料都控一行 // |false:從頭寫
            //    @out.Write("來自帳號: " + pc.AccountName + ", 來自玩家: " + pc.Name + ", " + messenge + "。" + "\r\n");
            //    @out.Close();
            //}
            //catch (IOException e)
            //{
            //    System.Console.WriteLine("以下是錯誤訊息: " + e.Message);
            //}
        }

        /// <summary>
        /// Bug Log
        /// </summary>
        /// <param name="pc">
        ///            玩家 </param>
        public static void writeBugLog(string messenge, L1PcInstance pc)
        {
            //try
            //{
            //    File BugLog = new File("log\\BugLog.log");
            //    if (BugLog.createNewFile())
            //    {
            //        @out = new StreamWriter(new StreamWriter("log\\BugLog.txt", false));
            //        @out.Write("※以下是所有玩家提供的BUG清單※" + "\r\n");
            //        @out.Write(messenge + "\r\n");
            //        @out.Close();
            //    }
            //    @out = new StreamWriter(new StreamWriter("log\\BugLog.txt", true));
            //    @out.Write("\r\n"); // 每次填寫資料都控一行
            //    @out.Write("來自玩家: " + pc.Name + ": " + messenge + "\r\n");
            //    @out.Close();

            //}
            //catch (IOException e)
            //{
            //    System.Console.WriteLine("以下是錯誤訊息: " + e.Message);
            //}
        }

        /// <summary>
        /// Armor Log
        /// </summary>
        /// <param name="pc">
        ///            玩家 </param>
        public static void writeArmorLog(string messenge, L1PcInstance pc)
        {
            //try
            //{
            //    File ArmorLog = new File("log\\ArmorLog.log");
            //    if (ArmorLog.createNewFile())
            //    {
            //        @out = new StreamWriter(new StreamWriter("log\\ArmorLog.log", false));
            //        @out.Write("※以下是[衝升防具]的所有紀錄※" + "\r\n");
            //        @out.Close();
            //    }
            //    @out = new StreamWriter(new StreamWriter("log\\ArmorLog.log", true));
            //    @out.Write("\r\n"); // 每次填寫資料都控一行
            //    @out.Write("來自帳號: " + pc.AccountName + ", 來自玩家: " + pc.Name + ", " + messenge + "。" + "\r\n");
            //    @out.Close();
            //}
            //catch (IOException e)
            //{
            //    System.Console.WriteLine("以下是錯誤訊息: " + e.Message);
            //}
        }

        /// <summary>
        /// Trade Log
        /// </summary>
        /// <param name="sender">
        ///            主動交易者 </param>
        /// <param name="receiver">
        ///            被交易者 </param>
        /// <param name="l1iteminstance1">
        ///            物品
        /// 
        ///  </param>
        public static void writeTradeLog(L1PcInstance sender, L1PcInstance receiver, L1ItemInstance l1iteminstance1)
        {
            //try
            //{
            //    File TradeLog = new File("log\\TradeLog.log");
            //    if (TradeLog.createNewFile())
            //    {
            //        @out = new StreamWriter(new StreamWriter("log\\TradeLog.log", false));
            //        @out.Write("※以下是玩家[交易]的所有紀錄※" + "\r\n");
            //        @out.Close();
            //    }
            //    @out = new StreamWriter(new StreamWriter("log\\TradeLog.log", true));
            //    @out.Write("\r\n"); // 每次填寫資料都控一行
            //    @out.Write("來自帳號: " + sender.AccountName + ", 來自玩家: " + sender.Name + ",將  +" + l1iteminstance1.EnchantLevel + l1iteminstance1.Name + "(" + l1iteminstance1.Count + "個)," + "交易給 " + " 帳號:" + receiver.AccountName + "的 " + receiver.Name + " 玩家。" + "\r\n");
            //    @out.Close();
            //}
            //catch (IOException e)
            //{
            //    System.Console.WriteLine("以下是錯誤訊息: " + e.Message);
            //}
        }

        /// <summary>
        /// Robots Log
        /// </summary>
        /// <param name="player">
        ///            使用外掛或加速器的玩家 </param>
        public static void writeRobotsLog(L1PcInstance player)
        {
            //try
            //{
            //    File RobotsLog = new File("log\\RobotsLog.log");
            //    if (RobotsLog.createNewFile())
            //    {
            //        @out = new StreamWriter(new StreamWriter("log\\RobotsLog.log", false));
            //        @out.Write("※以下是玩家[使用加速器&外掛]的所有紀錄※" + "\r\n");
            //        @out.Close();
            //    }
            //    @out = new StreamWriter(new StreamWriter("log\\RobotsLog.log", true));
            //    @out.Write("\r\n"); // 每次填寫資料都控一行
            //    @out.Write("加速器紀錄 → 來自帳號: " + player.AccountName + ", 來自玩家: " + player.Name + ", 〈時間〉" + TimeInform.getNowTime(3, 0) + "\r\n");
            //    @out.Close();
            //}
            //catch (IOException e)
            //{
            //    System.Console.WriteLine("以下是錯誤訊息: " + e.Message);
            //}
        }

        /// <summary>
        /// 丟棄物品紀錄 </summary>
        /// <param name="player"> </param>
        public static void writeDropLog(L1PcInstance player, L1ItemInstance item)
        {
            //try
            //{
            //    File DropLog = new File("log\\DropLog.log");
            //    if (DropLog.createNewFile())
            //    {
            //        @out = new StreamWriter(new StreamWriter("log\\DropLog.log", false));
            //        @out.Write("※以下是玩家[丟棄物品]的所有紀錄※" + "\r\n");
            //        @out.Close();
            //    }
            //    @out = new StreamWriter(new StreamWriter("log\\DropLog.log", true));
            //    @out.Write("\r\n"); // 每次填寫資料都控一行
            //    @out.Write("來自帳號: " + player.AccountName + "來自ip: " + player.NetConnection.Ip + ",來自玩家: " + player.Name + ",地點: " + item.Location + ",〈時間〉" + TimeInform.getNowTime(3, 0) + ",丟棄了: " + item.Count + " 個 " + item.Name + ",是否受祝福: " + item.Bless + ",加成值: " + item.EnchantLevel + ",屬性強化類型: " + item.AttrEnchantKind + ",屬性強化等級: " + item.AttrEnchantLevel + "\r\n");
            //    @out.Close();
            //}
            //catch (IOException e)
            //{
            //    System.Console.WriteLine("以下是錯誤訊息: " + e.Message);
            //}
        }
    }

}