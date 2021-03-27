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
namespace LineageServer.Serverpackets
{
	using Opcodes = LineageServer.Server.Opcodes;
	using L1PcInstance = LineageServer.Server.Model.Instance.L1PcInstance;

	/// <summary>
	/// 送出顯示龍門選單動作
	/// </summary>
	public class S_DragonGate : ServerBasePacket
	{
		private const string S_DRAGON_GATE = "[S] S_DragonGate";

		private byte[] _byte = null;

		public S_DragonGate(L1PcInstance pc, bool[] i)
		{
			WriteC(Opcodes.S_OPCODE_PACKETBOX);
			WriteC(0x66); // = 102
			WriteD(pc.Id);
			// true 可點選，false 不能點選
			WriteC(i[0] ? 1 : 0); // 安塔瑞斯
			WriteC(i[1] ? 1 : 0); // 法利昂
			WriteC(i[2] ? 1 : 0); // 林德拜爾
			WriteC(i[3] ? 1 : 0); // 巴拉卡斯
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

		public override string Type
		{
			get
			{
				return S_DRAGON_GATE;
			}
		}
	}

}