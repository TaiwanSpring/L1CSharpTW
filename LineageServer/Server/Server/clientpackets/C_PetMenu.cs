
using LineageServer.Server.Server.Model;
using LineageServer.Server.Server.Model.Instance;
using LineageServer.Server.Server.serverpackets;

namespace LineageServer.Server.Server.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來寵物選單的封包
    /// </summary>
    class C_PetMenu : ClientBasePacket
    {

        private const string C_PET_MENU = "[C] C_PetMenu";

        public C_PetMenu(sbyte[] abyte0, ClientThread clientthread) : base(abyte0)
        {

            L1PcInstance pc = clientthread.ActiveChar;
            if (pc == null)
            {
                return;
            }

            int petId = readD();
            L1PetInstance pet = (L1PetInstance)L1World.Instance.findObject(petId);

            if ((pet != null) && (pc != null))
            {
                pc.sendPackets(new S_PetInventory(pet));
            }
        }

        public override string Type
        {
            get
            {
                return C_PET_MENU;
            }
        }
    }

}