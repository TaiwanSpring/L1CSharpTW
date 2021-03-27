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
	using L1BoardTopic = LineageServer.Server.Templates.L1BoardTopic;

	public class S_BoardRead : ServerBasePacket
	{

//JAVA TO C# CONVERTER NOTE: Members cannot have the same name as their enclosing type:
		private const string S_BoardRead_Conflict = "[S] S_BoardRead";

		private byte[] _byte = null;

		public S_BoardRead(int number)
		{
			buildPacket(number);
		}

		private void buildPacket(int number)
		{
			L1BoardTopic topic = L1BoardTopic.findById(number);
			WriteC(Opcodes.S_OPCODE_BOARDREAD);
			WriteD(number);
			WriteS(topic.Name);
			WriteS(topic.Title);
			WriteS(topic.Date);
			WriteS(topic.Content);
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
				return S_BoardRead_Conflict;
			}
		}
	}

}