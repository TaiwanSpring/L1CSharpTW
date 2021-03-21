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

	public class S_DoActionGFX : ServerBasePacket
	{
		private const string S_DOACTIONGFX = "[S] S_SkillGFX";

		public static int ACTION_MAGIC = 0x16;

		private byte[] _byte = null;

		public S_DoActionGFX(int objectId, int actionId)
		{
			writeC(Opcodes.S_OPCODE_DOACTIONGFX);
			writeD(objectId);
			writeC(actionId);
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
				return S_DOACTIONGFX;
			}
		}
	}

}