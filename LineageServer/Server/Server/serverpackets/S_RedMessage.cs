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
namespace LineageServer.Server.Server.serverpackets
{
	using Opcodes = LineageServer.Server.Server.Opcodes;

	public class S_RedMessage : ServerBasePacket
	{

		private const string _S__18_REDMESSAGE = "[S] S_RedMessage";

		private byte[] _byte = null;

		public S_RedMessage(int type, string msg1)
		{
			buildPacket(type, msg1, null, null, 1);
		}

		public S_RedMessage(int type, string msg1, string msg2)
		{
			buildPacket(type, msg1, msg2, null, 2);
		}

		public S_RedMessage(int type, string msg1, string msg2, string msg3)
		{
			buildPacket(type, msg1, msg2, msg3, 3);
		}

		private void buildPacket(int type, string msg1, string msg2, string msg3, int check)
		{
			writeC(Opcodes.S_OPCODE_REDMESSAGE);
			writeH(type);
			if (check == 1)
			{
				if (msg1.Length <= 0)
				{
					writeC(0);
				}
				else
				{
					writeC(1);
					writeS(msg1);
				}
			}
			else if (check == 2)
			{
				writeC(2);
				writeS(msg1);
				writeS(msg2);
			}
			else if (check == 3)
			{
				writeC(3);
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
					_byte = _bao.toByteArray();
				}
				return _byte;
			}
		}

		public override string Type
		{
			get
			{
				return _S__18_REDMESSAGE;
			}
		}
	}

}