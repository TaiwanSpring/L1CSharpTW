
using LineageServer.Server.Model;
using LineageServer.Server.Model.Instance;
using LineageServer.Serverpackets;

namespace LineageServer.Clientpackets
{
    /// <summary>
    /// 處理收到由客戶端傳來寵物選單的封包
    /// </summary>
    class C_PetMenu : ClientBasePacket
    {

        private const string C_PET_MENU = "[C] C_PetMenu";

        public C_PetMenu(byte[] abyte0, ClientThread clientthread) : base(abyte0)
        {

            L1PcInstance pc = clientthread.ActiveChar;
            if (pc == null)
            {
                return;
            }

            int petId = ReadD();
            L1PetInstance pet = (L1PetInstance)Container.Instance.Resolve<IGameWorld>().findObject(petId);

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