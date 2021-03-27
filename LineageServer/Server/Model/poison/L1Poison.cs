using LineageServer.Server.Model.Instance;
using LineageServer.Server.Model.skill;
using LineageServer.Serverpackets;

namespace LineageServer.Server.Model.poison
{
    abstract class L1Poison
    {
        protected internal static bool isValidTarget(L1Character cha)
        {
            if (cha == null)
            {
                return false;
            }
            // 毒は重複しない
            if (cha.Poison != null)
            {
                return false;
            }

            if (!(cha is L1PcInstance))
            {
                return true;
            }

            L1PcInstance player = (L1PcInstance)cha;
            if (player.Inventory is L1PcInventory pcInventory)
            {
                if (pcInventory.checkEquipped(20298) ||
            pcInventory.checkEquipped(20117) ||
            pcInventory.checkEquipped(21115) ||
            pcInventory.checkEquipped(21116) ||
            pcInventory.checkEquipped(21117) ||
            pcInventory.checkEquipped(21118) ||
            player.hasSkillEffect(L1SkillId.VENOM_RESIST)) // 黑暗妖精魔法(毒性抵抗)
                {
                    return false;
                }
            }

            return true;
        }

        // 微妙・・・素直にsendPacketsをL1Characterへ引き上げるべきかもしれない
        protected internal static void sendMessageIfPlayer(L1Character cha, int msgId)
        {
            if (!(cha is L1PcInstance))
            {
                return;
            }

            L1PcInstance player = (L1PcInstance)cha;
            player.sendPackets(new S_ServerMessage(msgId));
        }

        /// <summary>
        /// この毒のエフェクトIDを返す。
        /// </summary>
        /// <seealso cref= S_Poison#S_Poison(int, int)
        /// </seealso>
        /// <returns> S_Poisonで使用されるエフェクトID </returns>
        public abstract int EffectId { get; }

        /// <summary>
        /// この毒の効果を取り除く。<br>
        /// </summary>
        /// <seealso cref= L1Character#curePoison() </seealso>
        public abstract void cure();
    }

}