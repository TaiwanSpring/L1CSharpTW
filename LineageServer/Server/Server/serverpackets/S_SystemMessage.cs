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

	public class S_SystemMessage : ServerBasePacket
	{
		private const string S_SYSTEM_MESSAGE = "[S] S_SystemMessage";

		private byte[] _byte = null;

		private readonly string _msg;

		/// <summary>
		/// クライアントにデータの存在しないオリジナルのメッセージを表示する。
		/// メッセージにnameid($xxx)が含まれている場合はオーバーロードされたもう一方を使用する。
		/// </summary>
		/// <param name="msg">
		///            - 表示する文字列 </param>
		public S_SystemMessage(string msg)
		{
			_msg = msg;
			writeC(Opcodes.S_OPCODE_GLOBALCHAT);
			writeC(0x09);
			writeS(msg);
		}

		/// <summary>
		/// クライアントにデータの存在しないオリジナルのメッセージを表示する。
		/// </summary>
		/// <param name="msg">
		///            - 表示する文字列 </param>
		/// <param name="nameid">
		///            - 文字列にnameid($xxx)が含まれている場合trueにする。 </param>
		public S_SystemMessage(string msg, bool nameid)
		{
			_msg = msg;
			writeC(Opcodes.S_OPCODE_NPCSHOUT);
			writeC(2);
			writeD(0);
			writeS(msg);
			// NPCチャットパケットであればnameidが解釈されるためこれを利用する
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

		public override string ToString()
		{
			return string.Format("{0}: {1}", S_SYSTEM_MESSAGE, _msg);
		}

		public override string Type
		{
			get
			{
				return S_SYSTEM_MESSAGE;
			}
		}
	}

}