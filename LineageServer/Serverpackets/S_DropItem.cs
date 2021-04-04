using LineageServer.Server;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Serverpackets
{
    class S_DropItem : ServerBasePacket
    {

        private const string _S__OB_DropItem = "[S] S_DropItem";

        private byte[] _byte = null;

        public S_DropItem(L1ItemInstance item)
        {
            buildPacket(item);
        }

        private void buildPacket(L1ItemInstance item)
        {
            // int addbyte = 0;
            // int addbyte1 = 1;
            // int addbyte2 = 13;
            // int setting = 4;

            string itemName = item.Item.UnidentifiedNameId;
            // 已鑑定
            int isId = item.Identified ? 1 : 0;
            if (isId == 1)
            {
                itemName = item.Item.IdentifiedNameId;
            }
            WriteC(Opcodes.S_OPCODE_DROPITEM);
            WriteH(item.X);
            WriteH(item.Y);
            WriteD(item.Id);
            WriteH(item.Item.GroundGfxId);
            WriteC(0);
            WriteC(0);
            if (item.NowLighting)
            {
                WriteC(item.Item.LightRange);
            }
            else
            {
                WriteC(0);
            }
            WriteC(0);
            WriteD(item.Count);
            WriteC(0);
            WriteC(0);
            if (item.Count > 1)
            {
                if (item.Item.ItemId == 40312 && item.KeyId != 0)
                { // 旅館鑰匙
                    WriteS(itemName + item.InnKeyName + " (" + item.Count + ")");
                }
                else
                {
                    WriteS(itemName + " (" + item.Count + ")");
                }
            }
            else
            {
                int itemId = item.Item.ItemId;
                if ((itemId == 20383) && (isId == 1))
                { // 軍馬頭盔
                    WriteS($"{itemName} [{item.ChargeCount}]");
                }
                else if (item.ChargeCount != 0 && (isId == 1))
                { // 可使用的次數
                    WriteS($"{itemName} ({item.ChargeCount})");
                }
                else if ((item.Item.LightRange != 0) && item.NowLighting)
                { // 燈具
                    WriteS($"{itemName} ($10)");
                }
                else if (item.Item.ItemId == 40312 && item.KeyId != 0)
                { // 旅館鑰匙
                    WriteS($"{itemName}{item.InnKeyName}");
                }
                else
                {
                    WriteS(itemName);
                }
            }
            WriteC(0);
            WriteD(0);
            WriteD(0);
            WriteC(255);
            WriteC(0);
            WriteC(0);
            WriteC(0);
            WriteH(65535);
            // WriteD(0x401799a);
            WriteD(0);
            WriteC(8);
            WriteC(0);
        }
        public override string Type
        {
            get
            {
                return _S__OB_DropItem;
            }
        }

    }

}