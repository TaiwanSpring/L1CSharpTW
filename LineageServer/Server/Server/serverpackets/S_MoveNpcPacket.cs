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
	using L1MonsterInstance = LineageServer.Server.Server.Model.Instance.L1MonsterInstance;

	public class S_MoveNpcPacket : ServerBasePacket
	{
		private const string _S__1F_S_MOVENPCPACKET = "[S] S_MoveNpcPacket";

		public S_MoveNpcPacket(L1MonsterInstance npc, int x, int y, int heading)
		{
			// npc.set_moving(true);

			WriteC(Opcodes.S_OPCODE_MOVEOBJECT);
			WriteD(npc.Id);
			WriteH(x);
			WriteH(y);
			WriteC(heading);
			WriteC(0x80); // 3.80C 更動
			WriteD(0x00000000);

			// npc.set_moving(false);
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
				return _S__1F_S_MOVENPCPACKET;
			}
		}
	}

}