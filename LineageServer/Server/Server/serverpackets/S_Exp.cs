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

	public class S_Exp : ServerBasePacket
	{

		private const string S_EXP = "[S] S_Exp";

		/// <summary>
		/// レベルと経験値データを送る。
		/// </summary>
		/// <param name="pc">
		///            - PC </param>
		public S_Exp(L1PcInstance pc)
		{
			writeC(Opcodes.S_OPCODE_EXP);
			writeC(pc.Level);
			writeExp(pc.Exp);

			// writeC(Opcodes.S_OPCODE_EXP);
			// writeC(0x39);// level
			// writeD(_objid);// ??
			// writeC(0x0A);// ??
			// writeH(getexp);// min exp
			// writeH(getexpreward);// max exp
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
				return S_EXP;
			}
		}
	}

}