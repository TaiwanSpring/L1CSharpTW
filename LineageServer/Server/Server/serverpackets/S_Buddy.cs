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
	using L1Buddy = LineageServer.Server.Server.Model.L1Buddy;

	public class S_Buddy : ServerBasePacket
	{
		private const string _S_Buddy = "[S] _S_Buddy";
		private const string _HTMLID = "buddy";

		private byte[] _byte = null;

		public S_Buddy(int objId, L1Buddy buddy)
		{
			buildPacket(objId, buddy);
		}

		private void buildPacket(int objId, L1Buddy buddy)
		{
			writeC(Opcodes.S_OPCODE_SHOWHTML);
			writeD(objId);
			writeS(_HTMLID);
			writeC(0x00);
			writeH(0x02);

			writeS(buddy.BuddyListString);
			writeS(buddy.OnlineBuddyListString);
		}

		public override sbyte[] Content
		{
			get
			{
				if (_byte == null)
				{
					_byte = _bao.toByteArray();
				}
				return _byte;
			}
		}

		public override string Type
		{
			get
			{
				return _S_Buddy;
			}
		}
	}

}