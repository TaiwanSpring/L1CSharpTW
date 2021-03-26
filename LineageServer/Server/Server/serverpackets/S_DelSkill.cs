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

	public class S_DelSkill : ServerBasePacket
	{

		public S_DelSkill(int i, int j, int k, int l, int i1, int j1, int k1, int l1, int i2, int j2, int k2, int l2, int i3, int j3, int k3, int l3, int i4, int j4, int k4, int l4, int i5, int j5, int k5, int l5, int m5, int n5, int o5, int p5)
		{
			int i6 = i1 + j1 + k1 + l1;
			int j6 = i2 + j2;
			WriteC(Opcodes.S_OPCODE_DELSKILL);
			if ((i6 > 0) && (j6 == 0))
			{
				WriteC(50);
			}
			else if (j6 > 0)
			{
				WriteC(100);
			}
			else
			{
				WriteC(32);
			}
			WriteC(i);
			WriteC(j);
			WriteC(k);
			WriteC(l);
			WriteC(i1);
			WriteC(j1);
			WriteC(k1);
			WriteC(l1);
			WriteC(i2);
			WriteC(j2);
			WriteC(k2);
			WriteC(l2);
			WriteC(i3);
			WriteC(j3);
			WriteC(k3);
			WriteC(l3);
			WriteC(i4);
			WriteC(j4);
			WriteC(k4);
			WriteC(l4);
			WriteC(i5);
			WriteC(j5);
			WriteC(k5);
			WriteC(l5);
			WriteC(m5);
			WriteC(n5);
			WriteC(o5);
			WriteC(p5);
			WriteD(0);
			WriteD(0);
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
				return "[S] S_DelSkill";
			}
		}

	}

}