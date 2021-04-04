using LineageServer.Server;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_OwnCharStatus2 : ServerBasePacket
    {

        /// <summary>
        /// 角色六大素質+負重更新<br> </summary>
        /// <param name="pc"> </param>
        /// <param name="type"> 0:不檢查重複的屬性  1:檢查重複的屬性次數 </param>
        public S_OwnCharStatus2(L1PcInstance pc, int type)
        {
            if (type == 0)
            {
                buildPacket(pc);
            }
            else if (type == 1)
            {
                int[] status = new int[] { pc.BaseStr, pc.BaseInt, pc.BaseWis, pc.BaseDex, pc.BaseCon, pc.BaseCha };
                for (int i = 0; i <= status.Length; i++)
                {
                    for (int j = i + 1; j <= status.Length; j++)
                    {
                        buildPacket(pc);
                    }
                }
            }

        }

        /// <summary>
        /// 更新六項能力值以及負重 </summary>
        private void buildPacket(L1PcInstance pc)
        {
            WriteC(Opcodes.S_OPCODE_OWNCHARSTATUS2);
            WriteC(pc.BaseStr);
            WriteC(pc.BaseInt);
            WriteC(pc.BaseWis);
            WriteC(pc.BaseDex);
            WriteC(pc.BaseCon);
            WriteC(pc.BaseCha);
            WriteC((pc.Inventory as L1PcInventory).Weight242);
        }
        public override string Type
        {
            get
            {
                return "[C] S_OwnCharStatus2";
            }
        }
    }

}