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

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_Sound : ServerBasePacket
	{

		private const string S_SOUND = "[S] S_Sound";

		private byte[] _byte = null;

		/// <summary>
		/// 効果音を鳴らす(soundフォルダのwavファイル)。
		/// </summary>
		/// <param name="sound"> </param>
		public S_Sound(int sound)
		{
			buildPacket(sound);
		}

		private void buildPacket(int sound)
		{
			WriteC(Opcodes.S_OPCODE_SOUND);
			WriteC(0); // repeat
			WriteH(sound);
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
				return S_SOUND;
			}
		}
	}

}