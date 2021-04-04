using LineageServer.Server;
using LineageServer.Server.Model;

namespace LineageServer.Serverpackets
{ 
	class S_HPMeter : ServerBasePacket
	{
		private const string _typeString = "[S] S_HPMeter";

		private byte[] _byte = null;

		public S_HPMeter(int objId, int hpRatio)
		{
			buildPacket(objId, hpRatio);
		}

		public S_HPMeter(L1Character cha)
		{
			int objId = cha.Id;
			int hpRatio = 100;
			if (0 < cha.getMaxHp())
			{
				hpRatio = 100 * cha.CurrentHp / cha.getMaxHp();
			}
			buildPacket(objId, hpRatio);
		}
		// 怪物血條判斷功能  溝想來源99NETS网游模拟娱乐社区
		public S_HPMeter(L1Character cha, bool MobHPBar)
		{
			int objId = cha.Id;
			int hpRatio = 100;
			if (MobHPBar)
			{
				if (0 < cha.getMaxHp())
				{
					hpRatio = 100 * cha.CurrentHp / cha.getMaxHp();
				}
				buildPacket(objId, hpRatio);
			}
			else
			{
				buildPacket(objId, 0xFF);
			}
		}
		// end
		private void buildPacket(int objId, int hpRatio)
		{
			WriteC(Opcodes.S_OPCODE_HPMETER);
			WriteD(objId);
			WriteH(hpRatio);
		}
		public override string Type
		{
			get
			{
				return _typeString;
			}
		}
	}

}