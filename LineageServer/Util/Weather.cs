using LineageServer.Server.Model;
using LineageServer.Serverpackets;

namespace LineageServer.Utils
{
    /// <summary>
    /// @category 隨機天氣控制
    /// @author L1J-TW
    /// @since 2010.12.19
    /// </summary>
    public class Weather
    {
        /// <summary>
        /// @宣告
        /// </summary>
        private static int[] WeatherId = new int[] { 0, 1, 2, 3, 16, 17, 18, 19 };

        /// <summary>
        /// @天氣 0<無雪雨> 1<小雪>、2<中雪> 3<大雪> 16<停止下雨> 17<小雨> 18<中雨> 19<大雨>
        /// </summary>
        public Weather()
        {
            int ran = RandomHelper.Next(8); // 讀取亂數
            L1World.Instance.Weather = WeatherId[ran];
            L1World.Instance.broadcastPacketToAll(new S_Weather(WeatherId[ran]));
        }
    }

}