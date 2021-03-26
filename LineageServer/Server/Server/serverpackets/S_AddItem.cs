using LineageServer.Server.Server.Model.Instance;

namespace LineageServer.Server.Server.serverpackets
{
    class S_AddItem : ServerBasePacket
    {

        private const string S_ADD_ITEM = "[S] S_AddItem";

        /// <summary>
        /// 增加物品到背包處理封包。
        /// </summary>
        public S_AddItem(L1ItemInstance item)
        {
            WriteC(Opcodes.S_OPCODE_ADDITEM);
            WriteD(item.Id);
            WriteH(item.Item.MagicCatalystType > 0 ? item.Item.MagicCatalystType : item.Item.ItemDescId > 0 ? item.Item.ItemDescId : item.Item.GroundGfxId);
            WriteC(item.Item.UseType);
            WriteC(item.ChargeCount);
            WriteH(item.get_gfxid());
            WriteC(item.Bless);
            WriteD(item.Count);
            WriteC(item.ItemStatusX); // 3.80C 物品驗證機制
            WriteS(item.ViewName);
            if (!item.Identified)
            { // 未鑑定
                WriteC(0);
            }
            else
            {
                byte[] status = item.StatusBytes;
                WriteC(status.Length);
                foreach (byte b in status)
                {
                    WriteC(b);
                }
            }
            WriteC(0x17);
            WriteC(0);
            WriteH(0);
            WriteH(0);
            if (item.Item.Type == 10)
            { // 如果是法書，傳出法術編號
                WriteC(0);
            }
            else
            {
                WriteC(item.EnchantLevel); // 物品武捲等級
            }
            WriteD(item.Id); // 3.80 物品世界流水編號
            WriteD(0);
            WriteD(0);
            WriteD(item.Bless >= 128 ? 3 : item.Item.Tradable ? 7 : 2); // 7:可刪除, 2: 不可刪除, 3: 封印狀態
            WriteC(0);

            /*WriteC(0x17);
			WriteD(0);
			WriteD(0);
			WriteD(0);
			WriteD(0);
			WriteD(0);
			WriteH(0);
			WriteC(0);*/
        }

        public override string Type
        {
            get
            {
                return S_ADD_ITEM;
            }
        }
    }

}