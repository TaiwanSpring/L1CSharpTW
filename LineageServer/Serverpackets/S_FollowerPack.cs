using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_FollowerPack : ServerBasePacket
    {

        private const string S_FOLLOWER_PACK = "[S] S_FollowerPack";

        private const int STATUS_POISON = 1;

        private byte[] _byte = null;

        public S_FollowerPack(L1FollowerInstance follower, L1PcInstance pc)
        {
            WriteC(Opcodes.S_OPCODE_CHARPACK);
            WriteH(follower.X);
            WriteH(follower.Y);
            WriteD(follower.Id);
            WriteH(follower.GfxId);
            WriteC(follower.Status);
            WriteC(follower.Heading);
            WriteC(follower.ChaLightSize);
            WriteC(follower.MoveSpeed);
            WriteD(0);
            WriteH(0);
            WriteS(follower.NameId);
            WriteS(follower.Title);
            int status = 0;
            if (follower.Poison != null)
            { // 毒状態
                if (follower.Poison.EffectId == 1)
                {
                    status |= STATUS_POISON;
                }
            }
            WriteC(status);
            WriteD(0);
            WriteS(null);
            WriteS(null);
            WriteC(0);
            WriteC(0xFF);
            WriteC(0);
            WriteC(follower.Level);
            WriteC(0);
            WriteC(0xFF);
            WriteC(0xFF);
        }
        public override string Type
        {
            get
            {
                return S_FOLLOWER_PACK;
            }
        }

    }

}