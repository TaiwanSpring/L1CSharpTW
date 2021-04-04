using LineageServer.Server;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_PetCtrlMenu : ServerBasePacket
    {
        public S_PetCtrlMenu(L1Character cha, L1NpcInstance npc, bool open)
        {
            WriteC(Opcodes.S_OPCODE_CHARRESET); // 3.80C 更動
            WriteC(0x0c);

            if (open)
            {
                WriteH(cha.PetList.Count * 3);
                WriteD(0x00000000);
                WriteD(npc.Id);
                WriteH(npc.MapId);
                WriteH(0x0000);
                WriteH(npc.X);
                WriteH(npc.Y);
                WriteS(npc.NameId);
            }
            else
            {
                WriteH(cha.PetList.Count * 3 - 3);
                WriteD(0x00000001);
                WriteD(npc.Id);
            }
        }
    }
}