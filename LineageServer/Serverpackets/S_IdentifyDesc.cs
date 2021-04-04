using LineageServer.Server;
using LineageServer.Server.Model.Instance;
using System;
using System.Text;
namespace LineageServer.Serverpackets
{
    class S_IdentifyDesc : ServerBasePacket
    {

        private byte[] _byte = null;

        /// <summary>
        /// 確認スクロール使用時のメッセージを表示する
        /// </summary>
        public S_IdentifyDesc(L1ItemInstance item)
        {
            buildPacket(item);
        }

        private void buildPacket(L1ItemInstance item)
        {
            WriteC(Opcodes.S_OPCODE_IDENTIFYDESC);
            WriteH(item.Item.ItemDescId);

            StringBuilder name = new StringBuilder();

            if (item.Item.Bless == 0)
            {
                name.Append("$227 "); // 祝福された
            }
            else if (item.Item.Bless == 2)
            {
                name.Append("$228 "); // 呪われた
            }

            name.Append(item.Item.IdentifiedNameId);

            // 旅館鑰匙
            if (item.Item.ItemId == 40312 && item.KeyId != 0)
            {
                name.Append(item.InnKeyName);
            }

            if (item.Item.Type2 == 1)
            { // weapon
                WriteH(134); // \f1%0：小さなモンスター打撃%1 大きなモンスター打撃%2
                WriteC(3);
                WriteS(name.ToString());
                WriteS($"{item.Item.DmgSmall}+{item.EnchantLevel}");
                WriteS($"{item.Item.DmgLarge}+{item.EnchantLevel}");

            }
            else if (item.Item.Type2 == 2)
            { // armor
                if (item.Item.ItemId == 20383)
                { // 騎馬用ヘルム
                    WriteH(137); // \f1%0：使用可能回数%1［重さ%2］
                    WriteC(3);
                    WriteS(name.ToString());
                    WriteS(item.ChargeCount.ToString());
                }
                else
                {
                    WriteH(135); // \f1%0：防御力%1 防御具
                    WriteC(2);
                    WriteS(name.ToString());
                    WriteS($"{Math.Abs(item.Item.get_ac())}+{item.EnchantLevel}");
                }

            }
            else if (item.Item.Type2 == 0)
            { // etcitem
                if (item.Item.Type == 1)
                { // wand
                    WriteH(137); // \f1%0：使用可能回数%1［重さ%2］
                    WriteC(3);
                    WriteS(name.ToString());
                    WriteS(item.ChargeCount.ToString());
                }
                else if (item.Item.Type == 2)
                { // light系アイテム
                    WriteH(138);
                    WriteC(2);
                    name.Append(": $231 "); // 残りの燃料
                    name.Append(item.RemainingTime.ToString());
                    WriteS(name.ToString());
                }
                else if (item.Item.Type == 7)
                { // food
                    WriteH(136); // \f1%0：満腹度%1［重さ%2］
                    WriteC(3);
                    WriteS(name.ToString());
                    WriteS((item.Item.FoodVolume).ToString());
                }
                else
                {
                    WriteH(138); // \f1%0：［重さ%1］
                    WriteC(2);
                    WriteS(name.ToString());
                }
                WriteS(item.Weight.ToString());
            }
        }
    }

}