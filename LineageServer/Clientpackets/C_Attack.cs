using LineageServer.Server;
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理客戶端傳來攻擊的封包
    /// </summary>
    class C_Attack : ClientBasePacket
    {

        public C_Attack(byte[] decrypt, ClientThread client) : base(decrypt)
        {

            L1PcInstance pc = client.ActiveChar;
            if ((pc == null) || pc.Ghost || pc.Dead || pc.Teleport || pc.Paralyzed || pc.Sleeped)
            {
                return;
            }

            int targetId = ReadD();
            int x = ReadH();
            int y = ReadH();

            GameObject target = L1World.Instance.findObject(targetId);

            // 確認是否可以攻擊
            if (pc.Inventory is L1PcInventory pcInventory && pcInventory.Weight242 >= 197)
            { // 是否超重
                pc.sendPackets(new S_ServerMessage(110)); // \f1アイテムが重すぎて戦闘することができません。
                return;
            }

            if (pc.Invisble)
            { // 是否隱形
                return;
            }

            if (pc.InvisDelay)
            { // 是否在隱形解除的延遲中
                return;
            }

            if (target is L1Character)
            {
                if ((target.MapId != pc.MapId) || (pc.Location.getLineDistance(target.Location) > 20D))
                { // 如果目標距離玩家太遠(外掛)
                    return;
                }
            }

            if (target is L1NpcInstance)
            {
                int hiddenStatus = ((L1NpcInstance)target).HiddenStatus;
                if ((hiddenStatus == L1NpcInstance.HIDDEN_STATUS_SINK) || (hiddenStatus == L1NpcInstance.HIDDEN_STATUS_FLY))
                { // 如果目標躲到土裡面，或是飛起來了
                    return;
                }
            }

            // 是否要檢查攻擊的間隔
            if (Config.CHECK_ATTACK_INTERVAL)
            {
                int result;
                result = pc.AcceleratorChecker.checkInterval(AcceleratorChecker.ACT_TYPE.ATTACK);
                if (result == AcceleratorChecker.R_DISPOSED)
                {
                    return;
                }
            }

            if (pc.hasSkillEffect(L1SkillId.ABSOLUTE_BARRIER))
            { // 取消絕對屏障
                pc.removeSkillEffect(L1SkillId.ABSOLUTE_BARRIER);
            }
            if (pc.hasSkillEffect(L1SkillId.MEDITATION))
            { // 取消冥想效果
                pc.removeSkillEffect(L1SkillId.MEDITATION);
            }

            pc.delInvis(); // 解除隱形狀態

            pc.RegenState = L1PcInstance.REGENSTATE_ATTACK;

            if ((target != null) && !((L1Character)target).Dead)
            {
                target.onAction(pc);
            }
            else
            { // 目標為空或死亡
                L1Character cha = new L1Character();
                cha.Id = targetId;
                cha.X = x;
                cha.Y = y;
                L1Attack atk = new L1Attack(pc, cha);
                atk.actionPc();
            }
        }
    }

}