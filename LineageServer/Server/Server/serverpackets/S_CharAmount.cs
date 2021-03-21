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
	using Config = LineageServer.Server.Config;
	using Account = LineageServer.Server.Server.Account;
	using ClientThread = LineageServer.Server.Server.ClientThread;
	using Opcodes = LineageServer.Server.Server.Opcodes;

	public class S_CharAmount : ServerBasePacket
	{

		private byte[] _byte = null;

		public S_CharAmount(int value, ClientThread client)
		{
			buildPacket(value, client);
		}

		private void buildPacket(int value, ClientThread client)
		{
			Account account = Account.load(client.AccountName);
			int characterSlot = account.CharacterSlot;
			int maxAmount = Config.DEFAULT_CHARACTER_SLOT + characterSlot;

			writeC(Opcodes.S_OPCODE_CHARAMOUNT);
			writeC(value);
			writeC(maxAmount); // 最大角色欄位數量
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