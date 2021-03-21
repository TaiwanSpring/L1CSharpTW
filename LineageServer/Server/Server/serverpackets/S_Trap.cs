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
	using L1TrapInstance = LineageServer.Server.Server.Model.Instance.L1TrapInstance;

	public class S_Trap : ServerBasePacket
	{
		public S_Trap(L1TrapInstance trap, string name)
		{

			writeC(Opcodes.S_OPCODE_DROPITEM);
			writeH(trap.X);
			writeH(trap.Y);
			writeD(trap.Id);
			writeH(7); // adena
			writeC(0);
			writeC(0);
			writeC(0);
			writeC(0);
			writeD(0);
			writeC(0);
			writeC(0);
			writeS(name);
			writeC(0);
			writeD(0);
			writeD(0);
			writeC(255);
			writeC(0);
			writeC(0);
			writeC(0);
			writeH(65535);
			// writeD(0x401799a);
			writeD(0);
			writeC(8);
			writeC(0);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public byte[] getContent() throws java.io.IOException
		public override sbyte[] Content
		{
			get
			{
				return Bytes;
			}
		}
	}

}