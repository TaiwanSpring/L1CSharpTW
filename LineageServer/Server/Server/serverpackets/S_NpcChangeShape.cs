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
namespace LineageServer.Server.Server.serverpackets
{
	using Opcodes = LineageServer.Server.Server.Opcodes;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket, S_SkillIconGFX, S_CharVisualUpdate

	public class S_NpcChangeShape : ServerBasePacket
	{

		private byte[] _byte = null;

		/// <summary>
		/// 使用於怪物變身 </summary>
		public S_NpcChangeShape(int objId, int polyId, int lawful, int status)
		{
			writeC(Opcodes.S_OPCODE_SPOLY);
			writeD(objId);
			writeD(0); // ???
			writeH(polyId);
			writeH(lawful); // 正義值
			writeH(status); // 狀態
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