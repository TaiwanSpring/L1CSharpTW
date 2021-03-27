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

	//Referenced classes of package l1j.server.server.serverpackets:
	//ServerBasePacket


	public class S_RuneSlot : ServerBasePacket
	{
		private const string S_RUNESLOT = "[S] S_RuneSlot";

		public static int RUNE_CLOSE_SLOT = 1;
		public static int RUNE_OPEN_SLOT = 2;

		public S_RuneSlot(int type, int slotNum)
		{
			WriteC(Opcodes.S_OPCODE_CHARRESET);
			WriteC(0x43);
			WriteD(type);
			WriteD(slotNum);
			WriteD(0);
			WriteD(0);
			WriteD(0);
			WriteH(0);
		}


		public override sbyte[] Content
		{
			get
			{
				return Bytes;
			}
		}

		public override string Type
		{
			get
			{
				return S_RUNESLOT;
			}
		}
	}


}