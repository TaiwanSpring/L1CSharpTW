using LineageServer.Server;
using LineageServer.Server.Model;

namespace LineageServer.Serverpackets
{
    class S_AttackPacketForNpc : ServerBasePacket
    {
        private const string S_ATTACK_PACKET_FOR_NPC = "[S] S_AttackPacketForNpc";

        private byte[] _byte = null;

        public S_AttackPacketForNpc(L1Character cha, int npcObjectId, int type)
        {
            buildpacket(cha, npcObjectId, type);
        }

        private void buildpacket(L1Character cha, int npcObjectId, int type)
        {
            WriteC(Opcodes.S_OPCODE_ATTACKPACKET);
            WriteC(type);
            WriteD(npcObjectId);
            WriteD(cha.Id);
            WriteH(0x01); // 3.3C damage
            WriteC(cha.Heading);
            WriteH(0x0000); // target x
            WriteH(0x0000); // target y
            WriteC(0x00); // 0x00:none 0x04:Claw 0x08:CounterMirror
        }

        public override string Type
        {
            get
            {
                return S_ATTACK_PACKET_FOR_NPC;
            }
        }
    }

}