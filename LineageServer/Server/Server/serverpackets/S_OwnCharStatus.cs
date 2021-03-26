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
	using L1GameTimeClock = LineageServer.Server.Server.Model.Gametime.L1GameTimeClock;

	// Referenced classes of package l1j.server.server.serverpackets:
	// ServerBasePacket

	public class S_OwnCharStatus : ServerBasePacket
	{
		private const string S_OWB_CHAR_STATUS = "[S] S_OwnCharStatus";

		private byte[] _byte = null;

		public S_OwnCharStatus(L1PcInstance pc)
		{
			int time = L1GameTimeClock.Instance.CurrentTime().Seconds;
			time = time - (time % 300);
			writeC(Opcodes.S_OPCODE_OWNCHARSTATUS);
			writeD(pc.Id);
			if (pc.Level < 1)
			{
				writeC(1);
			}
			else if (pc.Level > 127)
			{
				writeC(127);
			}
			else
			{
				writeC(pc.Level);
			}
			writeExp(pc.Exp);
			writeC(pc.Str);
			writeC(pc.Int);
			writeC(pc.Wis);
			writeC(pc.Dex);
			writeC(pc.Con);
			writeC(pc.Cha);
			writeH(pc.CurrentHp);
			writeH(pc.MaxHp);
			writeH(pc.CurrentMp);
			writeH(pc.MaxMp);
			writeC(pc.Ac);
			writeD(time);
			writeC(pc.get_food());
			writeC(pc.Inventory.Weight242);
			writeH(pc.Lawful);
			writeH(pc.Fire);
			writeH(pc.Water);
			writeH(pc.Wind);
			writeH(pc.Earth);
			writeD(pc.MonsKill); // 狩獵數量
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
				return S_OWB_CHAR_STATUS;
			}
		}
	}
}