using LineageServer.Server;
using LineageServer.Server.Model;
using LineageServer.Server.Types;

namespace LineageServer.Serverpackets
{
    class S_EffectLocation : ServerBasePacket
    {

        private byte[] _byte = null;

        /// <summary>
        /// 指定された位置へエフェクトを表示するパケットを構築する。
        /// </summary>
        /// <param name="pt"> - エフェクトを表示する位置を格納したPointオブジェクト </param>
        /// <param name="gfxId"> - 表示するエフェクトのID </param>
        public S_EffectLocation(Point pt, int gfxId) : this(pt.X, pt.Y, gfxId)
        {
        }

        /// <summary>
        /// 指定された位置へエフェクトを表示するパケットを構築する。
        /// </summary>
        /// <param name="loc"> - エフェクトを表示する位置を格納したL1Locationオブジェクト </param>
        /// <param name="gfxId"> - 表示するエフェクトのID </param>
        public S_EffectLocation(L1Location loc, int gfxId) : this(loc.X, loc.Y, gfxId)
        {
        }

        /// <summary>
        /// 指定された位置へエフェクトを表示するパケットを構築する。
        /// </summary>
        /// <param name="x"> - エフェクトを表示する位置のX座標 </param>
        /// <param name="y"> - エフェクトを表示する位置のY座標 </param>
        /// <param name="gfxId"> - 表示するエフェクトのID </param>
        public S_EffectLocation(int x, int y, int gfxId)
        {
            WriteC(Opcodes.S_OPCODE_EFFECTLOCATION);
            WriteH(x);
            WriteH(y);
            WriteH(gfxId);
            WriteC(0);
        }
    }
}