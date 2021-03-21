using System.Collections.Generic;

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
	using L1BoardTopic = LineageServer.Server.Server.Templates.L1BoardTopic;

	public class S_Board : ServerBasePacket
	{

		private const string S_BOARD = "[S] S_Board";

		private const int TOPIC_LIMIT = 8;

		private byte[] _byte = null;

		public S_Board(int boardObjId)
		{
			buildPacket(boardObjId, 0);
		}

		public S_Board(int boardObjId, int number)
		{
			buildPacket(boardObjId, number);
		}

		private void buildPacket(int boardObjId, int number)
		{
			IList<L1BoardTopic> topics = L1BoardTopic.index(number, TOPIC_LIMIT);
			writeC(Opcodes.S_OPCODE_BOARD);
			writeC(0); // DragonKeybbs = 1
			writeD(boardObjId);
			if (number == 0)
			{
				writeD(0x7FFFFFFF);
			}
			else
			{
				writeD(number);
			}
			writeC(topics.Count);
			if (number == 0)
			{
				writeC(0);
				writeH(300);
			}
			foreach (L1BoardTopic topic in topics)
			{
				writeD(topic.Id);
				writeS(topic.Name);
				writeS(topic.Date);
				writeS(topic.Title);
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
				return S_BOARD;
			}
		}
	}

}