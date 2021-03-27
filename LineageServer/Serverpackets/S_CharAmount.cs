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
	using Config = LineageServer.Server.Config;
	using Account = LineageServer.Server.Account;
	using ClientThread = LineageServer.Server.ClientThread;
	using Opcodes = LineageServer.Server.Opcodes;

	public class S_CharAmount : ServerBasePacket
	{

		private byte[] _byte = null;

		public S_CharAmount(int value, ClientThread client)
		{
			buildPacket(value, client);
		}

		private void buildPacket(int value, ClientThread client)
		{
			Account account = Account.Load(client.AccountName);
			int characterSlot = account.CharacterSlot;
			int maxAmount = Config.DEFAULT_CHARACTER_SLOT + characterSlot;

			WriteC(Opcodes.S_OPCODE_CHARAMOUNT);
			WriteC(value);
			WriteC(maxAmount); // 最大角色欄位數量
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