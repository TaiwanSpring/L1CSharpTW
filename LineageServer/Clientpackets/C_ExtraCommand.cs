
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來額外動作指令的封包
    /// </summary>
    class C_ExtraCommand : ClientBasePacket
    {
        private const string C_EXTRA_COMMAND = "[C] C_ExtraCommand";
        public C_ExtraCommand(byte[] abyte0, ClientThread client) : base(abyte0)
        {

            L1PcInstance pc = client.ActiveChar;
            if (pc == null)
            {
                return;
            }

            int actionId = ReadC();
            if (pc.Ghost)
            {
                return;
            }
            if (pc.Invisble)
            { // 隱形中
                return;
            }
            if (pc.Teleport)
            { // 傳送中
                return;
            }
            if (pc.hasSkillEffect(L1SkillId.SHAPE_CHANGE))
            { // 變深中
                int gfxId = pc.TempCharGfx;
                if ((gfxId != 6080) && (gfxId != 6094))
                { // 騎馬用的變身例外
                    return;
                }
            }
            S_DoActionGFX gfx = new S_DoActionGFX(pc.Id, actionId);
            pc.broadcastPacket(gfx); // 將動作送給附近的玩家
        }

        public override string Type
        {
            get
            {
                return C_EXTRA_COMMAND;
            }
        }
    }

}