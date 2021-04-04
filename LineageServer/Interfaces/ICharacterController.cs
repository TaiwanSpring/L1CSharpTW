using LineageServer.Server.Model.Instance;
using LineageServer.Server.Templates;

namespace LineageServer.Interfaces
{
    interface ICharacterController
    {
        L1CharName[] CharNameList { get; }
        void storeNewCharacter(L1PcInstance pc);
        void storeCharacter(L1PcInstance pc);
        void deleteCharacter(string accountName, string charName);
        L1PcInstance restoreCharacter(string charName);
        L1PcInstance loadCharacter(string charName);
        void ClearOnlineStatus();
        void updateOnlineStatus(L1PcInstance pc);
        void updatePartnerId(int targetId);
        void updatePartnerId(int targetId, int partnerId);
        void saveCharStatus(L1PcInstance pc);
        void restoreInventory(L1PcInstance pc);
        bool doesCharNameExist(string name);
        void loadAllCharName();
        void disconnectAllCharacters();
    }
}
