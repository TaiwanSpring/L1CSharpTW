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
	using L1DollInstance = LineageServer.Server.Server.Model.Instance.L1DollInstance;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket , S_DollPack

	public class S_DollPack : ServerBasePacket
	{

		private const string S_DOLLPACK = "[S] S_DollPack";

		private byte[] _byte = null;

		public S_DollPack(L1DollInstance doll)
		{
			/*
			 * int addbyte = 0; int addbyte1 = 1; int addbyte2 = 13; int setting =
			 * 4;
			 */
			writeC(Opcodes.S_OPCODE_CHARPACK);
			writeH(doll.X);
			writeH(doll.Y);
			writeD(doll.Id);
			writeH(doll.GfxId); // SpriteID in List.spr
			writeC(doll.Status); // Modes in List.spr
			writeC(doll.Heading);
			writeC(0); // (Bright) - 0~15
			writeC(doll.MoveSpeed); // 1段加速狀態
			writeD(0);
			writeH(0);
			writeS(doll.NameId);
			writeS(doll.Title);
			writeC((doll.BraveSpeed * 16)); // 狀態、2段加速狀態
			writeD(0); // ??
			writeS(null); // ??
			writeS(doll.Master != null ? doll.Master.Name : "");
			writeC(0); // ??
			writeC(0xFF);
			writeC(0);
			writeC(doll.Level); // PC = 0, Mon = Lv
			writeC(0);
			writeC(0xFF);
			writeC(0xFF);
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
				return S_DOLLPACK;
			}
		}

	}

}