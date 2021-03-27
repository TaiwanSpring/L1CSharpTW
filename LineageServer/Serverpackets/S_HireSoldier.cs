﻿/// <summary>
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

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_HireSoldier : ServerBasePacket
	{

		private const string S_HIRE_SOLDIER = "[S] S_HireSldier";

		private byte[] _byte = null;

		// HTMLを開いているときにこのパケットを送るとnpcdeloy-j.htmlが表示される
		// OKボタンを押すとC_127が飛ぶ
		public S_HireSoldier(L1PcInstance pc)
		{
			WriteC(Opcodes.S_OPCODE_HIRESOLDIER);
			WriteH(0); // ? クライアントが返すパケットに含まれる
			WriteH(0); // ? クライアントが返すパケットに含まれる
			WriteH(0); // 雇用された傭兵の総数
			WriteS(pc.Name);
			WriteD(0); // ? クライアントが返すパケットに含まれる
			WriteH(0); // 配置可能な傭兵数
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
				return S_HIRE_SOLDIER;
			}
		}
	}

}