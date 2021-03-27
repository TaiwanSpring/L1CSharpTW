using System.Numerics;

namespace LineageServer.Server
{
    public class Cipher
    {
        /* 靜態私用變數 */
        /// <summary>
        /// 將亂數數值混淆用的混淆密碼 (32位元,靜態唯讀) 該數值只有在Cipher初始化時才會被調用
        /// </summary>
        private const uint _1 = 0x9c30d539;
        /// <summary>
        /// 初始的解碼數值
        /// </summary>
        private const uint _2 = 0x930fd7e2;
        /// <summary>
        /// 將亂數數值混淆用的混淆密碼 (32位元,靜態唯讀) 該數值只有在Cipher初始化時才會被調用
        /// </summary>
        private const uint _3 = 0x7c72e993;
        /// <summary>
        /// 將封包數值混淆用的混淆密碼 (32位元,靜態唯讀) 該數值只有在編碼或解碼時才會被調用
        /// </summary>
        private const uint _4 = 0x287effc3;

        /* 動態私用變數 */
        /// <summary>
        /// 參考用的編碼鑰匙 (位元組陣列長度為8,相當於一個64位元的長整數)
        /// </summary>
        private readonly byte[] eb = new byte[8];
        /// <summary>
        /// 參考用的解碼鑰匙 (位元組陣列長度為8,相當於一個64位元的長整數)
        /// </summary>
        private readonly byte[] db = new byte[8];
        /// <summary>
        /// 參考用的封包鑰匙 (位元組陣列長度為4,相當於一個32位元的整數)
        /// </summary>
        private readonly byte[] tb = new byte[4];

        /// <summary>
        /// 初始化流程: 1.建立新的鑰匙暫存器(keys),將編碼鑰匙與混淆鑰匙(_1)進行混淆並帶入keys[0],將初始的解碼數值帶入key[1]
        /// 2.將key[0]向右反轉19個位元(0x13)並帶入key[0] 3.將key[0]與key[1]與混淆鑰匙(_3)進行混淆並帶入key[1]
        /// 4.將keys轉換為64位元的位元組陣列
        /// </summary>
        /// <param name="key">由亂數產生的編碼鑰匙 </param>
        public Cipher(int key)
        {
            uint[] keys = new uint[] { (uint)(key ^ _1), _2 };
            keys[0] = BitOperations.RotateLeft(keys[0], 0x13);
            keys[1] ^= keys[0] ^ _3;

            for (int i = 0; i < keys.Length; i++)
            {
                for (int j = 0; j < tb.Length; j++)
                {
                    eb[(i * 4) + j] = db[(i * 4) + j] = (byte)(keys[i] >> (j * 8) & 0xff);
                }
            }
        }

        /// <summary>
        /// 將未受保護的資料進行混淆,避免資料外流.
        /// </summary>
        /// <param name="data">未受保護的資料 </param>
        /// <returns> data, 受保護的資料 </returns>
        public virtual byte[] encrypt(byte[] data)
        {
            for (int i = 0; i < tb.Length; i++)
            {
                tb[i] = data[i];
            }

            data[0] ^= eb[0];

            for (int i = 1; i < data.Length; i++)
            {
                data[i] ^= (byte)(data[i - 1] ^ eb[i & 7]);
            }

            data[3] ^= eb[2];
            data[2] ^= (byte)(eb[3] ^ data[3]);
            data[1] ^= (byte)(eb[4] ^ data[2]);
            data[0] ^= (byte)(eb[5] ^ data[1]);
            update(eb, tb);
            return data;
        }

        /// <summary>
        /// 將受保護的資料進行還原,讓伺服器可以參考正確的資料.
        /// </summary>
        /// <param name="data">受保護的資料 </param>
        /// <returns> data, 原始的資料 </returns>
        public virtual byte[] decrypt(byte[] data)
        {
            data[0] ^= (byte)(db[5] ^ data[1]);
            data[1] ^= (byte)(db[4] ^ data[2]);
            data[2] ^= (byte)(db[3] ^ data[3]);
            data[3] ^= (byte)db[2];

            for (int i = data.Length - 1; i >= 1; i--)
            {
                data[i] ^= (byte)(data[i - 1] ^ db[i & 7]);
            }

            data[0] ^= db[0];
            update(db, data);
            return data;
        }

        /// <summary>
        /// 將指定的鑰匙進行混淆並與混淆鑰匙相加(_4)
        /// </summary>
        /// <param name="data">
        ///            , 受保護的資料 </param>
        /// <returns> data, 原始的資料 </returns>
        private void update(byte[] data, byte[] @ref)
        {
            for (int i = 0; i < tb.Length; i++)
            {
                data[i] ^= @ref[i];
            }

            long int32 = (((data[7] & 0xFF) << 24) | ((data[6] & 0xFF) << 16) | ((data[5] & 0xFF) << 8) | (data[4] & 0xFF)) + _4;

            for (int i = 0; i < tb.Length; i++)
            {
                data[i + 4] = (byte)(int32 >> (i * 8) & 0xff);
            }
        }
    }
}