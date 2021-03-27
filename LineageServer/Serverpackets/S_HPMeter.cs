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
	using L1Character = LineageServer.Server.Model.L1Character;

	public class S_HPMeter : ServerBasePacket
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

		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = memoryStream.toByteArray();
				}
    
				return _byte;
			}
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