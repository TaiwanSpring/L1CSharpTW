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
	using L1PcInstance = LineageServer.Server.Server.Model.Instance.L1PcInstance;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	class S_NewCharPacket : ServerBasePacket
	{
		private const string _S__25_NEWCHARPACK = "[S] New Char Packet";
		private byte[] _byte = null;

		public S_NewCharPacket(L1PcInstance pc)
		{
			buildPacket(pc);
		}

		private void buildPacket(L1PcInstance pc)
		{
			writeC(Opcodes.S_OPCODE_NEWCHARPACK);
			writeS(pc.Name);
			writeS("");
			writeC(pc.Type);
			writeC(pc.get_sex());
			writeH(pc.Lawful);
			writeH(pc.MaxHp);
			writeH(pc.MaxMp);
			writeC(pc.Ac);
			writeC(pc.Level);
			writeC(pc.Str);
			writeC(pc.Dex);
			writeC(pc.Con);
			writeC(pc.Wis);
			writeC(pc.Cha);
			writeC(pc.Int);
			writeC(0); // 是否為管理員
			writeD(pc.SimpleBirthday);
			writeC((pc.Level ^ pc.Str ^ pc.Dex ^ pc.Con ^ pc.Wis ^ pc.Cha ^ pc.Int) & 0xff); // XOR 驗證
		}

		public override byte[] Content
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
				return _S__25_NEWCHARPACK;
			}
		}

	}

}