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
	using L1Location = LineageServer.Server.Server.Model.L1Location;
	using Point = LineageServer.Server.Server.Types.Point;

	public class S_EffectLocation : ServerBasePacket
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
			writeC(Opcodes.S_OPCODE_EFFECTLOCATION);
			writeH(x);
			writeH(y);
			writeH(gfxId);
			writeC(0);
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
	}

}