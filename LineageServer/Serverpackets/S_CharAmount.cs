using LineageServer.Server;

namespace LineageServer.Serverpackets
{
    class S_CharAmount : ServerBasePacket
    {

        private byte[] _byte = null;

        public S_CharAmount(int value, ClientThread client)
        {
            buildPacket(value, client);
        }

        private void buildPacket(int value, ClientThread client)
        {
            Account account = Account.Load(client.AccountName);
            int characterSlot = account.CharacterSlot;
            int maxAmount = Config.DEFAULT_CHARACTER_SLOT + characterSlot;

            WriteC(Opcodes.S_OPCODE_CHARAMOUNT);
            WriteC(value);
            WriteC(maxAmount); // 最大角色欄位數量
        }
    }
}