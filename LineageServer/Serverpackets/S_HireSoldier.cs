using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
	class S_HireSoldier : ServerBasePacket
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
		public override string Type
		{
			get
			{
				return S_HIRE_SOLDIER;
			}
		}
	}

}