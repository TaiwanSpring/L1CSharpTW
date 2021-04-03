using LineageServer.Server;
using LineageServer.Server.Templates;
using System.Collections.Generic;
namespace LineageServer.Serverpackets
{
	class S_Board : ServerBasePacket
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
			WriteC(Opcodes.S_OPCODE_BOARD);
			WriteC(0); // DragonKeybbs = 1
			WriteD(boardObjId);
			if (number == 0)
			{
				WriteD(0x7FFFFFFF);
			}
			else
			{
				WriteD(number);
			}
			WriteC(topics.Count);
			if (number == 0)
			{
				WriteC(0);
				WriteH(300);
			}
			foreach (L1BoardTopic topic in topics)
			{
				WriteD(topic.Id);
				WriteS(topic.Name);
				WriteS(topic.Date);
				WriteS(topic.Title);
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