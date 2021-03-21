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
	using L1Character = LineageServer.Server.Server.Model.L1Character;
	using L1NpcInstance = LineageServer.Server.Server.Model.Instance.L1NpcInstance;

	public class S_PetCtrlMenu : ServerBasePacket
	{
		public S_PetCtrlMenu(L1Character cha, L1NpcInstance npc, bool open)
		{
			writeC(Opcodes.S_OPCODE_CHARRESET); // 3.80C 更動
			writeC(0x0c);

			if (open)
			{
				writeH(cha.PetList.Count * 3);
				writeD(0x00000000);
				writeD(npc.Id);
				writeH(npc.MapId);
				writeH(0x0000);
				writeH(npc.X);
				writeH(npc.Y);
				writeS(npc.NameId);
			}
			else
			{
				writeH(cha.PetList.Count * 3 - 3);
				writeD(0x00000001);
				writeD(npc.Id);
			}
		}

		public override sbyte[] Content
		{
			get
			{
				return Bytes;
			}
		}
	}

}