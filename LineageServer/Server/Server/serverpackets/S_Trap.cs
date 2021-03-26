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

			WriteC(Opcodes.S_OPCODE_DROPITEM);
			WriteH(trap.X);
			WriteH(trap.Y);
			WriteD(trap.Id);
			WriteH(7); // adena
			WriteC(0);
			WriteC(0);
			WriteC(0);
			WriteC(0);
			WriteD(0);
			WriteC(0);
			WriteC(0);
			WriteS(name);
			WriteC(0);
			WriteD(0);
			WriteD(0);
			WriteC(255);
			WriteC(0);
			WriteC(0);
			WriteC(0);
			WriteH(65535);
			// WriteD(0x401799a);
			WriteD(0);
			WriteC(8);
			WriteC(0);
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