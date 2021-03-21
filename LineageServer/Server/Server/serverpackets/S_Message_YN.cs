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
	using GameServer = LineageServer.Server.Server.GameServer;
	using Opcodes = LineageServer.Server.Server.Opcodes;

	public class S_Message_YN : ServerBasePacket
	{

		private byte[] _byte = null;

		public S_Message_YN(int type, string msg1)
		{
			buildPacket(type, msg1, null, null, 1);
		}

		public S_Message_YN(int type, string msg1, string msg2)
		{
			buildPacket(type, msg1, msg2, null, 2);
		}

		public S_Message_YN(int type, string msg1, string msg2, string msg3)
		{
			buildPacket(type, msg1, msg2, msg3, 3);
		}

		private void buildPacket(int type, string msg1, string msg2, string msg3, int check)
		{
			writeC(Opcodes.S_OPCODE_YES_NO);
			writeH(0x0000); // 3.51未知封包
			writeD(GameServer.YesNoCount);
			writeH(type);
			if (check == 1)
			{
				writeS(msg1);
			}
			else if (check == 2)
			{
				writeS(msg1);
				writeS(msg2);
			}
			else if (check == 3)
			{
				writeS(msg1);
				writeS(msg2);
				writeS(msg3);
			}
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
				return "[S] S_Message_YN";
			}
		}
	}

}