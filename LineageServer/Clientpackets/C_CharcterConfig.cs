using LineageServer.Server.DataSources;
using LineageServer.Server.Model.Instance;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 收到由客戶端傳來角色設定的封包
    /// </summary>
    class C_CharcterConfig : ClientBasePacket
    {

        private const string C_CHARCTER_CONFIG = "[C] C_CharcterConfig";
        public C_CharcterConfig(byte[] abyte0, ClientThread client) : base(abyte0)
        {
            if (Config.CHARACTER_CONFIG_IN_SERVER_SIDE)
            {
                L1PcInstance pc = client.ActiveChar;
                if (pc == null)
                {
                    return;
                }
                int length = ReadD() - 3;
                byte[] data = ReadByte();
                int count = CharacterConfigTable.Instance.countCharacterConfig(pc.Id);
                if (count == 0)
                {
                    CharacterConfigTable.Instance.storeCharacterConfig(pc.Id, length, data);
                }
                else
                {
                    CharacterConfigTable.Instance.updateCharacterConfig(pc.Id, length, data);
                }
            }
        }

        public override string Type
        {
            get
            {
                return C_CHARCTER_CONFIG;
            }
        }
    }

}