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

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_War : ServerBasePacket
	{

		private const string S_WAR = "[S] S_War";

		private byte[] _byte = null;

		public S_War(int type, string clan_name1, string clan_name2)
		{
			buildPacket(type, clan_name1, clan_name2);
		}

		private void buildPacket(int type, string clan_name1, string clan_name2)
		{
			// 1 : _血盟が_血盟に宣戦布告しました。
			// 2 : _血盟が_血盟に降伏しました。
			// 3 : _血盟と_血盟との戦争が終結しました。
			// 4 : _血盟 贏了對_血盟 的戰爭。
			// 6 : _血盟と_血盟が同盟を結びました。
			// 7 : _血盟と_血盟との同盟関係が解除されました。
			// 8 : あなたの血盟が現在_血盟と交戦中です。

			WriteC(Opcodes.S_OPCODE_WAR);
			WriteC(type);
			WriteS(clan_name1);
			WriteS(clan_name2);
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
				return S_WAR;
			}
		}
	}

}