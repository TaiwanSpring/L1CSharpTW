using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_DelSkill : ServerBasePacket
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

        public override string Type
        {
            get
            {
                return "[S] S_DelSkill";
            }
        }

    }

}