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

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_Door : ServerBasePacket
	{
		private const string S_DOOR = "[S] S_Door";
		private byte[] _byte = null;

		private const int PASS = 0;
		private const int NOT_PASS = 1;

		public S_Door(int x, int y, int direction, bool isPassable)
		{
			buildPacket(x, y, direction, isPassable);
		}

		private void buildPacket(int x, int y, int direction, bool isPassable)
		{
			WriteC(Opcodes.S_OPCODE_ATTRIBUTE);
			WriteH(x);
			WriteH(y);
			WriteC(direction); // ドアの方向 0: ／ 1: ＼
			WriteC(isPassable ? PASS : NOT_PASS);
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
				return S_DOOR;
			}
		}
	}

}